using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentStatusHistory
{
    public long EstablishmentStatusHistoryId { get; set; }

    public long EstablishmentId { get; set; }

    public long? OldStatusId { get; set; }

    public long NewStatusId { get; set; }

    public DateTime ChangedAt { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;

    public virtual EstablishmentStatus NewStatus { get; set; } = null!;

    public virtual EstablishmentStatus? OldStatus { get; set; }
}
