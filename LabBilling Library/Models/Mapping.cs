using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dictionary.mapping")]
[PrimaryKey("uid", AutoIncrement = true)]
public sealed class Mapping : IBaseEntity
{
    [Column("return_value")]
    public string SystemKey { get; set; }
    [Column("return_value_type")]
    public string SystemType { get; set; }
    [Column("sending_system")]
    public string InterfaceName { get; set; }
    [Column("sending_value")]
    public string InterfaceAlias { get; set; }
    [Column("uid")]
    public int uid { get; set; }

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
