using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentAggregate
{
    public long EstablishmentAggregateId { get; set; }

    public string Urn { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? EstablishmentNumber { get; set; }

    public int? StatusCode { get; set; }

    public DateOnly? StatusDate { get; set; }

    public string? EstablishmentTypeName { get; set; }

    public string? EducationPhaseName { get; set; }

    public DateOnly? OpenedDate { get; set; }

    public DateOnly? ClosedDate { get; set; }

    public long? GroupUid { get; set; }

    public string? GroupCode { get; set; }

    public string? GroupName { get; set; }

    public string? GroupTypeName { get; set; }

    public DateOnly? GroupOpenDate { get; set; }

    public string? SiteName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Town { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }

    public int? LocalAuthorityCode { get; set; }

    public string? LocalAuthorityName { get; set; }

    public int? StatutoryLowAge { get; set; }

    public int? StatutoryHighAge { get; set; }

    public string? Gender { get; set; }

    public string? ReligiousCharacter { get; set; }

    public DateOnly? OfstedInspectionDate { get; set; }

    public string? OfstedReportUrl { get; set; }

    public string? HeadteacherIdentifier { get; set; }

    public string? HeadteacherName { get; set; }

    public string? SenProvision { get; set; }

    public string? Website { get; set; }

    public string? TelephoneNumber { get; set; }
}
