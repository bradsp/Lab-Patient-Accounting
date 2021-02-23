using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("phy")]
    [PrimaryKey("uri", AutoIncrement = true)]
    public class Phy : IBaseEntity
    {
        public Guid rowguid { get; set; }
        public bool deleted { get; set; }
        public string upin { get; set; }
        public string ub92_upin { get; set; }
        public string tnh_num { get; set; }
        public string billing_npi { get; set; }
        public string pc_code { get; set; }
        public string cl_mnem { get; set; }
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string mid_init { get; set; }
        public string group1 { get; set; }
        public string addr_1 { get; set; }
        public string addr_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string reserved { get; set; }
        public int num_labels { get; set; }
        public DateTime? mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public double uri { get; set; }
        public string mt_mnem { get; set; }
        public string credentials { get; set; }
        public string ov_code { get; set; }
        public string docnbr { get; set; }
        [Ignore]
        public string mod_host { get; set; }

        public Pth pth = new Pth();

    }
}
