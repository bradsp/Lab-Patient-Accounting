using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dict_general_ledger_codes")]
    [PrimaryKey("level_1",AutoIncrement = false)]
    public class GLCode : IBaseEntity
    {
        public string gl_account_code { get; set; }
        public string level_1 { get; set; }
        public string level_2 { get; set; }
        public string level_3 { get; set; }
        public string description { get; set; }

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
