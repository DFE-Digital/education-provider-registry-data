output "url" {
  value = module.web_application.url
}

output "external_urls" {
  value = [
    module.web_application.url
  ]
# output "external_urls" {
#   value = [
#     module.web_application.url
#   ]
# }
# output "external_urls" {
#   value = [
#     module.web_application.url
#   ]
# }

# Azure Data Factory outputs
output "adf_id" {
  value       = try(module.azure_data_factory.data_factory_id, null)
  description = "The ID of the Azure Data Factory"
}

output "adf_name" {
  value       = try(module.azure_data_factory.data_factory_name, null)
  description = "The name of the Azure Data Factory"
}

output "adf_principal_id" {
  value       = try(module.azure_data_factory.data_factory_principal_id, null)
  description = "The principal ID of the Azure Data Factory managed identity"
  sensitive   = true
}
