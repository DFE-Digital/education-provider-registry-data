using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class EstablishmentBoarding
{
    public long EstablishmentBoardingId { get; set; }

    public long EstablishmentId { get; set; }

    public string? BoardingProvision { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
