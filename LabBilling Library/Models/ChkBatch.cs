using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.Models;

[TableName("chk_batch")]
[PrimaryKey("BatchNo", AutoIncrement = true)]
public sealed class ChkBatch : IBaseEntity
{
    public int BatchNo { get; set; }
    public string User { get; set; }
    public DateTime BatchDate { get; set; }
    public string BatchData { get; set; }
    public DateTime? PostedDate { get; set; }

    [ResultColumn]
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [ResultColumn]
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [ResultColumn]
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [ResultColumn]
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }

    [Ignore]
    public List<ChkBatchDetail> ChkBatchDetails { get; set; } = new List<ChkBatchDetail>();
}
