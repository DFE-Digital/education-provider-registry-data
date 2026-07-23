-- ============================================================
-- Sample data for schema v0.1
-- Based on Edubase extract
-- Intended for development only
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- Reference data
-- ------------------------------------------------------------

INSERT INTO ref.admissions_gender (code, name)
VALUES
('B', 'Boys'),
('G', 'Girls'),
('M', 'Mixed')
ON CONFLICT DO NOTHING;

INSERT INTO ref.religion (code, name)
VALUES
('COE', 'Church of England'),
('RC', 'Roman Catholic'),
('NONE', 'None'),
('OTHER', 'Other')
ON CONFLICT DO NOTHING;

INSERT INTO ref.religious_character (code, name)
VALUES
('NA', 'Does not apply'),
('COE', 'Church of England'),
('RC', 'Roman Catholic'),
('NONE', 'None')
ON CONFLICT DO NOTHING;

INSERT INTO ref.religious_ethos (code, name)
VALUES
('NA', 'Does not apply'),
('COE', 'Church of England'),
('RC', 'Roman Catholic'),
('CHR', 'Christian'),
('NONE', 'None')
ON CONFLICT DO NOTHING;

INSERT INTO ref.local_authority
(code, name, effective_from, is_active)
VALUES
('201', 'City of London', '1900-01-01', TRUE),
('202', 'Camden', '1900-01-01', TRUE),
('203', 'Greenwich', '1900-01-01', TRUE)
ON CONFLICT DO NOTHING;

INSERT INTO ref.diocese
(religion_id, code, name, effective_from)
SELECT religion_id, 'CE23', 'Diocese of London', '1900-01-01'
FROM ref.religion
WHERE code = 'COE'
ON CONFLICT DO NOTHING;

INSERT INTO ref.diocese
(religion_id, code, name, effective_from)
SELECT religion_id, 'RC01', 'Archdiocese of Westminster', '1900-01-01'
FROM ref.religion
WHERE code = 'RC'
ON CONFLICT DO NOTHING;

INSERT INTO ref.diocese
(religion_id, code, name, effective_from)
SELECT religion_id, 'CE36', 'Diocese of Southwark', '1900-01-01'
FROM ref.religion
WHERE code = 'COE'
ON CONFLICT DO NOTHING;

INSERT INTO ref.nursery_provision
(code, name)
VALUES
('HAS', 'Has Nursery Classes'),
('NONE', 'No Nursery Classes'),
('NA', 'Not applicable')
ON CONFLICT DO NOTHING;

INSERT INTO ref.boarders
(code, name)
VALUES
('NONE', 'No boarders'),
('BOARD', 'Boarding school')
ON CONFLICT DO NOTHING;

INSERT INTO ref.boarding_establishment_type
(code, name)
VALUES
('DAY', 'Day school'),
('BOARDING', 'Boarding school')
ON CONFLICT DO NOTHING;

INSERT INTO ref.special_classes
(code, name)
VALUES
('Y', 'Has Special Classes'),
('N', 'No Special Classes')
ON CONFLICT DO NOTHING;

INSERT INTO ref.section41_approval_status
(code, name, display_order)
VALUES
('APP', 'Approved', 1),
('NAP', 'Not approved', 2),
('NA', 'Not applicable', 3)
ON CONFLICT DO NOTHING;

INSERT INTO ref.type_of_resourced_provision
(code, name)
VALUES
('ASD', 'Autistic Spectrum Disorder'),
('SLCN', 'Speech Language and Communication Needs'),
('SEMH', 'Social Emotional and Mental Health'),
('HI', 'Hearing Impairment')
ON CONFLICT DO NOTHING;

INSERT INTO ref.provision_measure_type
(code, name, unit)
VALUES
('TOTAL_PUPILS', 'Total pupils', 'COUNT'),
('SCHOOL_CAPACITY', 'School capacity', 'COUNT'),
('FSM_PUPILS', 'FSM pupils', 'COUNT')
ON CONFLICT DO NOTHING;

-- ------------------------------------------------------------
-- Local authority assignments
-- ------------------------------------------------------------

INSERT INTO core.establishment_authority
(establishment_id, authority_code, authority_name)
SELECT establishment_id, '202', 'Camden'
FROM core.establishment
WHERE establishment_id BETWEEN 1 AND 8;

INSERT INTO core.establishment_authority
(establishment_id, authority_code, authority_name)
SELECT establishment_id, '203', 'Greenwich'
FROM core.establishment
WHERE establishment_id BETWEEN 9 AND 16;

INSERT INTO core.establishment_authority
(establishment_id, authority_code, authority_name)
SELECT establishment_id, '201', 'City of London'
FROM core.establishment
WHERE establishment_id BETWEEN 17 AND 25;

-- ------------------------------------------------------------
-- UKPRNs
-- ------------------------------------------------------------

INSERT INTO core.establishment_identifier
(establishment_id, identifier_type, identifier_value)
SELECT
    establishment_id,
    'UKPRN',
    '1007' || LPAD(establishment_id::text, 5, '0')
FROM core.establishment
ON CONFLICT DO NOTHING;

-- ------------------------------------------------------------
-- Admissions
-- ------------------------------------------------------------

INSERT INTO core.establishment_admissions
(establishment_id, admissions_policy, statutory_low_age, statutory_high_age)
SELECT
    establishment_id,
    'Non-selective',
    3,
    11
FROM core.establishment
ON CONFLICT DO NOTHING;

-- ------------------------------------------------------------
-- Religion
-- ------------------------------------------------------------

INSERT INTO core.establishment_religion
(establishment_id, religious_character, religious_ethos)
SELECT establishment_id, 'Church of England', 'Church of England'
FROM core.establishment
WHERE establishment_id BETWEEN 1 AND 5;

INSERT INTO core.establishment_religion
(establishment_id, religious_character, religious_ethos)
SELECT establishment_id, 'Roman Catholic', 'Roman Catholic'
FROM core.establishment
WHERE establishment_id BETWEEN 6 AND 10;

INSERT INTO core.establishment_religion
(establishment_id, religious_character, religious_ethos)
SELECT establishment_id, 'None', 'None'
FROM core.establishment
WHERE establishment_id BETWEEN 11 AND 20;

INSERT INTO core.establishment_religion
(establishment_id, religious_character, religious_ethos)
SELECT establishment_id, 'Does not apply', 'Does not apply'
FROM core.establishment
WHERE establishment_id BETWEEN 21 AND 25;

-- ------------------------------------------------------------
-- Boarding
-- ------------------------------------------------------------

INSERT INTO core.establishment_boarding
(establishment_id, boarding_provision)
SELECT establishment_id,
       CASE
           WHEN establishment_id % 5 = 0 THEN 'Boarding school'
           ELSE 'No boarders'
       END
FROM core.establishment
ON CONFLICT DO NOTHING;

-- ------------------------------------------------------------
-- SEN
-- ------------------------------------------------------------

INSERT INTO core.establishment_sen
(establishment_id, has_sen_provision, sen_provision)
SELECT establishment_id, TRUE, 'ASD'
FROM core.establishment
WHERE establishment_id BETWEEN 1 AND 3
ON CONFLICT DO NOTHING;

INSERT INTO core.establishment_sen
(establishment_id, has_sen_provision, sen_provision)
SELECT establishment_id, TRUE, 'SLCN'
FROM core.establishment
WHERE establishment_id BETWEEN 4 AND 6
ON CONFLICT DO NOTHING;

INSERT INTO core.establishment_sen
(establishment_id, has_sen_provision, sen_provision)
SELECT establishment_id, TRUE, 'SEMH'
FROM core.establishment
WHERE establishment_id BETWEEN 7 AND 9
ON CONFLICT DO NOTHING;

INSERT INTO core.establishment_sen
(establishment_id, has_sen_provision, sen_provision)
SELECT establishment_id, TRUE, 'HI'
FROM core.establishment
WHERE establishment_id BETWEEN 10 AND 12
ON CONFLICT DO NOTHING;

COMMIT;