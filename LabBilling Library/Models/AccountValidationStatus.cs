using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("acc_validation_status")]
    [PrimaryKey("account", AutoIncrement = false)]
    public sealed class AccountValidationStatus : IBaseEntity
    {

        public string account { get; set; }
        public string validation_text { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
