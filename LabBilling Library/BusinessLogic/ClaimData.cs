using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;

namespace LabBilling.Core
{
    public class ClaimData
    {
        public Account claimAccount;

        public string TransactionTypeCode { get; set; }
        public string TransactionSetPurpose { get; set; }

        public string SubmitterId { get; set; }
        public string SubmitterName { get; set; }
        public string SubmitterContactName { get; set; }
        public string SubmitterContactPhone { get; set; }
        public string SubmitterContactEmail { get; set; }

        public string ReceiverOrgName { get; set; }
        public string ReceiverId { get; set; }
        public string ProviderTaxonomyCode { get; set; }

        public string BillingProviderName { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipCode { get; set; }
        public string BillingProviderCountry { get; set; }
        public string BillingProviderTaxId { get; set; }
        public string BillingProviderUPIN { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProviderContactName { get; set; }
        public string BillingProviderContactPhone { get; set; }
        public string BillingProviderContactEmail { get; set; }

        public string PayToAddress { get; set; }
        public string PayToCity { get; set; }
        public string PayToState { get; set; }
        public string PayToZipCode { get; set; }
        public string PayToCountry { get; set; }

        public string PayToPlanName { get; set; }
        public string PayToPlanAddress { get; set; }
        public string PayToPlanCity { get; set; }
        public string PaytoPlanState { get; set; }
        public string PaytoPlanZipCode { get; set; }
        public string PayToPlanCountry { get; set; }
        public string PayToPlanPrimaryIdentifier { get; set; }
        public string PayToPlanSecondaryIdentifier { get; set; }
        public string PayToPlanTaxId { get; set; }

        public string ClaimIdentifier { get; set; }
        public string TotalChargeAmount { get; set; }
        public string FacilityCode { get; set; }
        public string ClaimFrequency { get; set; }
        public string ProviderSignatureIndicator { get; set; }
        public string ProviderAcceptAssignmentCode { get; set; }
        public string BenefitAssignmentCertificationIndicator { get; set; }
        public string ReleaseOfInformationCode { get; set; }
        public string PatientSignatureSourceCode { get; set; }
        public string RelatedCausesCode1 { get; set; }
        public string RelatedCausesCode2 { get; set; }
        public string RelatedCausesCode3 { get; set; }
        public string RelatedCausesStateCode { get; set; }
        public string RelatedCausesCountryCode { get; set; }
        public string SpecialProgramIndicator { get; set; }
        public string DelayReasonCode { get; set; }

        public DateTime? OnsetOfCurrentIllness { get; set; }
        public DateTime? InitialTreatmentDate { get; set; }
        public DateTime? DateOfAccident { get; set; }

        public double PatientAmountPaid { get; set; }
        public string CliaNumber { get; set; }

        public string ReferringProviderLastName { get; set; }
        public string ReferringProviderFirstName { get; set; }
        public string ReferringProviderMiddleName { get; set; }
        public string ReferringProviderSuffix { get; set; }
        public string ReferringProviderNPI { get; set; }

        public List<ClaimSubscriber> Subscribers { get; set; } = new List<ClaimSubscriber>();
        public List<ClaimLine> ClaimLines { get; set; } = new List<ClaimLine>();

        public ClaimData()
        {

        }

    }

    public class ClaimSubscriber
    {
        public string PayerResponsibilitySequenceCode { get; set; }
        public string IndividualRelationshipCode { get; set; }
        public string ReferenceIdentification { get; set; }
        public string PlanName { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string NameSuffix { get; set; }
        public string NamePrefix { get; set; }
        public string PrimaryIdentifier { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string SocSecNumber { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string PayerName { get; set; }
        public string PayerAddress { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZipCode { get; set; }
        public string PayerCountry { get; set; }
        public string PayerIdentifier { get; set; }
        public string PayerIdentificationQualifier { get; set; }
        public string InsuranceTypeCode { get; set; }
        public string CoordinationOfBenefitsCode { get; set; }
        public string ConditionResponseCode { get; set; }
        public string EmployementStatusCode { get; set; }
        public string ClaimFilingIndicatorCode { get; set; }

        public string BillingProviderSecondaryIdentifier { get; set; }

    }

    public class ClaimLine
    {
        public string ProcedureCode { get; set; }
        public string ProcedureModifier1 { get; set; }
        public string ProcedureModifier2 { get; set; }
        public string ProcedureModifier3 { get; set; }
        public string RevenueCode { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int Quantity { get; set; }
        public string DxPtr1 { get; set; }
        public string DxPtr2 { get; set; }
        public string DxPtr3 { get; set; }
        public string DxPtr4 { get; set; }
        public string EPSDTIndicator { get; set; }
        public string FamilyPlanningIndicator { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ControlNumber { get; set; }
    }

}
