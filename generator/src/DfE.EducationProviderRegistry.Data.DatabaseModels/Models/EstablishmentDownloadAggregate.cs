using System;
using System.Collections.Generic;

namespace DfE.EducationProviderRegistry.Data.DatabaseModels.Models;

public partial class EstablishmentDownloadAggregate
{
    public long EstablishmentDownloadAggregateId { get; set; }

    public long? Urn { get; set; }

    public string? EstablishmentNumber { get; set; }

    public string? ProviderName { get; set; }

    public int? LocalAuthorityId { get; set; }

    public string? LocalAuthority { get; set; }

    public int? PreviousLocalAuthorityId { get; set; }

    public string? PreviousLocalAuthority { get; set; }

    public int? EducationPhaseId { get; set; }

    public string? EducationPhase { get; set; }

    public int? ProviderTypeId { get; set; }

    public string? ProviderType { get; set; }

    public string? FurtherEducationType { get; set; }

    public bool? ApprovedIndependentSendSchoolSection41 { get; set; }

    public int? ProviderStatusId { get; set; }

    public string? ProviderStatus { get; set; }

    public DateOnly? OpenDate { get; set; }

    public int? ReasonProviderOpenedId { get; set; }

    public string? ReasonProviderOpened { get; set; }

    public DateOnly? ClosedDate { get; set; }

    public int? ReasonProviderClosedId { get; set; }

    public string? ReasonProviderClosed { get; set; }

    public long? UniqueReferenceNumber { get; set; }

    public long? UkProviderReferenceNumber { get; set; }

    public string? ProviderNumber { get; set; }

    public string? PreviousProviderNumber { get; set; }

    public int? NumberOfPupilsStudents { get; set; }

    public int? NumberOfMales { get; set; }

    public int? NumberOfFemales { get; set; }

    public int? ProviderCapacity { get; set; }

    public int? PupilsEligibleForFreeSchoolMeals { get; set; }

    public decimal? PercentagePupilsEligibleForFreeSchoolMeals { get; set; }

    public int? SpecialPupilsOnStatementOrEhcPlan { get; set; }

    public int? SpecialPupilsNotOnStatementOrEhcPlan { get; set; }

    public string? VisualImpairmentSen1 { get; set; }

    public string? HearingImpairmentSen2 { get; set; }

    public string? SpeechLanguageCommunicationSen3 { get; set; }

    public string? AutisticSpectrumDisorderSen4 { get; set; }

    public string? SocialEmotionalMentalHealthSen5 { get; set; }

    public string? MultiSensoryImpairmentSen6 { get; set; }

    public string? PhysicalDisabilitySen7 { get; set; }

    public string? ModerateLearningDifficultySen8 { get; set; }

    public string? SevereLearningDifficultySen9 { get; set; }

    public string? ProfoundMultipleLearningDifficultySen10 { get; set; }

    public string? SpecificLearningDifficultySen11 { get; set; }

    public string? OtherDifficultyDisabilitySen12 { get; set; }

    public string? TypeOfResourcedProvision { get; set; }

    public int? ResourcedProvisionPupilCount { get; set; }

    public int? ResourcedProvisionCapacity { get; set; }

    public int? SenUnitPupilCount { get; set; }

    public int? SenUnitCapacity { get; set; }

    public string? HeadTitle { get; set; }

    public string? HeadFirstName { get; set; }

    public string? HeadLastName { get; set; }

    public string? JobTitle { get; set; }

    public string? UniquePropertyReferenceNumber { get; set; }

    public string? Proprietor { get; set; }

    public string? Site { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? AddressLine3 { get; set; }

    public string? TownOrCity { get; set; }

    public string? County { get; set; }

    public string? Postcode { get; set; }

    public string? Country { get; set; }

    public string? Website { get; set; }

    public string? PhoneNumber { get; set; }

    public string? InspectorateName { get; set; }

    public string? BritishSchoolOverseasInspectorateName { get; set; }

    public string? InspectorateReport { get; set; }

    public DateOnly? DateOfLastInspectionVisit { get; set; }

    public DateOnly? DateOfNextInspectionVisit { get; set; }

    public int? SixthFormProvisionId { get; set; }

    public string? SixthFormProvision { get; set; }

    public bool? NurseryProvision { get; set; }

    public int? StatutoryLowAge { get; set; }

    public int? StatutoryHighAge { get; set; }

    public int? SpecialClassProvisionId { get; set; }

    public string? SpecialClassProvision { get; set; }

    public int? AdmissionsPolicyId { get; set; }

    public string? AdmissionsPolicy { get; set; }

    public int? GenderOfEntryId { get; set; }

    public string? GenderOfEntry { get; set; }

    public bool? BoardingEstablishment { get; set; }

    public int? BoardingProvisionId { get; set; }

    public string? BoardingProvision { get; set; }

    public DateOnly? CensusDate { get; set; }

    public DateOnly? DateLastChangedOrConfirmed { get; set; }

    public string? DistrictAdministrativeId { get; set; }

    public string? DistrictAdministrative { get; set; }

    public string? AdministrativeWardId { get; set; }

    public string? AdministrativeWard { get; set; }

    public string? ParliamentaryConstituencyId { get; set; }

    public string? ParliamentaryConstituency { get; set; }

    public string? UrbanRuralClassificationId { get; set; }

    public string? UrbanRuralClassification { get; set; }

    public string? GovernmentStatisticalServiceLocalAuthorityCode { get; set; }

    public decimal? Easting { get; set; }

    public decimal? Northing { get; set; }

    public string? MiddleSuperOutputAreaId { get; set; }

    public string? MiddleSuperOutputArea { get; set; }

    public string? LowerSuperOutputAreaId { get; set; }

    public string? LowerSuperOutputArea { get; set; }

    public string? GovernmentOfficeRegionId { get; set; }

    public string? GovernmentOfficeRegion { get; set; }

    public int? ReligiousCharacterId { get; set; }

    public string? ReligiousCharacter { get; set; }

    public string? ReligiousEthos { get; set; }

    public int? DioceseId { get; set; }

    public string? Diocese { get; set; }

    public int? ProviderTypeGroupId { get; set; }

    public string? ProviderTypeGroup { get; set; }

    public int? TrustSchoolFlagId { get; set; }

    public string? TrustSchoolFlag { get; set; }

    public string? TrustsId { get; set; }

    public string? Trusts { get; set; }

    public bool? FederationFlag { get; set; }

    public int? FederationId { get; set; }

    public string? Federation { get; set; }

    public string? CompaniesHouseNumber { get; set; }

    public int? QualificationsAwardingBodyId { get; set; }

    public string? QualificationsAwardingBody { get; set; }

    public string? QualificationsAwardingBodyReport { get; set; }

    public int? ProviderAccreditedId { get; set; }

    public string? ProviderAccredited { get; set; }

    public DateOnly? AccreditationExpiryDate { get; set; }

    public bool? SchoolSponsorFlag { get; set; }

    public string? SchoolSponsor { get; set; }

    public string? FurtherEducationHigherEducationIdentifier { get; set; }

    public bool? TeenMothersProvision { get; set; }

    public int? TeenMothersCapacity { get; set; }

    public bool? ChildcareFacilitiesProvision { get; set; }

    public bool? SenPupilReferralUnit { get; set; }

    public bool? EmotionalBehaviouralDifficultiesProvision { get; set; }

    public int? PupilReferralUnitCapacity { get; set; }

    public bool? FullTimeProvision { get; set; }

    public string? PupilsEducatedByOtherProviders { get; set; }
}
