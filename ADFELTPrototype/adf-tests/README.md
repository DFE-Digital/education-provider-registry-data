# Queries and checks

| File | Purpose |
| --- | --- |
| `01-raw-csv-baseline-source.sql` | Diagnostic query demonstrating why source codes cannot be copied as target IDs |
| `02-martin-establishment-source.sql` | Generated six-field mapped query used by the pipeline |
| `03-verify-gias.sql` | Read the four target rows, mapped meanings and UUIDs |
| `verify-batch-result.sql` | Read batch history, intake counts and current target values |
| `verify-explicit-batch.sql` | Transactional loader tests; rolls back and requires four target fixtures |
| `verify-fresh-batch.sql` | Loader tests for an isolated fresh database |

Use [Test-SpikeProject.ps1](../scripts/Test-SpikeProject.ps1) for file checks. Add `-DatabaseChecks` only when the four target records exist. [Test-FreshDatabase.ps1](../scripts/Test-FreshDatabase.ps1) creates and removes its own test database.

The diagnostic raw-code query is not part of the active load. Use the [batch pipeline](Batch-load.md) for reruns. No check script starts an ADF pipeline.
