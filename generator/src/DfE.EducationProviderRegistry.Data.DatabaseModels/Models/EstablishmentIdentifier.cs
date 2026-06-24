using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentIdentifier
{
    public long EstablishmentIdentifierId { get; set; }

    public long EstablishmentId { get; set; }

    public string IdentifierType { get; set; } = null!;

    public string IdentifierValue { get; set; } = null!;

    public virtual Establishment Establishment { get; set; } = null!;
}
