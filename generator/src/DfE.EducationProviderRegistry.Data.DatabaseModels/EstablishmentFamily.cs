using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class EstablishmentFamily
{
    public long EstablishmentFamilyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EstablishmentType> EstablishmentTypes { get; set; } = new List<EstablishmentType>();
}
