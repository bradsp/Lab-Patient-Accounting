using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("icd9desc")]
    [PrimaryKey("id", AutoIncrement = true)]
    public class DictDx : IBaseEntity
    {
        public string icd9_num { get; set; }
        public string icd9_desc { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string version { get; set; }
        public string AMA_year { get; set; }
        public bool deleted { get; set; }
        public int id { get; set; }

        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
