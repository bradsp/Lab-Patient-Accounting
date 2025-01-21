using System;
using System.Collections.Generic;

namespace LabBilling.Core.Services
{
    public class Loop2100
    {
        public string AccountNo { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientMiddleName { get; set; }
        public string PatientGender { get; set; }
        public DateTime? PatientDateOfBirth { get; set; }
        public string ClaimStatusCode { get; set; }
        public string ClaimChargeAmount { get; set; }
        public string ClaimPaymentAmount { get; set; }
        public string PatientResponsibilityAmount { get; set; }
        public string ClaimFilingIndicatorCode { get; set; }
        public string PayerClaimControlNumber { get; set; }
        public string FacilityTypeCode { get; set; }
        public string ClaimFrequencyCode { get; set; }
        public string PaidAmount { get; set; }
        public string AllowedAmount { get; set; }

        public List<Loop2110> Loop2110s { get; set; } = new List<Loop2110>();
    }




}
