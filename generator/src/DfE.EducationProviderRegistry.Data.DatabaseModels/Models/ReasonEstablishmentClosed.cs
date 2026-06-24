using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class ReasonEstablishmentClosed
{
    public long ReasonEstablishmentClosedId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<EstablishmentLifecycleEvent> EstablishmentLifecycleEvent { get; set; } = new List<EstablishmentLifecycleEvent>();
}
