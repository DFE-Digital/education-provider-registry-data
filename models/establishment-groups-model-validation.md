# Establishment Groups Model Validation

## Purpose

This document validates the introduction of establishment groups into the `education-provider-registry-data/models` vocabulary, taxonomy and ontology files.

The focus is the distinction between:

- `EstablishmentTypeGroup`: a provider-family classification for establishment types, such as `Academies`, `Free Schools` or `Local authority maintained schools`.
- `EstablishmentGroup`: a real organisation or governance structure to which establishments are linked, such as a multi-academy trust, federation, school sponsor or children's-centre group.

This distinction is foundational. The terms are similar, but they answer different business questions and must not collapse into one concept.

## Methodology

Validation was carried out on 22 June 2026 against the current workspace copy of:

- `education-provider-registry-data/models/establishment-details-conceptual-model.ttl`
- `education-provider-registry-data/models/establishment-details-vocabulary-skos.ttl`
- `education-provider-registry-data/models/establishment-details-taxonomy-skos.ttl`
- `education-provider-registry-data/models/establishment-details-shacl.ttl`
- `education-provider-registry-data/models/establishment-details-data-quality-shacl.ttl`

Local evidence used from `docs/data`:

- `docs/data/erd/core-establishment-erd.md`
- `docs/data/erd/groups-trusts-federations-local-authority-erd.md`
- `docs/data/investigation/group-field-metadata-analysis.md`
- `docs/data/erd/establishment-identifier-lifecycle-erd.md`
- `docs/data/ukprn-sync-data-flow.md`

External evidence used:

- GOV.UK, [Academy trusts: governance guide](https://www.gov.uk/government/publications/academy-trust-governance-guide/academy-trust-governance-guide)
- GOV.UK, [Academy trust handbook](https://www.gov.uk/government/publications/academy-trust-handbook)
- GOV.UK, [Maintained schools: governance guide](https://www.gov.uk/government/publications/maintained-schools-governance-guide/maintained-schools-governance-guide)
- GOV.UK, [Federations: guidance on the governance processes](https://www.gov.uk/government/publications/governance-in-federations)
- GOV.UK, [Open academies, free schools, studio schools and UTCs](https://www.gov.uk/government/publications/open-academies-and-academy-projects-in-development)

Checks performed:

- Parsed every `.ttl` file in `education-provider-registry-data/models` using `rdf-turtle`.
- Compared new OWL establishment-group classes with SKOS vocabulary concepts and taxonomy taxons.
- Checked the model against the current-state GIAS schema and code-derived evidence in `docs/data`.
- Checked the domain definitions against current GOV.UK guidance where authoritative public guidance exists.
- Reviewed whether the taxonomy and ontology keep provider-family classification separate from real group organisations.

Limitations:

- Public GIAS group detail pages were not used as evidence because automated fetches did not return usable page content in this session.
- The validation is a model-quality review, not a full semantic reasoner pass.
- It validates the current model snapshot only; the `education-provider-registry-data` repository is moving quickly and should be rechecked after TTL changes.

## Summary Finding

The introduction of establishment groups is directionally correct and materially improves the model. It separates real group organisations from provider-family establishment type groupings, introduces the right core structures for group membership and group-to-group relationships, and aligns well with current GIAS evidence.

However, it is not yet tight enough to treat as a final foundational model. The most important issue is a domain-definition error in the multi-academy trust wording: the model currently says a MAT operates academies under a single funding agreement, but GOV.UK defines a MAT as a single legal entity with a master funding agreement and supplemental funding agreements for each academy. There are also some modelling loosenesses around group status/lifecycle, group UID versus business group ID, archived group types, and the dual role of academy trusts as both accountable bodies and establishment groups.

Overall judgement: **valid but provisional; suitable for iteration, not yet suitable as a locked target ontology.**

## TTL Validation

All current TTL files parsed successfully.

| File | Statements |
| --- | ---: |
| `establishment-details-conceptual-model.ttl` | 1,028 |
| `establishment-details-data-quality-shacl.ttl` | 188 |
| `establishment-details-shacl.ttl` | 3,353 |
| `establishment-details-taxonomy-skos.ttl` | 846 |
| `establishment-details-vocabulary-skos.ttl` | 2,123 |

This confirms syntactic Turtle validity. It does not prove the domain model is semantically complete.

## What The Model Now Says

### Establishment Type Groups

`EstablishmentTypeGroup` remains the taxonomy facet for grouping establishment types into broad provider families.

Examples:

- `EstablishmentTypeGroupAcademies`
- `EstablishmentTypeGroupFreeSchools`
- `EstablishmentTypeGroupLaMaintainedSchools`
- `EstablishmentTypeGroupChildrensCentres`

Local GIAS evidence confirms this is used for search, display, resource text and type-specific behaviour. The physical schema looks like a one-to-many relationship from `EstablishmentTypeGroup` to `EstablishmentType`, but Java and C# add some many-to-many behaviour in application lookup code.

Validation: **Correct concept, but the future model should not hide many-to-many provider-family classification in application code.**

### Establishment Groups

The new ontology introduces:

- `EstablishmentGroup`
- `MultiAcademyTrust`
- `SecureSingleAcademyTrust`
- `Federation`
- `SchoolSponsor`
- `ChildrensCentreGroup`
- `ChildrensCentreCollaboration`
- `GenericTrust`
- `GroupMembership`
- `GroupRelationship`
- `GroupRelationshipType`
- `GroupId`
- `GroupUkprn`

The vocabulary adds matching SKOS concepts for all these classes. The taxonomy adds group-type taxons for the active establishment group types.

Validation: **The concept boundary is right. `EstablishmentGroup` is a real organisation or governance structure, not a provider-family tag.**

## Coverage Check

| Concept | OWL class | Vocabulary concept | Taxonomy taxon |
| --- | --- | --- | --- |
| `EstablishmentGroup` | Yes | Yes | No |
| `AcademyTrust` | Yes | Yes | No |
| `SingleAcademyTrust` | Yes | Yes | Yes |
| `MultiAcademyTrust` | Yes | Yes | Yes |
| `SecureSingleAcademyTrust` | Yes | Yes | Yes |
| `Federation` | Yes | Yes | Yes |
| `SchoolSponsor` | Yes | Yes | Yes |
| `ChildrensCentreGroup` | Yes | Yes | Yes |
| `ChildrensCentreCollaboration` | Yes | Yes | Yes |
| `GenericTrust` | Yes | Yes | Yes |
| `GroupMembership` | Yes | Yes | No |
| `GroupRelationship` | Yes | Yes | No |
| `GroupRelationshipType` | Yes | Yes | Yes |
| `GroupId` | Yes | Yes | No |
| `GroupUkprn` | Yes | Yes | No |

This is a good split. The taxonomy should contain classification values and facets, not every structural class. It is right that `GroupMembership`, `GroupRelationship`, `GroupId` and `GroupUkprn` are vocabulary/ontology concepts rather than group-type taxons.

`EstablishmentGroup` and `AcademyTrust` are also correctly absent from the `EstablishmentGroupType` taxonomy facet because they are broader conceptual classes, not direct GIAS group-type values.

## Group Type Taxonomy Validation

The taxonomy now models these active `EstablishmentGroupType` values:

| Taxon | Label | Source |
| --- | --- | --- |
| `Federation` | Federation | Group type code 01 |
| `GenericTrust` | Trust | Group type code 02 |
| `SchoolSponsor` | School sponsor | Group type code 05 |
| `MultiAcademyTrust` | Multi-academy trust | Group type code 06 |
| `ChildrensCentreGroup` | Children's centres group | Group type code 08 |
| `ChildrensCentreCollaboration` | Children's centres collaboration | Group type code 09 |
| `SingleAcademyTrust` | Single-academy trust | Group type code 10 |
| `SecureSingleAcademyTrust` | Secure single-academy trust | Group type code 11 |

Local `docs/data` evidence lists three additional group types as archived:

- `03` Collegiate
- `04` Partnership
- `07` Umbrella trust

Validation: **The active group-type coverage is coherent, but the taxonomy should add a scope note saying archived group types are intentionally excluded from the active taxonomy.**

Without that note, a reader may think the model accidentally missed codes 03, 04 and 07.

## Concept Boundary Findings

### Finding 1: `EstablishmentTypeGroup` And `EstablishmentGroupType` Are Correctly Separate

The current model preserves the essential distinction:

| Concept | Meaning | Example |
| --- | --- | --- |
| `EstablishmentTypeGroup` | Broad provider family for an establishment type. | `Academy converter` belongs to `Academies`. |
| `EstablishmentGroupType` | Classification of a real group organisation. | An `EstablishmentGroup` can be a `Multi-academy trust`. |

This aligns with `docs/data/erd/core-establishment-erd.md` and `docs/data/erd/groups-trusts-federations-local-authority-erd.md`.

Validation: **Good. This separation should be protected.**

Recommended improvement:

- Add cross-reference scope notes to both concepts warning that they are different despite similar names.

### Finding 2: `EstablishmentGroup` Is Defined Well

The vocabulary defines `EstablishmentGroup` as an organisation or governance structure to which establishments belong. It explicitly covers academy trusts, federations, school sponsors and children's-centre groups.

This matches the current GIAS evidence:

- `EstablishmentGroup` is a live aggregate with identity, type, address, contacts, lifecycle dates, Companies House number, UKPRN and local authority context.
- `GroupLink` links establishments to groups.
- `GroupRelationsLink` links one group to another.
- Group change requests, staff records, user scope and cache/projection tables all reference group identity.

Validation: **Good.**

Recommended improvement:

- Add a stronger note that `EstablishmentGroup` is a current-state umbrella aggregate and may decompose in the future target model into organisation identity, lifecycle, contact/address, identifiers and relationship records.

### Finding 3: `GroupMembership` Is The Right Abstraction For `GroupLink`

The ontology models an establishment as having zero or more `GroupMembership` records, and each membership points to exactly one `EstablishmentGroup`.

This is a better target concept than directly saying an establishment `memberOf` a group, because the current GIAS relationship has attributes:

- Effective date.
- Archived/current state.
- Link type.
- Children's-centre-specific link type.
- Change-request history.

Validation: **Good abstraction.**

Gap:

- The ontology does not yet model membership effective date, archived state, link type or lead-centre semantics.

Recommended improvement:

- Add `GroupMembershipEffectiveDate` and group-link-type classification concepts if group membership is expected to be part of the target logical model.

### Finding 4: `GroupRelationship` Is The Right Abstraction For `GroupRelationsLink`

The model introduces `GroupRelationship` and `GroupRelationshipType`, with values:

- `SuccessorMultiAcademyTrust`
- `PredecessorSingleAcademyTrust`

This aligns with local evidence that `GroupRelationsLink` is used around SAT-to-MAT conversion history.

Validation: **Good, but currently narrow.**

Gap:

- The relationship direction is underspecified. `relatesTo` gives the other group, but the current table has `linking_group` and `linked_group`. The target model will need clear source/current/target semantics if group histories are migrated.

Recommended improvement:

- Replace or supplement `relatesTo` with clearer properties such as `relationshipFromGroup` and `relationshipToGroup`, or explicitly define `hasGroupRelationship` as "relationship recorded from this group to the related group".

### Finding 5: Academy Trust Dual Role Is Correct, But Needs Cleaner OWL Semantics

The model says an academy trust is both:

- An accountability body, because it holds the funding agreement and is accountable for academies.
- An establishment group, because it groups one or more academy establishments.

This is correct. GOV.UK defines an academy trust as an independent charitable company limited by guarantee with a funding agreement with the Secretary of State. GOV.UK also defines MATs and SATs as trust structures accountable for their academies.

The ontology currently places `AcademyTrust` in the accountability section and makes it `rdfs:subClassOf epr:EstablishmentGroup`. It is also the range of `accountableToAcademyTrust`.

Validation: **The business meaning is correct.**

Looseness:

- The OWL class hierarchy does not show `AcademyTrust` as a subclass of any accountability-body class. It is used as the target of an accountability relationship, but not typed as an accountable body in the class hierarchy.

Recommended improvement:

- Consider introducing `AccountableBody` or `AccountabilityBody` as a class distinct from `EstablishmentAccountability`.
- Then model `AcademyTrust`, `LocalAuthority` and `Proprietor` as accountable bodies where appropriate.
- Keep `AcademyTrust` also as an `EstablishmentGroup`.

That would express the dual nature cleanly without making `EstablishmentAccountability` do two jobs.

## Domain-Definition Findings

### Finding 6: Multi-Academy Trust Definition Needs Correction

The current ontology says:

```text
An academy trust operating two or more academies under a single funding agreement with the Secretary of State.
```

The vocabulary says:

```text
A charitable company incorporated under the Companies Act 2006 that enters a single funding agreement with the Secretary of State to operate two or more academies or free schools.
```

GOV.UK's academy trust governance guide defines a MAT as a single legal entity and says it has both a master funding agreement and supplemental funding agreements for each academy.

Validation: **Needs correction.**

Recommended wording:

```text
A single legal academy-trust entity operating two or more academies, free schools, UTCs or studio schools. It has a master funding agreement with the Secretary of State and supplemental funding agreements for each academy.
```

This avoids implying that every academy in a MAT sits under only one undifferentiated funding agreement.

### Finding 7: Single-Academy Trust Definition Is Broadly Correct

The vocabulary says a SAT holds a funding agreement for exactly one academy. GOV.UK describes a SAT as a charitable company accountable for one academy with a single funding agreement with the Secretary of State.

Validation: **Good.**

Recommended improvement:

- Keep the existing definition, but align the ontology label with the vocabulary spelling: `Single-academy trust` as the preferred label, with `Single academy trust` as an alternative label.

### Finding 8: Federation Definition Is Strong

The model says a federation is a statutory arrangement where two or more maintained schools share a single federated governing body. It also states each school retains its own identity and that federations apply to maintained schools.

GOV.UK's maintained school governance guide defines a federated school as two or more maintained schools, including maintained nursery schools, operating under the governance of a single governing body. GOV.UK federation guidance also states that a federation is two or more maintained schools under one governing body.

Validation: **Good.**

Recommended improvement:

- Consider whether "budget" should remain in the definition. Local evidence says a federation can ask to pool funding, so "each member school retains its own budget" may be too absolute. A safer wording is "retains its own establishment identity, while some governance and funding arrangements may be shared or pooled under federation rules."

### Finding 9: School Sponsor Needs A Clearer Boundary From Academy Sponsor

The vocabulary sensibly notes that `SchoolSponsor` is a group organisation and `AcademySponsor` is part of the accountability layer for a specific academy.

Validation: **Good boundary, but fragile because the names are close.**

Recommended improvement:

- Add examples or scope notes explaining whether `SchoolSponsor` maps to `EstablishmentGroupType` code 05 and whether `AcademySponsor` maps to the academy sponsor value shown on establishment details.
- Avoid using these concepts interchangeably in downstream documentation.

### Finding 10: Children's Centre Group Definitions Are Under-Evidenced

The model includes:

- `ChildrensCentreGroup`
- `ChildrensCentreCollaboration`

Local GIAS evidence confirms these are active group types and that children's-centre group fields have special display semantics.

Validation: **Structurally correct, but definitions are thin.**

Gaps:

- The difference between "group" and "collaboration" is not explained beyond "operational cluster" versus "cross-group collaboration".
- Lead-centre semantics are not modelled beyond the definition text.
- Public external validation was not established in this session.

Recommended improvement:

- Add a follow-up data/domain question for the BA or service owner: what is the operational difference between a children's-centre group and a children's-centre collaboration, and what role does the lead centre play?

### Finding 11: Generic Trust Is Correctly Cautious

`GenericTrust` exists for `EstablishmentGroupType` code 02 and is explicitly distinguished from `AcademyTrust`.

Validation: **Good.**

Risk:

- The label `Trust` could be misread as an academy trust by business or technical readers.

Recommended improvement:

- Keep the local name `GenericTrust`.
- Add an alternative label or scope note such as "Legacy trust group type" if confirmed by data owners.

## Identifier And Lifecycle Findings

### Finding 12: Group UID, Group ID And UKPRN Need Sharper Separation

Local evidence shows:

- `EstablishmentGroup.id` is the internal group UID used by many relationships.
- `GroupId` is a business-facing GIAS group identifier.
- `GroupUkprn` is an external UKRLP provider reference number.
- Group UID allocation uses `UrnSequence` rows `CCGROUP` and `ESTGROUP`.
- Group UKPRN sync matches UKRLP values by Companies House number for establishment groups.

The ontology currently models `GroupId`, `GroupUkprn` and `CompaniesHouseNumber`, but it does not model group UID or the group UID allocation mechanism.

Validation: **Partially complete.**

Recommended improvement:

- Add `GroupUid` or `EstablishmentGroupUid` as a distinct identifier concept if UID is part of migration identity or public/group URL identity.
- Keep it distinct from `GroupId`.
- Consider adding value constraints or data-quality SHACL for group UKPRN and Companies House number.

### Finding 13: Group Lifecycle Is Not Yet Modelled

Local group evidence includes:

- `OpenDate`
- `ClosedDate`
- `CreatedInError`
- Derived group status

The ontology currently has establishment lifecycle concepts, but it does not explicitly model group lifecycle or group status.

Validation: **Incomplete for establishment groups.**

Recommended improvement:

- Add a `GroupLifecycle` concept if establishment groups will be first-class target entities.
- Model `GroupOpenDate`, `GroupClosedDate`, `GroupCreatedInError` and derived `GroupStatus`, or deliberately reuse generic lifecycle concepts with domain/range notes that cover both establishments and groups.

## Taxonomy Design Findings

### Finding 14: The New Group Facets Belong In Taxonomy

`EstablishmentGroupType` and `GroupRelationshipType` are both classification facets. It is therefore appropriate that they are in the taxonomy.

Validation: **Good.**

The taxonomy should not contain structural concepts such as `GroupMembership` or `GroupId`. Those belong in vocabulary and ontology.

### Finding 15: The Same URI Across Vocabulary, Taxonomy And OWL Is Acceptable Here

The model uses the same `epr:` URI for concepts such as `MultiAcademyTrust` as:

- An OWL class in the conceptual model.
- A SKOS concept in the vocabulary.
- A SKOS taxon in the taxonomy.

This is acceptable because the URI is being used consistently for the same business concept via OWL 2 punning and SKOS representation.

Validation: **Good, with one caveat.**

Caveat:

- The vocabulary and taxonomy definitions must stay coherent. If the taxonomy concept is only a group-type value but the vocabulary concept is a richer organisation class, the relationship should be documented. For the current group-type values this is still acceptable because the group type and organisation subtype are aligned.

## Prioritised Corrections

| Priority | Finding | Recommended action |
| --- | --- | --- |
| High | MAT definition says "single funding agreement" | Correct ontology and vocabulary wording to match GOV.UK: one legal entity, master funding agreement plus supplemental agreements. |
| High | Academy trust dual role is semantically right but not cleanly typed | Consider adding `AccountableBody` and making `AcademyTrust` both an accountable body and an `EstablishmentGroup`. |
| Medium | Archived group types are excluded without an explicit scope note | Add a taxonomy scope note listing archived group-type codes 03, 04 and 07 as intentionally out of active scope. |
| Medium | Group UID is not modelled distinctly from `GroupId` | Add `GroupUid`/`EstablishmentGroupUid` if group identity is migration-critical. |
| Medium | Group lifecycle/status not modelled | Add group lifecycle concepts or explicitly generalise lifecycle concepts to establishments and groups. |
| Medium | Group relationship direction is underspecified | Clarify `relatesTo` or add directional relationship properties. |
| Low | Children's-centre group/collaboration definitions are thin | Validate the operational distinction with the business. |
| Low | Federation definition may overstate independent budgets | Reword to allow lawful pooled funding under federation governance. |

## Conclusion

The establishment group model is a strong first introduction. It captures the most important conceptual move: `EstablishmentGroup` is a real organisation/governance structure, while `EstablishmentTypeGroup` is a provider-family classification.

The model is strongest where it follows current GIAS evidence:

- `EstablishmentGroup` as a group aggregate.
- `GroupMembership` as the abstraction over `GroupLink`.
- `GroupRelationship` as the abstraction over `GroupRelationsLink`.
- `EstablishmentGroupType` as a taxonomy facet for active group-type codes.

The model needs tightening before it becomes foundational target ontology:

- Correct the MAT funding-agreement definition.
- Clarify academy trust's dual role as accountable body and establishment group.
- Add explicit scope for archived group types.
- Decide whether group UID, group lifecycle and relationship direction are in scope for the conceptual model.

Recommended status: **provisionally valid, with correction required before baselining v1.1 as authoritative.**
