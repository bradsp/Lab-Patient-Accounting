using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dict_acc_validation")]
    [PrimaryKey("rule_id", AutoIncrement = true)]
    public sealed class DictAccValidation : IBaseEntity
    {
        public int rule_id { get; set; } // int, not null
        public string type_check { get; set; } // varchar(50), null
        public bool valid { get; set; } // bit, not null
        public string strSql { get; set; } // varchar(8000), not null
        public string error { get; set; } // varchar(256), null
        public DateTime mod_date { get; set; } // datetime, not null
        public string mod_prg { get; set; } // varchar(50), not null
        public string mod_user { get; set; } // varchar(50), not null
        public string mod_host { get; set; } // varchar(50), not null

        [Ignore]
        public Guid rowguid { get; set; }

    }

    [TableName("dict_acc_validation_criteria")]
    [PrimaryKey("uid", AutoIncrement = true)]
    public sealed class DictAccValidationCriterion : IBaseEntity
    {
        public int rule_id { get; set; } // int, not null
        public string fin_code { get; set; } // varchar(10), null
        public string ins_code { get; set; } // varchar(50), null
        public string bill_form { get; set; } // varchar(50), null
        public DateTime effective_date { get; set; } // datetime, not null
        public DateTime? expire_date { get; set; } // datetime, null
        public int uid { get; set; } // int, not null
        public DateTime mod_date { get; set; } // datetime, not null
        public string mod_prg { get; set; } // varchar(50), not null
        public string mod_user { get; set; } // varchar(50), not null
        public string mod_host { get; set; } // varchar(50), not null

        [Ignore]
        public Guid rowguid { get; set; }

    }
}

