-- PostgreSQL / gias2_development: show mapped meanings and stable identity.
SELECT e.urn,e.name,e.establishment_id,
       e.establishment_type_id,t.name AS target_type,
       e.education_phase_id,p.name AS target_phase
FROM establishment.establishment e
JOIN establishment.establishment_type t USING(establishment_type_id)
LEFT JOIN establishment.education_phase p USING(education_phase_id)
WHERE e.urn IN (134314,135905,136102,137603) ORDER BY e.urn;
