-- Completed ADF-shaped run IDs, excluding labelled local-cleanup batches.
SELECT batch_id,staged_rows,inserted_rows,updated_rows,unchanged_rows,applied_at
FROM etl.establishment_batch_load
WHERE batch_id ~ '^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$'
ORDER BY applied_at DESC LIMIT 6;
