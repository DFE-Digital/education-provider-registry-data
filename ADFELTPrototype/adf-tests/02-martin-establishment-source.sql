-- Four-establishment source query, generated from establishment-from-bau.sql.
-- Upstream SHA-256: b7668399cb6b76755990a6a5da74871c51729f57242489272b13df8efab3c057
WITH source_establishment AS (
    SELECT e.URN, e.UKPRN, e.EstablishmentNumber, e.EstablishmentName,
           et.name AS type_name, ep.name AS education_phase_name
    FROM dbo.Establishment AS e
    LEFT JOIN dbo.EstablishmentType AS et ON et.code = e.type_code
    LEFT JOIN dbo.EducationPhase AS ep ON ep.code = e.educationPhase_code
    WHERE e.URN IN (134314, 135905, 136102, 137603)
)
SELECT
    s.URN AS urn,
    s.UKPRN AS ukprn,
    s.EstablishmentNumber AS establishment_number,
    s.EstablishmentName AS name,
    CASE
        WHEN s.type_name = 'Community school' THEN 1
        WHEN s.type_name = 'Foundation special school' THEN 3
        WHEN s.type_name IN ('Academy sponsor led', 'Academy converter') THEN 4
        WHEN s.type_name IN ('Academy special sponsor led', 'Academy special converter') THEN 5
        WHEN s.type_name IN ('Academy alternative provision sponsor led', 'Academy alternative provision converter') THEN 6
        WHEN s.type_name IN ('Academy 16-19 sponsor led', 'Academy 16 to 19 converter') THEN 7
        WHEN s.type_name = 'Academy secure 16 to 19' THEN 8
        WHEN s.type_name = 'Voluntary controlled school' THEN 10
        WHEN s.type_name = 'Foundation school' THEN 11
        WHEN s.type_name = 'Other independent special school' THEN 15
        WHEN s.type_name = 'Other independent school' THEN 17
        WHEN s.type_name = 'University technical college' THEN 21
        WHEN s.type_name = 'Children''s centre' THEN 35
        ELSE NULL
    END AS establishment_type_id,
    CASE s.education_phase_name
        WHEN 'Nursery' THEN 1
        WHEN 'Primary' THEN 2
        WHEN 'Middle deemed primary' THEN 3
        WHEN 'Middle deemed secondary' THEN 4
        WHEN 'Secondary' THEN 5
        WHEN 'All-through' THEN 6
        WHEN '16 to 19' THEN 7
        WHEN 'Not applicable' THEN 8
        WHEN 'Not applicable (education phase)' THEN 8
        ELSE NULL
    END AS education_phase_id
FROM source_establishment AS s
ORDER BY s.URN;
