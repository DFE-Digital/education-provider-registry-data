-- SQL Server / gias_bau_test_local.
SELECT e.URN, e.EstablishmentName,
       e.type_code, t.name AS establishment_type,
       e.educationPhase_code, p.name AS education_phase
FROM dbo.Establishment AS e
LEFT JOIN dbo.EstablishmentType AS t ON t.code = e.type_code
LEFT JOIN dbo.EducationPhase AS p ON p.code = e.educationPhase_code
WHERE e.URN IN (134314, 135905, 136102, 137603)
ORDER BY e.URN;
