using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class SearchAggregate
{
    public string? ProviderId { get; set; }

    public string? ProviderName { get; set; }

    public string? LaEstab { get; set; }

    public string? ProviderTypeName { get; set; }

    public long? ProviderTypeId { get; set; }

    public string? ProviderAddress { get; set; }

    public string? CompaniesHouseNumber { get; set; }

    public string? UkProviderReferenceNumber { get; set; }

    public string? Postcode { get; set; }

    public string? County { get; set; }

    public string? Town { get; set; }

    public string? LocalAuthorityName { get; set; }

    public string? GroupId { get; set; }

    public string? GroupUid { get; set; }

    public int? AcademyCounts { get; set; }

    public string? ProviderCategory { get; set; }
}
