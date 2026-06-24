using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentProprietor
{
    public long EstablishmentProprietorId { get; set; }

    public long EstablishmentId { get; set; }

    public string? ProprietorName { get; set; }

    public string? ProprietorType { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
