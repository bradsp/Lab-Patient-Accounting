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
    public sealed class DictDx : IBaseEntity
    {
        [Column("icd9_num")]
        public string DxCode { get; set; }
        [Column("icd9_desc")]
        public string Description { get; set; }
        [Column("version")] 
        public string Version { get; set; }
        [Column("AMA_year")] 
        public string AmaYear { get; set; }
        [Column("deleted")]
        public bool IsDeleted { get; set; }
        [Column("id")]
        public int Id { get; set; }

        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
