# ADF definitions

`pipeline/runcopyprotectagainstdup.json` and the two files in `dataset/` match the published prototype's names. Linked services `SqlServer1` and `GiasPostgresOdbc` are configured in ADF; their credentials are not exported here.

[Build-AdfArtifacts.ps1](../scripts/Build-AdfArtifacts.ps1) refreshes the mapped query and the pipeline's embedded SQL from Martin's transform. [Test-SpikeProject.ps1](../scripts/Test-SpikeProject.ps1) checks file references, mappings, expressions and documentation links without contacting ADF.

After a change, apply the JSON through ADF's Code editor or deployment tooling and use ADF Validate. Deployment and pipeline runs are separate manual steps. This repository is not yet connected to ADF Git integration.

See [batch loading](../adf-tests/Batch-load.md) for activity behaviour.
