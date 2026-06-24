using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class Establishment
{
    public long EstablishmentId { get; set; }

    public string? Urn { get; set; }

    public string? Uid { get; set; }

    public string Name { get; set; } = null!;

    public string? EstablishmentNumber { get; set; }

    public long EstablishmentTypeId { get; set; }

    public long EstablishmentStatusId { get; set; }

    public long? HeadteacherRoleAssignmentId { get; set; }

    public virtual ICollection<Contact> Contact { get; set; } = new List<Contact>();

    public virtual EstablishmentAdmissions? EstablishmentAdmissions { get; set; }

    public virtual EstablishmentAlternativeProvision? EstablishmentAlternativeProvision { get; set; }

    public virtual ICollection<EstablishmentAuthority> EstablishmentAuthority { get; set; } = new List<EstablishmentAuthority>();

    public virtual EstablishmentBoarding? EstablishmentBoarding { get; set; }

    public virtual ICollection<EstablishmentGroupMembership> EstablishmentGroupMembership { get; set; } = new List<EstablishmentGroupMembership>();

    public virtual ICollection<EstablishmentIdentifier> EstablishmentIdentifier { get; set; } = new List<EstablishmentIdentifier>();

    public virtual ICollection<EstablishmentInspection> EstablishmentInspection { get; set; } = new List<EstablishmentInspection>();

    public virtual ICollection<EstablishmentLifecycleEvent> EstablishmentLifecycleEvent { get; set; } = new List<EstablishmentLifecycleEvent>();

    public virtual ICollection<EstablishmentProprietor> EstablishmentProprietor { get; set; } = new List<EstablishmentProprietor>();

    public virtual EstablishmentProvision? EstablishmentProvision { get; set; }

    public virtual ICollection<EstablishmentReligion> EstablishmentReligion { get; set; } = new List<EstablishmentReligion>();

    public virtual EstablishmentSen? EstablishmentSen { get; set; }

    public virtual ICollection<EstablishmentSenNeed> EstablishmentSenNeed { get; set; } = new List<EstablishmentSenNeed>();

    public virtual EstablishmentStatus EstablishmentStatus { get; set; } = null!;

    public virtual ICollection<EstablishmentStatusHistory> EstablishmentStatusHistory { get; set; } = new List<EstablishmentStatusHistory>();

    public virtual EstablishmentType EstablishmentType { get; set; } = null!;

    public virtual RoleAssignment? HeadteacherRoleAssignment { get; set; }

    public virtual ICollection<RoleAssignment> RoleAssignment { get; set; } = new List<RoleAssignment>();

    public virtual ICollection<Site> Site { get; set; } = new List<Site>();
}
