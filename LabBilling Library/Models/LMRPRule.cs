using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dictionary.lmrp")]
[PrimaryKey("uid", AutoIncrement = true)]
public sealed class LMRPRule : IBaseEntity
{
    [Column("cpt4")]
    public string CptCode { get; set; }
    [Column("beg_icd9")]
    public string BegDx { get; set; }
    [Column("end_icd9")]
    public string EndingDx { get; set; }
    [Column("payor")]
    public string Payor { get; set; }
    [Column("fincode")]
    public string FinCode { get; set; }
    [Column("rb_date")]
    public DateTime RBDate { get; set; }
    [Column("lmrp")]
    public string LmrpRule { get; set; }
    [Column("lmrp2")]
    public string LmrpRule2 { get; set; }
    [Column("rb_date2")]
    public DateTime RBDate2 { get; set; }
    [Column("chk_for_bad")]
    public int DxIsValid { get; set; }
    [Column("ama_year")]
    public string AmaYear { get; set; }
    public decimal uid { get; set; }
    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }

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

[TableName("dictionary.LMRPRuleDefinition")]
public class LMRPRuleDefinition
{
    [Column("cpt4")]
    public string CptCode { get; set; }
    [Column("payor")]
    public string Payor { get; set; }
    [Column("fincode")]
    public string FinCode { get; set; }
    [Column("rb_date")]
    public DateTime RBDate { get; set; }
    [Column("lmrp")]
    public string LmrpRule { get; set; }
    [Column("lmrp2")]
    public string LmrpRule2 { get; set; }
    [Column("chk_for_bad")]
    public int DxIsValid { get; set; }
    [Column("ama_year")]
    public string AmaYear { get; set; }
    [Column("expiration_date")]
    public DateTime ExpirationDate { get; set; }

}
