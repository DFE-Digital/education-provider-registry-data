using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentReligion
{
    public long EstablishmentReligionId { get; set; }

    public long? EstablishmentId { get; set; }

    public long? GroupId { get; set; }

    public string? ReligiousCharacter { get; set; }

    public string? ReligiousEthos { get; set; }

    public virtual Establishment? Establishment { get; set; }

    public virtual GroupRecord? Group { get; set; }
}
