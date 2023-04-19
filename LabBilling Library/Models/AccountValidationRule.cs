using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dict_acc_validation")]
    [PrimaryKey("rule_id", AutoIncrement = true)]
    public sealed class AccountValidationRule : IBaseEntity
    {
        public int rule_id { get; set; }
        public string type_check { get; set; }
        public bool valid { get; set; }
        public string strSql { get; set; }
        public string error { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_prg { get; set; }
        public string mod_user { get; set; }
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

        public List<AccountValidationCriteria> validationCriterion = new List<AccountValidationCriteria>();
    }

    [TableName("dict_acc_validation_criteria")]
    [PrimaryKey("uid", AutoIncrement = true)]
    public sealed class AccountValidationCriteria : IBaseEntity
    {
        public int rule_id { get; set; }
        public string fin_code { get; set; }
        public string ins_code { get; set; }
        public string bill_form { get; set; }
        public DateTime? effective_date { get; set; }
        public DateTime? expire_date { get; set; }
        public int uid { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_prg { get; set; }
        public string mod_user { get; set; }
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
