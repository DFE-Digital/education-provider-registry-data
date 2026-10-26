using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupAggregate
{
    public long GroupAggregateId { get; set; }

    public string GroupId { get; set; } = null!;

    public long GroupUid { get; set; }

    public string? Ukprn { get; set; }

    public string? CompaniesHouseNumber { get; set; }

    public string Name { get; set; } = null!;

    public string? GroupTypeName { get; set; }

    public string? HeadteacherIdentifier { get; set; }

    public string? HeadteacherName { get; set; }

    public string? ReligiousCharacter { get; set; }

    public string? Website { get; set; }

    public string? TelephoneNumber { get; set; }

    public string? SiteName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Town { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }

    public string? GroupStatusLabel { get; set; }

    public DateOnly? GroupStatusEffectiveDate { get; set; }
}
