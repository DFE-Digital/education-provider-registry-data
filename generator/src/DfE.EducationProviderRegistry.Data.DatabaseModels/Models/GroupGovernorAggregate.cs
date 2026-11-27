using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupGovernorAggregate
{
    public long GroupGovernorAggregateId { get; set; }

    public long GroupUid { get; set; }

    public string GovernorId { get; set; } = null!;

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public int RoleOrder { get; set; }

    public string? AppointingBody { get; set; }

    public DateOnly AppointmentStartDate { get; set; }

    public DateOnly? AppointmentEndDate { get; set; }
}
