param([switch]$DatabaseChecks)
$ErrorActionPreference='Stop'
$projectRoot=Split-Path $PSScriptRoot -Parent
$pipelinePath=Join-Path $projectRoot 'adf\pipeline\runcopyprotectagainstdup.json'
$pipeline=Get-Content -LiteralPath $pipelinePath -Raw | ConvertFrom-Json
$activities=@($pipeline.properties.activities)
$names=[System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($a in $activities) { if (-not $names.Add($a.name)) { throw 'Duplicate activity name.' } }
foreach ($a in $activities) {
    foreach ($d in $a.dependsOn) { if (-not $names.Contains($d.activity)) { throw "Missing dependency $($d.activity)" } }
}
$json=[System.IO.File]::ReadAllText($pipelinePath)
foreach ($m in [regex]::Matches($json,"activity\('([^']+)'\)")) {
    if (-not $names.Contains($m.Groups[1].Value)) { throw "Case/name mismatch in activity reference: $($m.Groups[1].Value)" }
}
$copy=@($activities | Where-Object type -eq 'Copy')[0]
$lookup=@($activities | Where-Object type -eq 'Lookup')[0]
$validation=@($activities | Where-Object type -eq 'IfCondition')[0]
if (-not $copy -or -not $lookup -or -not $validation) { throw 'Copy, Lookup and validation are required.' }
if ($copy.policy.retry -ne 0 -or $pipeline.properties.concurrency -ne 1) { throw 'Unsafe first-slice Copy retry/concurrency configuration.' }
if ($pipeline.properties.parameters.expectedRows.defaultValue -ne 4) { throw 'Expected four-row first slice.' }
if (-not $lookup.typeProperties.firstRowOnly) { throw 'Lookup must return firstRow for validation.' }
if ($lookup.typeProperties.source.query.value -notlike '*pipeline().parameters.expectedRows*') { throw 'Loader must enforce expectedRows before applying.' }
$mapping=@($copy.typeProperties.translator.mappings | ForEach-Object { $_.sink.name })
foreach ($column in @('urn','ukprn','establishment_number','name','establishment_type_id','education_phase_id','batch_id')) {
    if ($column -cnotin $mapping) { throw "Missing $column mapping." }
}
if ($mapping.Count -ne 7) { throw 'Unexpected additional mapped columns.' }
foreach ($ref in @($copy.inputs)+@($copy.outputs)+@($lookup.typeProperties.dataset)) {
    $datasetPath=Join-Path $projectRoot ('adf\dataset\'+$ref.referenceName+'.json')
    if (-not (Test-Path -LiteralPath $datasetPath)) { throw "Missing dataset definition: $datasetPath" }
    $null=Get-Content -LiteralPath $datasetPath -Raw | ConvertFrom-Json
}
$stage=Get-Content -LiteralPath (Join-Path $projectRoot 'adf\dataset\batchloadstagingarea.json') -Raw | ConvertFrom-Json
if ($stage.properties.typeProperties.tableName -ne '"etl"."establishment_batch_stage"') { throw 'Pipeline is not using explicit batch intake.' }

# Check expression delimiters while respecting ADF single-quoted strings/escapes.
foreach ($expression in @($lookup.typeProperties.source.query.value,$validation.typeProperties.expression.value)) {
    $depth=0; $quoted=$false
    for ($i=0;$i -lt $expression.Length;$i++) {
        $char=$expression[$i]
        if ($char -eq "'") {
            if ($quoted -and $i+1 -lt $expression.Length -and $expression[$i+1] -eq "'") { $i++; continue }
            $quoted=-not $quoted; continue
        }
        if (-not $quoted) {
            if ($char -eq '(') { $depth++ }
            if ($char -eq ')') { $depth--; if ($depth -lt 0) { throw 'Invalid expression parentheses.' } }
        }
    }
    if ($depth -ne 0 -or $quoted) { throw 'Unbalanced ADF expression.' }
}
$composePath=Join-Path $projectRoot 'database setup\docker-compose.yml'
$compose=[System.IO.File]::ReadAllText($composePath)
if ($compose.Contains('02-adf-establishment-rerun.sql')) { throw 'Trigger trial is still in active initialisation.' }
foreach ($m in [regex]::Matches($compose,'(?m)^\s*- \./([^:]+):/docker-entrypoint-initdb.d/')) {
    if (-not (Test-Path -LiteralPath (Join-Path (Split-Path $composePath -Parent) $m.Groups[1].Value))) { throw 'Missing Compose SQL mount.' }
}
foreach ($markdown in Get-ChildItem -LiteralPath $projectRoot -Recurse -Filter '*.md' | Where-Object { $_.FullName -notmatch '\\(\.runtime|\.git|\.work)\\' }) {
    $text=[System.IO.File]::ReadAllText($markdown.FullName)
    foreach ($m in [regex]::Matches($text,'\[[^\]]*\]\(([^)]+)\)')) {
        $link=$m.Groups[1].Value.Trim('<','>')
        if ($link -match '^(https?://|#)') { continue }
        $relative=[Uri]::UnescapeDataString(($link -split '#')[0])
        if ($relative -and -not (Test-Path -LiteralPath (Join-Path $markdown.DirectoryName $relative))) {
            throw "Broken local documentation link in $($markdown.Name): $link"
        }
    }
}
Write-Output 'Local pipeline/dataset references, mappings, expression delimiters, setup mounts and documentation links passed.'
if ($DatabaseChecks) {
    Push-Location (Join-Path $projectRoot 'database setup')
    try {
        docker compose config --quiet
        if ($LASTEXITCODE -ne 0) { throw 'Compose validation failed.' }
        Get-Content -LiteralPath (Join-Path $projectRoot 'adf-tests\verify-explicit-batch.sql') -Raw |
            docker compose exec -T postgresgiastarget psql -U postgres -d gias2_development -v ON_ERROR_STOP=1
        if ($LASTEXITCODE -ne 0) { throw 'Rolled-back batch integration tests failed.' }
    } finally { Pop-Location }
}
Write-Output 'File checks do not deploy or run ADF.'
