using PetaPoco;
using System;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.clienttype")]
    [PrimaryKey("type", AutoIncrement = false)]
    public class ClientType : IBaseEntity
    {
        [Column("type")]
        public int Type { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("admission_source_cd")]
        public string AdmissionSourceCode { get; set; }

        [Ignore]
        public DateTime mod_date { get; set; }
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
