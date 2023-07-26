using System;
using System.Text;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("charge_details")]
    [PrimaryKey("uri",AutoIncrement = true)]
    public sealed class ChrgDetail : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }

        [Column("chrg_num")]
        public int ChrgNo { get; set; }
        [Column("revcode")]
        public string RevenueCode { get; set; }
        [Column("billcode")]
        public string BillingCode { get; set; }
        [Column("cpt4")]
        public string Cpt4 { get; set; }
        [Column("modi")]
        public string Modifier { get; set; }
        [Column("modi2")]
        public string Modifer2 { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("qty")]
        public int Quantity { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
        [Column("discount_amount")]
        public double DiscountAmount { get; set; }


        [Column("posted_date")]
        public DateTime PostedDate { get; set; }

        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [Column("mod_user")]
        public string mod_user { get; set; }
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [Column("mod_host")]
        public string mod_host { get; set; }


        [Column("uri")]
        public int uri { get; set; }

        [Column("cl_mnem")]
        public string ClientMnem { get; set; }
        [Column("fin_code")]
        public string FinCode { get; set; }
        [Column("invoice")]
        public string Invoice { get; set; }
        [Column("fin_type")]
        public string FinancialType { get; set; }
        [Column("credited")]
        public bool IsCredited { get; set; }



        [Ignore]
        public RevenueCode RevenueCodeDetail { get; set; }
        //[Ignore]
        //public ChrgDiagnosisPointer DiagnosisPointer { get; set; } = new ChrgDiagnosisPointer();
        //[Ignore]
        //public string DiagCodePointer
        //{
        //    get
        //    {
        //        if (this.DiagnosisPointer == null)
        //            return "";
        //        else
        //            return this.DiagnosisPointer.DiagnosisPointer ?? "";
        //    }
        //}

        public override string ToString()
        { 
            string retVal = $"{Cpt4}|{Type}|{Amount}|{ChrgNo}";
            return retVal;
        }

    }

    public static class ChrgDetailStatus
    {
        public const string TC = "TC";
        public const string PC = "PC";
        public const string Norm = "NORM";
        public const string NA = "N/A";
        public const string Invoice = "INV";

        public static string GetStatus(string type)
        {
            switch (type)
            {
                case "TC":
                    return "TC";
                case "PC":
                    return "PC";
                case "NORM":
                    return "NORM";
                case "N/A":
                    return "N/A";
                case "INV":
                    return "INV";
                default:
                    return "NORM";
            }
        }
    }

}
