using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("insc")]
    [PrimaryKey("code", AutoIncrement = false)]
    public class InsCompany : IBaseEntity
    {
        public string code { get; set; }
        public string name { get; set; }
        public string addr1 { get; set; }
        public string addr2 { get; set; }
        public string citystzip { get; set; }
        public string provider_no_qualifier { get; set; }
        public string provider_no { get; set; }
        public string payer_no { get; set; }
        public string claimsnet_payer_id { get; set; }
        public string bill_form { get; set; }
        public int num_labels { get; set; }
        public string fin_code { get; set; }
        public string comment { get; set; }
        public bool is_mc_hmo { get; set; }
        public bool allow_outpatient_billing { get; set; }
        public string payor_code { get; set; }
        public string fin_class { get; set; }
        public bool bill_as_jmcgh { get; set; }
        public Guid rowguid { get; set; }
        public bool deleted { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

    }
}
