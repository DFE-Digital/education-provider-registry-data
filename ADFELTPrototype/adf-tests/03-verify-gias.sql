-- PostgreSQL verification after the ADF test: gias2_development.
SELECT e.urn, e.name, e.establishment_id,
       e.establishment_type_id, t.name AS establishment_type,
       e.education_phase_id, p.name AS education_phase
FROM establishment.establishment e
JOIN establishment.establishment_type t USING (establishment_type_id)
LEFT JOIN establishment.education_phase p USING (education_phase_id)
WHERE e.urn IN (134314, 135905, 136102, 137603)
ORDER BY e.urn;
-- Expected from Martin query:
-- 134314: type 4 / phase 6
-- 135905, 136102, 137603: type 4 / phase 5
-- All four should have a generated establishment_id UUID.
