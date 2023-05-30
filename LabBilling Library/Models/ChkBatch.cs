using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("chk_batch")]
    [PrimaryKey("BatchNo", AutoIncrement = true)]
    public sealed class ChkBatch : IBaseEntity
    {
        public int BatchNo { get; set; }
        public string User { get; set; }
        public DateTime BatchDate { get; set; }
        public string BatchData { get; set; }
        public DateTime? PostedDate { get; set; }

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

        [Ignore]
        public List<ChkBatchDetail> ChkBatchDetails { get; set; } = new List<ChkBatchDetail>();
    }
}
