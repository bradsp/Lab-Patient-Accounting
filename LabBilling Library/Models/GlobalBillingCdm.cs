using PetaPoco;
using System;
using ColumnAttribute = PetaPoco.ColumnAttribute;

namespace LabBilling.Core.Models;

[TableName("dictionary.dict_global_billing_cdms")]
[PrimaryKey("rowguid", AutoIncrement = false)]
public sealed class GlobalBillingCdm : IBaseEntity
{
    [Column("rowguid")]
    public Guid rowguid { get; set; }
    [Column("cdm")]
    public string Cdm { get; set; }
    [Column("comment")]
    public string Comment { get; set; }
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    [Column("effective_date")]
    public DateTime EffectiveDate { get; set; }
    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }

}
