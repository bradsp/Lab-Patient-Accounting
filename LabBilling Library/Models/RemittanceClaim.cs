using System;
using System.Collections.Generic;
using LabBilling.Core.Services;
using System.Security.Claims;
using PetaPoco;

namespace LabBilling.Core.Models;

[TableName("remittance_claim")]
[PrimaryKey("ClaimId", AutoIncrement = true)]
public class RemittanceClaim : IBaseEntity
{
    public int RemittanceId { get; set; }
    public int ClaimId { get; set; }
    public string AccountNo { get; set; }
    public string ClaimStatusCode { get; set; }
    public decimal ClaimChargeAmount { get; set; }
    public decimal ClaimPaymentAmount { get; set; }
    public decimal PatientResponsibilityAmount { get; set; }
    public string ClaimFilingIndicatorCode { get; set; }
    public string PayerClaimControlNumber { get; set; }
    public string FacilityTypeCode { get; set; }
    public string ClaimFrequencyCode { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal AllowedAmount { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }
    [Ignore]
    public List<RemittanceClaimDetail> ClaimDetails { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
    [Ignore]
    public string PatientName { get; set; }
    [Ignore]
    public ClaimProcessStatus ProcessStatus
    {
        get
        {
            //write notes to the account showing the type of remittance being posted.
            // 1 = Process for check posting
            // 2 = Not Processed unless the amount paid is not 0.00 then processed
            // 3 = Not Processed
            // 4 = Denied 
            // 22 = if amount paid is not = 0 put on Processed otherwise Not Processed
            // 19 = Process for check posting
            // 23 = if amount paid is not = 0 put on Processed otherwise Not Processed 04/23/2008 we think
            // default goes to Not Processed
            switch (ClaimStatusCode)
            {
                case "1":
                case "19":
                    return ClaimProcessStatus.Process;
                case "2":
                case "3":
                    return ClaimProcessStatus.NotProcessed;
                case "4":
                    return ClaimProcessStatus.Denied;
                case "22":
                case "23":
                    if (PaidAmount != Convert.ToDecimal(0.00))
                        return ClaimProcessStatus.Process;
                    else
                        return ClaimProcessStatus.NotProcessed;
                default:
                    return ClaimProcessStatus.NotProcessed;
            }
        }
    }
    public RemittanceClaim()
    {
        ClaimDetails = new List<RemittanceClaimDetail>();
    }
}

public enum ClaimProcessStatus
{
    Process = 1,
    NotProcessed = 2,
    Denied = 3
}
