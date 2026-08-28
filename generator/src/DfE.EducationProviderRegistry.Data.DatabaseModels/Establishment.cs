using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class Establishment
{
    public long EstablishmentId { get; set; }

    public string? Urn { get; set; }

    public string? Uid { get; set; }

    public string Name { get; set; } = null!;

    public string? EstablishmentNumber { get; set; }

    public string? Laestab { get; set; }

    public string? DfeNumber { get; set; }

    public long EstablishmentTypeId { get; set; }

    public long EstablishmentStatusId { get; set; }

    public long? HeadteacherRoleAssignmentId { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual EstablishmentAdmission? EstablishmentAdmission { get; set; }

    public virtual EstablishmentAlternativeProvision? EstablishmentAlternativeProvision { get; set; }

    public virtual ICollection<EstablishmentAuthority> EstablishmentAuthorities { get; set; } = new List<EstablishmentAuthority>();

    public virtual EstablishmentBoarding? EstablishmentBoarding { get; set; }

    public virtual ICollection<EstablishmentGroupMembership> EstablishmentGroupMemberships { get; set; } = new List<EstablishmentGroupMembership>();

    public virtual ICollection<EstablishmentIdentifier> EstablishmentIdentifiers { get; set; } = new List<EstablishmentIdentifier>();

    public virtual ICollection<EstablishmentInspection> EstablishmentInspections { get; set; } = new List<EstablishmentInspection>();

    public virtual ICollection<EstablishmentLifecycleEvent> EstablishmentLifecycleEvents { get; set; } = new List<EstablishmentLifecycleEvent>();

    public virtual ICollection<EstablishmentProprietor> EstablishmentProprietors { get; set; } = new List<EstablishmentProprietor>();

    public virtual EstablishmentProvision? EstablishmentProvision { get; set; }

    public virtual ICollection<EstablishmentReligion> EstablishmentReligions { get; set; } = new List<EstablishmentReligion>();

    public virtual EstablishmentSen? EstablishmentSen { get; set; }

    public virtual ICollection<EstablishmentSenNeed> EstablishmentSenNeeds { get; set; } = new List<EstablishmentSenNeed>();

    public virtual EstablishmentStatus EstablishmentStatus { get; set; } = null!;

    public virtual ICollection<EstablishmentStatusHistory> EstablishmentStatusHistories { get; set; } = new List<EstablishmentStatusHistory>();

    public virtual EstablishmentType EstablishmentType { get; set; } = null!;

    public virtual RoleAssignment? HeadteacherRoleAssignment { get; set; }

    public virtual ICollection<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();

    public virtual ICollection<Site> Sites { get; set; } = new List<Site>();
}
