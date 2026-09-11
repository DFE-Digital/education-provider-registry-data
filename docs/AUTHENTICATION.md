# Azure Data Factory CI/CD - Authentication Setup

This guide explains how to set up authentication for the ADF CI/CD workflows.

## Overview

The ADF CI/CD system uses Azure Workload Identity Federation (OIDC) for GitHub Actions to authenticate with Azure without storing long-lived secrets. This is the recommended approach for GitHub Actions.

## Prerequisites

- Azure subscription access
- Azure CLI installed locally
- GitHub repository admin access
- Service Principal creation permissions in Azure AD

## Setup Steps

### 1. Create Azure Service Principal

Create a service principal that will be used by GitHub Actions:

```bash
# Set variables
SUBSCRIPTION_ID="your-subscription-id"
SERVICE_PRINCIPAL_NAME="gha-eprdat-adf-ci-cd"
RESOURCE_GROUP="your-resource-group"

# Create service principal
az ad sp create-for-rbac \
  --name "$SERVICE_PRINCIPAL_NAME" \
  --role "Contributor" \
  --scopes "/subscriptions/$SUBSCRIPTION_ID"
```

This will output:
```json
{
  "appId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "displayName": "gha-eprdat-adf-ci-cd",
  "password": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "tenant": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
}
```

**Note**: The `appId` is your `AZURE_CLIENT_ID` and `tenant` is your `AZURE_TENANT_ID`.

### 2. Configure Workload Identity Federation

Set up federated credentials so GitHub Actions can authenticate without a password:

```bash
# Get your GitHub details
GITHUB_ORG="DFE-Digital"
GITHUB_REPO="education-provider-registry-data"
SERVICE_PRINCIPAL_CLIENT_ID="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"  # From appId above

# Add federated credential for main branch
az ad app federated-credential create \
  --id "$SERVICE_PRINCIPAL_CLIENT_ID" \
  --parameters '{
    "name": "gha-main-branch",
    "issuer": "https://token.actions.githubusercontent.com",
    "subject": "repo:'"$GITHUB_ORG"'/'"$GITHUB_REPO"':ref:refs/heads/main",
    "audiences": ["api://AzureADTokenExchange"],
    "description": "GitHub Actions for main branch"
  }'

# Add federated credential for pull requests (optional, for validation)
az ad app federated-credential create \
  --id "$SERVICE_PRINCIPAL_CLIENT_ID" \
  --parameters '{
    "name": "gha-pr-validation",
    "issuer": "https://token.actions.githubusercontent.com",
    "subject": "repo:'"$GITHUB_ORG"'/'"$GITHUB_REPO"':pull_request",
    "audiences": ["api://AzureADTokenExchange"],
    "description": "GitHub Actions for PR validation"
  }'
```

### 3. Configure Service Principal Roles

Grant the service principal the necessary permissions:

#### For Each ADF Resource

```bash
# Development
az role assignment create \
  --role "Data Factory Contributor" \
  --assignee-object-id "$(az ad sp show --id xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx --query objectId -o tsv)" \
  --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/s189t01-eprdat-dv-rg/providers/Microsoft.DataFactory/factories/s189t01-eprdat-development-adf"

# Test
az role assignment create \
  --role "Data Factory Contributor" \
  --assignee-object-id "$(az ad sp show --id xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx --query objectId -o tsv)" \
  --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/s189t01-eprdat-t-rg/providers/Microsoft.DataFactory/factories/s189t01-eprdat-test-adf"

# Production
az role assignment create \
  --role "Data Factory Contributor" \
  --assignee-object-id "$(az ad sp show --id xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx --query objectId -o tsv)" \
  --scope "/subscriptions/s189-teacher-services-cloud-prod/resourceGroups/s189p01-eprdat-pd-rg/providers/Microsoft.DataFactory/factories/s189p01-eprdat-prod-adf"
```

#### For Terraform Backend Storage

The service principal also needs access to the Terraform state storage account:

```bash
# Development/Test backend
az role assignment create \
  --role "Storage Blob Data Contributor" \
  --assignee-object-id "$(az ad sp show --id xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx --query objectId -o tsv)" \
  --scope "/subscriptions/$SUBSCRIPTION_ID/resourceGroups/s189t01-eprdat-dv-rg/providers/Microsoft.Storage/storageAccounts/s189t01eprdata.tfsa"

# Production backend (if using different subscription)
az role assignment create \
  --role "Storage Blob Data Contributor" \
  --assignee-object-id "$(az ad sp show --id xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx --query objectId -o tsv)" \
  --scope "/subscriptions/s189-teacher-services-cloud-prod/resourceGroups/s189p01-eprdat-pd-rg/providers/Microsoft.Storage/storageAccounts/s189p01eprdata.tfsa"
```

### 4. Add Secrets to GitHub Repository

Add the following secrets to your GitHub repository:

**Settings > Secrets and Variables > Actions**

| Secret Name | Value | Notes |
|---|---|---|
| `AZURE_CLIENT_ID` | Service Principal client ID (appId) | From step 1 |
| `AZURE_SUBSCRIPTION_ID` | Your Azure subscription ID | Can be different per environment |
| `AZURE_TENANT_ID` | Azure AD tenant ID (tenant) | From step 1 |

**Optional - for Teams notifications**:
| Secret Name | Value | Notes |
|---|---|---|
| `TEAMS_WEBHOOK_URL` | Teams incoming webhook URL | For deployment notifications |

### 5. Verify Setup

Test the authentication by running a workflow manually:

```bash
# Manually trigger the validation workflow
gh workflow run adf-validate.yml --ref main

# Check the workflow result
gh run list --workflow=adf-validate.yml --limit=5
```

Or directly in GitHub Actions:
1. Go to Actions tab
2. Select "ADF - Validate on PR"
3. Click "Run workflow" → "Run workflow"
4. Check the logs

## Permissions Summary

### Service Principal Roles Required

| Resource | Role | Scope |
|---|---|---|
| Azure Data Factory (dev, test, prod) | Data Factory Contributor | Each ADF resource |
| Terraform State Storage | Storage Blob Data Contributor | Storage account |
| Resource Groups | Reader | Resource groups (for `az` queries) |

### GitHub Token Permissions

The workflows use the built-in `GITHUB_TOKEN` which is automatically provided with:
- `contents: read` - For reading repository files
- `contents: write` - For committing exported definitions (deploy workflow)
- `id-token: write` - For OIDC token exchange with Azure

## Troubleshooting

### "This request is not authorized" Error

**Cause**: Service principal doesn't have the required permissions

**Solution**:
```bash
# List current role assignments
az role assignment list --assignee-object-id "YOUR_SP_ID" --output table

# If missing, add the Data Factory Contributor role
az role assignment create \
  --role "Data Factory Contributor" \
  --assignee-object-id "YOUR_SP_ID" \
  --scope "/subscriptions/YOUR_SUB_ID/resourceGroups/YOUR_RG/providers/Microsoft.DataFactory/factories/YOUR_ADF"
```

### "Credentials could not be automatically detected" Error

**Cause**: OIDC token not being exchanged correctly

**Solution**:
1. Verify federated credentials are configured
2. Check GitHub repository name matches exactly (case-sensitive)
3. Verify the workflow is running from the correct branch/PR

```bash
# List federated credentials
az ad app federated-credential list --id "YOUR_SP_CLIENT_ID"
```

### "Access Denied" to Storage Account

**Cause**: Service principal doesn't have storage permissions

**Solution**:
```bash
# Grant Storage Blob Data Contributor role
az role assignment create \
  --role "Storage Blob Data Contributor" \
  --assignee-object-id "YOUR_SP_ID" \
  --scope "/subscriptions/YOUR_SUB_ID/resourceGroups/YOUR_RG/providers/Microsoft.Storage/storageAccounts/YOUR_STORAGE_ACCOUNT"
```

### "Failed to retrieve federated token"

**Cause**: OIDC token endpoint unreachable or misconfigured

**Solution**:
1. Check GitHub Actions can reach `token.actions.githubusercontent.com`
2. Verify organization/repo name in federated credential exactly matches GitHub
3. Check if organization requires OIDC approval:
   ```bash
   # List GitHub connections
   az identity federated-credential list --resource-group YOUR_RG
   ```

## Security Best Practices

### 1. Use Workload Identity Federation

- ✅ Use OIDC (Workload Identity) - no long-lived secrets
- ❌ Don't store service principal password as secret

### 2. Principle of Least Privilege

- Grant only the minimum required roles
- Use resource-scoped permissions (not subscription-wide when possible)
- Review permissions quarterly

### 3. Secret Management

- Never commit credentials to Git
- Rotate OIDC configuration if compromised
- Use Key Vault for connection strings in ADF

### 4. Audit Logging

- Enable Azure audit logs: `az monitor diagnostic-settings create`
- Review workflow runs in GitHub Actions
- Set up alerts for failed deployments

### 5. Access Control

- Use GitHub branch protection rules
- Require PR reviews before merge
- Limit who can trigger `workflow_dispatch`

## Environment-Specific Configuration

If you have separate service principals per environment, update the secrets at the environment level:

**Settings > Environments**

Create environments: `development`, `test`, `production`

Then add environment-specific secrets:
- `AZURE_CLIENT_ID` (environment-specific)
- `AZURE_SUBSCRIPTION_ID` (environment-specific)
- `AZURE_TENANT_ID` (can be shared)

Update workflows to use environment context:
```yaml
jobs:
  deploy:
    environment: ${{ matrix.environment }}
    runs-on: ubuntu-latest
    env:
      AZURE_CLIENT_ID: ${{ secrets.AZURE_CLIENT_ID }}
```

## Azure CLI Reference

### Check Current Authentication

```bash
az account show
az ad signed-in-user show
```

### List Service Principals

```bash
az ad sp list --filter "displayname eq 'gha-eprdat-adf-ci-cd'" --output table
```

### Revoke Service Principal

```bash
az ad sp delete --id "YOUR_SERVICE_PRINCIPAL_ID"
```

### Reset OIDC Configuration

If you need to reconfigure OIDC:

```bash
# Remove old federated credentials
az ad app federated-credential delete --id "YOUR_SP_CLIENT_ID" --federated-credential-id "gha-main-branch"

# Add new ones (see step 2)
```

## References

- [Azure Workload Identity Federation for GitHub Actions](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure)
- [GitHub Actions - Use OIDC with Azure](https://docs.github.com/en/actions/deployment/security-hardening-your-deployments/about-security-hardening-with-openid-connect)
- [Azure Data Factory Managed Identity](https://learn.microsoft.com/en-us/azure/data-factory/data-factory-service-identity)
- [Azure RBAC](https://learn.microsoft.com/en-us/azure/role-based-access-control/overview)

## Support

For issues during setup:

1. Check the [Troubleshooting](#troubleshooting) section
2. Verify all command outputs match expectations
3. Test permissions with: `az role assignment list --assignee-object-id YOUR_SP_ID`
4. Contact the platform team with error messages
