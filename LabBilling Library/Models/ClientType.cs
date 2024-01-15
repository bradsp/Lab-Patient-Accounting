using PetaPoco;
using System;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.clienttype")]
    [PrimaryKey("type", AutoIncrement = false)]
    public sealed class ClientType : IBaseEntity
    {
        [Column("type")]
        public int Type { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("admission_source_cd")]
        public string AdmissionSourceCode { get; set; }

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
