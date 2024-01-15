using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.Models
{
    [TableName("client")]
    [PrimaryKey("cli_mnem",AutoIncrement = false)]
    public sealed class Client : IBaseEntity
    {

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cli_mnem")]
        public string ClientMnem { get; set; }

        [Column("cli_nme")]
        public string Name { get; set; }

        [Column("addr_1")]
        public string StreetAddress1 { get; set; }

        [Column("addr_2")]
        public string StreetAddress2 { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("st")]
        public string State { get; set; }

        [Column("zip")]
        public string ZipCode { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("fax")]
        public string Fax { get; set; }

        [Column("contact")]
        public string Contact { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("per_disc")]
        public double DefaultDiscount { get; set; }

        [Column("last_discount")]
        public double LastDiscount { get; set; }

        [Column("last_invoice")]
        public string LastInvoice { get; set; }

        [Column("last_invoice_date")]
        public DateTime? LastInvoiceDate { get; set; }

        [Column("mro_name")]
        public string MroName { get; set; }

        [Column("mro_addr1")]
        public string MroStreetAddress1{ get; set; }

        [Column("mro_addr2")]
        public string MroStreetAddress2 { get; set; }

        [Column("mro_city")]
        public string MroCity { get; set; }

        [Column("mro_st")]
        public string MroState { get; set; }

        [Column("mro_zip")]
        public string MroZipCode { get; set; }

        [Column("prn_loc")]
        public string prn_loc { get; set; }

        [Column("route")]
        public string Route { get; set; }

        [Column("county")]
        public string County { get; set; }

        [Column("email")]
        public string ContactEmail { get; set; }

        [Column("late_notice")]
        public string LateNotice { get; set; }

        [Column("late_notice_date")]
        public DateTime? LateNoticeDate { get; set; }

        [Column("statsFacility")]
        public string StatsFacility { get; set; }

        [Column("fee_schedule")]
        public string FeeSchedule { get; set; }

        [Column("client_class")]
        public string ClientClass { get; set; }

        [Column("client_maint_rep")]
        public string ClientMaintenanceRep { get; set; }

        [Column("client_sales_rep")]
        public string CLientSalesRep { get; set; }

        [Column("date_ord")]
        public bool PrintInvoiceInDateOrder { get; set; }
        [Column("print_cc")]
        public bool IncludeOnChargeCodeReport { get; set; }

        [Column("bill_at_disc")]
        public bool ShowDiscountedAmtOnBill { get; set; }

        [Column("do_not_bill")]
        public bool DoNotBill { get; set; }

        [Column("commission")]
        public bool Commission { get; set; }

        [Column("outpatient_billing")]
        public bool OutpatientBilling { get; set; }
        [Column("prn_cpt4")]
        public bool PrintCptOnInvoice { get; set; }

        [Column("electronic_billing_type")]
        public string ElectronicBillingType { get; set; }

        [Column("gl_code")]
        public string GlCode { get; set; }

        [Column("facilityNo")]
        public string FacilityNo { get; set; }

        [Column("bill_pc_charges")]
        public string BillProfCharges { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("old_phone")]
        public string OldPhone { get; set; }

        [Column("old_fax")]
        public string OldFax { get; set; }

        [Column("bill_to_client")]
        public string BillToClient { get; set; }

        [Column("bill_method")]
        public string BillMethod { get; set; }

        [Column("mod_date")]
        [ResultColumn]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        [ResultColumn]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        [ResultColumn]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        [ResultColumn]
        public string UpdatedHost { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }

        [Ignore]
        public List<ClientDiscount> Discounts { get; set; }
        [Ignore]
        public ClientType ClientType { get; set; }
        [Ignore]
        public List<Mapping> Mappings { get; set; } = new List<Mapping>();
    }
}
