using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    public class ClaimItem
    {
        public string status { get; set; }
        public string account { get; set; }
        public string pat_name { get; set; }
        public string ssn { get; set; }
        public string cl_mnem { get; set; }
        public string fin_code { get; set; }
        public string trans_date { get; set; }
        public string ins_plan_nme { get; set; }

    }
}
