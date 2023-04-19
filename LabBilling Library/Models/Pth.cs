using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("pth")]
    [PrimaryKey("pc_code", AutoIncrement = true)]
    public sealed class Pth : IBaseEntity
    {

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("pc_code")]
        public int PathId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("mc_pin")]
        public string MedicarePin { get; set; }

        [Column("bc_pin")]
        public string BlueCrossPin { get; set; }

        [Column("tlc_num")]
        public string TLCNumber { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
