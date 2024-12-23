using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;

namespace LabBilling.Core.Models;

[TableName("remittance_claim_detail")]
[PrimaryKey("Id", AutoIncrement = true)]
public class RemittanceClaimDetail : IBaseEntity
{
    public int Id { get; set; }
    public int RemittanceClaimId { get; set; }
    public string ProcedureCode { get; set; }
    public decimal LineItemChargeAmount { get; set; }
    public decimal MonetaryAmount { get; set; }
    public string RevenueCode { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal AllowedAmount { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }
    [Ignore]
    public List<RemittanceClaimAdjustment> Adjustments { get; set; } = new List<RemittanceClaimAdjustment>();

    [Ignore]
    public decimal ContractualAmount
    {
        get
        {
            return Adjustments.Where(a => a.ClaimAdjustmentGroupCode == "CO").Sum(a => a.AdjustmentAmount);
        }
    }
    
    [Ignore]
    public Guid rowguid { get; set; }
}
