param(
    [string]$ShaclPath = (Join-Path (Join-Path $PSScriptRoot "..\..") (Join-Path "models" "establishment-details-shacl.ttl")),
    [string]$OutputPath = (Join-Path (Join-Path $PSScriptRoot "..") (Join-Path "content" "models" "establishment-type-field-rules.md"))
)

$ErrorActionPreference = "Stop"

$resolvedShaclPath = Resolve-Path -LiteralPath $ShaclPath
$ttl = Get-Content -LiteralPath $resolvedShaclPath -Raw

# Class-to-leaf-types map for sh:targetClass on intermediate OWL classes
$classToTypes = @{
    'LaMaintainedSchool'                       = @('CommunitySchool', 'VoluntaryAidedSchool', 'VoluntaryControlledSchool', 'FoundationSchool', 'CommunitySpecialSchool', 'FoundationSpecialSchool', 'PupilReferralUnit', 'LocalAuthorityNurserySchool')
    'LaMainstreamSchool'                        = @('CommunitySchool', 'VoluntaryAidedSchool', 'VoluntaryControlledSchool', 'FoundationSchool')
    'LaMaintainedSpecialSchool'                 = @('CommunitySpecialSchool', 'FoundationSpecialSchool')
    'NonMaintainedAndIndependentSpecialSchool'  = @('NonMaintainedSpecialSchool', 'OtherIndependentSpecialSchool')
    'IndependentSchool'                         = @('CityTechnologyCollege', 'OtherIndependentSchool')
    'Academy'                                   = @('AcademySponsorLed', 'AcademyConverter', 'AcademySpecialSponsorLed', 'AcademySpecialConverter', 'AcademyAlternativeProvisionConverter', 'AcademyAlternativeProvisionSponsorLed', 'Academy1619Converter', 'Academy16To19SponsorLed', 'AcademySecure16To19')
    'MainstreamAcademy'                         = @('AcademySponsorLed', 'AcademyConverter')
    'AcademySpecial'                            = @('AcademySpecialSponsorLed', 'AcademySpecialConverter')
    'AcademyAlternativeProvision'               = @('AcademyAlternativeProvisionConverter', 'AcademyAlternativeProvisionSponsorLed')
    'Academy1619'                               = @('Academy1619Converter', 'Academy16To19SponsorLed')
    'FreeSchoolEstablishment'                   = @('FreeSchool', 'FreeSchoolSpecial', 'FreeSchoolAlternativeProvision', 'FreeSchool16To19', 'UniversityTechnicalCollege', 'StudioSchool')
    'MainstreamFreeSchool'                      = @('FreeSchool', 'UniversityTechnicalCollege', 'StudioSchool')
    'College'                                   = @('FurtherEducation', 'SixthFormCentre')
    'ChildrensCentreEstablishment'              = @('ChildrensCentre', 'ChildrensCentreLinkedSite')
}

# Preferred labels for establishment type local names
$typeLabels = @{
    "CommunitySchool"                              = "Community school"
    "VoluntaryAidedSchool"                         = "Voluntary aided school"
    "VoluntaryControlledSchool"                    = "Voluntary controlled school"
    "FoundationSchool"                             = "Foundation school"
    "CommunitySpecialSchool"                       = "Community special school"
    "FoundationSpecialSchool"                      = "Foundation special school"
    "PupilReferralUnit"                            = "Pupil referral unit"
    "LocalAuthorityNurserySchool"                  = "Local authority nursery school"
    "NonMaintainedSpecialSchool"                   = "Non-maintained special school"
    "OtherIndependentSpecialSchool"                = "Other independent special school"
    "CityTechnologyCollege"                        = "City technology college"
    "OtherIndependentSchool"                       = "Other independent school"
    "AcademySponsorLed"                            = "Academy sponsor led"
    "AcademySpecialSponsorLed"                     = "Academy special sponsor led"
    "AcademyConverter"                             = "Academy converter"
    "AcademyAlternativeProvisionConverter"         = "Academy alternative provision converter"
    "AcademyAlternativeProvisionSponsorLed"        = "Academy alternative provision sponsor led"
    "AcademySpecialConverter"                      = "Academy special converter"
    "Academy1619Converter"                         = "Academy 16-19 converter"
    "Academy16To19SponsorLed"                      = "Academy 16 to 19 sponsor led"
    "AcademySecure16To19"                          = "Academy secure 16 to 19"
    "FreeSchool"                                   = "Free schools"
    "FreeSchoolSpecial"                            = "Free schools special"
    "FreeSchoolAlternativeProvision"               = "Free schools alternative provision"
    "FreeSchool16To19"                             = "Free schools 16 to 19"
    "UniversityTechnicalCollege"                   = "University technical college"
    "StudioSchool"                                 = "Studio schools"
    "FurtherEducation"                             = "Further education"
    "SixthFormCentre"                              = "Sixth form centre"
    "HigherEducationInstitution"                   = "Higher education institution"
    "WelshEstablishment"                           = "Welsh establishment"
    "SecureUnits"                                  = "Secure units"
    "OffshoreSchools"                              = "Offshore schools"
    "ServiceChildrensEducation"                    = "Service children's education"
    "MiscellaneousEstablishment"                   = "Miscellaneous"
    "SpecialPost16Institution"                     = "Special post-16 institution"
    "BritishSchoolsOverseas"                       = "British schools overseas"
    "InstitutionFundedByOtherGovernmentDepartment" = "Institution funded by other government department"
    "ChildrensCentre"                              = "Children's centre"
    "ChildrensCentreLinkedSite"                    = "Children's centre linked site"
    "OnlineProvider"                               = "Online provider"
    "Establishment"                                = "All establishments"
}

function Get-FirstStringLiteral {
    param([string]$Block, [string]$Predicate)
    $pattern = [regex]::Escape($Predicate) + '\s+"((?:[^"\\]|\\.)*)"'
    $match = [regex]::Match($Block, $pattern)
    if ($match.Success) { return $match.Groups[1].Value }
    return ""
}

function Get-ConstraintLabel {
    param([string]$PropertyBlock)
    $minMatch = [regex]::Match($PropertyBlock, 'sh:minCount\s+(\d+)')
    $maxMatch = [regex]::Match($PropertyBlock, 'sh:maxCount\s+(\d+)')
    $min = if ($minMatch.Success) { [int]$minMatch.Groups[1].Value } else { $null }
    $max = if ($maxMatch.Success) { [int]$maxMatch.Groups[1].Value } else { $null }

    if ($null -ne $max -and $max -eq 0)                                           { return "Not applicable" }
    if ($null -ne $min -and $min -eq 1 -and $null -ne $max -and $max -eq 1)      { return "Required — exactly one" }
    if ($null -ne $min -and $min -eq 1)                                           { return "Required — at least one" }
    return "Optional"
}

function Escape-MarkdownTableCell {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return "" }
    return $Value -replace '\|', '\|'
}

# Extract each sh:NodeShape block. Shapes start at the beginning of a line with epr:<Name>.
$shapeMatches = [regex]::Matches(
    $ttl,
    '(?ms)^(epr:\w+)\r?\n\s+a sh:NodeShape\s*;(.*?)(?=^epr:|\z)'
)

if ($shapeMatches.Count -eq 0) {
    throw "No sh:NodeShape blocks found in $resolvedShaclPath"
}

$lines = @(
    "# Establishment Type Field Rules",
    "",
    "This page documents current GIAS field applicability rules by establishment type. It is generated from ``models/establishment-details-shacl.ttl`` during the site build. Do not edit this file directly.",
    "",
    "**This page is not a complete inventory of current GIAS Details-tab field labels.** It covers structural constraints — which fields must, must not, or may be present for each establishment type. Address and contact fields (headteacher, website, telephone) appear here as universal constraints. Value-level rules for dates, pupil numbers and capacity are in the separate [data quality rules page](../establishment-data-quality-rules/).",
    "",
    "Each section corresponds to a SHACL shape in the source file. Group shapes cover all types in a family. Type-specific shapes add additional rules for that type.",
    "",
    "| Rule | Meaning |",
    "| --- | --- |",
    "| Required — exactly one | The field must be present with exactly one value |",
    "| Required — at least one | The field must be present with at least one value |",
    "| Not applicable | The field must not be present for this establishment type |",
    "",
    "**Note on 'Local authority — accountability relationship (not applicable)':** This rule means the establishment type has no accountability relationship with a local authority in the data model. It does not mean the local authority geographic or administrative row is absent from the GIAS Details tab — that row appears for all establishment types in the current GIAS UI.",
    ""
)

foreach ($shapeMatch in $shapeMatches) {
    $localName = $shapeMatch.Groups[1].Value -replace '^epr:', ''
    $block     = $shapeMatch.Groups[2].Value

    $label   = Get-FirstStringLiteral -Block $block -Predicate 'rdfs:label'
    $comment = Get-FirstStringLiteral -Block $block -Predicate 'rdfs:comment'
    if ([string]::IsNullOrWhiteSpace($label)) { $label = $localName }

    # Group shapes and the universal shape get H2; type-specific shapes get H3
    $headingLevel = if ($localName -match 'Group|Universal') { '##' } else { '###' }

    $lines += '---'
    $lines += ''
    $lines += "$headingLevel $label"
    $lines += ''

    if (-not [string]::IsNullOrWhiteSpace($comment)) {
        $lines += $comment
        $lines += ''
    }

    # Covered types — from SPARQL VALUES clause or sh:targetClass
    $valuesMatch      = [regex]::Match($block, '(?s)VALUES \?type \{([^}]+)\}')
    $targetClassMatch = [regex]::Match($block, 'sh:targetClass (epr:\w+)')

    if ($valuesMatch.Success) {
        $typeLocalNames = [regex]::Matches($valuesMatch.Groups[1].Value, 'epr:(\w+)') |
            ForEach-Object { $_.Groups[1].Value }
        $coveredLabels = $typeLocalNames | ForEach-Object {
            if ($typeLabels.ContainsKey($_)) { $typeLabels[$_] } else { $_ }
        }
        $lines += "**Covers:** $($coveredLabels -join ', ')"
        $lines += ''
    }
    elseif ($targetClassMatch.Success) {
        $className = $targetClassMatch.Groups[1].Value -replace '^epr:', ''
        if ($classToTypes.ContainsKey($className)) {
            $coveredLabels = $classToTypes[$className] | ForEach-Object {
                if ($typeLabels.ContainsKey($_)) { $typeLabels[$_] } else { $_ }
            }
            $lines += "**Covers:** $($coveredLabels -join ', ')"
        } elseif ($typeLabels.ContainsKey($className)) {
            $lines += "**Covers:** $($typeLabels[$className])"
        } else {
            $lines += "**Covers:** $className"
        }
        $lines += ''
    }

    # Property constraints
    $propertyMatches = [regex]::Matches($block, '(?s)sh:property\s+\[\s*(.*?)\s*\]')

    if ($propertyMatches.Count -gt 0) {
        $lines += '| Field | Rule | Reason |'
        $lines += '| --- | --- | --- |'

        foreach ($propMatch in $propertyMatches) {
            $propBlock  = $propMatch.Groups[1].Value
            $fieldName  = Get-FirstStringLiteral -Block $propBlock -Predicate 'sh:name'
            $message    = Get-FirstStringLiteral -Block $propBlock -Predicate 'sh:message'
            $constraint = Get-ConstraintLabel -PropertyBlock $propBlock

            # Strip parenthetical qualifier from sh:name — the constraint column already carries it
            $fieldName = ($fieldName -replace '\s*\([^)]+\)\s*$', '').Trim()

            if (-not [string]::IsNullOrWhiteSpace($fieldName)) {
                $lines += "| $(Escape-MarkdownTableCell $fieldName) | $constraint | $(Escape-MarkdownTableCell $message) |"
            }
        }
        $lines += ''
    }
}

$lines += '---'
$lines += ''
$lines += "Source: [``models/establishment-details-shacl.ttl``](https://github.com/DFE-Digital/education-provider-registry-data/blob/main/models/establishment-details-shacl.ttl)"

$outputDir = Split-Path -Path $OutputPath -Parent
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
Set-Content -LiteralPath $OutputPath -Value $lines -Encoding UTF8

Write-Host "Generated $($shapeMatches.Count) shape sections in $OutputPath"
