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

    public virtual DbSet<EstablishmentAggregate> EstablishmentAggregate { get; set; }

    public virtual DbSet<EstablishmentGovernorAggregate> EstablishmentGovernorAggregate { get; set; }

    public virtual DbSet<GroupAggregate> GroupAggregate { get; set; }

    public virtual DbSet<GroupMemberAggregate> GroupMemberAggregate { get; set; }

    public virtual DbSet<SearchAggregate> SearchAggregate { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_trgm");

        modelBuilder.Entity<EstablishmentAggregate>(entity =>
        {
            entity.HasKey(e => e.EstablishmentAggregateId).HasName("establishment_aggregate_pkey");

            entity.ToTable("establishment_aggregate", "core");

            entity.HasIndex(e => e.Urn, "establishment_aggregate_urn_key").IsUnique();

            entity.HasIndex(e => e.GroupUid, "idx_establishment_aggregate_group_uid");

            entity.HasIndex(e => e.Urn, "idx_establishment_aggregate_urn");

            entity.Property(e => e.EstablishmentAggregateId).HasColumnName("establishment_aggregate_id");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line_1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line_2");
            entity.Property(e => e.ClosedDate).HasColumnName("closed_date");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.EducationPhaseName).HasColumnName("education_phase_name");
            entity.Property(e => e.EstablishmentNumber).HasColumnName("establishment_number");
            entity.Property(e => e.EstablishmentTypeName).HasColumnName("establishment_type_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.GroupCode).HasColumnName("group_code");
            entity.Property(e => e.GroupName).HasColumnName("group_name");
            entity.Property(e => e.GroupOpenDate).HasColumnName("group_open_date");
            entity.Property(e => e.GroupTypeName).HasColumnName("group_type_name");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.HeadteacherIdentifier).HasColumnName("headteacher_identifier");
            entity.Property(e => e.HeadteacherName).HasColumnName("headteacher_name");
            entity.Property(e => e.LocalAuthorityCode).HasColumnName("local_authority_code");
            entity.Property(e => e.LocalAuthorityName).HasColumnName("local_authority_name");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OfstedInspectionDate).HasColumnName("ofsted_inspection_date");
            entity.Property(e => e.OfstedReportUrl).HasColumnName("ofsted_report_url");
            entity.Property(e => e.OpenedDate).HasColumnName("opened_date");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.SenProvision).HasColumnName("sen_provision");
            entity.Property(e => e.SiteName).HasColumnName("site_name");
            entity.Property(e => e.StatusCode).HasColumnName("status_code");
            entity.Property(e => e.StatusDate).HasColumnName("status_date");
            entity.Property(e => e.StatutoryHighAge).HasColumnName("statutory_high_age");
            entity.Property(e => e.StatutoryLowAge).HasColumnName("statutory_low_age");
            entity.Property(e => e.TelephoneNumber).HasColumnName("telephone_number");
            entity.Property(e => e.Town).HasColumnName("town");
            entity.Property(e => e.Urn).HasColumnName("urn");
            entity.Property(e => e.Website).HasColumnName("website");
        });

        modelBuilder.Entity<EstablishmentGovernorAggregate>(entity =>
        {
            entity.HasKey(e => e.EstablishmentGovernorAggregateId).HasName("establishment_governor_aggregate_pkey");

            entity.ToTable("establishment_governor_aggregate", "core");

            entity.HasIndex(e => e.EstablishmentUrn, "idx_establishment_governor_aggregate_urn");

            entity.Property(e => e.EstablishmentGovernorAggregateId).HasColumnName("establishment_governor_aggregate_id");
            entity.Property(e => e.EstablishmentUrn).HasColumnName("establishment_urn");
            entity.Property(e => e.GovernorId).HasColumnName("governor_id");
            entity.Property(e => e.GovernorName).HasColumnName("governor_name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<GroupAggregate>(entity =>
        {
            entity.HasKey(e => e.GroupAggregateId).HasName("group_aggregate_pkey");

            entity.ToTable("group_aggregate", "core");

            entity.HasIndex(e => e.GroupId, "group_aggregate_group_id_key").IsUnique();

            entity.HasIndex(e => e.GroupId, "idx_group_aggregate_group_id");

            entity.HasIndex(e => e.GroupUid, "idx_group_aggregate_group_uid");

            entity.Property(e => e.GroupAggregateId).HasColumnName("group_aggregate_id");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line_1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line_2");
            entity.Property(e => e.CompaniesHouseNumber).HasColumnName("companies_house_number");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupStatusEffectiveDate).HasColumnName("group_status_effective_date");
            entity.Property(e => e.GroupStatusLabel).HasColumnName("group_status_label");
            entity.Property(e => e.GroupTypeName).HasColumnName("group_type_name");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.HeadteacherIdentifier).HasColumnName("headteacher_identifier");
            entity.Property(e => e.HeadteacherName).HasColumnName("headteacher_name");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.SiteName).HasColumnName("site_name");
            entity.Property(e => e.TelephoneNumber).HasColumnName("telephone_number");
            entity.Property(e => e.Town).HasColumnName("town");
            entity.Property(e => e.Ukprn).HasColumnName("ukprn");
            entity.Property(e => e.Website).HasColumnName("website");
        });

        modelBuilder.Entity<GroupMemberAggregate>(entity =>
        {
            entity.HasKey(e => e.GroupMemberAggregateId).HasName("group_member_aggregate_pkey");

            entity.ToTable("group_member_aggregate", "core");

            entity.HasIndex(e => e.EstablishmentUrn, "idx_group_member_aggregate_establishment_urn");

            entity.HasIndex(e => e.GroupId, "idx_group_member_aggregate_group_id");

            entity.Property(e => e.GroupMemberAggregateId).HasColumnName("group_member_aggregate_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EstablishmentName).HasColumnName("establishment_name");
            entity.Property(e => e.EstablishmentUrn).HasColumnName("establishment_urn");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<SearchAggregate>(entity =>
        {
            entity.HasKey(e => e.SearchAggregateId).HasName("search_aggregate_pkey");

            entity.ToTable("search_aggregate", "core");

            entity.HasIndex(e => e.CompaniesHouseNumber, "idx_search_provider_companies_house_number");

            entity.HasIndex(e => e.County, "idx_search_provider_county");

            entity.HasIndex(e => e.DfeNumber, "idx_search_provider_dfe_number");

            entity.HasIndex(e => e.GroupUid, "idx_search_provider_group_uid");

            entity.HasIndex(e => e.ProviderId, "idx_search_provider_id");

            entity.HasIndex(e => e.LaEstab, "idx_search_provider_laestab");

            entity.HasIndex(e => e.LocalAuthorityName, "idx_search_provider_local_authority_name");

            entity.HasIndex(e => e.ProviderName, "idx_search_provider_name");

            entity.HasIndex(e => e.ProviderName, "idx_search_provider_name_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.HasIndex(e => e.Postcode, "idx_search_provider_postcode");

            entity.HasIndex(e => e.Town, "idx_search_provider_town");

            entity.HasIndex(e => e.UkProviderReferenceNumber, "idx_search_provider_ukprn");

            entity.HasIndex(e => e.ProviderId, "search_aggregate_provider_id_key").IsUnique();

            entity.Property(e => e.SearchAggregateId).HasColumnName("search_aggregate_id");
            entity.Property(e => e.AcademyCounts).HasColumnName("academy_counts");
            entity.Property(e => e.CompaniesHouseNumber).HasColumnName("companies_house_number");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.DfeNumber).HasColumnName("dfe_number");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.LaEstab).HasColumnName("la_estab");
            entity.Property(e => e.LocalAuthorityName).HasColumnName("local_authority_name");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ProviderAddress).HasColumnName("provider_address");
            entity.Property(e => e.ProviderCategory).HasColumnName("provider_category");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.ProviderName).HasColumnName("provider_name");
            entity.Property(e => e.ProviderTypeId).HasColumnName("provider_type_id");
            entity.Property(e => e.ProviderTypeName).HasColumnName("provider_type_name");
            entity.Property(e => e.StatusCode).HasColumnName("status_code");
            entity.Property(e => e.Town).HasColumnName("town");
            entity.Property(e => e.UkProviderReferenceNumber).HasColumnName("uk_provider_reference_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
