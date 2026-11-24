using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupMemberAggregate
{
    public long GroupMemberAggregateId { get; set; }

    public long GroupUid { get; set; }

    public string EstablishmentUrn { get; set; } = null!;

    public string EstablishmentName { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
