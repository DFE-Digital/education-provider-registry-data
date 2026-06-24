using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentProvision
{
    public long EstablishmentProvisionId { get; set; }

    public long EstablishmentId { get; set; }

    public long? EducationPhaseId { get; set; }

    public long? NurseryProvisionId { get; set; }

    public long? OfficialSixthFormId { get; set; }

    public long? FurtherEducationTypeId { get; set; }

    public int? Fsm { get; set; }

    public decimal? PercentageFsm { get; set; }

    public virtual EducationPhase? EducationPhase { get; set; }

    public virtual Establishment Establishment { get; set; } = null!;
}
