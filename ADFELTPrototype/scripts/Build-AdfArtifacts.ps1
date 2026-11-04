param([string]$TransformPath)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
if (-not $TransformPath) {
    $TransformPath = Join-Path (Split-Path $projectRoot -Parent) 'education-provider-registry-docs\models\schema\establishment\transforms\establishment-from-bau.sql'
}
if (-not (Test-Path -LiteralPath $TransformPath)) {
    throw 'Martin transform not found. Supply -TransformPath.'
}

$original = [IO.File]::ReadAllText($TransformPath)
$typeRule = [regex]::Match($original, '(?s)CASE\s+WHEN s\.type_name.*?END AS establishment_type_id')
$phaseRule = [regex]::Match($original, '(?s)CASE s\.education_phase_name.*?END AS education_phase_id')
if (-not $typeRule.Success -or -not $phaseRule.Success) {
    throw 'Type/phase CASE blocks were not found. Check the upstream transform before regenerating.'
}
$hash = (Get-FileHash -LiteralPath $TransformPath -Algorithm SHA256).Hash.ToLowerInvariant()
$query = @"
-- Four-establishment source query, generated from establishment-from-bau.sql.
-- Upstream SHA-256: $hash
WITH source_establishment AS (
    SELECT e.URN, e.UKPRN, e.EstablishmentNumber, e.EstablishmentName,
           et.name AS type_name, ep.name AS education_phase_name
    FROM dbo.Establishment AS e
    LEFT JOIN dbo.EstablishmentType AS et ON et.code = e.type_code
    LEFT JOIN dbo.EducationPhase AS ep ON ep.code = e.educationPhase_code
    WHERE e.URN IN (134314, 135905, 136102, 137603)
)
SELECT
    s.URN AS urn,
    s.UKPRN AS ukprn,
    s.EstablishmentNumber AS establishment_number,
    s.EstablishmentName AS name,
    $($typeRule.Value),
    $($phaseRule.Value)
FROM source_establishment AS s
ORDER BY s.URN;
"@

$pipelinePath = Join-Path $projectRoot 'adf\pipeline\runcopyprotectagainstdup.json'
$pipeline = Get-Content -LiteralPath $pipelinePath -Raw | ConvertFrom-Json
$copy = @($pipeline.properties.activities | Where-Object type -eq 'Copy')
if ($copy.Count -ne 1) {
    throw 'Expected one Copy activity.'
}
$query = $query + [Environment]::NewLine
$copy[0].typeProperties.source.sqlReaderQuery = $query
$encoding = [Text.UTF8Encoding]::new($false)
[IO.File]::WriteAllText((Join-Path $projectRoot 'adf-tests\02-martin-establishment-source.sql'), $query, $encoding)
[IO.File]::WriteAllText($pipelinePath, ($pipeline | ConvertTo-Json -Depth 100) + [Environment]::NewLine, $encoding)
Write-Output 'Updated the source query and pipeline JSON. No Azure deployment performed.'
