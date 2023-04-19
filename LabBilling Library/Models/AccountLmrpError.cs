using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("ACC_LMRP")]
    [PrimaryKey("uri", AutoIncrement = true)]
    public sealed class AccountLmrpError : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }

        [Column("dos")] 
        public DateTime DateOfService { get; set; }

        [Column("fin_code")]
        public string FinancialCode { get; set; }

        [Column("cl_mnem")]
        public string ClientMnem { get; set; }

        [Column("erorr")]
        public string Error { get; set; }
        
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }

        public int uri { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
