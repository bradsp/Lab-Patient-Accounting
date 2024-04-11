using PetaPoco;
using System;
using System.Security.Policy;

namespace LabBilling.Core.Models;

[TableName("chrg_diag_pointer")]
[PrimaryKey("id", AutoIncrement = true)]
public sealed class ChrgDiagnosisPointer : IBaseEntity
{
    [Column("id")]
    public int Id { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("cdm")]
    public string CdmCode { get; set; }
    [Column("cpt")]
    public string CptCode { get; set; }
    [Column("diag_ptr")]
    public string DiagnosisPointer { get; set; }

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
    [Ignore]
    public string CdmDescription { get; set; }
    [Ignore]
    public string CptDescription { get; set; }

}
