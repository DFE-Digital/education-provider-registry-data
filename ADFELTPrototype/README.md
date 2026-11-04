# ADF Edubase-to-GIAS spike

This spike loads four establishments from a local SQL Server source into the proposed GIAS PostgreSQL schema. It uses Martin's establishment type and education-phase mapping rules.

```text
SQL Server query → ADF Copy → PostgreSQL staging
                              ↓
                     Lookup calls batch function
                              ↓
                        ADF validation
```

The source tables are reconstructed from Tony's CSV. They match the interface queried by Martin's transform, but are not a verified copy of the production Edubase schema.

## Start an existing setup

With Docker Desktop running, use these commands from the project root:

```powershell
docker compose -f ".\database setup\docker-compose.yml" up -d
Get-Service DIAHostService
```

If the service is stopped, start it from Administrator PowerShell with `Start-Service DIAHostService`. Confirm the runtime is Online in ADF. Open `runcopyprotectagainstdup`, keep `expectedRows = 4`, and click Debug. Follow the [demo steps](demo/README.md) to show an unchanged rerun and a source update.

For a new environment, follow [first-time setup](AzureDataFactorySetup.md). The source importer and CSV are separate dependencies; an ordinary demo does not need another import.

## Getting started

- [Setup](AzureDataFactorySetup.md): databases, source importer and ADF connections.
- [Batch loading](adf-tests/Batch-load.md): activities, validation and failure behaviour.
- [Demo](demo/README.md): initial load, rerun, update and restoration.
- [Source assumptions](PoC.md#source-assumptions): CSV differences and mapping gaps.
- [Results](PoC.md): observed evidence and remaining work.

The pipeline is [runcopyprotectagainstdup](adf/pipeline/runcopyprotectagainstdup.json). Its datasets are under `adf/dataset`; target setup is under `database setup`. The existing ADF names are retained so these files match the published prototype.

## Scope

The query selects URNs **134314, 135905, 136102 and 137603** and returns six fields: URN, UKPRN, establishment number, name, establishment type ID and education phase ID. PostgreSQL generates the establishment UUID on insert and preserves it on update.

Each run copies the full selection into staging. The batch function inserts new records, updates changed values and skips unchanged records. It does not extract only source changes or delete missing establishments.

Before a wider load we need to confirm the real Edubase schema, complete mapping/reference coverage, decide how to extract changes, and handle closures, deletions and staging retention. Lifecycle, addresses, sites and other related target tables are outside this slice.

## Development

Regenerate the mapped query and its embedded copy in the pipeline JSON:

```powershell
.\scripts\Build-AdfArtifacts.ps1
.\scripts\Test-SpikeProject.ps1
```

The generator reads the sibling `education-provider-registry-docs` checkout, or accepts `-TransformPath`. It does not publish to ADF. Linked-service credentials remain in ADF and are not exported.

Optional database checks, run manually outside the demo:

```powershell
.\scripts\Test-SpikeProject.ps1 -DatabaseChecks
.\scripts\Test-FreshDatabase.ps1
```

The first rolls back its test changes and needs the four target records to exist. The second creates and removes an isolated test database. Neither starts ADF.

Local dumps, captured UUIDs, CSV extracts and old trial files are excluded from Git. The source importer is a separate dependency in `CsvExporter/SqlServerCsvImporter`; this repository does not include it or the CSV. See [NOTICE.md](NOTICE.md) for the upstream SQL attribution.
