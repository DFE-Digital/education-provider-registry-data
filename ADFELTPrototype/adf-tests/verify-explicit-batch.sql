-- Integration tests using the existing four fixture establishments.
-- All intake, ledger and target test changes are rolled back.
BEGIN;
CREATE TEMP TABLE batch_before ON COMMIT DROP AS
SELECT * FROM establishment.establishment WHERE urn IN (134314,135905,136102,137603);
DO $$ BEGIN IF (SELECT count(*) FROM batch_before) <> 4 THEN
    RAISE EXCEPTION 'Four existing fixture rows required'; END IF; END $$;
DO $$
DECLARE
    v_batch text := 'test-' || gen_random_uuid()::text;
    v_other text := 'test-' || gen_random_uuid()::text;
    v_result record;
    v_original record;
    v_new_urn integer;
    v_new_id uuid;
BEGIN
    IF EXISTS (SELECT 1 FROM pg_trigger WHERE tgrelid='etl.establishment_batch_stage'::regclass
               AND NOT tgisinternal AND tgenabled <> 'D') THEN
        RAISE EXCEPTION 'Explicit batch intake must have no automatic load trigger';
    END IF;

    INSERT INTO etl.establishment_batch_stage
        (batch_id,urn,ukprn,establishment_number,name,establishment_type_id,education_phase_id)
    SELECT v_batch,urn,ukprn,establishment_number,name || ' [batch test]',establishment_type_id,education_phase_id
    FROM batch_before;
    IF EXISTS (SELECT 1 FROM batch_before b JOIN establishment.establishment e USING(urn)
               WHERE b.name IS DISTINCT FROM e.name) THEN
        RAISE EXCEPTION 'Copy to batch intake unexpectedly updated the target';
    END IF;
    -- Wrong expected count must not apply anything.
    BEGIN
        PERFORM * FROM etl.apply_establishment_batch(v_batch,5);
        RAISE EXCEPTION 'Count mismatch was accepted' USING ERRCODE='P0002';
    EXCEPTION WHEN SQLSTATE 'P0001' THEN NULL;
    END;
    SELECT * INTO v_result FROM etl.apply_establishment_batch(v_batch,4);
    IF v_result.status <> 'applied' OR v_result.updated_rows <> 4 OR v_result.inserted_rows <> 0 THEN
        RAISE EXCEPTION 'Expected four explicit target updates';
    END IF;
    IF EXISTS (SELECT 1 FROM batch_before b JOIN establishment.establishment e USING(urn)
               WHERE e.establishment_id<>b.establishment_id OR e.name<>b.name || ' [batch test]') THEN
        RAISE EXCEPTION 'Explicit update did not preserve UUIDs';
    END IF;
    SELECT * INTO v_result FROM etl.apply_establishment_batch(v_batch,4);
    IF v_result.status <> 'already_applied' THEN RAISE EXCEPTION 'Retry was not idempotent'; END IF;

    -- Applied payload changes must fail instead of silently reusing old counts.
    UPDATE etl.establishment_batch_stage SET name='changed after apply'
    WHERE batch_id=v_batch AND urn=136102;
    BEGIN
        PERFORM * FROM etl.apply_establishment_batch(v_batch,4);
        RAISE EXCEPTION 'Changed applied payload was accepted' USING ERRCODE='P0002';
    EXCEPTION WHEN SQLSTATE 'P0001' THEN NULL;
    END;

    INSERT INTO etl.establishment_batch_stage
        (batch_id,urn,ukprn,establishment_number,name,establishment_type_id,education_phase_id)
    SELECT v_other,e.urn,e.ukprn,e.establishment_number,e.name,e.establishment_type_id,e.education_phase_id
    FROM establishment.establishment e WHERE e.urn IN (134314,135905,136102,137603);
    SELECT * INTO v_result FROM etl.apply_establishment_batch(v_other,4);
    IF v_result.unchanged_rows<>4 OR v_result.updated_rows<>0 THEN
        RAISE EXCEPTION 'New run of unchanged data was not a no-op';
    END IF;

    SELECT candidate INTO v_new_urn FROM generate_series(999990,999999) candidate
    WHERE NOT EXISTS (SELECT 1 FROM establishment.establishment e WHERE e.urn=candidate)
    ORDER BY candidate LIMIT 1;
    IF v_new_urn IS NULL THEN RAISE EXCEPTION 'No unused test URN available'; END IF;
    v_other := 'test-' || gen_random_uuid()::text;
    INSERT INTO etl.establishment_batch_stage (batch_id,urn,name,establishment_type_id,education_phase_id)
    VALUES (v_other,v_new_urn,'New batch fixture',4,5);
    SELECT * INTO v_result FROM etl.apply_establishment_batch(v_other,1);
    SELECT establishment_id INTO STRICT v_new_id FROM establishment.establishment WHERE urn=v_new_urn;
    IF v_result.inserted_rows<>1 THEN RAISE EXCEPTION 'New URN was not inserted'; END IF;
    SELECT * INTO v_result FROM etl.apply_establishment_batch(v_other,1);
    IF (SELECT establishment_id FROM establishment.establishment WHERE urn=v_new_urn)<>v_new_id THEN
        RAISE EXCEPTION 'Insert replay changed its UUID';
    END IF;

    -- Constraint failure during the target INSERT must roll back every row and ledger entry.
    v_other := 'test-' || gen_random_uuid()::text;
    SELECT * INTO v_original FROM establishment.establishment WHERE urn=136102;
    INSERT INTO etl.establishment_batch_stage
        (batch_id,urn,name,establishment_type_id,education_phase_id,ukprn,establishment_number)
    VALUES (v_other,136102,'Must roll back',4,5,v_original.ukprn,v_original.establishment_number),
           (v_other,9999999,'Invalid target range',4,5,NULL,NULL);
    BEGIN
        PERFORM * FROM etl.apply_establishment_batch(v_other,2);
        RAISE EXCEPTION 'Invalid target range was accepted' USING ERRCODE='P0002';
    EXCEPTION WHEN check_violation THEN NULL;
    END;
    IF EXISTS (SELECT 1 FROM etl.establishment_batch_load l WHERE l.batch_id=v_other)
       OR (SELECT name FROM establishment.establishment WHERE urn=136102)<>v_original.name THEN
        RAISE EXCEPTION 'Failed load left partial target/ledger changes';
    END IF;

    -- Duplicate input URNs are rejected; failed batches are not mixed with other runs.
    v_other := 'test-' || gen_random_uuid()::text;
    INSERT INTO etl.establishment_batch_stage (batch_id,urn,name,establishment_type_id,education_phase_id)
    VALUES (v_other,136102,'duplicate one',4,5),(v_other,136102,'duplicate two',4,5);
    BEGIN
        PERFORM * FROM etl.apply_establishment_batch(v_other,2);
        RAISE EXCEPTION 'Duplicate URNs were accepted' USING ERRCODE='P0002';
    EXCEPTION WHEN SQLSTATE 'P0001' THEN NULL;
    END;
    RAISE NOTICE 'Explicit batch tests passed: no automatic load, update/insert/no-op, UUID retention, retries, count/payload guards, isolation and atomic failure';
END $$;
ROLLBACK;
