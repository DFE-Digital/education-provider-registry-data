# Batch loading

The pipeline [runcopyprotectagainstdup](../adf/pipeline/runcopyprotectagainstdup.json) has three activities:

```text
Stage establishments → Apply Establishments Batch → Validate applied batch
       Copy                    Lookup                   If Condition
```

Each activity depends on the previous one succeeding. Pipeline concurrency is 1.

## Stage establishments

SQL Server executes the [mapped source query](02-martin-establishment-source.sql). ADF copies its six output fields into `etl.establishment_batch_stage`, adding `batch_id = @pipeline().RunId` as the seventh field. Do not map the staging identity or timestamp columns.

The intake table has no automatic loading trigger. Copy retries are disabled because a partial Copy can leave records in staging; another Copy into the same batch could introduce duplicate URNs. After a failed Copy, fix the cause and start a new pipeline run.

## Apply Establishments Batch

Lookup uses the same ODBC dataset, Query mode and First row only. Its query is:

```text
@concat('SELECT * FROM etl.apply_establishment_batch(''', pipeline().RunId, ''', ', string(pipeline().parameters.expectedRows), ');')
```

`expectedRows` is an Int parameter with default 4. The function checks the row count, duplicate URNs, required mapped values and target reference IDs before writing. It then matches by URN, inserts new establishments, updates changed fields and skips unchanged records. Existing UUIDs are preserved.

Target writes and the successful-batch ledger entry commit together. A failure rolls them back; copied intake rows remain available for diagnosis. Lookup retries are set to 2. Repeating an applied batch returns `already_applied` with its saved counts; changing its staged payload is rejected.

**This SELECT writes target data. Do not use Lookup preview as a read-only inspection.**

## Validate applied batch

The If Condition checks:

- Returned batch ID equals the pipeline run ID.
- Status is `applied` or `already_applied`.
- Staged count equals `expectedRows` and Copy's `rowsCopied`.
- Inserted + updated + unchanged equals the staged count.

Its false branch runs `Fail unexpected batch result`. Validation happens after the database commit; it cannot undo that commit and does not compare individual field values or UUIDs. Use [03-verify-gias.sql](03-verify-gias.sql) to inspect those.

| Run | Inserted | Updated | Unchanged |
| --- | ---: | ---: | ---: |
| Four records absent from target | 4 | 0 | 0 |
| Same data, new run | 0 | 0 | 4 |
| One source name changed | 0 | 1 | 3 |

Inspect the batch ledger and staging counts with [verify-batch-result.sql](verify-batch-result.sql).

## Limits

Every run extracts the full selected snapshot. Incremental extraction, source deletions, snapshot ordering and staging retention are not implemented. The function locks staging and target writes during application; its concurrency and performance need assessing before larger loads. Do not run other writers against this target during the demo.

This slice loads one target table. Wider loading needs reference data first, establishment UUIDs resolved by URN, and dependent tables loaded in order. Addresses precede sites; sites precede establishment-to-site links.
