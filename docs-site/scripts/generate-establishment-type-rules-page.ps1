param(
    [string]$ShaclPath  = (Join-Path (Join-Path $PSScriptRoot "..\..") (Join-Path "models" "establishment-details-shacl.ttl")),
    [string]$OutputPath = (Join-Path (Join-Path $PSScriptRoot "..") (Join-Path "content" "models" "establishment-field-rules-by-type.md"))
)

$ErrorActionPreference = "Stop"

$resolvedShaclPath = Resolve-Path -LiteralPath $ShaclPath
$ttl = Get-Content -LiteralPath $resolvedShaclPath -Raw

# ── helpers ───────────────────────────────────────────────────────────────────

function Get-StringLiteral {
    param([string]$Block, [string]$Predicate)
    $m = [regex]::Match($Block, [regex]::Escape($Predicate) + '\s+"((?:[^"\\]|\\.)*)"')
    if ($m.Success) { return $m.Groups[1].Value }
    return ''
}

function Strip-Qualifier {
    param([string]$Name)
    return ($Name -replace '\s*\([^)]+\)\s*$', '').Trim()
}

function Escape-Md {
    param([string]$Value)
    return $Value -replace '\|', '\|'
}

function Get-EffectiveConstraintLabel {
    param([object[]]$Rules)
    $minVals = @($Rules | Where-Object { $null -ne $_.MinCount } | ForEach-Object { $_.MinCount })
    $maxVals = @($Rules | Where-Object { $null -ne $_.MaxCount } | ForEach-Object { $_.MaxCount })

    $effectiveMin = if ($minVals.Count -gt 0) { ($minVals | Measure-Object -Maximum).Maximum } else { $null }
    $effectiveMax = if ($maxVals.Count -gt 0) { ($maxVals | Measure-Object -Minimum).Minimum  } else { $null }

    if ($null -ne $effectiveMax -and $effectiveMax -eq 0)                                                          { return 'Not applicable' }
    if ($null -ne $effectiveMin -and $effectiveMin -ge 1 -and $null -ne $effectiveMax -and $effectiveMax -eq 1)    { return 'Required — exactly one' }
    if ($null -ne $effectiveMin -and $effectiveMin -ge 1)                                                          { return 'Required — at least one' }
    return 'Optional'
}

# ── canonical type list (ordered by type code) ────────────────────────────────

$typeMeta = [ordered]@{
    'CommunitySchool'                              = @{ Code = '01'; Label = "Community school" }
    'VoluntaryAidedSchool'                         = @{ Code = '02'; Label = "Voluntary aided school" }
    'VoluntaryControlledSchool'                    = @{ Code = '03'; Label = "Voluntary controlled school" }
    'FoundationSchool'                             = @{ Code = '05'; Label = "Foundation school" }
    'CityTechnologyCollege'                        = @{ Code = '06'; Label = "City technology college" }
    'CommunitySpecialSchool'                       = @{ Code = '07'; Label = "Community special school" }
    'NonMaintainedSpecialSchool'                   = @{ Code = '08'; Label = "Non-maintained special school" }
    'OtherIndependentSpecialSchool'                = @{ Code = '10'; Label = "Other independent special school" }
    'OtherIndependentSchool'                       = @{ Code = '11'; Label = "Other independent school" }
    'FoundationSpecialSchool'                      = @{ Code = '12'; Label = "Foundation special school" }
    'PupilReferralUnit'                            = @{ Code = '14'; Label = "Pupil referral unit" }
    'LocalAuthorityNurserySchool'                  = @{ Code = '15'; Label = "Local authority nursery school" }
    'FurtherEducation'                             = @{ Code = '18'; Label = "Further education" }
    'AcademySponsorLed'                            = @{ Code = '28'; Label = "Academy sponsor led" }
    'HigherEducationInstitution'                   = @{ Code = '29'; Label = "Higher education institution" }
    'WelshEstablishment'                           = @{ Code = '30'; Label = "Welsh establishment" }
    'SixthFormCentre'                              = @{ Code = '31'; Label = "Sixth form centre" }
    'SpecialPost16Institution'                     = @{ Code = '32'; Label = "Special post-16 institution" }
    'AcademySpecialSponsorLed'                     = @{ Code = '33'; Label = "Academy special sponsor led" }
    'AcademyConverter'                             = @{ Code = '34'; Label = "Academy converter" }
    'FreeSchool'                                   = @{ Code = '35'; Label = "Free school" }
    'FreeSchoolSpecial'                            = @{ Code = '36'; Label = "Free school special" }
    'BritishSchoolsOverseas'                       = @{ Code = '37'; Label = "British schools overseas" }
    'FreeSchoolAlternativeProvision'               = @{ Code = '38'; Label = "Free school alternative provision" }
    'FreeSchool16To19'                             = @{ Code = '39'; Label = "Free school 16 to 19" }
    'UniversityTechnicalCollege'                   = @{ Code = '40'; Label = "University technical college" }
    'StudioSchool'                                 = @{ Code = '41'; Label = "Studio school" }
    'AcademyAlternativeProvisionConverter'         = @{ Code = '42'; Label = "Academy alternative provision converter" }
    'AcademyAlternativeProvisionSponsorLed'        = @{ Code = '43'; Label = "Academy alternative provision sponsor led" }
    'AcademySpecialConverter'                      = @{ Code = '44'; Label = "Academy special converter" }
    'Academy1619Converter'                         = @{ Code = '45'; Label = "Academy 16-19 converter" }
    'Academy16To19SponsorLed'                      = @{ Code = '46'; Label = "Academy 16 to 19 sponsor led" }
    'ChildrensCentre'                              = @{ Code = '47'; Label = "Children's centre" }
    'ChildrensCentreLinkedSite'                    = @{ Code = '48'; Label = "Children's centre linked site" }
    'OnlineProvider'                               = @{ Code = '49'; Label = "Online provider" }
    'AcademySecure16To19'                          = @{ Code = '57'; Label = "Academy secure 16 to 19" }
}

# ── parse all NodeShape blocks ────────────────────────────────────────────────

$shapeBlocks = [regex]::Matches($ttl, '(?ms)^(epr:\w+)\r?\n\s+a sh:NodeShape\s*;(.*?)(?=^epr:|\z)')

$shapes = [System.Collections.Generic.List[PSCustomObject]]::new()

foreach ($sm in $shapeBlocks) {
    $localName = $sm.Groups[1].Value
    $block     = $sm.Groups[2].Value

    $label = Get-StringLiteral $block 'rdfs:label'
    if ([string]::IsNullOrWhiteSpace($label)) { $label = $localName -replace '^epr:', '' }

    $isUniversal = $block -match 'sh:targetClass\s+epr:Establishment'

    $targetTypes = @()
    $vm = [regex]::Match($block, '(?s)VALUES \?type \{([^}]+)\}')
    if ($vm.Success) {
        $targetTypes = @([regex]::Matches($vm.Groups[1].Value, 'epr:(\w+)') |
                         ForEach-Object { $_.Groups[1].Value })
    }

    $propList = [System.Collections.Generic.List[PSCustomObject]]::new()
    foreach ($pm in [regex]::Matches($block, '(?s)sh:property\s+\[\s*(.*?)\s*\]')) {
        $pb   = $pm.Groups[1].Value
        $name = Get-StringLiteral $pb 'sh:name'
        if ([string]::IsNullOrWhiteSpace($name)) { continue }
        $minM = [regex]::Match($pb, 'sh:minCount\s+(\d+)')
        $maxM = [regex]::Match($pb, 'sh:maxCount\s+(\d+)')
        $propList.Add([pscustomobject]@{
            Name       = $name
            Stripped   = Strip-Qualifier $name
            MinCount   = if ($minM.Success) { [int]$minM.Groups[1].Value } else { $null }
            MaxCount   = if ($maxM.Success) { [int]$maxM.Groups[1].Value } else { $null }
            ShapeLabel = $label
        })
    }

    # Priority controls order within a type's field list: universal first, then groups, then specifics
    $priority = if ($isUniversal) { 0 } elseif ($localName -match 'Group') { 1 } else { 2 }

    $shapes.Add([pscustomobject]@{
        LocalName   = $localName
        Label       = $label
        IsUniversal = $isUniversal
        TargetTypes = $targetTypes
        Properties  = $propList.ToArray()
        Priority    = $priority
    })
}

# ── build per-type output ─────────────────────────────────────────────────────

$lines = [System.Collections.Generic.List[string]]::new()

$lines.Add('# Establishment Field Rules by Type')
$lines.Add('')
$lines.Add('This page is generated from ``models/establishment-details-shacl.ttl`` during the site build. Do not edit this file directly.')
$lines.Add('')
$lines.Add('Each section shows the complete field rules for one establishment type, combining all SHACL shapes that apply to it. Where the same field appears in both a general shape and a type-specific shape, the most restrictive rule applies.')
$lines.Add('')
$lines.Add('See the [structural shape rules page](../establishment-type-field-rules/) for the underlying per-shape rules and the reasoning behind each constraint.')
$lines.Add('')
$lines.Add('| Rule | Meaning |')
$lines.Add('| --- | --- |')
$lines.Add('| Required — exactly one | The field must be present with exactly one value |')
$lines.Add('| Required — at least one | The field must be present with at least one value |')
$lines.Add('| Optional | The field may be present but is not required |')
$lines.Add('| Not applicable | The field must not be present for this establishment type |')
$lines.Add('')

$typesOutput = 0

foreach ($typeLocalName in $typeMeta.Keys) {
    $meta = $typeMeta[$typeLocalName]

    $applicable = @($shapes |
        Where-Object { $_.IsUniversal -or ($_.TargetTypes -contains $typeLocalName) } |
        Sort-Object Priority)

    if ($applicable.Count -eq 0) { continue }

    $typesOutput++

    $lines.Add('---')
    $lines.Add('')
    $lines.Add("## $($meta.Label) (type $($meta.Code))")
    $lines.Add('')

    # Collect all properties from all applicable shapes in priority order.
    # Group by stripped field name; first-seen ordering preserved.
    $fieldOrder  = [System.Collections.Generic.List[string]]::new()
    $fieldGroups = @{}

    foreach ($shape in $applicable) {
        foreach ($prop in $shape.Properties) {
            $key = $prop.Stripped
            if (-not $fieldGroups.ContainsKey($key)) {
                $fieldGroups[$key] = [System.Collections.Generic.List[PSCustomObject]]::new()
                $fieldOrder.Add($key)
            }
            $fieldGroups[$key].Add($prop)
        }
    }

    $lines.Add('| Field | Rule |')
    $lines.Add('| --- | --- |')

    foreach ($key in $fieldOrder) {
        $constraint = Get-EffectiveConstraintLabel -Rules $fieldGroups[$key].ToArray()
        if ($constraint -eq 'Not applicable') { continue }
        $lines.Add("| $(Escape-Md $key) | $constraint |")
    }

    $lines.Add('')
}

$lines.Add('---')
$lines.Add('')
$lines.Add("Source: [``models/establishment-details-shacl.ttl``](https://github.com/DFE-Digital/education-provider-registry-data/blob/main/models/establishment-details-shacl.ttl)")

$outputDir = Split-Path -Path $OutputPath -Parent
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
Set-Content -LiteralPath $OutputPath -Value $lines.ToArray() -Encoding UTF8

Write-Host "Generated per-type field rules for $typesOutput establishment types in $OutputPath"
