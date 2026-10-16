# Run the demo

Use `runcopyprotectagainstdup` with `expectedRows = 4`. Allow about ten minutes.

## Start the existing environment

From the project root, with Docker Desktop running:

```powershell
docker compose -f ".\database setup\docker-compose.yml" up -d
```

If the integration runtime service is stopped, start it in Administrator PowerShell:

```powershell
Start-Service DIAHostService
```

Confirm `LocalSetupIntegrationRuntime` is **Online** in ADF and test both linked-service connections. Open the pipeline and your SQL Server/PostgreSQL connections in DBeaver. Don't rerun the CSV importer for an ordinary demo: it rebuilds the prepared source tables.

For a new machine or empty source database, complete [setup](../AzureDataFactorySetup.md) first.

## 1. Show the source and target

Run [01-source.sql](01-source.sql) against SQL Server, then [02-target.sql](02-target.sql) against PostgreSQL. Both select URNs 134314, 135905, 136102 and 137603.

Explain that Martin's mapping rules translate source type/phase values into GIAS IDs. The source is reconstructed from Tony's CSV; it is not a verified production BAU copy.

## 2. Load and rerun

Click **Debug** in ADF with `expectedRows = 4`. Wait for all three activities to succeed. Open **Apply Establishments Batch → Output** and refresh the PostgreSQL query.

| Target before the run | Inserted | Updated | Unchanged |
| --- | ---: | ---: | ---: |
| None of the four schools exists | 4 | 0 | 0 |
| All four already match the source | 0 | 0 | 4 |

Don't clear the database just to get an insert result. An existing target is fine for the rerun demonstration. If an initial-load reset has been deliberately prepared, note the new UUIDs after loading.

Click **Debug** again. Expect **0 inserted, 0 updated, 4 unchanged**, with the same UUIDs.

## 3. Change one school

From the project root:

```powershell
.\scripts\Set-DemoSource.ps1 -Mode Update
```

This changes only URN 136102's source name to `Co-op Academy Stoke-On-Trent [ADF demo]`. Refresh both query tabs: the source changes immediately; GIAS retains the old name until the load.

Run ADF again. Expect **0 inserted, 1 updated, 3 unchanged**. Refresh GIAS and point out the changed name and preserved UUID.

## 4. Restore the name

```powershell
.\scripts\Set-DemoSource.ps1 -Mode Restore
```

Run ADF once more. Expect **0 inserted, 1 updated, 3 unchanged**. Confirm both databases show the original name. Restoring the source alone does not restore GIAS.

[03-batch-evidence.sql](03-batch-evidence.sql) shows recent successful batches. After a fresh initial load, you can capture the new UUID baseline with `Test-DemoReadiness.ps1 -CaptureBaseline`; use `Test-DemoReadiness.ps1` for subsequent checks at the restored state.

## Explain what happened

- **Copy:** SQL Server executes the mapping query. ADF/SHIR copies the result into staging with the run ID.
- **Lookup:** calls `etl.apply_establishment_batch`. PostgreSQL validates the batch and applies inserts/updates in one transaction. There is no automatic loading trigger on this staging table.
- **Validation:** ADF checks the returned batch ID, status and counts after commit. It cannot roll back an applied batch.

We currently copy the full selected snapshot, then update only changed target rows. Incremental extraction and deletion handling remain follow-up work. See [results](../PoC.md) and [batch loading](../adf-tests/Batch-load.md).

## If a step fails

Check the failed activity's error before rerunning. A partial Copy can leave intake rows; fix the cause and start a new run. A failed apply rolls back its target writes. If the source still has the demo suffix, restore it and run ADF to restore the target.

Don't reset Docker volumes or rerun the core schema to fix a demo failure. Optional backup helpers are `Backup-DemoTarget.ps1` and `Test-DemoBackup.ps1`; they keep files under `.runtime/backups`, and backup testing restores into a separate temporary database.
