using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EducationPhaseGroup
{
    public long EducationPhaseGroupId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<EducationPhase> EducationPhase { get; set; } = new List<EducationPhase>();
}
