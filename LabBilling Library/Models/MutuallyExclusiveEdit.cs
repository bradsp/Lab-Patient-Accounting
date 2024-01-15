using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.dict_C_MEEDIT")]
    [PrimaryKey("MutexEditId", AutoIncrement = true)]
    public sealed class MutuallyExclusiveEdit : IBaseEntity
    {
        [Column("ME_1")]
        public string Cpt1 { get; set; }
        [Column("ME_2")]
        public string Cpt2 { get; set; }
        [Column("effective_date")]
        public string EffectiveDate { get; set; }
        [Column("deletion_date")]
        public string DeletionDate { get; set; }
        [Column("prior_rebundled_code_indicator")]
        public string PriorRebundledCodeIndicator { get; set; }
        [Column("standard_policy_statement")]
        public string StandardPolicyStatement { get; set; }
        [Column("cci_indicator")]
        public string CCIIndicator { get; set; }
        public int MutexEditId { get; set; }
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
