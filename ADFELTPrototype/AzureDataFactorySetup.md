# First-time local setup

Already have the databases and ADF pipeline? Use the [demo guide](demo/README.md). The steps below are for creating or rebuilding the local source environment.

Prerequisites: Docker Desktop, PowerShell, .NET for the source importer, an ADF factory, a registered self-hosted integration runtime (SHIR), and a 64-bit PostgreSQL ODBC driver on the runtime host.

## Databases

From the project root:

```powershell
docker compose -f ".\database setup\docker-compose.yml" up -d
```

| Role | Service | Host endpoint | Database | Local test login |
| --- | --- | --- | --- | --- |
| Source | sqlserveredubase | 127.0.0.1,1433 | gias_bau_test_local | sa / Password123! |
| Target | postgresgiastarget | 127.0.0.1:5434 | gias2_development | postgres / postgres |

These plain-text credentials are for the local test containers. The target's initialisation runs the core schema, first-slice reference seed and batch loader, in that order. Only type 4 and phases 5/6 are seeded.

Initialisation runs only on an empty PostgreSQL data directory. The core SQL drops and recreates the establishment schema. Restarting Docker does not clear the database; keep the existing data volume intact during the demo.

## Source data

The source importer is maintained separately in the `CsvExporter` checkout. With that repository and `education-provider-registry-docs` beside this one:

```powershell
Push-Location ..\CsvExporter
try {
    dotnet run --project .\SqlServerCsvImporter\SqlServerCsvImporter.csproj -- `
        --csv ".\CsvExporter\data\edubasealldata.csv" `
        --verify-transform "..\education-provider-registry-docs\models\schema\establishment\transforms\establishment-from-bau.sql"
} finally {
    Pop-Location
}
```

Run the import after SQL Server is ready to accept connections. The command uses Tony's CSV at the explicit path shown; change --csv if it is elsewhere. It loads 52,400 rows into `dbo.staging_table`, validates them, and populates `dbo.Establishment` and six lookup tables. `dbo.EstablishmentAdditionalAddresses` is empty. Import and source-table population are transactional.

ADF reads the physical source tables, not the raw CSV staging table. Editing raw staging alone will not change what ADF reads. Rerunning the importer replaces the prepared source data, so do not do that during the name-change demo.

This is a reconstructed source, not a BAU database export. See [source assumptions](PoC.md#source-assumptions) before extending the mappings.

## ADF connections

Register a self-hosted integration runtime on the Windows host as `LocalSetupIntegrationRuntime`. Both linked services use it. Start the existing `DIAHostService` service if necessary, then confirm the runtime is Online in ADF.

**SqlServer1** uses the SQL Server connector, SQL authentication and the source settings above. For this local container, encryption is mandatory and Trust server certificate is checked. The dataset [SqlServerTable1](adf/dataset/SqlServerTable1.json) points to `dbo.Establishment`.

**GiasPostgresOdbc** uses ODBC, Basic authentication and the target login above. Find the actual installed driver name:

```powershell
Get-OdbcDriver | Where-Object Name -like '*PostgreSQL*'
```

For example, if the installed name is `PostgreSQL ODBC Driver(UNICODE)`:

```text
Driver={PostgreSQL ODBC Driver(UNICODE)};Server=127.0.0.1;Port=5434;Database=gias2_development;
```

The dataset [batchloadstagingarea](adf/dataset/batchloadstagingarea.json) points to `"etl"."establishment_batch_stage"`. Configure credentials in the linked service, not in exported JSON.

Use the same endpoints in DBeaver with its SQL Server and PostgreSQL drivers. For SQL Server's local certificate, set `encrypt=true` and `trustServerCertificate=true`.

## Pipeline

Use [runcopyprotectagainstdup.json](adf/pipeline/runcopyprotectagainstdup.json) and both dataset definitions. Apply them through ADF's Code editor or your deployment tooling, then use ADF Validate and Publish all. When ready to run manually, click Debug with expectedRows = 4. Local file checks do not prove a successful service run.

The source query applies Martin's mapping rules before copying. Source codes are not target reference IDs: academy codes 28/34 map to target type 4, and source Secondary phase code 4 maps to target phase 5. Omit `establishment_id` from the mapping so PostgreSQL generates it.

See [batch loading](adf-tests/Batch-load.md) for the three activities and [the demo](demo/README.md) for manual runs.

## Connector choice

The local ODBC linked service supports Lookup but not Script activity. Lookup calls the PostgreSQL batch function and returns one summary row. For an Azure Database for PostgreSQL target, the version 2 connector supports Script activity; we would validate that replacement against the deployed target. The function can retain the transactional loading logic.

- [SQL Server connector](https://learn.microsoft.com/en-us/azure/data-factory/connector-sql-server)
- [ODBC connector](https://learn.microsoft.com/en-us/azure/data-factory/connector-odbc)
- [Azure Database for PostgreSQL connector](https://learn.microsoft.com/en-us/azure/data-factory/connector-azure-database-for-postgresql)
