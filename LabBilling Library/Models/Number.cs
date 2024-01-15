using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("number")]
    [PrimaryKey("keyfield", AutoIncrement = true)]
    public sealed class Number : IBaseEntity
    {
        public string keyfield { get; set; } // varchar(15), not null
        public decimal? cnt { get; set; } // numeric(15,0), null

        [Ignore]
        DateTime IBaseEntity.UpdatedDate { get; set; }
        [Ignore] 
        string IBaseEntity.UpdatedUser { get; set; }
        [Ignore] 
        string IBaseEntity.UpdatedApp { get; set; }
        [Ignore] 
        string IBaseEntity.UpdatedHost { get; set; }
        [Ignore] 
        Guid IBaseEntity.rowguid { get; set; }
    }
}
