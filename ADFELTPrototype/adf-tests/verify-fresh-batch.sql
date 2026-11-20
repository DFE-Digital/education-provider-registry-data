-- Isolated fresh-target check. All fixture data changes are rolled back.
BEGIN;
DO $$ BEGIN
    IF EXISTS (SELECT 1 FROM establishment.establishment) THEN
        RAISE EXCEPTION 'Run this check only against an empty, isolated target';
    END IF;
    IF to_regclass('etl.establishment_stage') IS NOT NULL THEN
        RAISE EXCEPTION 'Archived trigger intake should not be installed by fresh setup';
    END IF;
END $$;
INSERT INTO etl.establishment_batch_stage (batch_id,urn,name,establishment_type_id,education_phase_id)
VALUES ('fresh-first',134314,'Fresh fixture 1',4,6),('fresh-first',135905,'Fresh fixture 2',4,5),
       ('fresh-first',136102,'Fresh fixture 3',4,5),('fresh-first',137603,'Fresh fixture 4',4,5);
DO $$ DECLARE result record; BEGIN
    SELECT * INTO result FROM etl.apply_establishment_batch('fresh-first',4);
    IF result.inserted_rows<>4 THEN RAISE EXCEPTION 'Fresh target should insert four'; END IF;
    SELECT * INTO result FROM etl.apply_establishment_batch('fresh-first',4);
    IF result.status<>'already_applied' THEN RAISE EXCEPTION 'Fresh retry should be idempotent'; END IF;
END $$;
CREATE TEMP TABLE fresh_ids ON COMMIT DROP AS SELECT urn,establishment_id FROM establishment.establishment;
INSERT INTO etl.establishment_batch_stage
    (batch_id,urn,ukprn,establishment_number,name,establishment_type_id,education_phase_id)
SELECT 'fresh-update',urn,ukprn,establishment_number,
       CASE WHEN urn=136102 THEN name || ' updated' ELSE name END,
       establishment_type_id,education_phase_id FROM establishment.establishment;
DO $$ DECLARE result record; BEGIN
    SELECT * INTO result FROM etl.apply_establishment_batch('fresh-update',4);
    IF result.updated_rows<>1 OR result.unchanged_rows<>3 THEN
        RAISE EXCEPTION 'Fresh update counts incorrect';
    END IF;
    IF EXISTS (SELECT 1 FROM fresh_ids i JOIN establishment.establishment e USING(urn)
               WHERE i.establishment_id<>e.establishment_id) THEN
        RAISE EXCEPTION 'Fresh update changed UUIDs';
    END IF;
    RAISE NOTICE 'Fresh setup passed: no archived trigger, initial insert, safe retry, one update and UUID retention';
END $$;
ROLLBACK;
