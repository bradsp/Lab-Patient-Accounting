using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.ComponentModel;

namespace LabBilling.Core.Models
{
    [TableName("acc_alert")]
    [PrimaryKey("account", AutoIncrement = false)]
    public sealed class AccountAlert : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("alert")]
        public bool Alert { get; set; }

        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
