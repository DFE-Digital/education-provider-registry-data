# Results and remaining work

## Observed results

| Check | Result |
| --- | --- |
| Copy raw CSV codes as target IDs | Rejected with PostgreSQL 23503: establishment type 28 was not seeded |
| Apply Martin's type/phase mappings | Four establishments loaded with the expected target IDs and generated UUIDs |
| Copy followed by explicit Lookup | Unchanged batches reported four unchanged records |
| Change one source name | One updated, three unchanged; the existing UUID was preserved |
| Local PostgreSQL and ODBC tests | Insert, update, unchanged replay, count/duplicate/payload guards and rollback paths passed |
| Fresh database setup | Schema, reference subset and repeatable batch setup passed in an isolated database |
| Target backup | Restored successfully into an isolated database |

The earlier two-activity ADF runs can be found by these IDs:

- `69e2723b-2011-4877-8701-9108cf293487`: four unchanged.
- `6a1dc176-5b8c-4d0e-bb7d-89ac76f03f6f`: one updated, three unchanged.
- `f54d6c80-9e82-4955-b2d4-159a56e2c41f`: four unchanged.

The three-activity definition, including `Validate applied batch`, was inspected in published ADF on 13 October 2026. Publication is confirmed; an end-to-end result for that version has not been independently recorded here. Local cleanup batches are not evidence of an ADF run.

Generated UUIDs are instance-specific. After intentionally clearing and reloading the four demo records, capture their new UUIDs rather than comparing against an older baseline.

## Source assumptions

The CSV extract had 52,400 rows and 135 headers. The reconstruction creates an establishment table and six code/name lookups to match Martin's source query. It also converts numeric/date fields and renames fields such as SchoolWebsite to WebsiteAddress and PhaseOfEducation_code to educationPhase_code.

County codes and additional addresses are unavailable in the extract. Nursery/specialist codes are reconstructed from labels for wider transform checks. These choices need checking against real Edubase before using the simulation as production evidence.

At upstream commit `b4e8e48b9ccec695b013269d7b459a3b93cf0d5c`, the CSV had 13,453 rows with an unmapped establishment type and 580 with an unmapped education phase; overlap gives 13,461 distinct affected rows. For example, the CSV's `16 plus` label does not match the transform's `16 to 19` branch. This is a coverage finding against that snapshot, not proof the production lookup values are wrong. The four demo URNs have mappings.

## Before production

- Verify the real Edubase schema, types and reference values.
- Resolve mapping gaps: the inspected CSV has 13,461 distinct rows with an unmapped type or phase. The counts are from the inspected CSV and transform snapshot, not a fresh production scan.
- Decide between snapshot comparison and incremental extraction. Confirm source change timestamps or tracking capabilities, lookup changes, deletions and successful-load checkpoints.
- Measure full-volume runtime, source load, staging growth and ADF cost.
- Add remaining fields and related tables, with reference and foreign-key dependencies covered.
- Validate the target connector, deployment, permissions and operational recovery.

The current spike proves the six-field mapping and repeatable batch application. It does not establish complete migration coverage or a finished production design.
