using PetaPoco;
using System;

namespace LabBilling.Models
{
    [TableName("client")]
    [PrimaryKey("cli_mnem",AutoIncrement = false)]
    public class Client : IBaseEntity
    {
        public bool deleted { get; set; }
        public string cli_mnem { get; set; }
        public string cli_nme { get; set; }
        public string addr_1 { get; set; }
        public string addr_2 { get; set; }
        public string city { get; set; }
        public string st { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string contact { get; set; }
        public string comment { get; set; }
        public int type { get; set; }
        public double per_disc { get; set; }
        public double last_discount { get; set; }
        public string last_invoice { get; set; }
        public DateTime? last_invoice_date { get; set; }
        public string mro_name { get; set; }
        public string mro_addr1 { get; set; }
        public string mro_addr2 { get; set; }
        public string mro_city { get; set; }
        public string mro_st { get; set; }
        public string mro_zip { get; set; }
        public string prn_loc { get; set; }
        public string route { get; set; }
        public string county { get; set; }
        public string email { get; set; }
        public string late_notice { get; set; }
        public DateTime? late_notice_date { get; set; }
        public string statsFacility { get; set; }
        public string fee_schedule { get; set; }
        public string client_class { get; set; }
        public string client_maint_rep { get; set; }
        public string client_sales_rep { get; set; }
        [Column("date_ord")]
        public bool PrintInvoiceInDateOrder { get; set; }
        [Column("print_cc")]
        public bool IncludeOnChargeCodeReport { get; set; }
        public bool bill_at_disc { get; set; }
        public bool do_not_bill { get; set; }
        public bool commission { get; set; }
        public bool outpatient_billing { get; set; }
        [Column("prn_cpt4")]
        public bool PrintCptOnInvoice { get; set; }
        public string electronic_billing_type { get; set; }
        public string gl_code { get; set; }
        public string facilityNo { get; set; }
        public string bill_pc_charges { get; set; }
        public string notes { get; set; }
        public string old_phone { get; set; }
        public string old_fax { get; set; }
        public string bill_to_client { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }

    }
}
