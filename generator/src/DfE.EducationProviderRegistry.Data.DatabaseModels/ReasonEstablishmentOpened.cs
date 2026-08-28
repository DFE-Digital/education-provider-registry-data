using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class ReasonEstablishmentOpened
{
    public long ReasonEstablishmentOpenedId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<EstablishmentLifecycleEvent> EstablishmentLifecycleEvents { get; set; } = new List<EstablishmentLifecycleEvent>();
}
