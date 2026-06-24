using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupType
{
    public long GroupTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<GroupRecord> GroupRecord { get; set; } = new List<GroupRecord>();
}
