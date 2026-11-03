using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class SearchAggregate
{
    public long SearchAggregateId { get; set; }

    public string? ProviderId { get; set; }

    public string? ProviderName { get; set; }

    public string? LaEstab { get; set; }

    public string? DfeNumber { get; set; }

    public string? ProviderTypeName { get; set; }

    public string? ProviderAddress { get; set; }

    public string? CompaniesHouseNumber { get; set; }

    public string? UkProviderReferenceNumber { get; set; }

    public string? Postcode { get; set; }

    public string? County { get; set; }

    public string? Town { get; set; }

    public string? LocalAuthorityName { get; set; }

    public string? Status { get; set; }

    public string? EducationPhase { get; set; }

    public string? FurtherEducationType { get; set; }

    public string? Admissions { get; set; }

    public string? Boarding { get; set; }

    public string? Nursery { get; set; }

    public string? Gender { get; set; }

    public int? AgeLow { get; set; }

    public int? AgeHigh { get; set; }

    public string? GroupName { get; set; }

    public string? GroupId { get; set; }

    public string? GroupUid { get; set; }

    public int? GroupStatus { get; set; }

    public string? GroupType { get; set; }

    public int? AcademyCounts { get; set; }

    public DateOnly? OpenedDate { get; set; }

    public DateOnly? ClosedDate { get; set; }

    public string? ProviderCategory { get; set; }

    public List<string>? Sen { get; set; }

    public string? Diocese { get; set; }

    public string? ReligiousCharacter { get; set; }

    public string? ReligiousEthos { get; set; }

    public string? Section41 { get; set; }

    public string? SpecialClass { get; set; }
}
