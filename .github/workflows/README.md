# GitHub Actions Workflows - Azure Data Factory

This directory contains GitHub Actions workflows for Azure Data Factory (ADF) CI/CD.

## Workflow Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                   ADF CI/CD Workflow System                      │
└─────────────────────────────────────────────────────────────────┘

On Pull Request:
  PR with adf/ or terraform changes
          ↓
  adf-validate.yml (entry point)
          ↓
  validate-adf.yml (reusable)
          ├─ Terraform validation
          ├─ JSON schema validation
          ├─ Secrets detection
          └─ Naming conventions
          ↓
  ✓ Blocks merge if validation fails

On Push to Main:
  Commit to main (terraform/application/adf.tf)
          ↓
  adf-deploy-infra.yml (entry point)
          ↓
  deploy-adf-infra.yml (reusable)
          ├─ Deploy to development
          ├─ Deploy to test
          └─ Deploy to production
          ↓
  Artifacts: tfplan, outputs.json

  Commit to main (adf/factory/*, adf/linkedService/*)
          ↓
  adf-deploy-code.yml (entry point)
          ↓
  deploy-adf-code.yml (reusable)
          ├─ Export from development ADF
          ├─ Commit exports to Git
          ├─ Import to test ADF
          └─ Import to production ADF
          ↓
  ✓ Automated deployment chain
```

## Local Entry-Point Workflows

These workflows are specific to the education-provider-registry-data project.

### 1. adf-validate.yml

**Purpose**: Validate ADF changes on pull requests

**Trigger**:
- Pull request to `main` with changes to:
  - `adf/**` (any ADF definition files)
  - `terraform/application/adf.tf`
  - `terraform/application/variables.tf`
  - `terraform/application/output.tf`
  - `.github/workflows/adf-*.yml`
- `workflow_dispatch` (manual trigger)

**Status**: ✅ Required check (blocks merge if failed)

**Execution Time**: ~2-3 minutes

**What It Does**:
1. Checks out code
2. Calls `validate-adf.yml` reusable workflow
3. Validates all ADF JSON files
4. Checks for hardcoded secrets
5. Validates resource naming
6. Validates Terraform configuration

**Success Criteria**:
- All JSON files valid
- No hardcoded credentials
- Resource names follow conventions
- Terraform plan succeeds

### 2. adf-deploy-infra.yml

**Purpose**: Deploy ADF infrastructure (Terraform)

**Trigger**:
- Push to `main` with changes to:
  - `terraform/application/adf.tf`
  - `terraform/application/variables.tf`
  - `terraform/application/output.tf`
  - `terraform/application/config/*.tfvars.json`
- `workflow_dispatch` with optional environments input

**Manual Trigger Options**:
```
Environments input: "development,test,production" (default)
```

**Execution Time**: ~5-10 minutes per environment

**What It Does**:
1. Authenticate with Azure
2. For each environment (dev → test → prod):
   - Clone Terraform modules
   - Terraform init
   - Terraform plan
   - Terraform apply
   - Export outputs

**Artifacts**:
- `tfplan-{environment}` - Terraform plan file
- `tfoutputs-{environment}` - Terraform outputs (adf_name, adf_id, adf_principal_id)

**Rollback**:
```bash
# Manually run terraform destroy (not automated)
cd terraform/application
terraform init
terraform destroy -var-file "config/production.tfvars.json"
```

### 3. adf-deploy-code.yml

**Purpose**: Deploy ADF code (definitions) to all environments

**Trigger**:
- Push to `main` with changes to:
  - `adf/factory/**` (pipelines)
  - `adf/linkedService/**` (linked services)
  - `adf/managedVirtualNetwork/**` (managed network)
- `workflow_dispatch` (manual trigger)

**Execution Time**: ~5-15 minutes (depending on ADF complexity)

**What It Does**:

1. **Export Phase** (from development):
   - Authenticate with Azure
   - Export pipelines, datasets, linked services, triggers from dev ADF
   - Save as JSON files
   - Create manifest.json with export summary

2. **Commit Phase**:
   - Check for changes in exported files
   - If changed: commit to Git with workflow reference
   - If unchanged: skip commit

3. **Import to Test**:
   - Authenticate with Azure
   - Import all definitions to test ADF
   - Validate import completion

4. **Import to Production**:
   - Authenticate with Azure
   - Import all definitions to prod ADF
   - Validate import completion

**Artifacts**:
- `adf-exports` - Exported definitions (30-day retention)

**Success Summary**: Posted in workflow run summary

**Failure Handling**:
- Export failure blocks subsequent stages
- Individual import failures are logged but don't block (triggers are non-critical)
- Check workflow logs for details

## Reusable Workflows

These workflows are designed to be imported and reused in other repositories.

### validate-adf.yml

**Type**: Reusable workflow

**Location**: `.github/workflows/validate-adf.yml`

**Inputs**:
```yaml
working-directory:     # Default: "."
terraform-path:        # Default: "terraform/application"
adf-path:             # Default: "adf"
environment:          # Default: "development"
```

**Jobs**:
1. `validate-terraform` - Terraform format, validation
2. `validate-adf-json` - JSON schema and secrets
3. `validate-naming-conventions` - Resource naming
4. `summary` - Aggregates results

**Usage in Other Repos**:
```yaml
jobs:
  validate:
    uses: DFE-Digital/education-provider-registry-data/.github/workflows/validate-adf.yml@main
    with:
      working-directory: "."
      terraform-path: "path/to/terraform"
      adf-path: "path/to/adf"
```

### deploy-adf-infra.yml

**Type**: Reusable workflow

**Location**: `.github/workflows/deploy-adf-infra.yml`

**Inputs**:
```yaml
working-directory:   # Default: "."
terraform-path:      # Default: "terraform/application"
environments:        # JSON array, default: ["development", "test", "production"]
```

**Secrets Required**:
- `azure-client-id`
- `azure-subscription-id`
- `azure-tenant-id`

**Jobs**:
1. `deploy-infrastructure` (per environment) - Terraform apply
2. `summary` - Checks all deployments succeeded

**Usage in Other Repos**:
```yaml
jobs:
  deploy:
    uses: DFE-Digital/education-provider-registry-data/.github/workflows/deploy-adf-infra.yml@main
    with:
      working-directory: "./your-path"
      terraform-path: "./your-path/terraform"
      environments: '["development", "production"]'
    secrets:
      azure-client-id: ${{ secrets.AZURE_CLIENT_ID }}
      azure-subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
      azure-tenant-id: ${{ secrets.AZURE_TENANT_ID }}
```

### deploy-adf-code.yml

**Type**: Reusable workflow

**Location**: `.github/workflows/deploy-adf-code.yml`

**Inputs**:
```yaml
working-directory:   # Default: "."
adf-path:           # Default: "adf"
terraform-path:      # Default: "terraform/application"
environments:        # JSON array, default: ["development", "test", "production"]
```

**Secrets Required**:
- `azure-client-id`
- `azure-subscription-id`
- `azure-tenant-id`
- `github-token` (optional, for committing exports)

**Jobs**:
1. `export-from-dev` - Export from development
2. `commit-exports` - Commit to Git if changed
3. `deploy-to-test` - Import definitions
4. `deploy-to-production` - Import definitions
5. `summary` - Final status

**Usage in Other Repos**:
```yaml
jobs:
  deploy-code:
    uses: DFE-Digital/education-provider-registry-data/.github/workflows/deploy-adf-code.yml@main
    with:
      working-directory: "./your-path"
      adf-path: "./your-path/adf"
      terraform-path: "./your-path/terraform"
    secrets:
      azure-client-id: ${{ secrets.AZURE_CLIENT_ID }}
      azure-subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
      azure-tenant-id: ${{ secrets.AZURE_TENANT_ID }}
      github-token: ${{ secrets.GITHUB_TOKEN }}
```

## Environment Variables & Configuration

### Development Environment
- **Cluster**: test
- **Resource Group**: s189t01-eprdat-dv-rg
- **ADF Name**: s189t01-eprdat-development-adf
- **Git Integration**: ✓ Enabled
- **Terraform State**: s189t01eprdata.tfsa (development key)

### Test Environment
- **Cluster**: test
- **Resource Group**: s189t01-eprdat-t-rg
- **ADF Name**: s189t01-eprdat-test-adf
- **Git Integration**: ✗ Disabled
- **Terraform State**: s189t01eprdata.tfsa (test key)

### Production Environment
- **Cluster**: production
- **Resource Group**: s189p01-eprdat-pd-rg
- **ADF Name**: s189p01-eprdat-prod-adf
- **Git Integration**: ✗ Disabled
- **Terraform State**: s189p01eprdata.tfsa (production key)
- **Subscription**: s189-teacher-services-cloud-prod

## Troubleshooting Workflows

### Check Workflow Status

```bash
# List recent workflow runs
gh run list --workflow=adf-validate.yml --limit=10

# View specific run details
gh run view RUN_ID

# View run logs
gh run view RUN_ID --log
```

### Debug Workflow Failures

1. **Check the workflow logs**:
   - Go to Actions tab
   - Click on the failed run
   - Expand the step that failed
   - Read error messages

2. **Common failures**:
   - ❌ Terraform validation: Check `terraform validate` output
   - ❌ JSON validation: Run `jq empty file.json` locally
   - ❌ Secrets detected: Review hardcoded values
   - ❌ Azure auth: Check service principal permissions

3. **Re-run workflow**:
   ```bash
   gh run rerun RUN_ID
   ```

4. **Download artifacts for inspection**:
   ```bash
   gh run download RUN_ID -n tfplan-development
   ```

### Manual Troubleshooting

```bash
# Export ADF locally for testing
./adf/scripts/export.sh \
  --adf-name "s189t01-eprdat-development-adf" \
  --resource-group "s189t01-eprdat-dv-rg" \
  --environment "development"

# Validate Terraform
cd terraform/application
terraform init -backend=false
terraform validate
terraform fmt -check

# Validate JSON
find adf -name "*.json" -exec jq empty {} \;
```

## Performance Optimization

### Reduce Workflow Duration

1. **Parallel Environments** (if not dependent):
   - Change `max-parallel` in workflow from 1 to 2-3
   - ⚠️ Requires separate subscriptions per environment

2. **Cache Terraform Modules**:
   ```yaml
   - uses: actions/cache@v4
     with:
       path: terraform/application/vendor
       key: terraform-modules-${{ hashFiles('terraform/application/.terraform.lock.hcl') }}
   ```

3. **Shallow Clone**:
   ```yaml
   - uses: actions/checkout@v6
     with:
       fetch-depth: 1  # Only latest commit
   ```

### Cost Optimization

- Workflow execution minutes count toward free tier (2000 min/month)
- Use `workflow_dispatch` for expensive operations
- Clean up old artifacts (set retention-days)
- Use smaller runners if appropriate

## Security Considerations

### Workflow Permissions

- `id-token: write` - Required for OIDC token exchange
- `contents: read` - Safe, read-only access
- `contents: write` - Only needed for commit-exports job

### Secret Protection

- Secrets not shown in logs (GitHub masks them)
- Review exported definitions for secrets before commit
- Use Key Vault for production connection strings

### Branch Protection

Add required status checks:
```
Settings > Branches > Branch Protection Rules > main
- Require status checks: adf-validate
- Require branches to be up to date
- Require code reviews
```

## Related Documentation

- [ADF CI/CD Guide](../adf/README.md)
- [Authentication Setup](../docs/AUTHENTICATION.md)
- [Terraform Modules - ADF](../../terraform-modules/aks/azure_data_factory/README.md)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Azure Data Factory Documentation](https://learn.microsoft.com/en-us/azure/data-factory/)

## Support

For workflow issues:
1. Check GitHub Actions logs
2. Review this documentation
3. Check Azure CLI command output
4. Contact the platform team

## Version History

- **v1.0** (2026-09-09)
  - Initial release
  - Validate, deploy infrastructure, deploy code workflows
  - Support for dev → test → prod environments
  - Workload Identity Federation authentication
