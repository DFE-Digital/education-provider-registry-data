using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentSen
{
    public long EstablishmentSenId { get; set; }

    public long EstablishmentId { get; set; }

    public bool? HasSenProvision { get; set; }

    public string? SenProvision { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
