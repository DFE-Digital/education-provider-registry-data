using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentSenNeed
{
    public long EstablishmentSenNeedId { get; set; }

    public long EstablishmentId { get; set; }

    public string SenNeedType { get; set; } = null!;

    public virtual Establishment Establishment { get; set; } = null!;
}
