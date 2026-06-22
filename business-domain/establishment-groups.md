# Establishment Groups

## Source Links

- [GOV.UK, Academy trusts: governance guide](https://www.gov.uk/government/publications/academy-trust-governance-guide/academy-trust-governance-guide)
- [GOV.UK, Academy trust handbook](https://www.gov.uk/government/publications/academy-trust-handbook)
- [GOV.UK, Maintained schools: governance guide](https://www.gov.uk/government/publications/maintained-schools-governance-guide/maintained-schools-governance-guide)
- [GOV.UK, Federations: guidance on the governance processes](https://www.gov.uk/government/publications/governance-in-federations)
- [GOV.UK, Open academies, free schools, studio schools and UTCs](https://www.gov.uk/government/publications/open-academies-and-academy-projects-in-development)

## Business Definition

An establishment group is an organisation or governance structure that sits around one or more establishments.

It is not the establishment itself. It is the wider body, relationship or governance arrangement that explains how establishments are connected, governed, sponsored or managed together.

In GIAS business language, establishment groups include:

- Academy trusts.
- Multi-academy trusts.
- Single-academy trusts.
- Secure single-academy trusts.
- Federations.
- School sponsors.
- Children's centre groups.
- Children's centre collaborations.
- Generic or legacy trust-style groups.

The core business question is:

```text
Which organisation, trust, federation, sponsor or group is this establishment linked to?
```

That is a different question from:

```text
What type of establishment is this?
```

An establishment can be an academy, free school, community school, nursery school or children's centre. Separately, it may belong to a trust, federation, sponsor relationship or children's-centre group.

## Why Establishment Groups Matter

Establishment groups matter because the education system is not only a list of individual schools and providers.

Some responsibilities sit at establishment level. For example, an individual school has its own name, URN, address, inspection history and local provision.

Other responsibilities sit at group level. For example, an academy trust has a board, members, trustees, funding agreement responsibilities, financial controls and governance duties across the academies it operates.

Some relationships sit between establishments. For example, maintained schools may federate under one governing body while remaining separate schools.

GIAS needs to understand both levels:

- The individual establishment.
- The group or organisation around it.

Without establishment groups, it is difficult to answer questions such as:

- Which academies are operated by this trust?
- Is this school part of a federation?
- Which establishments are associated with this sponsor?
- Which children's centres belong to this group or collaboration?
- Which governance people are recorded at trust or group level?
- Is a group open, closed or created in error?
- Which UKPRN, Companies House number or group identifier applies to the group rather than to a school?

## Academy Trusts

An academy trust is the legal organisation responsible for one or more academies.

GOV.UK defines an academy trust as an independent charitable company limited by guarantee with a funding agreement with the Secretary of State for Education. It can run one or more academies, including free schools, university technical colleges and studio schools.

This is why an academy trust is both:

- An accountable body for academies.
- An establishment group that links together the academies it operates.

The trust has its own governance structure. GOV.UK identifies the board as the accountable body for the academy trust, operating at a strategic level. The trust board is made up of academy trustees, who are also company directors and charity trustees.

The Academy Trust Handbook adds the financial and control context. Academy trusts must comply with the handbook as a condition of their funding agreement. The handbook provides the framework for financial management and other controls for academy trusts as publicly funded bodies.

In business terms:

```text
The academy is the establishment.
The academy trust is the legal group organisation accountable for that establishment.
```

## Multi-Academy Trusts

A multi-academy trust, or MAT, is an academy trust responsible for more than one academy.

GOV.UK describes a MAT as a single legal entity. It is a charitable company with a board of academy trustees that is accountable for all the academies in the trust. It has a master funding agreement and supplemental funding agreements for each academy.

This matters because a MAT should not be understood as a loose federation of separate academy companies. The MAT is one legal organisation. The individual academies remain separate establishments, but they sit within the same trust.

In business terms:

```text
A MAT is one trust.
It operates multiple academy establishments.
The academies remain individual establishments.
The trust board is accountable for the trust as a whole.
```

Example:

```text
Multi-academy trust: River Learning Trust

Linked establishments:
- River Primary Academy
- River Secondary Academy
- River Studio School

Each academy has its own establishment record.
The MAT is the group organisation around them.
```

## Single-Academy Trusts

A single-academy trust, or SAT, is an academy trust responsible for one academy.

GOV.UK describes a SAT as a charitable company with a board of academy trustees that is accountable for one academy. It has a single funding agreement with the Secretary of State for Education.

In business terms:

```text
A SAT is still an academy trust.
It is still a group organisation.
It happens to operate one academy rather than several.
```

This distinction is important. A single-academy trust is not the same thing as the academy establishment itself. The trust is the legal organisation. The academy is the school or provider operated by that trust.

## Secure Single-Academy Trusts

A secure single-academy trust is a specialised form of single-academy trust associated with secure 16 to 19 academy provision.

For business modelling, it should be treated as:

- A trust-style group organisation.
- A specialised SAT subtype.
- Connected to a secure academy establishment.

The important point is that the secure trust classification describes the group organisation, not just the establishment type.

## Free Schools, UTCs And Studio Schools

Free schools, university technical colleges and studio schools are often discussed as establishment types, but GOV.UK academy trust guidance is clear that academy trusts can operate these schools.

This means there are two related but different pieces of information:

| Question | Example Answer |
| --- | --- |
| What type of establishment is this? | Free school, UTC, studio school, academy converter. |
| Which group operates it? | A single-academy trust or multi-academy trust. |

The GOV.UK open academies dataset reinforces this landscape by publishing open academies, free schools, studio schools and UTCs, and academy projects in development.

For business purposes, do not collapse the establishment type into the group type. A free school is an establishment type. The academy trust is the group organisation.

## Federations

A federation is a statutory maintained-school governance arrangement.

GOV.UK federation guidance defines a federation as two or more maintained schools operating under the governance of a single governing body. The guidance applies to maintained schools and maintained nursery schools.

This is different from a multi-academy trust.

In a federation:

- The member schools remain maintained schools.
- The member schools remain separate establishments.
- The federation creates one shared governing body.
- The federation does not create one new school replacing the member schools.

In business terms:

```text
A federation is a shared maintained-school governing body.
It is not an academy trust.
It is not a new establishment.
```

Example:

```text
Federation: North and South Primary Federation

Member establishments:
- North Street Primary School
- South Road Primary School

The schools remain separate establishments.
The federation records the shared maintained-school governance arrangement.
```

Academies do not normally join maintained-school federations. Academy group structures are handled through academy trusts.

## School Sponsors

A school sponsor is a body connected with the sponsorship of schools, especially sponsor-led academy arrangements.

In business terms, sponsorship is about support, intervention, governance strength and improvement responsibility. A sponsor may be involved in bringing a school into the academy sector or supporting a sponsored academy.

This should be kept distinct from the academy trust itself.

The practical distinction is:

| Concept | Business Meaning |
| --- | --- |
| Academy trust | The legal organisation accountable for one or more academies. |
| School sponsor | A sponsoring body associated with support, sponsorship or improvement of one or more schools or academies. |

In some real-world cases the sponsor and trust may be closely related. For modelling and business explanation, it is still useful to keep the concepts separate because they answer different questions.

## Children's Centre Groups And Collaborations

Children's centres can also be represented through group and collaboration arrangements.

A children's centre group is best understood as an operational grouping of children's centres, often with a lead-centre arrangement.

A children's centre collaboration is best understood as a collaboration arrangement between children's centres or groups.

These concepts are more GIAS-specific than academy trusts and maintained-school federations. The GOV.UK sources listed above do not define these group types in the same way they define academy trusts or federations. They should therefore be treated as service-domain concepts that need business-owner confirmation when the target model is baselined.

The important business point is still clear:

```text
The children's centre remains the establishment.
The group or collaboration is the relationship around one or more children's centres.
```

## Generic Trusts

GIAS also has a generic trust-style group type.

This should not be confused with an academy trust. In business explanation, use "generic trust" or "legacy trust group" where possible, because simply saying "trust" can mislead readers into assuming an academy trust.

This concept should be retained where legacy records or current GIAS data still use it, but it needs careful wording in future models.

## Establishment Groups Are Not Establishment Type Groups

This is the main distinction to preserve.

| Concept | Business Question | Example |
| --- | --- | --- |
| Establishment type | What kind of provider is this establishment? | Community school, academy converter, free school, children's centre. |
| Establishment type group | Which broad provider family does that establishment type belong to? | Academies, free schools, local authority maintained schools. |
| Establishment group | Which real organisation or governance structure is this establishment linked to? | A MAT, SAT, federation, sponsor or children's-centre group. |
| Establishment group type | What kind of group organisation is it? | Multi-academy trust, federation, school sponsor. |

Example:

```text
Establishment:
Oakfield Academy

Establishment type:
Academy converter

Establishment type group:
Academies

Establishment group:
Oakfield Learning Trust

Establishment group type:
Multi-academy trust
```

These are all useful facts, but they are not the same fact.

## Group Membership

Group membership records the link between an establishment and an establishment group.

In business terms:

```text
This establishment belongs to, is governed by, is sponsored by, or is otherwise linked to this group.
```

The membership may need its own facts, such as:

- When the establishment joined the group.
- Whether the link is current.
- Whether the group relationship has a special role, such as lead centre.
- Whether the link has changed because of conversion, closure, reorganisation or trust change.

This is why group membership should be treated as a relationship in its own right, not just as a text field on the establishment.

## Group Relationships

Some relationships are between groups rather than between an establishment and a group.

For example, a single-academy trust may later become part of, or be succeeded by, a multi-academy trust. The individual academy may remain recognisable as an establishment, but the group structure around it has changed.

In business terms:

```text
Group relationships preserve history between group organisations.
```

This is important for understanding trust changes over time, not just the current group membership.

## What GIAS Needs To Know About Establishment Groups

For business purposes, GIAS may need to record:

- The group name.
- The group type.
- Whether the group is open, closed or created in error.
- The group's identifiers, such as UID, group ID, UKPRN or Companies House number.
- The establishments linked to the group.
- The dates when establishments joined or left the group.
- Group-to-group relationships, especially where trust structures change.
- Governance people associated with the group.
- Group contact and address information.

This does not mean every establishment group has every field. It means the group is a first-class business object, separate from the establishments it links together.

## Business Rules And Boundaries

The following boundaries should guide future modelling and service design:

- An establishment group is not itself an establishment.
- An academy trust is both an accountable body and an establishment group.
- A MAT is one legal trust entity with multiple academy establishments.
- A SAT is one legal trust entity with one academy establishment.
- A federation applies to maintained schools, not academies.
- A federation does not merge its member schools into one establishment.
- A school sponsor should not be treated as identical to the academy trust unless the business evidence says it is the same organisation in a particular case.
- Children's centre groups and collaborations should be treated as GIAS business concepts that need clear business-owner definitions.
- Establishment type group and establishment group type must remain separate concepts.

## Plain-English Glossary

| Term | Plain-English Meaning |
| --- | --- |
| Establishment | The individual school, academy, college, children's centre or provider record. |
| Establishment group | An organisation or governance structure around one or more establishments. |
| Academy trust | The legal organisation accountable for one or more academies. |
| MAT | One academy-trust legal entity accountable for multiple academies. |
| SAT | One academy-trust legal entity accountable for one academy. |
| Federation | Two or more maintained schools under one governing body. |
| School sponsor | A body associated with sponsoring or supporting schools, especially sponsor-led academy arrangements. |
| Children's centre group | An operational grouping of children's centres. |
| Children's centre collaboration | A collaboration arrangement involving children's centres or groups. |
| Group membership | The relationship linking an establishment to a group. |
| Group relationship | A relationship between two groups, often used to preserve trust-structure history. |

## Business Implication

The target business view should treat establishment groups as first-class business objects.

That means:

- Do not store group meaning only as a label on an establishment.
- Do not confuse provider type with group membership.
- Do not treat all groups as if they were academy trusts.
- Keep the establishment, the group, and the membership relationship distinct.
- Preserve group history where it explains trust changes, federation changes or children's-centre group changes.

The clean business model is:

```text
Establishment
  has a type
  may be linked to one or more establishment groups

Establishment group
  has a group type
  may have identifiers and lifecycle information
  may be linked to one or more establishments
  may have relationships with other groups
```

This gives GIAS a clearer way to represent the real education landscape: individual establishments, the organisations around them, and the relationships between the two.
