﻿using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("chk_batch_details")]
[PrimaryKey("id", AutoIncrement = true)]
public sealed class ChkBatchDetail : IBaseEntity
{
    [Column("batch")]
    public double Batch { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("chk_no")]
    public string CheckNo { get; set; }
    [Column("source")]
    public string Source { get; set; }
    [Column("status")]
    public string Status { get; set; }
    [Column("contractual")]
    public double Contractual { get; set; }
    [Column("amt_paid")]
    public double AmtPaid { get; set; }
    [Column("write_off")]
    public double WriteOffAmount { get; set; }
    [Column("w_off_date")]
    public DateTime? WriteOffDate { get; set; }
    [Column("write_off_code")]
    public string WriteOffCode { get; set; }
    [Column("comment")]
    public string Comment { get; set; }
    [Column("chk_date")]
    public DateTime? CheckDate { get; set; }
    [Column("date_rec")]
    public DateTime? DateReceived { get; set; }
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    [Column("id")] public double Id { get; set; }

    [Ignore]
    public string PatientName { get; set; }
    [Ignore]
    public double Balance { get; set; }

    [Ignore]
    public Guid rowguid { get; set; }
}
