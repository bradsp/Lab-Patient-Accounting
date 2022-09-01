using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("notes")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class AccountNote : IBaseEntity
    {

        [Column("account")]
        public string Account { get; set; }

        [Column("comment")]
        public string Comment { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        public Guid rowguid { get; set; }
    }
}
