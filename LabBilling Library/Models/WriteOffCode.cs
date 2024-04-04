using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dictionary.dict_write_off_codes")]
[PrimaryKey("write_off_code", AutoIncrement = false)]
public sealed class WriteOffCode : IBaseEntity
{
    [Column("write_off_code")]
    public string Code { get; set; }

    [Column("write_off_description")]
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
