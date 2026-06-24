using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentLifecycleEvent
{
    public long EstablishmentLifecycleEventId { get; set; }

    public long EstablishmentId { get; set; }

    public string EventType { get; set; } = null!;

    public long? OpenedReasonId { get; set; }

    public long? ClosedReasonId { get; set; }

    public DateOnly EventDate { get; set; }

    public virtual ReasonEstablishmentClosed? ClosedReason { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;

    public virtual ReasonEstablishmentOpened? OpenedReason { get; set; }
}
