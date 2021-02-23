using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("fin")]
    [PrimaryKey("fin_code", AutoIncrement = false)]
    public class Fin : IBaseEntity
    {
        public string fin_code { get; set; }
        public string res_party { get; set; }
        public string form_type { get; set; }
        public string chrgsource { get; set; }
        public string type { get; set; }
        public string h1500 { get; set; }
        public string ub92 { get; set; }
        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        public bool deleted { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
