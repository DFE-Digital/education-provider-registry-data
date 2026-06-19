# Establishment Type Field Rules Gap Analysis

## Purpose

This note validates whether the published Education Provider Registry field-rules page expresses the same fields as the observed current GIAS public `Details` tab evidence by establishment type.

Sources compared:

- Published page: <https://dfe-digital.github.io/education-provider-registry-data/models/establishment-type-field-rules/>
- Observed Details-tab evidence: `docs/data/extract-data/establishment-fields/establishment-details-tab-fields-by-type.md`
- Published-page source: `education-provider-registry-data/models/establishment-details-shacl.ttl`

## Methodology

The published page was checked live on 19 June 2026. It states that it is generated from `models/establishment-details-shacl.ttl`, so the detailed comparison used the local SHACL source file as the machine-readable representation of the published content.

This analysis was regenerated on 19 June 2026 after pulling the latest `education-provider-registry-data/models/establishment-details-shacl.ttl`.

For each observed establishment type in `establishment-details-tab-fields-by-type.md`:

1. The observed rendered `Details` tab fields were extracted from the Markdown table.
2. The applicable SHACL shapes were identified from the universal shape, group shapes and type-specific shapes.
3. The generated/published field names were normalised lightly for punctuation and casing.
4. Each observed UI field was compared against the published field-rule names.
5. A field was recorded as a contradiction where the observed `Details` tab includes it but the published rule says it is `Not applicable`.

Important limitations:

- This is an exact field-label/applicability comparison, not a semantic equivalence exercise.
- The published field-rules page is a SHACL applicability-rule view. It is not currently a complete rendered Details-tab field matrix.
- Some differences are expected because the SHACL model uses conceptual wrapper fields such as `Establishment identity`, `Establishment lifecycle`, `Establishment classification`, `Education, admissions and provision`, and `SEN and resourced provision`.
- Some observed UI labels aggregate lower-level concepts, for example `ID`, `Address`, `Headteacher / Principal`, `Ofsted rating and reports`, and `Data from other services`.
- The source evidence contains 41 observed establishment types. Five ERD-defined types were not observed in the extracts and are outside this validation.

## Summary Finding

The published field-rules page does **not** express exactly the same fields as the observed Details-tab evidence.

Across the 41 observed establishment types:

| Result | Count |
| --- | ---: |
| Exact matches | 0 |
| Incomplete only | 25 |
| Direct contradictions | 16 |

The latest SHACL file is much closer to the observed Details-tab matrix than the previous version. It now includes many more field-level rules for identity, contact, lifecycle, inspection, measures, SEN/resource provision, PRU-specific records, online-provider records and children's-centre fields.

It still does not exactly align with the observed rendered Details-tab evidence. The remaining structural issue is that the published page is a validation-rule view over a conceptual model, while the comparison source is a rendered UI field list. Some observed labels remain absent because the SHACL uses conceptual wrapper fields or target-model names rather than current UI labels.

The most important direct contradictions are where the current Details tab shows a field but the published page marks the same field as not applicable. These include:

- `Admissions policy` for PRUs, local-authority special schools, local-authority nursery schools, non-maintained/independent special schools, special academies, alternative provision academies and 16-19 academies.
- `Nursery provision` for special schools, special academies, alternative provision academies and 16-19 academies.
- `Diocese` and `Type of resourced provision` for City technology college.

## Type-Level Results

| Type code | Establishment type | Observed Details fields | Published rule fields | Matched labels | Observed fields missing from published rules | Contradictory fields | Result |
| --- | --- | ---: | ---: | ---: | ---: | --- | --- |
| `01` | Community school | 35 | 43 | 20 | 15 |  | Incomplete |
| `02` | Voluntary aided school | 35 | 43 | 20 | 15 |  | Incomplete |
| `03` | Voluntary controlled school | 35 | 43 | 20 | 15 |  | Incomplete |
| `05` | Foundation school | 36 | 43 | 20 | 16 |  | Incomplete |
| `06` | City technology college | 37 | 43 | 20 | 17 | Diocese; Type of resourced provision | Contradiction |
| `07` | Community special school | 35 | 42 | 20 | 15 | Admissions policy; Nursery provision | Contradiction |
| `08` | Non-maintained special school | 34 | 40 | 18 | 16 | Admissions policy | Contradiction |
| `10` | Other independent special school | 34 | 40 | 18 | 16 | Admissions policy | Contradiction |
| `11` | Other independent school | 34 | 43 | 19 | 15 |  | Incomplete |
| `12` | Foundation special school | 35 | 42 | 20 | 15 | Admissions policy; Nursery provision | Contradiction |
| `14` | Pupil referral unit | 30 | 37 | 15 | 15 | Admissions policy | Contradiction |
| `15` | Local authority nursery school | 34 | 42 | 19 | 15 | Admissions policy | Contradiction |
| `18` | Further education | 23 | 31 | 10 | 13 |  | Incomplete |
| `24` | Secure units | 17 | 19 | 7 | 10 |  | Incomplete |
| `25` | Offshore schools | 18 | 19 | 7 | 11 |  | Incomplete |
| `26` | Service children's education | 20 | 19 | 7 | 13 |  | Incomplete |
| `27` | Miscellaneous | 17 | 19 | 7 | 10 |  | Incomplete |
| `28` | Academy sponsor led | 37 | 44 | 21 | 16 |  | Incomplete |
| `29` | Higher education institutions | 21 | 34 | 12 | 9 |  | Incomplete |
| `30` | Welsh establishment | 18 | 33 | 11 | 7 |  | Incomplete |
| `31` | Sixth form centres | 20 | 31 | 9 | 11 |  | Incomplete |
| `32` | Special post 16 institution | 23 | 32 | 13 | 10 |  | Incomplete |
| `33` | Academy special sponsor led | 39 | 36 | 20 | 19 | Admissions policy; Nursery provision | Contradiction |
| `34` | Academy converter | 39 | 44 | 23 | 16 |  | Incomplete |
| `35` | Free schools | 39 | 43 | 22 | 17 |  | Incomplete |
| `36` | Free schools special | 39 | 36 | 20 | 19 | Admissions policy; Nursery provision | Contradiction |
| `37` | British schools overseas | 23 | 34 | 14 | 9 |  | Incomplete |
| `38` | Free schools alternative provision | 36 | 35 | 18 | 18 | Admissions policy; Nursery provision | Contradiction |
| `39` | Free schools 16 to 19 | 38 | 35 | 18 | 20 | Admissions policy; Nursery provision | Contradiction |
| `40` | University technical college | 37 | 43 | 21 | 16 |  | Incomplete |
| `41` | Studio schools | 37 | 43 | 21 | 16 |  | Incomplete |
| `42` | Academy alternative provision converter | 36 | 35 | 18 | 18 | Admissions policy; Nursery provision | Contradiction |
| `43` | Academy alternative provision sponsor led | 36 | 35 | 18 | 18 | Admissions policy; Nursery provision | Contradiction |
| `44` | Academy special converter | 38 | 36 | 18 | 20 | Admissions policy; Nursery provision | Contradiction |
| `45` | Academy 16-19 converter | 38 | 36 | 18 | 20 | Admissions policy; Nursery provision | Contradiction |
| `46` | Academy 16 to 19 sponsor led | 39 | 36 | 20 | 19 | Admissions policy; Nursery provision | Contradiction |
| `47` | Children's centre | 15 | 34 | 11 | 4 |  | Incomplete |
| `48` | Children's centre linked site | 13 | 34 | 10 | 3 |  | Incomplete |
| `49` | Online provider | 28 | 38 | 9 | 19 |  | Incomplete |
| `56` | Institution funded by other government department | 17 | 19 | 7 | 10 |  | Incomplete |
| `57` | Academy secure 16 to 19 | 17 | 28 | 8 | 9 |  | Incomplete |

## Interpretation

The published page should not currently be described as an exact field matrix for the public Details tab.

It is better described as a provisional SHACL rule view for conceptual data-model validation. That is useful, but it is still a different artefact from `establishment-details-tab-fields-by-type.md`.

Several previous contradictions have been resolved in the latest SHACL by adding current Details-tab display concepts or by distinguishing relationship applicability from displayed fields.

The remaining contradictions and label gaps should be reviewed against the intended meaning of the SHACL page. If a rule is target-domain applicability rather than current public UI visibility, the page should say that explicitly.

## Recommended Corrections

1. Decide whether the published page is intended to be a current Details-tab field matrix or a target validation-rule view.
2. If it is intended to be a current Details-tab field matrix, regenerate it from `establishment-details-tab-fields-by-type.md` or another complete per-type field source.
3. If it is intended to be a target validation-rule view, rename or reword the page so it does not imply exact current UI field coverage.
4. For each `Not applicable` rule, distinguish:
   - not applicable in the current public Details tab;
   - not applicable as an accountability/provision relationship in the target conceptual model;
   - not applicable in the target validation model even though the current UI displays a legacy/geographic/display field.
5. Revisit the direct contradictions before using the SHACL page as validation evidence for migration or supplier contracts.

## Highest-Priority Contradictions To Review

| Field | Affected observed types | Why it matters |
| --- | --- | --- |
| Admissions policy | PRU, local authority nursery, LA special schools, non-maintained/independent special schools, academy special, academy alternative provision, academy 16-19, free school special, free school alternative provision, free school 16-19 | This field is visible in the observed Details-tab evidence but still marked not applicable in SHACL. |
| Nursery provision | LA special schools, academy special, academy alternative provision, academy 16-19, free school special, free school alternative provision, free school 16-19 | Current UI visibility and target applicability appear to be mixed. |
| Diocese | City technology college | The observed Details-tab evidence shows Diocese, while the latest SHACL marks it not applicable for the matching independent-school pattern. |
| Type of resourced provision | City technology college | The observed Details-tab evidence shows this field, while the latest SHACL marks resourced provision not applicable. |
