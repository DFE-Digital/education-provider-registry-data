using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentGovernorAggregate
{
    public long EstablishmentGovernorAggregateId { get; set; }

    public string EstablishmentUrn { get; set; } = null!;

    public string GovernorId { get; set; } = null!;

    public string GovernorName { get; set; } = null!;

    public DateOnly? StartDate { get; set; }
}
