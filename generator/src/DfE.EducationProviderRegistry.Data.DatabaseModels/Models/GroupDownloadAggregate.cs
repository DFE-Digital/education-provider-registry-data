using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupDownloadAggregate
{
    public long GroupDownloadAggregateId { get; set; }

    public string? GroupId { get; set; }

    public long? GroupUid { get; set; }

    public string? GroupName { get; set; }

    public int? GroupTypeId { get; set; }

    public string? GroupType { get; set; }

    public int? GroupStatusId { get; set; }

    public string? GroupStatus { get; set; }

    public DateOnly? OpenDate { get; set; }

    public DateOnly? ClosedDate { get; set; }

    public DateOnly? IncorporatedOn { get; set; }

    public string? CompaniesHouseNumber { get; set; }

    public string? UkProviderReferenceNumber { get; set; }

    public string? HeadOfGroupTitle { get; set; }

    public string? HeadOfGroupFirstName { get; set; }

    public string? HeadOfGroupLastName { get; set; }

    public string? GroupAddressLine1 { get; set; }

    public string? GroupAddressLine2 { get; set; }

    public string? GroupAddressLine3 { get; set; }

    public string? GroupTownOrCity { get; set; }

    public string? GroupCounty { get; set; }

    public string? GroupPostcode { get; set; }
}
