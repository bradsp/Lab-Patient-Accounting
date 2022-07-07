using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("cbill_hist")]
    public class InvoiceHistory : IBaseEntity
    {
        public string cl_mnem { get; set; }
        [ResultColumn]
        public string ClientName { get; set; }
        public DateTime? thru_date { get; set; }
        public string invoice { get; set; }
        public double bal_forward { get; set; }
        public double total_chrg { get; set; }
        public double discount { get; set; }
        public double balance_due { get; set; }
        public double payments { get; set; }
        public double true_balance_due { get; set; }
        public string cbill_html { get; set; }
        public byte[] cbill_filestream { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
