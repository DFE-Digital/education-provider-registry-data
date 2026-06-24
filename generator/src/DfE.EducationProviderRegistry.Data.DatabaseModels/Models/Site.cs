using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class Site
{
    public long SiteId { get; set; }

    public long EstablishmentId { get; set; }

    public string? Name { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? Town { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
