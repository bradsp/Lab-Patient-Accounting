using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("chrg_details")]
    [PrimaryKey("uri",AutoIncrement = true)]
    public class ChrgDetail : IBaseEntity
    {

        [Column("chrg_num")]
        public int ChrgNo { get; set; }
        [Column("revcode")]
        public string RevenueCode { get; set; }
        [Column("cpt4")]
        public string Cpt4 { get; set; }
        [Column("modi")]
        public string Modifier { get; set; }
        [Column("modi2")]
        public string Modifer2 { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
        //[Column("diagnosis_code_ptr")]
        //public string DiagCodePointer { get; set; }
        [Column("mt_req_no")]
        public string LISReqNo { get; set; }
        [Column("order_code")]
        public string OrderCode { get; set; }
        [Column("bill_type")]
        public string BillType { get; set; }
        [Column("bill_method")]
        public string BillMethod { get; set; }
        [Column("pointer_set")]
        public bool PointerSet { get; set; }
        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [Column("mod_user")]
        public string mod_user { get; set; }
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("uri")]
        public int uri { get; set; }

        [Ignore]
        [Column("mod_host")]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

        [Ignore]
        public RevenueCode RevenueCodeDetail { get; set; }
        [Ignore]
        public ChrgDiagnosisPointer DiagnosisPointer { get; set; } = new ChrgDiagnosisPointer();

    }
}
