using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentGroupMembership
{
    public long EstablishmentGroupMembershipId { get; set; }

    public long EstablishmentId { get; set; }

    public long GroupId { get; set; }

    public string? MembershipCategory { get; set; }

    public string? MembershipFlag { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;

    public virtual GroupRecord Group { get; set; } = null!;
}
