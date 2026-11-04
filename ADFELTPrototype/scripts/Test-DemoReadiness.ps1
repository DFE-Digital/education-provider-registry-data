param([switch]$CaptureBaseline)
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
Import-Module (Join-Path $PSScriptRoot 'DemoTools.psm1') -Force
& (Join-Path $PSScriptRoot 'Test-SpikeProject.ps1')
$issues=[System.Collections.Generic.List[string]]::new()
try {
    $service=Get-Service -Name DIAHostService -ErrorAction Stop
    if ($service.Status -ne 'Running') { $issues.Add('Integration Runtime service is stopped. Start it from Administrator PowerShell or Configuration Manager.') }
    else { Write-Output 'PASS: local Integration Runtime service is running (check Online status in ADF separately).' }
} catch { $issues.Add('Could not inspect the Integration Runtime service; check it manually.') }
$source=@(Get-DemoSource)
$target=@(Get-DemoTarget)
if ($source.Count -ne 4 -or $target.Count -ne 4) { $issues.Add('Expected four source and four target fixtures.') }
foreach ($row in $source) {
    $match=@($target | Where-Object { $_.urn -eq $row.urn })
    if ($match.Count -ne 1) { continue }
    foreach ($field in @('name','ukprn','establishment_number','establishment_type_id','education_phase_id')) {
        if ($row.$field -ne $match[0].$field) { $issues.Add("Source/target differ for URN $($row.urn), field $field. Finish/reset the demo and rerun ADF.") }
    }
    $phase=if ($row.urn -eq 134314) { 6 } else { 5 }
    if ($row.establishment_type_id -ne 4 -or $row.education_phase_id -ne $phase) { $issues.Add("Unexpected mapped IDs for URN $($row.urn).") }
}
if (@($source | Where-Object { $_.name -like '*[[]ADF demo[]]*' -or $_.name -like '*[[]ADF rerun test[]]*' }).Count) {
    $issues.Add('A temporary demo name is still present in the source.')
}
$sourceCount=Invoke-DemoSqlServer -Sql 'SET NOCOUNT ON; SELECT COUNT_BIG(*) FROM dbo.Establishment;'
if ([long]$sourceCount -ne 52400) { $issues.Add('Expected 52,400 rows in the reconstructed source.') }
$triggerCount=Invoke-DemoPostgres -Sql "SELECT count(*) FROM pg_trigger WHERE tgrelid='etl.establishment_batch_stage'::regclass AND NOT tgisinternal AND tgenabled <> 'D';"
if ([int]$triggerCount -ne 0) { $issues.Add('Active batch intake unexpectedly has an automatic trigger.') }
$runtime=Join-Path $root '.runtime'
New-Item -ItemType Directory -Path $runtime -Force | Out-Null
$baselinePath=Join-Path $runtime 'demo-baseline.json'
if ($CaptureBaseline) {
    # Do not capture the temporary edit as the desired baseline.
    if ($issues | Where-Object { $_ -notlike '*Runtime*' }) { throw 'Fix fixture/data issues before capturing the baseline.' }
    $baseline=[ordered]@{capturedUtc=[DateTimeOffset]::UtcNow.ToString('o');source=$source;target=$target}
    [System.IO.File]::WriteAllText($baselinePath,($baseline | ConvertTo-Json -Depth 20),[System.Text.UTF8Encoding]::new($false))
    Write-Output 'Captured fixture values and target UUIDs in .runtime/demo-baseline.json.'
} elseif (Test-Path -LiteralPath $baselinePath) {
    $baseline=Get-Content -LiteralPath $baselinePath -Raw | ConvertFrom-Json
    foreach ($before in $baseline.target) {
        $current=@($target | Where-Object urn -eq $before.urn)
        if ($current.Count -ne 1 -or $current[0].establishment_id -ne $before.establishment_id) { $issues.Add("Target UUID changed for URN $($before.urn).") }
    }
}
if ($issues.Count) {
    foreach ($issue in $issues) { Write-Output "ACTION: $issue" }
    throw 'Demo preflight has unresolved local items.'
}
Write-Output 'PASS: four fixtures match, UUIDs retained, source count correct and intake trigger-free.'
Write-Output 'ADF still must show IR Online and a successful three-activity run; this script does not verify cloud deployment.'
