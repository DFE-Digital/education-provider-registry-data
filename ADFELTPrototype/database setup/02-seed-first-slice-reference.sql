-- Minimal reference subset from Martin's definitions for the four-URN PoC.
-- Run after the core target schema. Does not seed the full migration scope.
BEGIN;
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM establishment.establishment_type
               WHERE establishment_type_id = 4 AND name <> 'Mainstream academy')
       OR EXISTS (SELECT 1 FROM establishment.education_phase
                  WHERE (education_phase_id = 5 AND name <> 'Secondary')
                     OR (education_phase_id = 6 AND name <> 'All-through')) THEN
        RAISE EXCEPTION 'Existing reference IDs conflict with Martin''s first-slice definitions';
    END IF;
END $$;
INSERT INTO establishment.establishment_type (establishment_type_id, name)
VALUES (4, 'Mainstream academy') ON CONFLICT (establishment_type_id) DO NOTHING;
INSERT INTO establishment.education_phase (education_phase_id, name)
VALUES (5, 'Secondary'), (6, 'All-through') ON CONFLICT (education_phase_id) DO NOTHING;
COMMIT;
