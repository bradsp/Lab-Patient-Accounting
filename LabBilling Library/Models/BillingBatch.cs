using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.Models;

[TableName("data_billing_batch")]
[PrimaryKey("batch", AutoIncrement = true)]
public sealed class BillingBatch : IBaseEntity
{

    [Column("batch")]
    public double Batch { get; set; }

    [Column("run_date")]
    public DateTime RunDate { get; set; }

    [Column("run_user")]
    public string RunUser { get; set; }

    [Column("x12_text")]
    public string X12Text { get; set; }

    [Column("claim_count")]
    public int ClaimCount { get; set; }

    [Column("TotalBilled")]
    public double TotalBilled { get; set; }

    [Column("BatchType")]
    public string BatchType { get; set; }

    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    public Guid rowguid { get; set; }

    [Ignore]
    public List<BillingActivity> BillingActivities { get; set; } = new List<BillingActivity>();
}
