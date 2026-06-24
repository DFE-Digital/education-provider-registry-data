using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class Title
{
    public long TitleId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Person> Person { get; set; } = new List<Person>();
}
