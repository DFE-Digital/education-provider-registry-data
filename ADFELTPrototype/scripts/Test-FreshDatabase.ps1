$ErrorActionPreference='Stop'
$projectRoot=Split-Path $PSScriptRoot -Parent
$testDatabase='adf_fresh_'+[guid]::NewGuid().ToString('N').Substring(0,16)
$created=$false
Push-Location (Join-Path $projectRoot 'database setup')
try {
    $exists=docker compose exec -T postgresgiastarget psql -U postgres -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname='$testDatabase';"
    if ($LASTEXITCODE -ne 0 -or $exists -eq '1') { throw 'Cannot safely create isolated fresh test database.' }
    docker compose exec -T postgresgiastarget createdb -U postgres $testDatabase
    if ($LASTEXITCODE -ne 0) { throw 'Test database creation failed.' }
    $created=$true
    foreach ($file in @('core-establishment-schema.sql','02-seed-first-slice-reference.sql','03-adf-establishment-batch.sql','03-adf-establishment-batch.sql')) {
        Get-Content -LiteralPath $file -Raw | docker compose exec -T postgresgiastarget psql -U postgres -d $testDatabase -v ON_ERROR_STOP=1 -q
        if ($LASTEXITCODE -ne 0) { throw "Fresh setup failed: $file" }
    }
    Get-Content -LiteralPath (Join-Path $projectRoot 'adf-tests\verify-fresh-batch.sql') -Raw |
        docker compose exec -T postgresgiastarget psql -U postgres -d $testDatabase -v ON_ERROR_STOP=1
    if ($LASTEXITCODE -ne 0) { throw 'Fresh batch checks failed.' }
} finally {
    if ($created) {
        docker compose exec -T postgresgiastarget dropdb -U postgres $testDatabase
        if ($LASTEXITCODE -ne 0) { Write-Warning "Remove this task-created test database manually: $testDatabase" }
    }
    Pop-Location
}
