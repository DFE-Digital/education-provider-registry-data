module "data_factory" {
  count  = var.enable_adf ? 1 : 0
  source = "./vendor/modules/aks/azure/azure_data_factory"

  environment             = var.environment
  azure_resource_prefix   = var.azure_resource_prefix
  service_name            = var.service_name
  service_short           = var.service_short
  config_short            = var.config_short
  azure_enable_monitoring = false
}