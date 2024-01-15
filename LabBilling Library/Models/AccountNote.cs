using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("notes")]
    [PrimaryKey("id", AutoIncrement = true)]
    public sealed class AccountNote : IBaseEntity
    {

        [Column("account")]
        public string Account { get; set; }

        [Column("comment")]
        public string Comment { get; set; }
        [Column("mod_date")]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        public string UpdatedHost { get; set; }
        public Guid rowguid { get; set; }
        [Column("id")]
        public double Id { get; set; }
    }
}
