module "azure_data_factory" {
  source = "../../../terraform-modules/azure/azure_data_factory"

  azure_resource_prefix = var.azure_resource_prefix
  environment           = var.environment
  service_name         = var.service_name
  service_short         = var.service_short
  config_short          = var.config_short
  
  # Git repository configuration - only enabled for development environment
  git_repository = var.environment == "development" ? {
    repository_name    = "education-provider-registry-data"
    branch_name        = "main"      
  } : null

  git_enabled_environment = "development"
}
