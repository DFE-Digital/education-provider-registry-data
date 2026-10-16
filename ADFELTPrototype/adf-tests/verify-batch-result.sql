-- Target inspection after an ADF trial, on gias2_development.
SELECT batch_id,staged_rows,inserted_rows,updated_rows,unchanged_rows,applied_at
FROM etl.establishment_batch_load ORDER BY applied_at DESC;
SELECT batch_id,count(*) AS intake_rows FROM etl.establishment_batch_stage
GROUP BY batch_id ORDER BY batch_id;
SELECT e.urn,e.name,e.establishment_id,e.establishment_type_id,e.education_phase_id
FROM establishment.establishment e WHERE e.urn IN (134314,135905,136102,137603) ORDER BY e.urn;
