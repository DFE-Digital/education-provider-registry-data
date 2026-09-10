namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public class SearchAggregate
{
    public string ProviderId { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string? LaEstab { get; set; } = string.Empty;
    public string ProviderTypeName { get; set; } = string.Empty;
    public int ProviderTypeId { get; set; }
    public string? ProviderAddress { get; set; } = string.Empty;
    public string? CompaniesHouseNumber { get; set; } = string.Empty;
    public string? UkProviderReferenceNumber { get; set; } = string.Empty;
    public string? Postcode { get; set; } = string.Empty;
    public string? County { get; set; } = string.Empty;
    public string? Town { get; set; } = string.Empty;
    public string? LocalAuthorityName { get; set; } = string.Empty;
    public string? GroupId { get; set; } = string.Empty;
    public int? AcademyCounts { get; set; }
    public string ProviderCategory { get; set; } = string.Empty;
}