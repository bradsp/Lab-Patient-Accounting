using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("amt")]
    [PrimaryKey("uri",AutoIncrement = true)]
    public class ChrgDetail : IBaseEntity
    {
        public int chrg_num { get; set; }
        public string revcode { get; set; }
        public string cpt4 { get; set; }
        public string modi { get; set; }
        public string modi2 { get; set; }
        public string type { get; set; }
        public double amount { get; set; }
        public string diagnosis_code_ptr { get; set; }
        public string mt_req_no { get; set; }
        public string order_code { get; set; }
        public string bill_type { get; set; }
        public string bill_method { get; set; }
        public bool pointer_set { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public bool deleted { get; set; }

        public int uri { get; set; }

        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
