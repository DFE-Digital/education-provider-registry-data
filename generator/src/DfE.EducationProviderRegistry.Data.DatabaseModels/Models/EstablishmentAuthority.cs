using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentAuthority
{
    public long EstablishmentAuthorityId { get; set; }

    public long EstablishmentId { get; set; }

    public string? AuthorityCode { get; set; }

    public string? AuthorityName { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
