using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentInspection
{
    public long EstablishmentInspectionId { get; set; }

    public long EstablishmentId { get; set; }

    public string? InspectionBody { get; set; }

    public DateOnly? InspectionDate { get; set; }

    public string? InspectionOutcome { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
