param([Parameter(Mandatory)][string]$ManifestPath)
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$compose=Join-Path $root 'database setup\docker-compose.yml'
Import-Module (Join-Path $PSScriptRoot 'DemoTools.psm1') -Force
$manifest=Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json
if ((Get-FileHash -LiteralPath $manifest.dumpPath -Algorithm SHA256).Hash -ne $manifest.sha256) { throw 'Backup hash mismatch.' }
$testDatabase='adf_demo_restore_'+[guid]::NewGuid().ToString('N').Substring(0,12)
$remotePath='/tmp/'+$testDatabase+'.dump'
$container=(docker compose -f $compose ps -q postgresgiastarget | Select-Object -First 1)
$created=$false
if (-not $container) { throw 'Target container unavailable.' }
try {
    docker compose -f $compose exec -T postgresgiastarget createdb -U postgres $testDatabase
    if ($LASTEXITCODE -ne 0) { throw 'Could not create task-specific restore test database.' }
    $created=$true
    docker cp $manifest.dumpPath "${container}:$remotePath"
    if ($LASTEXITCODE -ne 0) { throw 'Restore file copy failed.' }
    docker compose -f $compose exec -T postgresgiastarget pg_restore -U postgres -d $testDatabase --no-owner --exit-on-error $remotePath
    if ($LASTEXITCODE -ne 0) { throw 'Backup restore failed.' }
    $rows=Invoke-DemoPostgres -Database $testDatabase -Sql "SELECT coalesce(json_agg(row_to_json(t) ORDER BY urn)::text,'[]') FROM (SELECT urn,name,establishment_id FROM establishment.establishment WHERE urn IN (134314,135905,136102,137603)) t;" | ConvertFrom-Json
    foreach ($before in $manifest.target) {
        $after=@($rows | Where-Object urn -eq $before.urn)
        if ($after.Count -ne 1 -or $after[0].establishment_id -ne $before.establishment_id -or $after[0].name -ne $before.name) { throw 'Restored fixture identity/value mismatch.' }
    }
    $function=Invoke-DemoPostgres -Database $testDatabase -Sql "SELECT to_regprocedure('etl.apply_establishment_batch(text,integer)') IS NOT NULL;"
    if ($function -ne 't') { throw 'Loading function missing from backup.' }
    Write-Output 'Backup restored successfully into an isolated database; four names/UUIDs and load function verified.'
} finally {
    if ($created) { docker compose -f $compose exec -T postgresgiastarget dropdb -U postgres $testDatabase }
    docker compose -f $compose exec -T postgresgiastarget rm -f -- $remotePath
}
