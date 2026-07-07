using System;
using System.Collections.Generic;
using DfE.EducationProviderRegistry.Data.DatabaseModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Context;

public partial class EducationProviderRegistryDbContext : DbContext
{
    public EducationProviderRegistryDbContext(DbContextOptions<EducationProviderRegistryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contact { get; set; }

    public virtual DbSet<EducationPhase> EducationPhase { get; set; }

    public virtual DbSet<EducationPhaseGroup> EducationPhaseGroup { get; set; }

    public virtual DbSet<Establishment> Establishment { get; set; }

    public virtual DbSet<EstablishmentAdmissions> EstablishmentAdmissions { get; set; }

    public virtual DbSet<EstablishmentAlternativeProvision> EstablishmentAlternativeProvision { get; set; }

    public virtual DbSet<EstablishmentAuthority> EstablishmentAuthority { get; set; }

    public virtual DbSet<EstablishmentBoarding> EstablishmentBoarding { get; set; }

    public virtual DbSet<EstablishmentFamily> EstablishmentFamily { get; set; }

    public virtual DbSet<EstablishmentGroupMembership> EstablishmentGroupMembership { get; set; }

    public virtual DbSet<EstablishmentIdentifier> EstablishmentIdentifier { get; set; }

    public virtual DbSet<EstablishmentInspection> EstablishmentInspection { get; set; }

    public virtual DbSet<EstablishmentLifecycleEvent> EstablishmentLifecycleEvent { get; set; }

    public virtual DbSet<EstablishmentProprietor> EstablishmentProprietor { get; set; }

    public virtual DbSet<EstablishmentProvision> EstablishmentProvision { get; set; }

    public virtual DbSet<EstablishmentReligion> EstablishmentReligion { get; set; }

    public virtual DbSet<EstablishmentSen> EstablishmentSen { get; set; }

    public virtual DbSet<EstablishmentSenNeed> EstablishmentSenNeed { get; set; }

    public virtual DbSet<EstablishmentStatus> EstablishmentStatus { get; set; }

    public virtual DbSet<EstablishmentStatusHistory> EstablishmentStatusHistory { get; set; }

    public virtual DbSet<EstablishmentType> EstablishmentType { get; set; }

    public virtual DbSet<GroupIdentifier> GroupIdentifier { get; set; }

    public virtual DbSet<GroupRecord> GroupRecord { get; set; }

    public virtual DbSet<GroupType> GroupType { get; set; }

    public virtual DbSet<Person> Person { get; set; }

    public virtual DbSet<ReasonEstablishmentClosed> ReasonEstablishmentClosed { get; set; }

    public virtual DbSet<ReasonEstablishmentOpened> ReasonEstablishmentOpened { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<RoleAssignment> RoleAssignment { get; set; }

    public virtual DbSet<RoleType> RoleType { get; set; }

    public virtual DbSet<Site> Site { get; set; }

    public virtual DbSet<Title> Title { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("contact_pkey");

            entity.ToTable("contact", "core");

            entity.HasIndex(e => e.EstablishmentId, "idx_contact_establishment_id");

            entity.HasIndex(e => e.GroupId, "idx_contact_group_id");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.TelephoneNumber).HasColumnName("telephone_number");
            entity.Property(e => e.Website).HasColumnName("website");

            entity.HasOne(d => d.Establishment).WithMany(p => p.Contact)
                .HasForeignKey(d => d.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_contact_establishment");

            entity.HasOne(d => d.Group).WithMany(p => p.Contact)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_contact_group");
        });

        modelBuilder.Entity<EducationPhase>(entity =>
        {
            entity.HasKey(e => e.EducationPhaseId).HasName("education_phase_pkey");

            entity.ToTable("education_phase", "ref");

            entity.HasIndex(e => e.Code, "education_phase_code_key").IsUnique();

            entity.Property(e => e.EducationPhaseId).HasColumnName("education_phase_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.EducationPhaseGroupId).HasColumnName("education_phase_group_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.EducationPhaseGroup).WithMany(p => p.EducationPhase)
                .HasForeignKey(d => d.EducationPhaseGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_education_phase_group");
        });

        modelBuilder.Entity<EducationPhaseGroup>(entity =>
        {
            entity.HasKey(e => e.EducationPhaseGroupId).HasName("education_phase_group_pkey");

            entity.ToTable("education_phase_group", "ref");

            entity.HasIndex(e => e.Code, "education_phase_group_code_key").IsUnique();

            entity.Property(e => e.EducationPhaseGroupId).HasColumnName("education_phase_group_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.HasKey(e => e.EstablishmentId).HasName("establishment_pkey");

            entity.ToTable("establishment", "core");

            entity.HasIndex(e => e.Uid, "establishment_uid_key").IsUnique();

            entity.HasIndex(e => e.Urn, "establishment_urn_key").IsUnique();

            entity.HasIndex(e => e.Name, "idx_establishment_name_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.HasIndex(e => e.EstablishmentStatusId, "idx_establishment_status_id");

            entity.HasIndex(e => e.EstablishmentTypeId, "idx_establishment_type_id");

            entity.HasIndex(e => e.Uid, "idx_establishment_uid");

            entity.HasIndex(e => e.Urn, "idx_establishment_urn");

            entity.HasIndex(e => e.Urn, "idx_establishment_urn_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.EstablishmentNumber).HasColumnName("establishment_number");
            entity.Property(e => e.EstablishmentStatusId).HasColumnName("establishment_status_id");
            entity.Property(e => e.EstablishmentTypeId).HasColumnName("establishment_type_id");
            entity.Property(e => e.HeadteacherRoleAssignmentId).HasColumnName("headteacher_role_assignment_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Uid).HasColumnName("uid");
            entity.Property(e => e.Urn).HasColumnName("urn");

            entity.HasOne(d => d.EstablishmentStatus).WithMany(p => p.Establishment)
                .HasForeignKey(d => d.EstablishmentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_establishment_status");

            entity.HasOne(d => d.EstablishmentType).WithMany(p => p.Establishment)
                .HasForeignKey(d => d.EstablishmentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_establishment_type");

            entity.HasOne(d => d.HeadteacherRoleAssignment).WithMany(p => p.Establishment)
                .HasForeignKey(d => d.HeadteacherRoleAssignmentId)
                .HasConstraintName("fk_establishment_headteacher_role_assignment");
        });

        modelBuilder.Entity<EstablishmentAdmissions>(entity =>
        {
            entity.HasKey(e => e.EstablishmentAdmissionsId).HasName("establishment_admissions_pkey");

            entity.ToTable("establishment_admissions", "core");

            entity.HasIndex(e => e.EstablishmentId, "establishment_admissions_establishment_id_key").IsUnique();

            entity.Property(e => e.EstablishmentAdmissionsId).HasColumnName("establishment_admissions_id");
            entity.Property(e => e.AdmissionsPolicy).HasColumnName("admissions_policy");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.StatutoryHighAge).HasColumnName("statutory_high_age");
            entity.Property(e => e.StatutoryLowAge).HasColumnName("statutory_low_age");

            entity.HasOne(d => d.Establishment).WithOne(p => p.EstablishmentAdmissions)
                .HasForeignKey<EstablishmentAdmissions>(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_admissions_establishment");
        });

        modelBuilder.Entity<EstablishmentAlternativeProvision>(entity =>
        {
            entity.HasKey(e => e.EstablishmentAlternativeProvisionId).HasName("establishment_alternative_provision_pkey");

            entity.ToTable("establishment_alternative_provision", "core");

            entity.HasIndex(e => e.EstablishmentId, "establishment_alternative_provision_establishment_id_key").IsUnique();

            entity.Property(e => e.EstablishmentAlternativeProvisionId).HasColumnName("establishment_alternative_provision_id");
            entity.Property(e => e.AlternativeProvisionType).HasColumnName("alternative_provision_type");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");

            entity.HasOne(d => d.Establishment).WithOne(p => p.EstablishmentAlternativeProvision)
                .HasForeignKey<EstablishmentAlternativeProvision>(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_alternative_provision_establishment");
        });

        modelBuilder.Entity<EstablishmentAuthority>(entity =>
        {
            entity.HasKey(e => e.EstablishmentAuthorityId).HasName("establishment_authority_pkey");

            entity.ToTable("establishment_authority", "core");

            entity.Property(e => e.EstablishmentAuthorityId).HasColumnName("establishment_authority_id");
            entity.Property(e => e.AuthorityCode).HasColumnName("authority_code");
            entity.Property(e => e.AuthorityName).HasColumnName("authority_name");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentAuthority)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_authority_establishment");
        });

        modelBuilder.Entity<EstablishmentBoarding>(entity =>
        {
            entity.HasKey(e => e.EstablishmentBoardingId).HasName("establishment_boarding_pkey");

            entity.ToTable("establishment_boarding", "core");

            entity.HasIndex(e => e.EstablishmentId, "establishment_boarding_establishment_id_key").IsUnique();

            entity.Property(e => e.EstablishmentBoardingId).HasColumnName("establishment_boarding_id");
            entity.Property(e => e.BoardingProvision).HasColumnName("boarding_provision");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");

            entity.HasOne(d => d.Establishment).WithOne(p => p.EstablishmentBoarding)
                .HasForeignKey<EstablishmentBoarding>(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_boarding_establishment");
        });

        modelBuilder.Entity<EstablishmentFamily>(entity =>
        {
            entity.HasKey(e => e.EstablishmentFamilyId).HasName("establishment_family_pkey");

            entity.ToTable("establishment_family", "ref");

            entity.HasIndex(e => e.Code, "establishment_family_code_key").IsUnique();

            entity.Property(e => e.EstablishmentFamilyId).HasColumnName("establishment_family_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<EstablishmentGroupMembership>(entity =>
        {
            entity.HasKey(e => e.EstablishmentGroupMembershipId).HasName("establishment_group_membership_pkey");

            entity.ToTable("establishment_group_membership", "core");

            entity.HasIndex(e => e.EstablishmentId, "idx_establishment_group_membership_establishment_id");

            entity.HasIndex(e => e.GroupId, "idx_establishment_group_membership_group_id");

            entity.Property(e => e.EstablishmentGroupMembershipId).HasColumnName("establishment_group_membership_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.MembershipCategory).HasColumnName("membership_category");
            entity.Property(e => e.MembershipFlag).HasColumnName("membership_flag");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentGroupMembership)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_group_membership_establishment");

            entity.HasOne(d => d.Group).WithMany(p => p.EstablishmentGroupMembership)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("fk_establishment_group_membership_group");
        });

        modelBuilder.Entity<EstablishmentIdentifier>(entity =>
        {
            entity.HasKey(e => e.EstablishmentIdentifierId).HasName("establishment_identifier_pkey");

            entity.ToTable("establishment_identifier", "core");

            entity.HasIndex(e => e.EstablishmentId, "idx_establishment_identifier_establishment_id");

            entity.HasIndex(e => new { e.EstablishmentId, e.IdentifierType, e.IdentifierValue }, "uq_establishment_identifier").IsUnique();

            entity.Property(e => e.EstablishmentIdentifierId).HasColumnName("establishment_identifier_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.IdentifierType).HasColumnName("identifier_type");
            entity.Property(e => e.IdentifierValue).HasColumnName("identifier_value");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentIdentifier)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_identifier_establishment");
        });

        modelBuilder.Entity<EstablishmentInspection>(entity =>
        {
            entity.HasKey(e => e.EstablishmentInspectionId).HasName("establishment_inspection_pkey");

            entity.ToTable("establishment_inspection", "core");

            entity.Property(e => e.EstablishmentInspectionId).HasColumnName("establishment_inspection_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.InspectionBody).HasColumnName("inspection_body");
            entity.Property(e => e.InspectionDate).HasColumnName("inspection_date");
            entity.Property(e => e.InspectionOutcome).HasColumnName("inspection_outcome");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentInspection)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_inspection_establishment");
        });

        modelBuilder.Entity<EstablishmentLifecycleEvent>(entity =>
        {
            entity.HasKey(e => e.EstablishmentLifecycleEventId).HasName("establishment_lifecycle_event_pkey");

            entity.ToTable("establishment_lifecycle_event", "core");

            entity.HasIndex(e => e.EstablishmentId, "idx_establishment_lifecycle_event_establishment_id");

            entity.Property(e => e.EstablishmentLifecycleEventId).HasColumnName("establishment_lifecycle_event_id");
            entity.Property(e => e.ClosedReasonId).HasColumnName("closed_reason_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.EventDate).HasColumnName("event_date");
            entity.Property(e => e.EventType).HasColumnName("event_type");
            entity.Property(e => e.OpenedReasonId).HasColumnName("opened_reason_id");

            entity.HasOne(d => d.ClosedReason).WithMany(p => p.EstablishmentLifecycleEvent)
                .HasForeignKey(d => d.ClosedReasonId)
                .HasConstraintName("fk_establishment_lifecycle_event_closed_reason");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentLifecycleEvent)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_lifecycle_event_establishment");

            entity.HasOne(d => d.OpenedReason).WithMany(p => p.EstablishmentLifecycleEvent)
                .HasForeignKey(d => d.OpenedReasonId)
                .HasConstraintName("fk_establishment_lifecycle_event_opened_reason");
        });

        modelBuilder.Entity<EstablishmentProprietor>(entity =>
        {
            entity.HasKey(e => e.EstablishmentProprietorId).HasName("establishment_proprietor_pkey");

            entity.ToTable("establishment_proprietor", "core");

            entity.Property(e => e.EstablishmentProprietorId).HasColumnName("establishment_proprietor_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.ProprietorName).HasColumnName("proprietor_name");
            entity.Property(e => e.ProprietorType).HasColumnName("proprietor_type");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentProprietor)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_proprietor_establishment");
        });

        modelBuilder.Entity<EstablishmentProvision>(entity =>
        {
            entity.HasKey(e => e.EstablishmentProvisionId).HasName("establishment_provision_pkey");

            entity.ToTable("establishment_provision", "core");

            entity.HasIndex(e => e.EstablishmentId, "establishment_provision_establishment_id_key").IsUnique();

            entity.Property(e => e.EstablishmentProvisionId).HasColumnName("establishment_provision_id");
            entity.Property(e => e.EducationPhaseId).HasColumnName("education_phase_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.Fsm).HasColumnName("fsm");
            entity.Property(e => e.FurtherEducationTypeId).HasColumnName("further_education_type_id");
            entity.Property(e => e.NurseryProvisionId).HasColumnName("nursery_provision_id");
            entity.Property(e => e.OfficialSixthFormId).HasColumnName("official_sixth_form_id");
            entity.Property(e => e.PercentageFsm)
                .HasPrecision(5, 2)
                .HasColumnName("percentage_fsm");

            entity.HasOne(d => d.EducationPhase).WithMany(p => p.EstablishmentProvision)
                .HasForeignKey(d => d.EducationPhaseId)
                .HasConstraintName("fk_establishment_provision_education_phase");

            entity.HasOne(d => d.Establishment).WithOne(p => p.EstablishmentProvision)
                .HasForeignKey<EstablishmentProvision>(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_provision_establishment");
        });

        modelBuilder.Entity<EstablishmentReligion>(entity =>
        {
            entity.HasKey(e => e.EstablishmentReligionId).HasName("establishment_religion_pkey");

            entity.ToTable("establishment_religion", "core");

            entity.Property(e => e.EstablishmentReligionId).HasColumnName("establishment_religion_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.ReligiousEthos).HasColumnName("religious_ethos");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentReligion)
                .HasForeignKey(d => d.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_establishment_religion_establishment");

            entity.HasOne(d => d.Group).WithMany(p => p.EstablishmentReligion)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_establishment_religion_group");
        });

        modelBuilder.Entity<EstablishmentSen>(entity =>
        {
            entity.HasKey(e => e.EstablishmentSenId).HasName("establishment_sen_pkey");

            entity.ToTable("establishment_sen", "core");

            entity.HasIndex(e => e.EstablishmentId, "establishment_sen_establishment_id_key").IsUnique();

            entity.Property(e => e.EstablishmentSenId).HasColumnName("establishment_sen_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.HasSenProvision).HasColumnName("has_sen_provision");
            entity.Property(e => e.SenProvision).HasColumnName("sen_provision");

            entity.HasOne(d => d.Establishment).WithOne(p => p.EstablishmentSen)
                .HasForeignKey<EstablishmentSen>(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_sen_establishment");
        });

        modelBuilder.Entity<EstablishmentSenNeed>(entity =>
        {
            entity.HasKey(e => e.EstablishmentSenNeedId).HasName("establishment_sen_need_pkey");

            entity.ToTable("establishment_sen_need", "core");

            entity.Property(e => e.EstablishmentSenNeedId).HasColumnName("establishment_sen_need_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.SenNeedType).HasColumnName("sen_need_type");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentSenNeed)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_sen_need_establishment");
        });

        modelBuilder.Entity<EstablishmentStatus>(entity =>
        {
            entity.HasKey(e => e.EstablishmentStatusId).HasName("establishment_status_pkey");

            entity.ToTable("establishment_status", "ref");

            entity.HasIndex(e => e.Code, "establishment_status_code_key").IsUnique();

            entity.Property(e => e.EstablishmentStatusId).HasColumnName("establishment_status_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<EstablishmentStatusHistory>(entity =>
        {
            entity.HasKey(e => e.EstablishmentStatusHistoryId).HasName("establishment_status_history_pkey");

            entity.ToTable("establishment_status_history", "core");

            entity.HasIndex(e => e.EstablishmentId, "idx_establishment_status_history_establishment_id");

            entity.Property(e => e.EstablishmentStatusHistoryId).HasColumnName("establishment_status_history_id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("changed_at");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.NewStatusId).HasColumnName("new_status_id");
            entity.Property(e => e.OldStatusId).HasColumnName("old_status_id");

            entity.HasOne(d => d.Establishment).WithMany(p => p.EstablishmentStatusHistory)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_establishment_status_history_establishment");

            entity.HasOne(d => d.NewStatus).WithMany(p => p.EstablishmentStatusHistoryNewStatus)
                .HasForeignKey(d => d.NewStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_establishment_status_history_new_status");

            entity.HasOne(d => d.OldStatus).WithMany(p => p.EstablishmentStatusHistoryOldStatus)
                .HasForeignKey(d => d.OldStatusId)
                .HasConstraintName("fk_establishment_status_history_old_status");
        });

        modelBuilder.Entity<EstablishmentType>(entity =>
        {
            entity.HasKey(e => e.EstablishmentTypeId).HasName("establishment_type_pkey");

            entity.ToTable("establishment_type", "ref");

            entity.HasIndex(e => e.Code, "establishment_type_code_key").IsUnique();

            entity.Property(e => e.EstablishmentTypeId).HasColumnName("establishment_type_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.EstablishmentFamilyId).HasColumnName("establishment_family_id");
            entity.Property(e => e.IsEarlyYears).HasColumnName("is_early_years");
            entity.Property(e => e.IsFurtherEducation).HasColumnName("is_further_education");
            entity.Property(e => e.IsGroup).HasColumnName("is_group");
            entity.Property(e => e.IsSchool).HasColumnName("is_school");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.EstablishmentFamily).WithMany(p => p.EstablishmentType)
                .HasForeignKey(d => d.EstablishmentFamilyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_establishment_type_family");
        });

        modelBuilder.Entity<GroupIdentifier>(entity =>
        {
            entity.HasKey(e => e.GroupIdentifierId).HasName("group_identifier_pkey");

            entity.ToTable("group_identifier", "core");

            entity.HasIndex(e => e.GroupId, "idx_group_identifier_group_id");

            entity.HasIndex(e => new { e.GroupId, e.IdentifierType, e.IdentifierValue }, "uq_group_identifier").IsUnique();

            entity.Property(e => e.GroupIdentifierId).HasColumnName("group_identifier_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.IdentifierType).HasColumnName("identifier_type");
            entity.Property(e => e.IdentifierValue).HasColumnName("identifier_value");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupIdentifier)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("fk_group_identifier_group");
        });

        modelBuilder.Entity<GroupRecord>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("group_record_pkey");

            entity.ToTable("group_record", "core");

            entity.HasIndex(e => e.Code, "group_record_code_key").IsUnique();

            entity.HasIndex(e => e.GroupTypeId, "idx_group_record_group_type_id");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.GroupTypeId).HasColumnName("group_type_id");
            entity.Property(e => e.HeadteacherRoleAssignmentId).HasColumnName("headteacher_role_assignment_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.GroupType).WithMany(p => p.GroupRecord)
                .HasForeignKey(d => d.GroupTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_group_record_group_type");

            entity.HasOne(d => d.HeadteacherRoleAssignment).WithMany(p => p.GroupRecord)
                .HasForeignKey(d => d.HeadteacherRoleAssignmentId)
                .HasConstraintName("fk_group_record_headteacher_role_assignment");

            entity.HasMany(d => d.ChildGroup).WithMany(p => p.ParentGroup)
                .UsingEntity<Dictionary<string, object>>(
                    "GroupGroupMembership",
                    r => r.HasOne<GroupRecord>().WithMany()
                        .HasForeignKey("ChildGroupId")
                        .HasConstraintName("fk_group_group_membership_child"),
                    l => l.HasOne<GroupRecord>().WithMany()
                        .HasForeignKey("ParentGroupId")
                        .HasConstraintName("fk_group_group_membership_parent"),
                    j =>
                    {
                        j.HasKey("ParentGroupId", "ChildGroupId").HasName("group_group_membership_pkey");
                        j.ToTable("group_group_membership", "core");
                        j.HasIndex(new[] { "ChildGroupId" }, "idx_group_group_membership_child_group_id");
                        j.IndexerProperty<long>("ParentGroupId").HasColumnName("parent_group_id");
                        j.IndexerProperty<long>("ChildGroupId").HasColumnName("child_group_id");
                    });

            entity.HasMany(d => d.ParentGroup).WithMany(p => p.ChildGroup)
                .UsingEntity<Dictionary<string, object>>(
                    "GroupGroupMembership",
                    r => r.HasOne<GroupRecord>().WithMany()
                        .HasForeignKey("ParentGroupId")
                        .HasConstraintName("fk_group_group_membership_parent"),
                    l => l.HasOne<GroupRecord>().WithMany()
                        .HasForeignKey("ChildGroupId")
                        .HasConstraintName("fk_group_group_membership_child"),
                    j =>
                    {
                        j.HasKey("ParentGroupId", "ChildGroupId").HasName("group_group_membership_pkey");
                        j.ToTable("group_group_membership", "core");
                        j.HasIndex(new[] { "ChildGroupId" }, "idx_group_group_membership_child_group_id");
                        j.IndexerProperty<long>("ParentGroupId").HasColumnName("parent_group_id");
                        j.IndexerProperty<long>("ChildGroupId").HasColumnName("child_group_id");
                    });
        });

        modelBuilder.Entity<GroupType>(entity =>
        {
            entity.HasKey(e => e.GroupTypeId).HasName("group_type_pkey");

            entity.ToTable("group_type", "ref");

            entity.HasIndex(e => e.Code, "group_type_code_key").IsUnique();

            entity.Property(e => e.GroupTypeId).HasColumnName("group_type_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("person_pkey");

            entity.ToTable("person", "core");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.DisplayName).HasColumnName("display_name");
            entity.Property(e => e.FamilyName).HasColumnName("family_name");
            entity.Property(e => e.GivenName).HasColumnName("given_name");
            entity.Property(e => e.TitleId).HasColumnName("title_id");

            entity.HasOne(d => d.Title).WithMany(p => p.Person)
                .HasForeignKey(d => d.TitleId)
                .HasConstraintName("fk_person_title");
        });

        modelBuilder.Entity<ReasonEstablishmentClosed>(entity =>
        {
            entity.HasKey(e => e.ReasonEstablishmentClosedId).HasName("reason_establishment_closed_pkey");

            entity.ToTable("reason_establishment_closed", "ref");

            entity.HasIndex(e => e.Code, "reason_establishment_closed_code_key").IsUnique();

            entity.Property(e => e.ReasonEstablishmentClosedId).HasColumnName("reason_establishment_closed_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<ReasonEstablishmentOpened>(entity =>
        {
            entity.HasKey(e => e.ReasonEstablishmentOpenedId).HasName("reason_establishment_opened_pkey");

            entity.ToTable("reason_establishment_opened", "ref");

            entity.HasIndex(e => e.Code, "reason_establishment_opened_code_key").IsUnique();

            entity.Property(e => e.ReasonEstablishmentOpenedId).HasColumnName("reason_establishment_opened_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role", "core");

            entity.HasIndex(e => e.PersonId, "idx_role_person_id");

            entity.HasIndex(e => e.RoleTypeId, "idx_role_role_type_id");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.RoleTypeId).HasColumnName("role_type_id");

            entity.HasOne(d => d.Person).WithMany(p => p.Role)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("fk_role_person");

            entity.HasOne(d => d.RoleType).WithMany(p => p.Role)
                .HasForeignKey(d => d.RoleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role_role_type");
        });

        modelBuilder.Entity<RoleAssignment>(entity =>
        {
            entity.HasKey(e => e.RoleAssignmentId).HasName("role_assignment_pkey");

            entity.ToTable("role_assignment", "core");

            entity.HasIndex(e => e.EstablishmentId, "idx_role_assignment_establishment_id");

            entity.HasIndex(e => e.GroupId, "idx_role_assignment_group_id");

            entity.HasIndex(e => e.RoleId, "idx_role_assignment_role_id");

            entity.Property(e => e.RoleAssignmentId).HasColumnName("role_assignment_id");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.PreferredJobTitle).HasColumnName("preferred_job_title");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.EstablishmentNavigation).WithMany(p => p.RoleAssignment)
                .HasForeignKey(d => d.EstablishmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_role_assignment_establishment");

            entity.HasOne(d => d.Group).WithMany(p => p.RoleAssignment)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_role_assignment_group");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleAssignment)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_role_assignment_role");
        });

        modelBuilder.Entity<RoleType>(entity =>
        {
            entity.HasKey(e => e.RoleTypeId).HasName("role_type_pkey");

            entity.ToTable("role_type", "ref");

            entity.HasIndex(e => e.Code, "role_type_code_key").IsUnique();

            entity.Property(e => e.RoleTypeId).HasColumnName("role_type_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.HasKey(e => e.SiteId).HasName("site_pkey");

            entity.ToTable("site", "core");

            entity.Property(e => e.SiteId).HasColumnName("site_id");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line_1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line_2");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.EstablishmentId).HasColumnName("establishment_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.Town).HasColumnName("town");

            entity.HasOne(d => d.Establishment).WithMany(p => p.Site)
                .HasForeignKey(d => d.EstablishmentId)
                .HasConstraintName("fk_site_establishment");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.TitleId).HasName("title_pkey");

            entity.ToTable("title", "ref");

            entity.HasIndex(e => e.Name, "title_name_key").IsUnique();

            entity.Property(e => e.TitleId).HasColumnName("title_id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
