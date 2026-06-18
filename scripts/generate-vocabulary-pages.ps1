param(
    [string]$VocabularyPath = (Join-Path (Join-Path $PSScriptRoot "..") (Join-Path "models" "establishment-details-vocabulary-skos.ttl")),
    [string]$OutputRoot = (Join-Path (Join-Path (Join-Path $PSScriptRoot "..") "docs-site") (Join-Path "content" "vocabulary"))
)

$ErrorActionPreference = "Stop"

$resolvedVocabularyPath = Resolve-Path -LiteralPath $VocabularyPath
$resolvedOutputRoot = New-Item -ItemType Directory -Force -Path $OutputRoot
$ttl = Get-Content -LiteralPath $resolvedVocabularyPath -Raw

function Get-PredicateText {
    param(
        [string]$Block,
        [string]$Predicate
    )

    $escapedPredicate = [regex]::Escape($Predicate)
    $match = [regex]::Match($Block, "(?ms)$escapedPredicate\s+(.*?)(?:\s*;\s*|\s*\.\s*(?:\r?\n|$))")
    if ($match.Success) {
        return $match.Groups[1].Value.Trim()
    }

    return ""
}

function Get-Literals {
    param(
        [string]$Block,
        [string]$Predicate
    )

    $predicateText = Get-PredicateText -Block $Block -Predicate $Predicate
    if ([string]::IsNullOrWhiteSpace($predicateText)) {
        return @()
    }

    return [regex]::Matches($predicateText, '"((?:[^"\\]|\\.)*)"(?:@en|\^\^xsd:[A-Za-z]+)?') |
        ForEach-Object {
            $_.Groups[1].Value.Replace('\"', '"')
        }
}

function Get-Refs {
    param(
        [string]$Block,
        [string]$Predicate
    )

    $predicateText = Get-PredicateText -Block $Block -Predicate $Predicate
    if ([string]::IsNullOrWhiteSpace($predicateText)) {
        return @()
    }

    return [regex]::Matches($predicateText, 'epr:([A-Za-z][A-Za-z0-9]*)') |
        ForEach-Object {
            $_.Groups[1].Value
        }
}

function Escape-MarkdownTableCell {
    param([AllowNull()][string]$Value)

    if ([string]::IsNullOrWhiteSpace($Value)) {
        return ""
    }

    return ($Value -replace '\|', '\|' -replace "`r?`n", "<br>")
}

function Join-Values {
    param([string[]]$Values)

    if ($null -eq $Values -or $Values.Count -eq 0) {
        return ""
    }

    return ($Values | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) -join "<br>"
}

$conceptMatches = [regex]::Matches(
    $ttl,
    "(?ms)^epr:([A-Za-z][A-Za-z0-9]*)\s*\r?\n\s+a\s+skos:Concept\s*;\s*(.*?)(?=^\S|\z)"
)

$concepts = foreach ($match in $conceptMatches) {
    $localName = $match.Groups[1].Value
    $block = $match.Groups[2].Value
    $preferredLabel = (Get-Literals -Block $block -Predicate "skos:prefLabel" | Select-Object -First 1)

    if ([string]::IsNullOrWhiteSpace($preferredLabel)) {
        $preferredLabel = $localName
    }

    [pscustomobject]@{
        LocalName = $localName
        PreferredLabel = $preferredLabel
        AlternativeLabels = @(Get-Literals -Block $block -Predicate "skos:altLabel")
        Definition = (Get-Literals -Block $block -Predicate "skos:definition" | Select-Object -First 1)
        ScopeNotes = @(Get-Literals -Block $block -Predicate "skos:scopeNote")
        Status = (Get-Literals -Block $block -Predicate "epr:status" | Select-Object -First 1)
        Broader = @(Get-Refs -Block $block -Predicate "skos:broader")
        Related = @(Get-Refs -Block $block -Predicate "skos:related")
        Sources = @(Get-Literals -Block $block -Predicate "dcterms:source")
    }
}

if ($concepts.Count -eq 0) {
    throw "No skos:Concept blocks were found in $resolvedVocabularyPath"
}

$conceptLookup = @{}
foreach ($concept in $concepts) {
    $conceptLookup[$concept.LocalName] = $concept
}

function Format-ConceptLinks {
    param(
        [string[]]$LocalNames,
        [hashtable]$ConceptLookup
    )

    if ($null -eq $LocalNames -or $LocalNames.Count -eq 0) {
        return ""
    }

    return ($LocalNames | ForEach-Object {
        if ($ConceptLookup.ContainsKey($_)) {
            $label = $ConceptLookup[$_].PreferredLabel
            "[$label](../$_/)"
        }
        else {
            "``epr:$($_)``"
        }
    }) -join "<br>"
}

foreach ($concept in $concepts | Sort-Object PreferredLabel, LocalName) {
    $canonicalUri = "https://dfe-digital.github.io/education-provider-registry-data-prototype/vocabulary/$($concept.LocalName)/"
    $sourceTtl = "https://github.com/DFE-Digital/education-provider-registry-data-prototype/blob/main/models/establishment-details-vocabulary-skos.ttl"

    $lines = @(
        "# $($concept.PreferredLabel)",
        "",
        "[Back to vocabulary index](../)",
        "",
        "| Property | Value |",
        "| --- | --- |",
        "| Canonical URI | <$canonicalUri> |",
        "| Compact identifier | ``epr:$($concept.LocalName)`` |",
        "| Preferred label | $(Escape-MarkdownTableCell $concept.PreferredLabel) |",
        "| Alternative labels | $(Escape-MarkdownTableCell (Join-Values $concept.AlternativeLabels)) |",
        "| Status | $(Escape-MarkdownTableCell $concept.Status) |",
        "| Broader concepts | $(Escape-MarkdownTableCell (Format-ConceptLinks -LocalNames $concept.Broader -ConceptLookup $conceptLookup)) |",
        "| Related concepts | $(Escape-MarkdownTableCell (Format-ConceptLinks -LocalNames $concept.Related -ConceptLookup $conceptLookup)) |",
        "",
        "## Definition",
        "",
        $(if ([string]::IsNullOrWhiteSpace($concept.Definition)) { "No definition recorded." } else { $concept.Definition }),
        "",
        "## Scope Notes",
        ""
    )

    if ($concept.ScopeNotes.Count -eq 0) {
        $lines += "No scope notes recorded."
    }
    else {
        foreach ($note in $concept.ScopeNotes) {
            $lines += "- $note"
        }
    }

    $lines += @(
        "",
        "## Source",
        "",
        "Source TTL: <$sourceTtl>"
    )

    if ($concept.Sources.Count -gt 0) {
        $lines += ""
        $lines += "Recorded source notes:"
        foreach ($source in $concept.Sources) {
            $lines += "- $source"
        }
    }

    Set-Content -LiteralPath (Join-Path $resolvedOutputRoot "$($concept.LocalName).md") -Value $lines -Encoding UTF8
}

$indexLines = @(
    "# Vocabulary",
    "",
    "This index is generated from `models/establishment-details-vocabulary-skos.ttl`.",
    "",
    "| Concept | Compact identifier | Status | Definition |",
    "| --- | --- | --- | --- |"
)

foreach ($concept in $concepts | Sort-Object PreferredLabel, LocalName) {
    $definition = Escape-MarkdownTableCell $concept.Definition
    $indexLines += "| [$($concept.PreferredLabel)](./$($concept.LocalName)/) | ``epr:$($concept.LocalName)`` | $(Escape-MarkdownTableCell $concept.Status) | $definition |"
}

Set-Content -LiteralPath (Join-Path $resolvedOutputRoot "index.md") -Value $indexLines -Encoding UTF8

Write-Host "Generated $($concepts.Count) vocabulary concept pages in $resolvedOutputRoot"
