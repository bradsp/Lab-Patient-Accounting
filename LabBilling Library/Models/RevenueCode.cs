using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dictionary.revcode")]
[PrimaryKey("code", AutoIncrement = false)]
public sealed class RevenueCode : IBaseEntity
{

    [Column("code")]
    public string Code { get; set; }

    [Column("description")]
    public string Description { get; set; }

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
