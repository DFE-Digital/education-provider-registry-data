using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class GroupType
{
    public long GroupTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<GroupRecord> GroupRecords { get; set; } = new List<GroupRecord>();
}
