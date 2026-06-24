using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class Role
{
    public long RoleId { get; set; }

    public long PersonId { get; set; }

    public long RoleTypeId { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<RoleAssignment> RoleAssignment { get; set; } = new List<RoleAssignment>();

    public virtual RoleType RoleType { get; set; } = null!;
}
