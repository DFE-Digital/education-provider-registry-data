# Azure Data Factory CI/CD

This directory contains the Azure Data Factory (ADF) code and CI/CD configuration for the Education Provider Registry Data project.

## Overview

The ADF CI/CD system provides:

- **Automatic validation** of ADF definitions on pull requests
- **Environment parity** through automated export/import from development to test and production
- **Single source of truth** maintained in the `main` branch
- **Infrastructure as Code** using Terraform for ADF provisioning
- **Reusable workflows** for other projects

## Directory Structure

```
adf/
├── factory/                  # ADF factory configuration (generated)
├── linkedService/            # Linked services definitions (generated)
├── managedVirtualNetwork/    # Managed virtual network config (generated)
├── definitions/              # Environment-specific exports
│   ├── dev/                  # Development environment (exported from ADF)
│   │   ├── linkedServices/
│   │   ├── datasets/
│   │   ├── pipelines/
│   │   ├── triggers/
│   │   └── manifest.json
│   ├── test/                 # Test environment (imported from dev)
│   └── prod/                 # Production environment (imported from dev)
├── scripts/                  # Automation scripts
│   ├── export.sh            # Export ADF definitions
│   └── import.sh            # Import ADF definitions
├── validation/              # Validation schemas and rules
└── README.md               # This file
```

## How It Works

### 1. Development Workflow

1. **Local Changes**: Create/modify ADF pipelines, datasets, and linked services in the Azure Portal ADF UI
2. **GitHub Integration**: Development ADF is connected to this repo's `main` branch at `adf/` folder
3. **Publish**: When you publish in ADF UI, changes are automatically committed to Git
4. **PR Creation**: Changes in Git automatically trigger validation workflow on PR

### 2. Validation (on Pull Request)

When you create a PR with ADF changes:

```
PR with adf/ changes
    ↓
    Trigger: adf-validate.yml
    ↓
    ├─ Validate Terraform configuration
    ├─ Validate ADF JSON schemas
    ├─ Check for hardcoded secrets
    └─ Validate naming conventions
    ↓
    Blocks merge if validation fails
```

### 3. Infrastructure Deployment (on Merge to main - Terraform changes)

When infrastructure Terraform files are merged to main:

```
Merge to main (terraform/application/adf.tf)
    ↓
    Trigger: adf-deploy-infra.yml
    ↓
    Deploy dev ADF
        ↓
    Deploy test ADF
        ↓
    Deploy prod ADF
    ↓
    Terraform outputs stored in artifacts
```

### 4. Code Deployment (on Merge to main - ADF code changes)

When ADF definitions are merged to main:

```
Merge to main (adf/factory/* or adf/linkedService/*)
    ↓
    Trigger: adf-deploy-code.yml
    ↓
    Export from dev ADF
        ↓
    Commit exports to Git
        ↓
    Import to test ADF
        ↓
    Import to prod ADF
    ↓
    Workflow summary in GitHub
```

## Local Development

### Prerequisites

- Azure CLI (`az`) installed and authenticated
- Bash shell
- Access to Azure subscription
- Git

### Export ADF Definitions

Export the current state of the development ADF to Git:

```bash
./adf/scripts/export.sh \
  --adf-name "s189t01-eprdat-development-adf" \
  --resource-group "s189t01-eprdat-dv-rg" \
  --environment "development"
```

This creates/updates JSON files in `adf/definitions/dev/`.

### Import ADF Definitions

Import definitions from Git to any ADF instance:

```bash
# Import to test environment
./adf/scripts/import.sh \
  --adf-name "s189t01-eprdat-test-adf" \
  --resource-group "s189t01-eprdat-t-rg" \
  --environment "test"

# Import to production environment
./adf/scripts/import.sh \
  --adf-name "s189p01-eprdat-prod-adf" \
  --resource-group "s189p01-eprdat-pd-rg" \
  --environment "prod"
```

### Validate Locally

Run the same validations that execute in the PR workflow:

```bash
# Validate Terraform
cd terraform/application
terraform init -backend=false
terraform validate
terraform fmt -check -recursive
cd -

# Validate JSON schemas
find adf -type f -name "*.json" ! -path "*/node_modules/*" -exec jq empty {} \;

# Check for secrets
grep -r -i 'password.*:' adf/ --include="*.json" || echo "No obvious secrets found"
```

## GitHub Actions Workflows

### Validation Workflow (`adf-validate.yml`)

**Trigger**: Pull request with changes to `adf/` or `terraform/application/adf*.tf`

**Validation Steps**:
- Terraform format and validation
- JSON schema validation with `jq`
- Hardcoded secrets detection
- ADF resource naming conventions

**Status Check**: Required to pass before merge

### Infrastructure Deployment (`adf-deploy-infra.yml`)

**Trigger**: Push to `main` with changes to `terraform/application/adf.tf`

**Actions**:
- Deploy/update ADF Terraform module to development
- Deploy/update ADF Terraform module to test
- Deploy/update ADF Terraform module to production

**Artifacts**: Terraform plans and outputs stored for audit

### Code Deployment (`adf-deploy-code.yml`)

**Trigger**: Push to `main` with changes to `adf/factory/`, `adf/linkedService/`, or `adf/managedVirtualNetwork/`

**Actions**:
1. Export ADF definitions from development environment
2. Commit exported definitions to Git (if changed)
3. Import definitions to test environment
4. Import definitions to production environment

## Terraform Configuration

The ADF infrastructure is defined in `terraform/application/adf.tf` and uses the `aks/azure_data_factory` module from terraform-modules.

### Environment-Specific Configuration

Each environment has its own `.tfvars.json` configuration:

- `terraform/application/config/development.tfvars.json`
- `terraform/application/config/test.tfvars.json`
- `terraform/application/config/production.tfvars.json`

### ADF Variables

Available Terraform variables for ADF configuration:

```hcl
variable "adf_sql_connections"         # SQL Server connections
variable "adf_postgresql_connections"  # PostgreSQL connections
variable "adf_storage_connections"     # Storage account connections
variable "adf_create_storage_account"  # Create standard storage for runtime
variable "adf_managed_virtual_network_enabled"  # Enable managed VNet
```

### ADF Outputs

Terraform exports the following outputs for use in scripts:

```hcl
output "adf_name"        # Azure Data Factory name
output "adf_id"          # Azure Data Factory resource ID
output "adf_principal_id" # Managed identity principal ID
```

## Environment Details

### Development

- **ADF Name**: `s189t01-eprdat-development-adf`
- **Resource Group**: `s189t01-eprdat-dv-rg`
- **Git Integration**: **ENABLED** (connected to GitHub `main` branch, `adf/` folder)
- **Publishing**: Changes published in ADF UI → Committed to Git

### Test

- **ADF Name**: `s189t01-eprdat-test-adf`
- **Resource Group**: `s189t01-eprdat-t-rg`
- **Git Integration**: Disabled (read-only, updated via import script)
- **Updates**: Pulled from development via export/import workflow

### Production

- **ADF Name**: `s189p01-eprdat-prod-adf`
- **Resource Group**: `s189p01-eprdat-pd-rg`
- **Git Integration**: Disabled (read-only, updated via import script)
- **Updates**: Pulled from development via export/import workflow
- **Manual Approval**: Automatic (can be changed to require approval)

## Authentication

### GitHub Actions

The workflows use Azure Workload Identity Federation for authentication:

**Required Secrets** (GitHub Secrets/Variables):
- `AZURE_CLIENT_ID` - Azure service principal client ID
- `AZURE_SUBSCRIPTION_ID` - Azure subscription ID
- `AZURE_TENANT_ID` - Azure AD tenant ID

### Service Principal

A service principal needs the following roles:

- **Data Factory Contributor** on each ADF resource
- **Reader** on resource groups (for queries)

### Setup Instructions

See [AUTHENTICATION.md](../docs/AUTHENTICATION.md) for detailed setup steps.

## Troubleshooting

### Export Script Issues

**Problem**: Script fails to find ADF
```bash
Error: Azure Data Factory not found
```

**Solution**: Verify ADF name and resource group
```bash
az datafactory show --name "YOUR_ADF_NAME" --resource-group "YOUR_RG_NAME"
```

**Problem**: Permission denied
```bash
Error: This request is not authorized to perform this operation
```

**Solution**: Ensure your Azure CLI session has the correct role
```bash
az account show  # Verify you're logged in
az role assignment list --assignee $(az ad signed-in-user show --query objectId) --scope /subscriptions/YOUR_SUB_ID
```

### Import Script Issues

**Problem**: Dataset or pipeline import fails
```bash
Failed to import dataset: xxx
```

**Solution**: Check dependency order (linked services → datasets → pipelines)
- Linked services must be imported first
- Datasets depend on linked services
- Pipelines depend on datasets

**Problem**: Connection strings not working
```bash
Error: The connection string is invalid
```

**Solution**: Verify Key Vault secrets are available
- Check that the ADF managed identity can read from Key Vault
- Verify secret names match in Key Vault and ADF linked services

### Workflow Issues

**Problem**: Workflow times out
```bash
The job was not completed within the maximum duration of 1440 minutes.
```

**Solution**: Check for hung imports/exports
- Monitor Azure CLI commands
- Increase timeout in workflow (not recommended long-term)
- Check ADF UI for stuck operations

**Problem**: Permission denied in workflow
```bash
Error: The client 'xxx' with object id 'xxx' does not have authorization
```

**Solution**: Verify service principal has correct roles
```bash
# Add role assignment
az role assignment create \
  --role "Data Factory Contributor" \
  --assignee-object-id "YOUR_SP_ID" \
  --scope "/subscriptions/YOUR_SUB_ID/resourceGroups/YOUR_RG/providers/Microsoft.DataFactory/factories/YOUR_ADF"
```

## Reusability

The workflows are designed to be reusable across projects. To use in another repository:

### 1. Copy Workflow Files

```bash
# From education-provider-registry-data
cp .github/workflows/validate-adf.yml ../your-other-repo/.github/workflows/
cp .github/workflows/deploy-adf-infra.yml ../your-other-repo/.github/workflows/
cp .github/workflows/deploy-adf-code.yml ../your-other-repo/.github/workflows/
```

### 2. Copy Scripts

```bash
cp -r adf/scripts ../your-other-repo/adf/scripts/
```

### 3. Customize Entry Points

Edit the local workflow files (adf-validate.yml, adf-deploy-infra.yml, adf-deploy-code.yml) to match your project structure.

### 4. Configure Secrets

Add the required secrets to your new repository:
- `AZURE_CLIENT_ID`
- `AZURE_SUBSCRIPTION_ID`
- `AZURE_TENANT_ID`

### 5. Update Paths

Modify the `working-directory`, `terraform-path`, and `adf-path` inputs to match your project structure.

## Best Practices

### 1. Always Validate Locally

Before pushing, validate your changes:
```bash
terraform validate
jq empty adf/**/*.json
```

### 2. Use Meaningful Resource Names

ADF resource names (pipelines, datasets, etc.) should:
- Use lowercase letters and hyphens only
- Be descriptive (e.g., `export-gias-to-epr` not `export1`)
- Follow team naming conventions

### 3. Keep Secrets in Key Vault

Never commit hardcoded connection strings or credentials to Git. Instead:
- Store secrets in Azure Key Vault
- Reference via ADF linked services
- Let managed identity pull secrets at runtime

### 4. Review Exports Before Committing

When the export/import workflow commits exported definitions:
- Review the Git diff to understand what changed
- Check for unintended modifications
- Comment on PR if something looks wrong

### 5. Test in Lower Environments First

Always test in development/test before production:
1. Create pipeline in dev ADF UI
2. Publish in ADF UI (commits to Git)
3. Trigger validation and code deployment workflow
4. Verify definitions reach test environment
5. Only then proceed to production

## Related Documentation

- [Terraform Modules - ADF](../../terraform-modules/aks/azure_data_factory/README.md)
- [GitHub Workflows Documentation](.github/workflows/README.md)
- [Authentication Setup](../docs/AUTHENTICATION.md)
- [Azure Data Factory Documentation](https://learn.microsoft.com/en-us/azure/data-factory/)

## Support

For issues or questions:

1. Check the [Troubleshooting](#troubleshooting) section
2. Review GitHub Actions workflow logs
3. Check Azure Data Factory activity log
4. Contact the platform team

## License

See [LICENSE](../LICENSE) for details.
