using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[PetaPoco.TableName("patdx")]
[PrimaryKey("uid", AutoIncrement = true)]
public sealed class PatDx : IBaseEntity
{
    [Column("deleted")]
    public bool IsDeleted { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("dx_number")]
    public int DxNumber { get; set; }
    [Column("diagnosis")]
    public string Diagnosis { get; set; }
    [Column("version")]
    public string Version { get; set; }
    [Column("code_qualifier")]
    public string CodeQualifier { get; set; }
    [Column("is_error")]
    public bool IsError { get; set; }
    [Column("import_file")]
    public string ImportFile { get; set; }
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }

    [Column("posted_date")]
    public DateTime? PostedDate { get; set; }
    public int uid { get; set; }

    [Ignore]
    public string DxDescription { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }

}
