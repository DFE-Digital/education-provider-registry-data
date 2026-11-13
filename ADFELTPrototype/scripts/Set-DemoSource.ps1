param([Parameter(Mandatory)][ValidateSet('Update','Restore')][string]$Mode)
$ErrorActionPreference='Stop'
Import-Module (Join-Path $PSScriptRoot 'DemoTools.psm1') -Force
$original='Co-op Academy Stoke-On-Trent'
$changed=$original+' [ADF demo]'
$newName=if ($Mode -eq 'Update') { $changed } else { $original }
$allowedName=if ($Mode -eq 'Update') { $original } else { $changed }
$sql=@"
SET NOCOUNT ON; SET XACT_ABORT ON; BEGIN TRANSACTION;
IF NOT EXISTS (SELECT 1 FROM dbo.Establishment WHERE URN=136102 AND EstablishmentName IN (N'$original',N'$changed'))
    THROW 51000, 'Unexpected source state; refusing to overwrite another change', 1;
UPDATE dbo.Establishment SET EstablishmentName=N'$newName'
WHERE URN=136102 AND EstablishmentName=N'$allowedName';
SELECT URN,EstablishmentName FROM dbo.Establishment WHERE URN=136102 FOR JSON PATH;
COMMIT;
"@
Write-Output (Invoke-DemoSqlServer -Sql $sql)
Write-Output 'Source only changed. Run ADF next to propagate it to GIAS; do not run the importer during this demonstration.'
