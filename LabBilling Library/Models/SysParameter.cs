using System;
using System.Runtime.Serialization;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using PetaPoco;
using Utilities;

namespace LabBilling.Core.Models
{
    [TableName("system")]
    [PrimaryKey("KeyName", AutoIncrement = false)]
    public sealed class SysParameter : IBaseEntity
    {
        [Column("key_name")]
        public string key_name { get; set; }

        [Column("KeyName")]
        public string KeyName { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Column("programs")]
        public string Programs { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Column("update_prg")]
        public string UpdatePrg { get; set; }

        [Column("button")]
        public string Button { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("dataType")]
        public string DataType { get; set; }

        [Column("mod_date")]
        [ResultColumn]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        [ResultColumn]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        [ResultColumn]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        [ResultColumn]
        public string UpdatedHost { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }

    }

}
