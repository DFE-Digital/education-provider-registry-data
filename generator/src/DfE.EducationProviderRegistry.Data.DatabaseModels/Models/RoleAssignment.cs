using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class RoleAssignment
{
    public long RoleAssignmentId { get; set; }

    public long RoleId { get; set; }

    public long? EstablishmentId { get; set; }

    public long? GroupId { get; set; }

    public string? PreferredJobTitle { get; set; }

    public virtual ICollection<Establishment> Establishment { get; set; } = new List<Establishment>();

    public virtual Establishment? EstablishmentNavigation { get; set; }

    public virtual GroupRecord? Group { get; set; }

    public virtual ICollection<GroupRecord> GroupRecord { get; set; } = new List<GroupRecord>();

    public virtual Role Role { get; set; } = null!;
}
