using System;
using System.Runtime.Serialization;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using PetaPoco;
using RFClassLibrary;

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

        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }

    }

}
