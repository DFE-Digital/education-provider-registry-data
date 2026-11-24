using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupEstablishmentAggregate
{
    public long GroupEstablishmentAggregateId { get; set; }

    public string GroupId { get; set; } = null!;

    public string EstablishmentUrn { get; set; } = null!;

    public string EstablishmentName { get; set; } = null!;
}
