using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels;

public partial class GroupRecord
{
    public long GroupId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public long GroupTypeId { get; set; }

    public long? HeadteacherRoleAssignmentId { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<EstablishmentGroupMembership> EstablishmentGroupMemberships { get; set; } = new List<EstablishmentGroupMembership>();

    public virtual ICollection<EstablishmentReligion> EstablishmentReligions { get; set; } = new List<EstablishmentReligion>();

    public virtual ICollection<GroupIdentifier> GroupIdentifiers { get; set; } = new List<GroupIdentifier>();

    public virtual GroupType GroupType { get; set; } = null!;

    public virtual RoleAssignment? HeadteacherRoleAssignment { get; set; }

    public virtual ICollection<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();

    public virtual ICollection<GroupRecord> ChildGroups { get; set; } = new List<GroupRecord>();

    public virtual ICollection<GroupRecord> ParentGroups { get; set; } = new List<GroupRecord>();
}
