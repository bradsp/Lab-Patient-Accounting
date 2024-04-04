using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dict_general_ledger_codes")]
[PrimaryKey("level_1", AutoIncrement = false)]
public sealed class GLCode : IBaseEntity
{
    public string gl_account_code { get; set; }
    public string level_1 { get; set; }
    public string level_2 { get; set; }
    public string level_3 { get; set; }
    public string description { get; set; }

    [Ignore]
    public DateTime UpdatedDate { get; set; }
    [Ignore]
    public string UpdatedUser { get; set; }
    [Ignore]
    public string UpdatedApp { get; set; }
    [Ignore]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }

}
