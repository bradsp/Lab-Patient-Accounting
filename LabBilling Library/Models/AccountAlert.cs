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

        [Column("mod_date")]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        public string UpdatedHost { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
