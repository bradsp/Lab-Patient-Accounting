using System;
using System.Text;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("chrg_details")]
    [PrimaryKey("uri",AutoIncrement = true)]
    public sealed class ChrgDetail : IBaseEntity
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
        [Column("mt_req_no")]
        public string LISReqNo { get; set; }
        [Column("order_code")]
        public string OrderCode { get; set; }
        [Column("pointer_set")]
        public bool PointerSet { get; set; }
        [Column("mod_date")]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        [Ignore]
        public string UpdatedHost { get; set; }

        [Column("uri")]
        public int uri { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }

        [Ignore]
        public string CptDescription { get; set; }

        [Ignore]
        public RevenueCode RevenueCodeDetail { get; set; }
        [Ignore]
        public ChrgDiagnosisPointer DiagnosisPointer { get; set; } = new ChrgDiagnosisPointer();
        [Ignore]
        public string DiagCodePointer
        {
            get
            {
                if (this.DiagnosisPointer == null)
                    return "";
                else
                    return this.DiagnosisPointer.DiagnosisPointer ?? "";
            }
        }

        public override string ToString()
        { 
            string retVal = $"{Cpt4}|{Type}|{Amount}|{ChrgNo}";
            return retVal;
        }

    }
}
