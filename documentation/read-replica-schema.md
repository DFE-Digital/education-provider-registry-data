# Read replica schema

The Education Provider Registry web application does not query source provider databases directly. 

Instead, an ETL process continuously projects provider data into a dedicated read replica, producing aggregate views. 

These aggregates represent the web-facing view of the domain and are optimised for retrieval rather than transaction processing.

See the [schema](https://github.com/DFE-Digital/education-provider-registry-data/blob/main/db/init/001-baseline-schema-180626.sql)

By introducing this read layer the application is decouple from transactional systems in
- availability
- performance characteristics
- schema evolution

Changes in operational databases can be absorbed within the ETL layer, while web consumers continue to interact with a stable, purpose-built read model. This approach reduces coupling, protects operational workloads, and enables the read platform to evolve independently to meet reporting and search requirements

## Detail

### establishment_aggregate and establishment_governor_aggregate

- Supports web consumers retrieving establishment details e.g. `/establisments/{urn}`

### `group_aggregate` and `group_member_aggregate`

- Supports web consumers retrieving group details e.g. `/groups/{groupUid}`

### search_aggregate

- Supports web consumers querying and filtering providers (Establishments|Groups)
