using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("insc")]
    [PrimaryKey("code", AutoIncrement = false)]
    public class InsCompany : IBaseEntity
    {

        [Column("code")]
        public string InsuranceCode { get; set; }

        [Column("name")]
        public string PlanName { get; set; }

        [Column("addr1")]
        public string Address1 { get; set; }

        [Column("addr2")]
        public string Address2 { get; set; }

        [Column("citystzip")]
        public string CityStateZip { get; set; }
        [Column("city")]
        public string City { get; set; }
        [Column("state")]
        public string State { get; set; }
        [Column("zipcode")]
        public string Zip { get; set; }
        [Column("provider_no_qualifier")]
        public string ProviderNoQualifer { get; set; }

        [Column("provider_no")]
        public string ProviderNo { get; set; }

        [Column("payer_no")]
        public string PayerNo { get; set; }

        [Column("claimsnet_payer_id")]
        public string ClaimsNetPayerId { get; set; }

        [Column("bill_form")]
        public string BillForm { get; set; }

        [Column("fin_code")]
        public string FinancialCode { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Column("is_mc_hmo")]
        public bool IsMedicareHmo { get; set; }

        [Column("allow_outpatient_billing")]
        public bool AllowOutpatientBilling { get; set; }

        [Column("payor_code")]
        public string PayorCode { get; set; }

        [Column("nthrive_payer_no")]
        public string NThrivePayerNo { get; set; }

        [Column("fin_class")]
        public string FinancialClass { get; set; }

        [Column("bill_as_jmcgh")]
        public bool BillAsJmcgh { get; set; }

        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }


        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }



    }
}
