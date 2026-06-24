using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class Person
{
    public long PersonId { get; set; }

    public long? TitleId { get; set; }

    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    public string? DisplayName { get; set; }

    public virtual ICollection<Role> Role { get; set; } = new List<Role>();

    public virtual Title? Title { get; set; }
}
