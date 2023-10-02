using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("vw_chrg_bill")]
    public class ClaimChargeView : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }        
        [Column("qty")]
        public int Qty { get; set; }
        [Column("retail")]
        public double RetailAmount { get; set; }        
        [Column("amount")]
        public double Amount { get; set; }
        [Column("cdm")]
        public string ChargeId { get; set; }
        [Column("cpt4")]
        public string CptCode { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("modi")]
        public string Modifier { get; set; }
        [Column("revcode")]
        public string RevenueCode { get; set; }        
        [Column("modi2")]
        public string Modifier2 { get; set; }
        [Column("diagnosis_code_ptr")]
        public string DiagnosisCodePointer { get; set; }

        [Ignore]
        public RevenueCode RevenueCodeDetail { get; set; }
        [Ignore]
        public Cdm Cdm { get; set; }

        [Ignore]
        public DateTime mod_date { get; set; }
        [Ignore] 
        public string mod_user { get; set; }
        [Ignore] 
        public string mod_prg { get; set; }
        [Ignore] 
        public string mod_host { get; set; }
        [Ignore] 
        public Guid rowguid { get; set; }
    }
}
