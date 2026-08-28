using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class RoleAssignment
{
    public long RoleAssignmentId { get; set; }

    public long RoleId { get; set; }

    public long? EstablishmentId { get; set; }

    public long? GroupId { get; set; }

    public string? PreferredJobTitle { get; set; }

    public virtual Establishment? Establishment { get; set; }

    public virtual ICollection<Establishment> Establishments { get; set; } = new List<Establishment>();

    public virtual GroupRecord? Group { get; set; }

    public virtual ICollection<GroupRecord> GroupRecords { get; set; } = new List<GroupRecord>();

    public virtual Role Role { get; set; } = null!;
}
