using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("icd9desc")]
[PrimaryKey("id", AutoIncrement = true)]
public sealed class DictDx : IBaseEntity
{
    [Column("icd9_num")]
    public string DxCode { get; set; }
    [Column("icd9_desc")]
    public string Description { get; set; }
    [Column("version")]
    public string Version { get; set; }
    [Column("AMA_year")]
    public string AmaYear { get; set; }
    [Column("deleted")]
    public bool IsDeleted { get; set; }
    [Column("id")]
    public int Id { get; set; }

    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }

    [Ignore]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }

}
