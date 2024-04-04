using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("cbill_hist")]
[PrimaryKey("invoice", AutoIncrement = false)]
public sealed class InvoiceHistory : IBaseEntity
{

    [Column("cl_mnem")]
    public string ClientMnem { get; set; }

    [ResultColumn]
    public string ClientName { get; set; }

    [Column("thru_date")]
    public DateTime? ThroughDate { get; set; }

    [Column("invoice")]
    public string InvoiceNo { get; set; }

    [Column("bal_forward")]
    public double BalanceForward { get; set; }

    [Column("total_chrg")]
    public double TotalCharges { get; set; }

    [Column("discount")]
    public double Discount { get; set; }

    [Column("balance_due")]
    public double BalanceDue { get; set; }

    [Column("payments")]
    public double Payments { get; set; }

    [Column("true_balance_due")]
    public double TrueBalanceDue { get; set; }

    [Column("cbill_html")]
    public string InvoiceData { get; set; }

    [Column("invoice_filename")]
    public string InvoiceFilename { get; set; }

    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }

    [Ignore]
    public Guid rowguid { get; set; }
}
