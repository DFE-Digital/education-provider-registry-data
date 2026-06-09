-- ============================================================
-- Establishments
-- ============================================================

CREATE TABLE IF NOT EXISTS EstablishmentType (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL
);

CREATE TABLE IF NOT EXISTS EducationPhase (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL
);

CREATE TABLE IF NOT EXISTS EstablishmentStatus (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL
);

CREATE TABLE IF NOT EXISTS Establishment (
    URN INTEGER PRIMARY KEY,
    EstablishmentName VARCHAR(255) NOT NULL,
    EstablishmentTypeId INTEGER NOT NULL REFERENCES EstablishmentType(id),
    EducationPhaseId INTEGER NOT NULL REFERENCES EducationPhase(id),
    SchoolWebsite VARCHAR(255),
    TelephoneNum VARCHAR(50),
    Street VARCHAR(255),
    Town VARCHAR(255),
    Postcode VARCHAR(20),
    EstablishmentStatusId INTEGER NOT NULL REFERENCES EstablishmentStatus(id)
);

-- ============================================================
-- EstablishmentGroups
-- ============================================================

-- Group type lookup (e.g., MAT, Federation, Trust)
CREATE TABLE IF NOT EXISTS EstablishmentGroupType (
    code VARCHAR(10) PRIMARY KEY,
    name TEXT NOT NULL
);

-- Establishment group (e.g., a MAT)
CREATE TABLE IF NOT EXISTS EstablishmentGroup (
    id INT PRIMARY KEY,
    name TEXT NOT NULL,
    type_code VARCHAR(10) NOT NULL REFERENCES EstablishmentGroupType(code)
);

-- Link table: which establishments belong to which group
CREATE TABLE IF NOT EXISTS GroupLink (
    group_id INT NOT NULL REFERENCES EstablishmentGroup(id),
    urn INT NOT NULL REFERENCES Establishment(URN),
    PRIMARY KEY (group_id, urn)
);
