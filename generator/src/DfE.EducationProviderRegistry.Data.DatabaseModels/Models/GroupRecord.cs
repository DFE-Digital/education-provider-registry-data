using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class GroupRecord
{
    public long GroupId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public long GroupTypeId { get; set; }

    public long? HeadteacherRoleAssignmentId { get; set; }

    public virtual ICollection<Contact> Contact { get; set; } = new List<Contact>();

    public virtual ICollection<EstablishmentGroupMembership> EstablishmentGroupMembership { get; set; } = new List<EstablishmentGroupMembership>();

    public virtual ICollection<EstablishmentReligion> EstablishmentReligion { get; set; } = new List<EstablishmentReligion>();

    public virtual ICollection<GroupIdentifier> GroupIdentifier { get; set; } = new List<GroupIdentifier>();

    public virtual GroupType GroupType { get; set; } = null!;

    public virtual RoleAssignment? HeadteacherRoleAssignment { get; set; }

    public virtual ICollection<RoleAssignment> RoleAssignment { get; set; } = new List<RoleAssignment>();

    public virtual ICollection<GroupRecord> ChildGroup { get; set; } = new List<GroupRecord>();

    public virtual ICollection<GroupRecord> ParentGroup { get; set; } = new List<GroupRecord>();
}
