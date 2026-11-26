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

    public virtual DbSet<EstablishmentDownloadAggregate> EstablishmentDownloadAggregate { get; set; }

    public virtual DbSet<EstablishmentGovernorAggregate> EstablishmentGovernorAggregate { get; set; }

    public virtual DbSet<GroupAggregate> GroupAggregate { get; set; }

    public virtual DbSet<GroupDownloadAggregate> GroupDownloadAggregate { get; set; }

    public virtual DbSet<GroupEstablishmentAggregate> GroupEstablishmentAggregate { get; set; }

    public virtual DbSet<GroupGovernorAggregate> GroupGovernorAggregate { get; set; }

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
            entity.Property(e => e.Diocese).HasColumnName("diocese");
            entity.Property(e => e.EducationPhaseName).HasColumnName("education_phase_name");
            entity.Property(e => e.EstablishmentNumber).HasColumnName("establishment_number");
            entity.Property(e => e.EstablishmentTypeName).HasColumnName("establishment_type_name");
            entity.Property(e => e.FsmNumber).HasColumnName("fsm_number");
            entity.Property(e => e.FsmPercentage).HasColumnName("fsm_percentage");
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
            entity.Property(e => e.NumberOfBoys).HasColumnName("number_of_boys");
            entity.Property(e => e.NumberOfGirls).HasColumnName("number_of_girls");
            entity.Property(e => e.NumberOfPupils).HasColumnName("number_of_pupils");
            entity.Property(e => e.OfstedInspectionDate).HasColumnName("ofsted_inspection_date");
            entity.Property(e => e.OfstedReportUrl).HasColumnName("ofsted_report_url");
            entity.Property(e => e.OpenedDate).HasColumnName("opened_date");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.ReligiousEthos).HasColumnName("religious_ethos");
            entity.Property(e => e.Sen).HasColumnName("sen");
            entity.Property(e => e.SenProvision).HasColumnName("sen_provision");
            entity.Property(e => e.SiteName).HasColumnName("site_name");
            entity.Property(e => e.StatusCode).HasColumnName("status_code");
            entity.Property(e => e.StatusDate).HasColumnName("status_date");
            entity.Property(e => e.StatutoryHighAge).HasColumnName("statutory_high_age");
            entity.Property(e => e.StatutoryLowAge).HasColumnName("statutory_low_age");
            entity.Property(e => e.TelephoneNumber).HasColumnName("telephone_number");
            entity.Property(e => e.Town).HasColumnName("town");
            entity.Property(e => e.Ukprn).HasColumnName("ukprn");
            entity.Property(e => e.Urn).HasColumnName("urn");
            entity.Property(e => e.Website).HasColumnName("website");
        });

        modelBuilder.Entity<EstablishmentDownloadAggregate>(entity =>
        {
            entity.HasKey(e => e.EstablishmentDownloadAggregateId).HasName("establishment_download_aggregate_pkey");

            entity.ToTable("establishment_download_aggregate", "core");

            entity.HasIndex(e => e.Urn, "establishment_download_aggregate_urn_key").IsUnique();

            entity.HasIndex(e => e.UkProviderReferenceNumber, "idx_establishment_download_aggregate_ukprn");

            entity.HasIndex(e => e.Urn, "idx_establishment_download_aggregate_urn");

            entity.Property(e => e.EstablishmentDownloadAggregateId).HasColumnName("establishment_download_aggregate_id");
            entity.Property(e => e.AccreditationExpiryDate).HasColumnName("accreditation_expiry_date");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line_1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line_2");
            entity.Property(e => e.AddressLine3).HasColumnName("address_line_3");
            entity.Property(e => e.AdministrativeWard).HasColumnName("administrative_ward");
            entity.Property(e => e.AdministrativeWardId).HasColumnName("administrative_ward_id");
            entity.Property(e => e.AdmissionsPolicy).HasColumnName("admissions_policy");
            entity.Property(e => e.AdmissionsPolicyId).HasColumnName("admissions_policy_id");
            entity.Property(e => e.ApprovedIndependentSendSchoolSection41).HasColumnName("approved_independent_send_school_section_41");
            entity.Property(e => e.AutisticSpectrumDisorderSen4).HasColumnName("autistic_spectrum_disorder_sen_4");
            entity.Property(e => e.BoardingEstablishment).HasColumnName("boarding_establishment");
            entity.Property(e => e.BoardingProvision).HasColumnName("boarding_provision");
            entity.Property(e => e.BoardingProvisionId).HasColumnName("boarding_provision_id");
            entity.Property(e => e.BritishSchoolOverseasInspectorateName).HasColumnName("british_school_overseas_inspectorate_name");
            entity.Property(e => e.CensusDate).HasColumnName("census_date");
            entity.Property(e => e.ChildcareFacilitiesProvision).HasColumnName("childcare_facilities_provision");
            entity.Property(e => e.ClosedDate).HasColumnName("closed_date");
            entity.Property(e => e.CompaniesHouseNumber).HasColumnName("companies_house_number");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.DateLastChangedOrConfirmed).HasColumnName("date_last_changed_or_confirmed");
            entity.Property(e => e.DateOfLastInspectionVisit).HasColumnName("date_of_last_inspection_visit");
            entity.Property(e => e.DateOfNextInspectionVisit).HasColumnName("date_of_next_inspection_visit");
            entity.Property(e => e.Diocese).HasColumnName("diocese");
            entity.Property(e => e.DioceseId).HasColumnName("diocese_id");
            entity.Property(e => e.DistrictAdministrative).HasColumnName("district_administrative");
            entity.Property(e => e.DistrictAdministrativeId).HasColumnName("district_administrative_id");
            entity.Property(e => e.Easting).HasColumnName("easting");
            entity.Property(e => e.EducationPhase).HasColumnName("education_phase");
            entity.Property(e => e.EducationPhaseId).HasColumnName("education_phase_id");
            entity.Property(e => e.EmotionalBehaviouralDifficultiesProvision).HasColumnName("emotional_behavioural_difficulties_provision");
            entity.Property(e => e.EstablishmentNumber).HasColumnName("establishment_number");
            entity.Property(e => e.Federation).HasColumnName("federation");
            entity.Property(e => e.FederationFlag).HasColumnName("federation_flag");
            entity.Property(e => e.FederationId).HasColumnName("federation_id");
            entity.Property(e => e.FullTimeProvision).HasColumnName("full_time_provision");
            entity.Property(e => e.FurtherEducationHigherEducationIdentifier).HasColumnName("further_education_higher_education_identifier");
            entity.Property(e => e.FurtherEducationType).HasColumnName("further_education_type");
            entity.Property(e => e.GenderOfEntry).HasColumnName("gender_of_entry");
            entity.Property(e => e.GenderOfEntryId).HasColumnName("gender_of_entry_id");
            entity.Property(e => e.GovernmentOfficeRegion).HasColumnName("government_office_region");
            entity.Property(e => e.GovernmentOfficeRegionId).HasColumnName("government_office_region_id");
            entity.Property(e => e.GovernmentStatisticalServiceLocalAuthorityCode).HasColumnName("government_statistical_service_local_authority_code");
            entity.Property(e => e.HeadFirstName).HasColumnName("head_first_name");
            entity.Property(e => e.HeadLastName).HasColumnName("head_last_name");
            entity.Property(e => e.HeadTitle).HasColumnName("head_title");
            entity.Property(e => e.HearingImpairmentSen2).HasColumnName("hearing_impairment_sen_2");
            entity.Property(e => e.InspectorateName).HasColumnName("inspectorate_name");
            entity.Property(e => e.InspectorateReport).HasColumnName("inspectorate_report");
            entity.Property(e => e.JobTitle).HasColumnName("job_title");
            entity.Property(e => e.LocalAuthority).HasColumnName("local_authority");
            entity.Property(e => e.LocalAuthorityId).HasColumnName("local_authority_id");
            entity.Property(e => e.LowerSuperOutputArea).HasColumnName("lower_super_output_area");
            entity.Property(e => e.LowerSuperOutputAreaId).HasColumnName("lower_super_output_area_id");
            entity.Property(e => e.MiddleSuperOutputArea).HasColumnName("middle_super_output_area");
            entity.Property(e => e.MiddleSuperOutputAreaId).HasColumnName("middle_super_output_area_id");
            entity.Property(e => e.ModerateLearningDifficultySen8).HasColumnName("moderate_learning_difficulty_sen_8");
            entity.Property(e => e.MultiSensoryImpairmentSen6).HasColumnName("multi_sensory_impairment_sen_6");
            entity.Property(e => e.Northing).HasColumnName("northing");
            entity.Property(e => e.NumberOfFemales).HasColumnName("number_of_females");
            entity.Property(e => e.NumberOfMales).HasColumnName("number_of_males");
            entity.Property(e => e.NumberOfPupilsStudents).HasColumnName("number_of_pupils_students");
            entity.Property(e => e.NurseryProvision).HasColumnName("nursery_provision");
            entity.Property(e => e.OpenDate).HasColumnName("open_date");
            entity.Property(e => e.OtherDifficultyDisabilitySen12).HasColumnName("other_difficulty_disability_sen_12");
            entity.Property(e => e.ParliamentaryConstituency).HasColumnName("parliamentary_constituency");
            entity.Property(e => e.ParliamentaryConstituencyId).HasColumnName("parliamentary_constituency_id");
            entity.Property(e => e.PercentagePupilsEligibleForFreeSchoolMeals).HasColumnName("percentage_pupils_eligible_for_free_school_meals");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.PhysicalDisabilitySen7).HasColumnName("physical_disability_sen_7");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.PreviousLocalAuthority).HasColumnName("previous_local_authority");
            entity.Property(e => e.PreviousLocalAuthorityId).HasColumnName("previous_local_authority_id");
            entity.Property(e => e.PreviousProviderNumber).HasColumnName("previous_provider_number");
            entity.Property(e => e.ProfoundMultipleLearningDifficultySen10).HasColumnName("profound_multiple_learning_difficulty_sen_10");
            entity.Property(e => e.Proprietor).HasColumnName("proprietor");
            entity.Property(e => e.ProviderAccredited).HasColumnName("provider_accredited");
            entity.Property(e => e.ProviderAccreditedId).HasColumnName("provider_accredited_id");
            entity.Property(e => e.ProviderCapacity).HasColumnName("provider_capacity");
            entity.Property(e => e.ProviderName).HasColumnName("provider_name");
            entity.Property(e => e.ProviderNumber).HasColumnName("provider_number");
            entity.Property(e => e.ProviderStatus).HasColumnName("provider_status");
            entity.Property(e => e.ProviderStatusId).HasColumnName("provider_status_id");
            entity.Property(e => e.ProviderType).HasColumnName("provider_type");
            entity.Property(e => e.ProviderTypeGroup).HasColumnName("provider_type_group");
            entity.Property(e => e.ProviderTypeGroupId).HasColumnName("provider_type_group_id");
            entity.Property(e => e.ProviderTypeId).HasColumnName("provider_type_id");
            entity.Property(e => e.PupilReferralUnitCapacity).HasColumnName("pupil_referral_unit_capacity");
            entity.Property(e => e.PupilsEducatedByOtherProviders).HasColumnName("pupils_educated_by_other_providers");
            entity.Property(e => e.PupilsEligibleForFreeSchoolMeals).HasColumnName("pupils_eligible_for_free_school_meals");
            entity.Property(e => e.QualificationsAwardingBody).HasColumnName("qualifications_awarding_body");
            entity.Property(e => e.QualificationsAwardingBodyId).HasColumnName("qualifications_awarding_body_id");
            entity.Property(e => e.QualificationsAwardingBodyReport).HasColumnName("qualifications_awarding_body_report");
            entity.Property(e => e.ReasonProviderClosed).HasColumnName("reason_provider_closed");
            entity.Property(e => e.ReasonProviderClosedId).HasColumnName("reason_provider_closed_id");
            entity.Property(e => e.ReasonProviderOpened).HasColumnName("reason_provider_opened");
            entity.Property(e => e.ReasonProviderOpenedId).HasColumnName("reason_provider_opened_id");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.ReligiousCharacterId).HasColumnName("religious_character_id");
            entity.Property(e => e.ReligiousEthos).HasColumnName("religious_ethos");
            entity.Property(e => e.ResourcedProvisionCapacity).HasColumnName("resourced_provision_capacity");
            entity.Property(e => e.ResourcedProvisionPupilCount).HasColumnName("resourced_provision_pupil_count");
            entity.Property(e => e.SchoolSponsor).HasColumnName("school_sponsor");
            entity.Property(e => e.SchoolSponsorFlag).HasColumnName("school_sponsor_flag");
            entity.Property(e => e.SenPupilReferralUnit).HasColumnName("sen_pupil_referral_unit");
            entity.Property(e => e.SenUnitCapacity).HasColumnName("sen_unit_capacity");
            entity.Property(e => e.SenUnitPupilCount).HasColumnName("sen_unit_pupil_count");
            entity.Property(e => e.SevereLearningDifficultySen9).HasColumnName("severe_learning_difficulty_sen_9");
            entity.Property(e => e.Site).HasColumnName("site");
            entity.Property(e => e.SixthFormProvision).HasColumnName("sixth_form_provision");
            entity.Property(e => e.SixthFormProvisionId).HasColumnName("sixth_form_provision_id");
            entity.Property(e => e.SocialEmotionalMentalHealthSen5).HasColumnName("social_emotional_mental_health_sen_5");
            entity.Property(e => e.SpecialClassProvision).HasColumnName("special_class_provision");
            entity.Property(e => e.SpecialClassProvisionId).HasColumnName("special_class_provision_id");
            entity.Property(e => e.SpecialPupilsNotOnStatementOrEhcPlan).HasColumnName("special_pupils_not_on_statement_or_ehc_plan");
            entity.Property(e => e.SpecialPupilsOnStatementOrEhcPlan).HasColumnName("special_pupils_on_statement_or_ehc_plan");
            entity.Property(e => e.SpecificLearningDifficultySen11).HasColumnName("specific_learning_difficulty_sen_11");
            entity.Property(e => e.SpeechLanguageCommunicationSen3).HasColumnName("speech_language_communication_sen_3");
            entity.Property(e => e.StatutoryHighAge).HasColumnName("statutory_high_age");
            entity.Property(e => e.StatutoryLowAge).HasColumnName("statutory_low_age");
            entity.Property(e => e.TeenMothersCapacity).HasColumnName("teen_mothers_capacity");
            entity.Property(e => e.TeenMothersProvision).HasColumnName("teen_mothers_provision");
            entity.Property(e => e.TownOrCity).HasColumnName("town_or_city");
            entity.Property(e => e.TrustSchoolFlag).HasColumnName("trust_school_flag");
            entity.Property(e => e.TrustSchoolFlagId).HasColumnName("trust_school_flag_id");
            entity.Property(e => e.Trusts).HasColumnName("trusts");
            entity.Property(e => e.TrustsId).HasColumnName("trusts_id");
            entity.Property(e => e.TypeOfResourcedProvision).HasColumnName("type_of_resourced_provision");
            entity.Property(e => e.UkProviderReferenceNumber).HasColumnName("uk_provider_reference_number");
            entity.Property(e => e.UniquePropertyReferenceNumber).HasColumnName("unique_property_reference_number");
            entity.Property(e => e.UniqueReferenceNumber).HasColumnName("unique_reference_number");
            entity.Property(e => e.UrbanRuralClassification).HasColumnName("urban_rural_classification");
            entity.Property(e => e.UrbanRuralClassificationId).HasColumnName("urban_rural_classification_id");
            entity.Property(e => e.Urn).HasColumnName("urn");
            entity.Property(e => e.VisualImpairmentSen1).HasColumnName("visual_impairment_sen_1");
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

            entity.HasIndex(e => e.GroupUid, "group_aggregate_group_uid_key").IsUnique();

            entity.HasIndex(e => e.GroupId, "idx_group_aggregate_group_id");

            entity.HasIndex(e => e.GroupUid, "idx_group_aggregate_group_uid");

            entity.Property(e => e.GroupAggregateId).HasColumnName("group_aggregate_id");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line_1");
            entity.Property(e => e.AddressLine2).HasColumnName("address_line_2");
            entity.Property(e => e.ClosedDate).HasColumnName("closed_date");
            entity.Property(e => e.CompaniesHouseNumber).HasColumnName("companies_house_number");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupStatusEffectiveDate).HasColumnName("group_status_effective_date");
            entity.Property(e => e.GroupStatusLabel).HasColumnName("group_status_label");
            entity.Property(e => e.GroupTypeCode).HasColumnName("group_type_code");
            entity.Property(e => e.GroupTypeName).HasColumnName("group_type_name");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.HeadteacherIdentifier).HasColumnName("headteacher_identifier");
            entity.Property(e => e.HeadteacherName).HasColumnName("headteacher_name");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OpenedDate).HasColumnName("opened_date");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.SiteName).HasColumnName("site_name");
            entity.Property(e => e.TelephoneNumber).HasColumnName("telephone_number");
            entity.Property(e => e.Town).HasColumnName("town");
            entity.Property(e => e.Ukprn).HasColumnName("ukprn");
            entity.Property(e => e.Website).HasColumnName("website");
        });

        modelBuilder.Entity<GroupDownloadAggregate>(entity =>
        {
            entity.HasKey(e => e.GroupDownloadAggregateId).HasName("group_download_aggregate_pkey");

            entity.ToTable("group_download_aggregate", "core");

            entity.HasIndex(e => e.GroupUid, "group_download_aggregate_group_uid_key").IsUnique();

            entity.Property(e => e.GroupDownloadAggregateId).HasColumnName("group_download_aggregate_id");
            entity.Property(e => e.ClosedDate).HasColumnName("closed_date");
            entity.Property(e => e.CompaniesHouseNumber).HasColumnName("companies_house_number");
            entity.Property(e => e.GroupAddressLine1).HasColumnName("group_address_line_1");
            entity.Property(e => e.GroupAddressLine2).HasColumnName("group_address_line_2");
            entity.Property(e => e.GroupAddressLine3).HasColumnName("group_address_line_3");
            entity.Property(e => e.GroupCounty).HasColumnName("group_county");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupName).HasColumnName("group_name");
            entity.Property(e => e.GroupPostcode).HasColumnName("group_postcode");
            entity.Property(e => e.GroupStatus).HasColumnName("group_status");
            entity.Property(e => e.GroupStatusId).HasColumnName("group_status_id");
            entity.Property(e => e.GroupTownOrCity).HasColumnName("group_town_or_city");
            entity.Property(e => e.GroupType).HasColumnName("group_type");
            entity.Property(e => e.GroupTypeId).HasColumnName("group_type_id");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.HeadOfGroupFirstName).HasColumnName("head_of_group_first_name");
            entity.Property(e => e.HeadOfGroupLastName).HasColumnName("head_of_group_last_name");
            entity.Property(e => e.HeadOfGroupTitle).HasColumnName("head_of_group_title");
            entity.Property(e => e.IncorporatedOn).HasColumnName("incorporated_on");
            entity.Property(e => e.OpenDate).HasColumnName("open_date");
            entity.Property(e => e.UkProviderReferenceNumber).HasColumnName("uk_provider_reference_number");
        });

        modelBuilder.Entity<GroupEstablishmentAggregate>(entity =>
        {
            entity.HasKey(e => e.GroupEstablishmentAggregateId).HasName("group_establishment_aggregate_pkey");

            entity.ToTable("group_establishment_aggregate", "core");

            entity.HasIndex(e => e.EstablishmentUrn, "idx_group_establishment_aggregate_establishment_urn");

            entity.HasIndex(e => e.GroupUid, "idx_group_establishment_aggregate_group_uid");

            entity.Property(e => e.GroupEstablishmentAggregateId).HasColumnName("group_establishment_aggregate_id");
            entity.Property(e => e.EstablishmentName).HasColumnName("establishment_name");
            entity.Property(e => e.EstablishmentUrn).HasColumnName("establishment_urn");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
        });

        modelBuilder.Entity<GroupGovernorAggregate>(entity =>
        {
            entity.HasKey(e => e.GroupGovernorAggregateId).HasName("group_governor_aggregate_pkey");

            entity.ToTable("group_governor_aggregate", "core");

            entity.HasIndex(e => e.GovernorId, "idx_group_governor_aggregate_governor_id");

            entity.HasIndex(e => e.GroupUid, "idx_group_governor_aggregate_group_uid");

            entity.Property(e => e.GroupGovernorAggregateId).HasColumnName("group_governor_aggregate_id");
            entity.Property(e => e.AppointingBody).HasColumnName("appointing_body");
            entity.Property(e => e.AppointmentEndDate).HasColumnName("appointment_end_date");
            entity.Property(e => e.AppointmentStartDate).HasColumnName("appointment_start_date");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.GovernorId).HasColumnName("governor_id");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.RoleOrder).HasColumnName("role_order");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<GroupMemberAggregate>(entity =>
        {
            entity.HasKey(e => e.GroupMemberAggregateId).HasName("group_member_aggregate_pkey");

            entity.ToTable("group_member_aggregate", "core");

            entity.HasIndex(e => e.EstablishmentUrn, "idx_group_member_aggregate_establishment_urn");

            entity.HasIndex(e => e.GroupUid, "idx_group_member_aggregate_group_uid");

            entity.Property(e => e.GroupMemberAggregateId).HasColumnName("group_member_aggregate_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EstablishmentName).HasColumnName("establishment_name");
            entity.Property(e => e.EstablishmentUrn).HasColumnName("establishment_urn");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<SearchAggregate>(entity =>
        {
            entity.HasKey(e => e.SearchAggregateId).HasName("search_aggregate_pkey");

            entity.ToTable("search_aggregate", "core");

            entity.HasIndex(e => e.Admissions, "idx_search_provider_admissions");

            entity.HasIndex(e => e.AgeHigh, "idx_search_provider_age_high");

            entity.HasIndex(e => e.AgeLow, "idx_search_provider_age_low");

            entity.HasIndex(e => e.Boarding, "idx_search_provider_boarding");

            entity.HasIndex(e => e.CompaniesHouseNumber, "idx_search_provider_companies_house_number");

            entity.HasIndex(e => e.County, "idx_search_provider_county");

            entity.HasIndex(e => e.DfeNumber, "idx_search_provider_dfe_number");

            entity.HasIndex(e => e.Diocese, "idx_search_provider_diocese");

            entity.HasIndex(e => e.EducationPhase, "idx_search_provider_education_phase");

            entity.HasIndex(e => e.FurtherEducationType, "idx_search_provider_further_education_type");

            entity.HasIndex(e => e.Gender, "idx_search_provider_gender");

            entity.HasIndex(e => e.GroupType, "idx_search_provider_group_type");

            entity.HasIndex(e => e.GroupUid, "idx_search_provider_group_uid");

            entity.HasIndex(e => e.ProviderId, "idx_search_provider_id");

            entity.HasIndex(e => e.LaEstab, "idx_search_provider_laestab");

            entity.HasIndex(e => e.LocalAuthorityName, "idx_search_provider_local_authority_name");

            entity.HasIndex(e => e.ProviderName, "idx_search_provider_name");

            entity.HasIndex(e => e.ProviderName, "idx_search_provider_name_trgm")
                .HasMethod("gin")
                .HasOperators(new[] { "gin_trgm_ops" });

            entity.HasIndex(e => e.Nursery, "idx_search_provider_nursery");

            entity.HasIndex(e => e.Postcode, "idx_search_provider_postcode");

            entity.HasIndex(e => e.ReligiousCharacter, "idx_search_provider_religious_character");

            entity.HasIndex(e => e.ReligiousEthos, "idx_search_provider_religious_ethos");

            entity.HasIndex(e => e.Section41, "idx_search_provider_section_41");

            entity.HasIndex(e => e.Sen, "idx_search_provider_sen").HasMethod("gin");

            entity.HasIndex(e => e.SixthForm, "idx_search_provider_sixth_form");

            entity.HasIndex(e => e.SpecialClass, "idx_search_provider_special_class");

            entity.HasIndex(e => e.StatusCode, "idx_search_provider_status_code");

            entity.HasIndex(e => e.StatusName, "idx_search_provider_status_name");

            entity.HasIndex(e => e.Town, "idx_search_provider_town");

            entity.HasIndex(e => e.UkProviderReferenceNumber, "idx_search_provider_ukprn");

            entity.HasIndex(e => e.ProviderId, "search_aggregate_provider_id_key").IsUnique();

            entity.Property(e => e.SearchAggregateId).HasColumnName("search_aggregate_id");
            entity.Property(e => e.AcademyCounts).HasColumnName("academy_counts");
            entity.Property(e => e.Admissions).HasColumnName("admissions");
            entity.Property(e => e.AgeHigh).HasColumnName("age_high");
            entity.Property(e => e.AgeLow).HasColumnName("age_low");
            entity.Property(e => e.Boarding).HasColumnName("boarding");
            entity.Property(e => e.ClosedDate).HasColumnName("closed_date");
            entity.Property(e => e.CompaniesHouseNumber).HasColumnName("companies_house_number");
            entity.Property(e => e.County).HasColumnName("county");
            entity.Property(e => e.DfeNumber).HasColumnName("dfe_number");
            entity.Property(e => e.Diocese).HasColumnName("diocese");
            entity.Property(e => e.EducationPhase).HasColumnName("education_phase");
            entity.Property(e => e.FurtherEducationType).HasColumnName("further_education_type");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupName).HasColumnName("group_name");
            entity.Property(e => e.GroupStatus).HasColumnName("group_status");
            entity.Property(e => e.GroupType).HasColumnName("group_type");
            entity.Property(e => e.GroupUid).HasColumnName("group_uid");
            entity.Property(e => e.LaEstab).HasColumnName("la_estab");
            entity.Property(e => e.LocalAuthorityName).HasColumnName("local_authority_name");
            entity.Property(e => e.Nursery).HasColumnName("nursery");
            entity.Property(e => e.OpenedDate).HasColumnName("opened_date");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.ProviderAddress).HasColumnName("provider_address");
            entity.Property(e => e.ProviderCategory).HasColumnName("provider_category");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.ProviderName).HasColumnName("provider_name");
            entity.Property(e => e.ProviderTypeName).HasColumnName("provider_type_name");
            entity.Property(e => e.ReligiousCharacter).HasColumnName("religious_character");
            entity.Property(e => e.ReligiousEthos).HasColumnName("religious_ethos");
            entity.Property(e => e.Section41).HasColumnName("section_41");
            entity.Property(e => e.Sen).HasColumnName("sen");
            entity.Property(e => e.SixthForm).HasColumnName("sixth_form");
            entity.Property(e => e.SpecialClass).HasColumnName("special_class");
            entity.Property(e => e.StatusCode).HasColumnName("status_code");
            entity.Property(e => e.StatusName).HasColumnName("status_name");
            entity.Property(e => e.Town).HasColumnName("town");
            entity.Property(e => e.UkProviderReferenceNumber).HasColumnName("uk_provider_reference_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
