using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Core.Models;

[TableName("remittance_claim")]
[PrimaryKey("Id", AutoIncrement = true)]
public class RemittanceClaim : IBaseEntity
{
    public int Id { get; set; }
    public int RemittanceFileId { get; set; }
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
    public List<RemittanceClaimDetail> ClaimDetails { get; set; } = new List<RemittanceClaimDetail>();

    [Ignore]
    public Guid rowguid { get; set; }
}
