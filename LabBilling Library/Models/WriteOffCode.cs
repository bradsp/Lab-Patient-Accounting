using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.dict_write_off_codes")]
    [PrimaryKey("write_off_code", AutoIncrement = false)]
    public class WriteOffCode : IBaseEntity
    {
        [Column("write_off_code")]
        public string Code { get; set; }

        [Column("write_off_description")]
        public string Description { get; set; }

        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
