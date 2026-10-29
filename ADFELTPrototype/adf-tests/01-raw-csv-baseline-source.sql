-- SQL Server raw-CSV baseline, source database gias_bau_test_local.
-- Intentionally copies source codes as target IDs to demonstrate the mismatch.
-- This is a diagnostic baseline, NOT the intended production mapping.
-- Source type codes 28/34 do not match the target's seeded type ID 4.
SELECT
    CONVERT(int, urn) AS urn,
    CONVERT(bigint, ukprn) AS ukprn,
    CONVERT(int, establishmentnumber) AS establishment_number,
    establishmentname AS name,
    CONVERT(int, typeofestablishment_code) AS establishment_type_id,
    CONVERT(int, phaseofeducation_code) AS education_phase_id
FROM dbo.staging_table
WHERE urn IN ('134314', '135905', '136102', '137603')
ORDER BY CONVERT(int, urn);
