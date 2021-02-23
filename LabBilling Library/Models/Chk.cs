using System;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("chk")]
    [PrimaryKey("pay_no",AutoIncrement = true)]
    public class Chk : IBaseEntity
    {
        public bool deleted { get; set; }
        public double pay_no { get; set; }
        public string account { get; set; }
        public DateTime? chk_date { get; set; }
        public DateTime? date_rec { get; set; }
        public string chk_no { get; set; }
        public double amt_paid { get; set; }
        public double write_off { get; set; }
        public double contractual { get; set; }
        public string status { get; set; } = "NEW";
        public string source { get; set; }
        public string fin_code { get; set; }
        public DateTime? w_off_date { get; set; }
        public string invoice { get; set; }
        public double batch { get; set; }
        public string comment { get; set; }
        public bool bad_debt { get; set; }
        public string cpt4Code { get; set; }
        public string post_file { get; set; }
        public string write_off_code { get; set; }
        public DateTime? eft_date { get; set; }
        public string eft_number { get; set; }
        public DateTime? post_date { get; set; }
        public string ins_code { get; set; }
        public string claim_adj_code { get; set; }
        public string claim_adj_group_code { get; set; }
        public string facility_code { get; set; }
        public string claim_no { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }
        [ResultColumn]
        public DateTime? mod_date_audit { get; set; }

        public Guid rowguid { get; set; }
        public Guid chrg_rowguid { get; set; }
    }
}
