using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupIdentifier
{
    public long GroupIdentifierId { get; set; }

    public long GroupId { get; set; }

    public string IdentifierType { get; set; } = null!;

    public string IdentifierValue { get; set; } = null!;

    public virtual GroupRecord Group { get; set; } = null!;
}
