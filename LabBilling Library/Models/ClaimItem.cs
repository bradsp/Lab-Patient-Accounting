using PetaPoco;

namespace LabBilling.Core.Models;

public sealed class ClaimItem
{
    [Column("status")]
    public string Status { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("pat_name")]
    public string PatName { get; set; }
    [Column("ssn")]
    public string SocSecNum { get; set; }
    [Column("cl_mnem")]
    public string ClientMnem { get; set; }
    [Column("fin_code")]
    public string FinCode { get; set; }
    [Column("trans_date")]
    public string TransactionDate { get; set; }
    [Column("plan_nme")]
    public string InsPlanName { get; set; }

}
