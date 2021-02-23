using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("chrg")]
    [PrimaryKey("chrg_num", AutoIncrement = true)]
    public class Chrg : IBaseEntity
    {

        public bool credited { get; set; }
        public int chrg_num { get; set; }
        public string account { get; set; }
        public string status { get; set; }
        public DateTime? service_date { get; set; }
        public DateTime? hist_date { get; set; }
        public string cdm { get; set; }
        [ResultColumn]
        public string cdm_desc { get; set; }
        public int qty { get; set; }
        public double net_amt { get; set; }
        public string comment { get; set; }
        public string invoice { get; set; }
        public string fin_type { get; set; }
        public string mt_req_no { get; set; }
        public DateTime? post_date { get; set; }
        public string fin_code { get; set; }
        public string performing_site { get; set; }
        public string bill_method { get; set; }
        public string post_file { get; set; }
        public string lname { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string name_suffix { get; set; }
        public string name_prefix { get; set; }
        public string pat_name { get; set; }
        public string order_site { get; set; }
        public string pat_ssn { get; set; }
        public string unitno { get; set; }
        public string location { get; set; }
        public string responsiblephy { get; set; }
        public string mt_mnem { get; set; }
        public string action { get; set; }
        public string facility { get; set; }
        public string referencereq { get; set; }
        public DateTime? pat_dob { get; set; }
        public string chrg_err { get; set; }
        public string istemp { get; set; }
        [ResultColumn]
        public int age_on_date_of_service { get; set; }
        public double retail { get; set; }
        public double inp_price { get; set; }
        [ResultColumn]
        public double calc_amt { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }
        
        public Guid rowguid { get; set; }

        [Ignore]
        public List<Amt> ChrgDetails { get; set; } = new List<Amt>();
    }

    [TableName("InvoiceChargeView")]
    public class InvoiceChargeView : IBaseEntity
    {
        public string account { get; set; }
        public DateTime trans_date { get; set; }
        public int qty { get; set; }
        public double inp_amt { get; set; }
        public double retail { get; set; }
        public double amount { get; set; }
        public string cdm { get; set; }
        public string descript { get; set; }

        [Ignore]
        public DateTime? mod_date { get; set; }
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
