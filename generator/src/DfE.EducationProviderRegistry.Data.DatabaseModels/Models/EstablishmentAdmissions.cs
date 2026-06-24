using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentAdmissions
{
    public long EstablishmentAdmissionsId { get; set; }

    public long EstablishmentId { get; set; }

    public string? AdmissionsPolicy { get; set; }

    public int? StatutoryLowAge { get; set; }

    public int? StatutoryHighAge { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
