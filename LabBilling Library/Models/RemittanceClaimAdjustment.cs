using System;
using PetaPoco;

namespace LabBilling.Core.Models;

[TableName("remittance_claim_adjustment")]
[PrimaryKey("Id", AutoIncrement = true)]
public class RemittanceClaimAdjustment : IBaseEntity
{
    public int Id { get; set; }
    public int RemittanceClaimDetailId { get; set; }
    public string ClaimAdjustmentGroupCode { get; set; }
    public string AdjustmentReasonCode { get; set; }
    public decimal AdjustmentAmount { get; set; }
    public int AdjustmentQuantity { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }

    [Ignore]
    public Guid rowguid { get; set; }
}
