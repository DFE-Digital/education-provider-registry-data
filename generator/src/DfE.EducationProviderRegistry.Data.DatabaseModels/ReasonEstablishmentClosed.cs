using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class ReasonEstablishmentClosed
{
    public long ReasonEstablishmentClosedId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<EstablishmentLifecycleEvent> EstablishmentLifecycleEvents { get; set; } = new List<EstablishmentLifecycleEvent>();
}
