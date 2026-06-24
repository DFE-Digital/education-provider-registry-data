using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentStatus
{
    public long EstablishmentStatusId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Establishment> Establishment { get; set; } = new List<Establishment>();

    public virtual ICollection<EstablishmentStatusHistory> EstablishmentStatusHistoryNewStatus { get; set; } = new List<EstablishmentStatusHistory>();

    public virtual ICollection<EstablishmentStatusHistory> EstablishmentStatusHistoryOldStatus { get; set; } = new List<EstablishmentStatusHistory>();
}
