Write-Host "Generating dynamic SQL seed file..."

# IMPORTANT: inside the container, /seed is the mounted folder
$seedPath = "/seed/002-seed.sql"

# Ensure output folder exists
$folder = Split-Path $seedPath
if (!(Test-Path $folder)) {
    New-Item -ItemType Directory -Path $folder | Out-Null
}

$sql = @()
$sql += "-- AUTO-GENERATED SEED FILE"
$sql += "-- Generated: $(Get-Date -Format o)"
$sql += ""

# ============================
# Lookup tables
# ============================
$sql += @"
-- ============================
-- Lookup table seed data
-- ============================

INSERT INTO EstablishmentType (name) VALUES
('Voluntary aided school'),
('British schools overseas')
ON CONFLICT DO NOTHING;

INSERT INTO EducationPhase (name) VALUES
('Primary'),
('Not applicable')
ON CONFLICT DO NOTHING;

INSERT INTO EstablishmentStatus (name) VALUES
('Closed'),
('Open')
ON CONFLICT DO NOTHING;

"@

# ============================
# Establishment Group Types (MISSING BEFORE — NOW FIXED)
# ============================
$sql += @"
-- ============================
-- Establishment Group Type seed data
-- ============================

INSERT INTO EstablishmentGroupType (code, name) VALUES
('MAT', 'Multi-Academy Trust'),
('FED', 'Federation'),
('INT', 'International Group')
ON CONFLICT DO NOTHING;

"@

# ============================
# Establishments
# ============================
$establishmentCount = 10000

$sql += "-- ============================"
$sql += "-- Establishment seed data"
$sql += "-- ============================"

$sql += "INSERT INTO Establishment (URN, EstablishmentName, EstablishmentTypeId, EducationPhaseId, SchoolWebsite, TelephoneNum, Street, Town, Postcode, EstablishmentStatusId) VALUES"

$estRows = @()

for ($i = 1; $i -le $establishmentCount; $i++) {
    $urn = 100000 + $i
    $name = "School $i"
    $street = "Street $i"
    $town = "Town $i"
    $postcode = "PC$i"

    # TypeId = 1, PhaseId = 1, StatusId = 2 (Open)
    $estRows += "($urn, '$name', 1, 1, NULL, NULL, '$street', '$town', '$postcode', 2)"
}

$sql += ($estRows -join ",`n") + ";"

# ============================
# Establishment Groups
# ============================
$groupCount = 10000

$sql += "-- ============================"
$sql += "-- Establishment Group seed data"
$sql += "-- ============================"

$sql += "INSERT INTO EstablishmentGroup (id, name, type_code) VALUES"

$groupRows = @()

for ($i = 1; $i -le $groupCount; $i++) {
    $gid = 10000 + $i
    $gname = "Group $i"

    # Rotate types: MAT, FED, INT
    $type = switch ($i % 3) {
        0 { "MAT" }
        1 { "FED" }
        2 { "INT" }
    }

    $groupRows += "($gid, '$gname', '$type')"
}

$sql += ($groupRows -join ",`n") + ";"

# ============================
# GroupLink
# ============================
$sql += "-- ============================"
$sql += "-- GroupLink seed data"
$sql += "-- ============================"

$sql += "INSERT INTO GroupLink (group_id, urn) VALUES"

$linkRows = @()

for ($i = 1; $i -le $groupCount; $i++) {
    $gid = 10000 + $i
    $urn = 100000 + $i
    $linkRows += "($gid, $urn)"
}

$sql += ($linkRows -join ",`n") + ";"

# ============================
# Write file
# ============================
$sql -join "`n" | Set-Content -Path $seedPath -Encoding UTF8

Write-Host "Seed file generated at $seedPath"
