using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class Contact
{
    public long ContactId { get; set; }

    public long? EstablishmentId { get; set; }

    public long? GroupId { get; set; }

    public string? Website { get; set; }

    public string? TelephoneNumber { get; set; }

    public virtual Establishment? Establishment { get; set; }

    public virtual GroupRecord? Group { get; set; }
}
