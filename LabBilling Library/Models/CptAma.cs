using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.cpt4_ama")]
    [PrimaryKey("cpt4", AutoIncrement = false)]
    public class CptAma : IBaseEntity
    {
        [Column("cpt4")]
        public string Cpt { get; set; }
        [Column("short_desc")]
        public string ShortDescription { get; set; }
        [Column("med_description")]
        public string MediumDescription { get; set; }

        [Ignore]
        public DateTime UpdatedDate { get; set; }
        [Ignore]
        public string UpdatedUser { get; set; }
        [Ignore]
        public string UpdatedApp { get; set; }
        [Ignore]
        public string UpdatedHost { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
