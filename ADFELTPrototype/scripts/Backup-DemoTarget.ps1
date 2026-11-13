$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$compose=Join-Path $root 'database setup\docker-compose.yml'
Import-Module (Join-Path $PSScriptRoot 'DemoTools.psm1') -Force
$target=@(Get-DemoTarget)
if ($target.Count -ne 4) { throw 'Expected four target fixtures before taking the demo backup.' }
$token=[DateTimeOffset]::UtcNow.ToString('yyyyMMdd-HHmmss')+'-'+[guid]::NewGuid().ToString('N').Substring(0,6)
$backupDirectory=Join-Path $root '.runtime\backups'
New-Item -ItemType Directory -Path $backupDirectory -Force | Out-Null
$dumpPath=Join-Path $backupDirectory ('gias2-'+$token+'.dump')
$remotePath='/tmp/gias2-'+$token+'.dump'
$container=(docker compose -f $compose ps -q postgresgiastarget | Select-Object -First 1)
if ($LASTEXITCODE -ne 0 -or -not $container) { throw 'Target container is not running.' }
try {
    docker compose -f $compose exec -T postgresgiastarget pg_dump -U postgres -d gias2_development -Fc -f $remotePath
    if ($LASTEXITCODE -ne 0) { throw 'Target backup failed.' }
    docker cp "${container}:$remotePath" $dumpPath
    if ($LASTEXITCODE -ne 0) { throw 'Backup copy failed.' }
    $manifest=[ordered]@{createdUtc=[DateTimeOffset]::UtcNow.ToString('o');database='gias2_development';dumpPath=$dumpPath;sha256=(Get-FileHash -LiteralPath $dumpPath -Algorithm SHA256).Hash;target=$target}
    $manifestPath=$dumpPath+'.json'
    [System.IO.File]::WriteAllText($manifestPath,($manifest | ConvertTo-Json -Depth 20),[System.Text.UTF8Encoding]::new($false))
    Write-Output "Backup manifest: $manifestPath"
} finally {
    # Remove only this task's explicitly named temporary container file.
    docker compose -f $compose exec -T postgresgiastarget rm -f -- $remotePath
}
