using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("phy")]
    [PrimaryKey("uri", AutoIncrement = true)]
    public class Phy : IBaseEntity
    {
        public Guid rowguid { get; set; }
        [Column("deleted")]
        public bool deleted { get; set; }
        [Column("upin")]
        public string upin { get; set; }
        [Column("ub92_upin")]
        public string ub92_upin { get; set; }
        [Column("tnh_num")]
        public string tnh_num { get; set; }
        [Column("billing_npi")]
        public string billing_npi { get; set; }
        [Column("pc_code")]
        public string pc_code { get; set; }
        [Column("cl_mnem")]
        public string cl_mnem { get; set; }
        [Column("last_name")]
        public string last_name { get; set; }
        [Column("first_name")]
        public string first_name { get; set; }
        [Column("mid_ini")]
        public string mid_init { get; set; }
        [Column("group1")]
        public string group1 { get; set; }
        [Column("addr_1")]
        public string addr_1 { get; set; }
        [Column("addr_2")]
        public string addr_2 { get; set; }
        [Column("city")]
        public string city { get; set; }
        [Column("state")]
        public string state { get; set; }
        [Column("zip")]
        public string zip { get; set; }
        [Column("phone")]
        public string phone { get; set; }
        [Column("reserved")]
        public string reserved { get; set; }
        [Column("num_labels")]
        public int num_labels { get; set; }
        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [Column("mod_user")]
        public string mod_user { get; set; }
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [Column("uri")]
        public double uri { get; set; }
        [Column("mt_mnem")]
        public string mt_mnem { get; set; }
        [Column("credentials")]
        public string credentials { get; set; }
        [Column("ov_code")]
        public string ov_code { get; set; }
        [Column("docnbr")]
        public string docnbr { get; set; }
        [Ignore]
        [Column("mod_host")]
        public string mod_host { get; set; }

        public Pth Pathologist { get; set; } = new Pth();

    }
}
