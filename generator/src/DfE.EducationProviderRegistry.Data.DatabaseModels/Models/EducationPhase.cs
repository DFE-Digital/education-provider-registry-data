using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EducationPhase
{
    public long EducationPhaseId { get; set; }

    public long EducationPhaseGroupId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual EducationPhaseGroup EducationPhaseGroup { get; set; } = null!;

    public virtual ICollection<EstablishmentProvision> EstablishmentProvision { get; set; } = new List<EstablishmentProvision>();
}
