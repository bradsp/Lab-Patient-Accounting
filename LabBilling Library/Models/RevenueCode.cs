using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.revcode")]
    [PrimaryKey("code", AutoIncrement = false)]
    public sealed class RevenueCode : IBaseEntity
    {

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
