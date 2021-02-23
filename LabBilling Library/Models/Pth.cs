using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("pth")]
    [PrimaryKey("pc_code", AutoIncrement = true)]
    public class Pth : IBaseEntity
    {
        public bool deleted { get; set; }
        public int pc_code { get; set; }
        public string name { get; set; }
        public string mc_pin { get; set; }
        public string bc_pin { get; set; }
        public string tlc_num { get; set; }
        public DateTime? mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
