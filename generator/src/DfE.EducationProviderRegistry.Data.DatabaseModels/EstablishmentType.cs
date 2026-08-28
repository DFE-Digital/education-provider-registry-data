using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class EstablishmentType
{
    public long EstablishmentTypeId { get; set; }

    public long EstablishmentFamilyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsSchool { get; set; }

    public bool IsGroup { get; set; }

    public bool IsEarlyYears { get; set; }

    public bool IsFurtherEducation { get; set; }

    public virtual EstablishmentFamily EstablishmentFamily { get; set; } = null!;

    public virtual ICollection<Establishment> Establishments { get; set; } = new List<Establishment>();
}
