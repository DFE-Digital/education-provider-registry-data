$ErrorActionPreference='Stop'
$script:ProjectRoot=Split-Path $PSScriptRoot -Parent
$script:ComposePath=Join-Path $script:ProjectRoot 'database setup\docker-compose.yml'

function Invoke-DemoPostgres {
    param([Parameter(Mandatory)][string]$Sql,[string]$Database='gias2_development')
    $result=docker compose -f $script:ComposePath exec -T postgresgiastarget psql -U postgres -d $Database -v ON_ERROR_STOP=1 -qAt -c $Sql
    if ($LASTEXITCODE -ne 0) { throw 'PostgreSQL demo query failed.' }
    return ($result -join [Environment]::NewLine).Trim()
}
function Invoke-DemoSqlServer {
    param([Parameter(Mandatory)][string]$Sql)
    # This helper reads the fixed four-row/six-field demo slice; JSON fits within 4096 characters.
    $result=docker compose -f $script:ComposePath exec -T sqlserveredubase /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Password123!' -C -d gias_bau_test_local -b -h -1 -y 4096 -w 65535 -Q $Sql
    if ($LASTEXITCODE -ne 0) { throw 'SQL Server demo query failed.' }
    return ($result -join [Environment]::NewLine).Trim()
}
function Get-DemoTarget {
    $sql=@'
SELECT coalesce(json_agg(row_to_json(t) ORDER BY urn)::text,'[]')
FROM (SELECT urn,name,establishment_id,ukprn,establishment_number,establishment_type_id,education_phase_id
      FROM establishment.establishment WHERE urn IN (134314,135905,136102,137603)) t;
'@
    return @(Invoke-DemoPostgres -Sql $sql | ConvertFrom-Json)
}
function Get-DemoSource {
    $query=[System.IO.File]::ReadAllText((Join-Path $script:ProjectRoot 'adf-tests\02-martin-establishment-source.sql'))
    if (-not $query.Contains('ORDER BY s.URN;')) { throw 'Source query format changed; inspect demo helper.' }
    $query='SET NOCOUNT ON;'+[Environment]::NewLine+$query.Replace('ORDER BY s.URN;','ORDER BY s.URN FOR JSON PATH;')
    return @(Invoke-DemoSqlServer -Sql $query | ConvertFrom-Json)
}
Export-ModuleMember -Function Invoke-DemoPostgres,Invoke-DemoSqlServer,Get-DemoTarget,Get-DemoSource
