using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class EducationPhaseGroup
{
    public long EducationPhaseGroupId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<EducationPhase> EducationPhases { get; set; } = new List<EducationPhase>();
}
