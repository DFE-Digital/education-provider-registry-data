using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentAlternativeProvision
{
    public long EstablishmentAlternativeProvisionId { get; set; }

    public long EstablishmentId { get; set; }

    public string? AlternativeProvisionType { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
