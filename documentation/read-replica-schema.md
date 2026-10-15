# Read replica schema

The Education Provider Registry web application does not query source provider databases directly. 

Instead, an ETL process continuously projects provider data into a dedicated read replica, producing aggregate views including `establishment_aggregate`, `group_aggregate`, and `search_aggregate`. 

These aggregates represent the web-facing view of the domain and are optimised for retrieval rather than transaction processing.

Endpoints such as `/establishment/{id}` are served from these read aggregates. 
- Providing a fully materialised representation of an establishment
- Not requiring runtime joins across multiple source systems.

Search experiences are similarly backed by `search_aggregate`, enabling efficient querying of a denormalised search model.

By introducing this read layer the application is decouple from transactional systems in
- availability
- performance characteristics
- schema evolution

Changes in operational databases can be absorbed within the ETL layer, while web consumers continue to interact with a stable, purpose-built read model.

This approach reduces coupling, protects operational workloads, and enables the read platform to evolve independently to meet reporting and search requirements
