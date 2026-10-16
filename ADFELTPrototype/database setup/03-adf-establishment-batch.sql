-- Batch loader for PostgreSQL, called by the ADF ODBC Lookup activity.
-- Initial setup: run after the core target schema and first-slice reference seed.
-- Additive and rerunnable. This intake table has no automatic load trigger.
BEGIN;
DO $$
BEGIN
    IF to_regclass('establishment.establishment') IS NULL THEN
        RAISE EXCEPTION 'The core establishment target schema is required';
    END IF;
END $$;
CREATE SCHEMA IF NOT EXISTS etl;
CREATE TABLE IF NOT EXISTS etl.establishment_batch_stage (
    staging_id bigint GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    received_at timestamptz NOT NULL DEFAULT clock_timestamp(),
    batch_id text NOT NULL CHECK (length(batch_id) BETWEEN 1 AND 128),
    urn integer NOT NULL,
    ukprn numeric,
    establishment_number integer,
    name text,
    establishment_type_id integer,
    education_phase_id integer
);
CREATE INDEX IF NOT EXISTS ix_establishment_batch_stage_batch
    ON etl.establishment_batch_stage (batch_id);
CREATE TABLE IF NOT EXISTS etl.establishment_batch_load (
    batch_id text PRIMARY KEY,
    payload_digest text NOT NULL,
    staged_rows integer NOT NULL,
    inserted_rows integer NOT NULL,
    updated_rows integer NOT NULL,
    unchanged_rows integer NOT NULL,
    applied_at timestamptz NOT NULL DEFAULT clock_timestamp(),
    CHECK (staged_rows = inserted_rows + updated_rows + unchanged_rows)
);

CREATE OR REPLACE FUNCTION etl.apply_establishment_batch(
    p_batch_id text, p_expected_rows integer
)
RETURNS TABLE (
    batch_id text, status text, staged_rows integer,
    inserted_rows integer, updated_rows integer, unchanged_rows integer
)
LANGUAGE plpgsql
SET search_path = pg_catalog
AS $$
DECLARE
    v_count integer;
    v_digest text;
    v_inserted integer;
    v_updated integer;
    v_unchanged integer;
    v_bad_urn integer;
    v_saved etl.establishment_batch_load%ROWTYPE;
BEGIN
    IF p_batch_id IS NULL OR btrim(p_batch_id) = '' OR length(p_batch_id) > 128 THEN
        RAISE EXCEPTION 'A nonblank pipeline batch ID is required';
    END IF;
    IF p_expected_rows IS NULL OR p_expected_rows < 0 THEN
        RAISE EXCEPTION 'Expected rows must be a nonnegative Copy rowsCopied count';
    END IF;

    -- Retry calls for one batch serialize; stage data remains fixed during apply.
    PERFORM pg_advisory_xact_lock(hashtextextended(p_batch_id, 0));
    LOCK TABLE etl.establishment_batch_stage IN SHARE MODE;
    SELECT count(*)::integer INTO v_count
    FROM etl.establishment_batch_stage s WHERE s.batch_id = p_batch_id;
    IF v_count <> p_expected_rows THEN
        RAISE EXCEPTION 'Batch % has % staged rows, expected %', p_batch_id, v_count, p_expected_rows;
    END IF;
    IF EXISTS (SELECT 1 FROM etl.establishment_batch_stage s WHERE s.batch_id = p_batch_id
               GROUP BY s.urn HAVING count(*) > 1) THEN
        RAISE EXCEPTION 'Batch % contains duplicate URNs; recopy using a new pipeline run', p_batch_id;
    END IF;

    SELECT md5(coalesce(string_agg(jsonb_build_array(
               s.urn, s.ukprn, s.establishment_number, s.name,
               s.establishment_type_id, s.education_phase_id)::text,
               E'\n' ORDER BY s.urn), '')) INTO v_digest
    FROM etl.establishment_batch_stage s WHERE s.batch_id = p_batch_id;
    SELECT l.* INTO v_saved FROM etl.establishment_batch_load l WHERE l.batch_id = p_batch_id;
    IF FOUND THEN
        IF v_saved.staged_rows <> v_count OR v_saved.payload_digest <> v_digest THEN
            RAISE EXCEPTION 'Previously applied batch % has changed; use a new pipeline run ID', p_batch_id;
        END IF;
        RETURN QUERY SELECT p_batch_id, 'already_applied'::text,
            v_saved.staged_rows, v_saved.inserted_rows, v_saved.updated_rows, v_saved.unchanged_rows;
        RETURN;
    END IF;

    SELECT s.urn INTO v_bad_urn FROM etl.establishment_batch_stage s
    WHERE s.batch_id=p_batch_id AND (s.name IS NULL OR btrim(s.name)=''
          OR s.establishment_type_id IS NULL OR s.education_phase_id IS NULL)
    ORDER BY s.urn LIMIT 1;
    IF FOUND THEN
        RAISE EXCEPTION 'Batch % has missing name or unmapped type/phase for URN %', p_batch_id, v_bad_urn;
    END IF;
    SELECT s.urn INTO v_bad_urn FROM etl.establishment_batch_stage s
    LEFT JOIN establishment.establishment_type t ON t.establishment_type_id=s.establishment_type_id
    LEFT JOIN establishment.education_phase p ON p.education_phase_id=s.education_phase_id
    WHERE s.batch_id=p_batch_id AND (t.establishment_type_id IS NULL OR p.education_phase_id IS NULL)
    ORDER BY s.urn LIMIT 1;
    IF FOUND THEN
        RAISE EXCEPTION 'Batch % has an absent target type/phase reference for URN %', p_batch_id, v_bad_urn
            USING ERRCODE='23503';
    END IF;

    -- Serialize target writes during classification and this small PoC batch load.
    LOCK TABLE establishment.establishment IN SHARE ROW EXCLUSIVE MODE;
    SELECT count(*) FILTER (WHERE e.establishment_id IS NULL)::integer,
           count(*) FILTER (WHERE e.establishment_id IS NOT NULL AND
             (e.ukprn,e.establishment_number,e.name,e.establishment_type_id,e.education_phase_id)
             IS DISTINCT FROM
             (s.ukprn,s.establishment_number,s.name,s.establishment_type_id,s.education_phase_id))::integer,
           count(*) FILTER (WHERE e.establishment_id IS NOT NULL AND
             (e.ukprn,e.establishment_number,e.name,e.establishment_type_id,e.education_phase_id)
             IS NOT DISTINCT FROM
             (s.ukprn,s.establishment_number,s.name,s.establishment_type_id,s.education_phase_id))::integer
    INTO v_inserted, v_updated, v_unchanged
    FROM etl.establishment_batch_stage s
    LEFT JOIN establishment.establishment e ON e.urn=s.urn
    WHERE s.batch_id=p_batch_id;

    INSERT INTO establishment.establishment AS target (
        urn,ukprn,establishment_number,name,establishment_type_id,education_phase_id
    )
    SELECT s.urn,s.ukprn,s.establishment_number,s.name,s.establishment_type_id,s.education_phase_id
    FROM etl.establishment_batch_stage s WHERE s.batch_id=p_batch_id ORDER BY s.urn
    ON CONFLICT (urn) DO UPDATE SET
        ukprn=EXCLUDED.ukprn,
        establishment_number=EXCLUDED.establishment_number,
        name=EXCLUDED.name,
        establishment_type_id=EXCLUDED.establishment_type_id,
        education_phase_id=EXCLUDED.education_phase_id
    WHERE (target.ukprn,target.establishment_number,target.name,
           target.establishment_type_id,target.education_phase_id)
          IS DISTINCT FROM
          (EXCLUDED.ukprn,EXCLUDED.establishment_number,EXCLUDED.name,
           EXCLUDED.establishment_type_id,EXCLUDED.education_phase_id);

    INSERT INTO etl.establishment_batch_load (
        batch_id,payload_digest,staged_rows,inserted_rows,updated_rows,unchanged_rows
    ) VALUES (p_batch_id,v_digest,v_count,v_inserted,v_updated,v_unchanged);
    RETURN QUERY SELECT p_batch_id,'applied'::text,v_count,v_inserted,v_updated,v_unchanged;
END;
$$;
COMMENT ON TABLE etl.establishment_batch_stage IS
    'ADF batch intake with no load trigger. batch_id is pipeline RunId. Explicit apply function loads completed batches.';
COMMENT ON TABLE etl.establishment_batch_load IS
    'Successfully applied batch payloads and counts; supports safe replay of the explicit Lookup load call.';
COMMIT;
