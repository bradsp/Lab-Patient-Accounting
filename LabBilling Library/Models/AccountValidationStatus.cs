using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("acc_validation_status")]
[PrimaryKey("account", AutoIncrement = false)]
public sealed class AccountValidationStatus : IBaseEntity
{
    [Column("account")]
    public string Account { get; set; }
    [Column("validation_text")]
    public string ValidationText { get; set; }
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
