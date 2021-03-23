using System;
using System.Collections.Generic;
using PetaPoco;


namespace LabBilling.Core.Models
{
    [TableName("acc")]
    [PrimaryKey("account", AutoIncrement = false)]
    public class Account : IBaseEntity
    {
        public bool deleted { get; set; }
        public string account { get; set; }
        public string pat_name { get; set; }
        public string cl_mnem { get; set; }
        public string fin_code { get; set; }
        public DateTime? trans_date { get; set; }
        public DateTime? cbill_date { get; set; }
        public string status { get; set; }
        public string ssn { get; set; }
        public int num_comments { get; set; }
        public string meditech_account { get; set; }
        public string original_fincode { get; set; }
        public string oereqno { get; set; }
        public string mri { get; set; }
        public DateTime? post_date { get; set; }
        public string ov_order_id { get; set; }
        public string ov_pat_id { get; set; }
        public int bill_priority { get; set; }
        public string guarantorID { get; set; }
        public string HNE_NUMBER { get; set; }
        public DateTime? trans_date_time { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public Pat Pat { get; set; } = new Pat();
        [Ignore]
        public List<Ins> Insurances { get; set; } = new List<Ins>();
        [Ignore]
        public List<Chrg> Charges { get; set; } = new List<Chrg>();
        [Ignore]
        public List<Chk> Payments { get; set; } = new List<Chk>();
        [Ignore]
        public Guid rowguid { get; set; }

    }

    [TableName("acc")]
    [PrimaryKey("account",AutoIncrement = false)]
    public class AccountSummary
    {
        public bool deleted { get; set; }
        public string account { get; set; }
        public string pat_name { get; set; }
        public string cl_mnem { get; set; }
        public string fin_code { get; set; }
        public DateTime? trans_date { get; set; }
        public DateTime? cbill_date { get; set; }
        public string status { get; set; }
        public string ssn { get; set; }
        public int num_comments { get; set; }
        public string meditech_account { get; set; }
        public string original_fincode { get; set; }
        public string oereqno { get; set; }
        public string mri { get; set; }
        public DateTime? post_date { get; set; }
        public string ov_order_id { get; set; }
        public string ov_pat_id { get; set; }
        public int bill_priority { get; set; }
        public string guarantorID { get; set; }
        public string HNE_NUMBER { get; set; }
        public DateTime trans_date_time { get; set; }
        [ResultColumn]
        public double Balance { get; set; }
        [ResultColumn]
        public double TotalCharges { get; set; }
        [ResultColumn]
        public double TotalPayments { get; set; }
        [ResultColumn]
        public double TotalContractual { get; set; }
        [ResultColumn]
        public double TotalWriteOff { get; set; }
        [ResultColumn]
        public double TotalBadDebt { get; set; }
        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }


        [Ignore]
        public string FullInfo => $"{ account } { pat_name } { cl_mnem } { trans_date }";
        [ResultColumn]
        public string ClientName { get; set; }
    }

    [TableName("vw_cbill_select")]
    public class InvoiceSelect : IBaseEntity
    {
        public string account { get; set; }
        public string cl_mnem { get; set; }
        public DateTime trans_date { get; set; }
        public string pat_name { get; set; }
        public string fin_code { get; set; }

        [Ignore]
        public DateTime? mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

    [TableName("vw_cbill_select")]
    public class UnbilledAccounts
    {

        public string cl_mnem { get; set; }
        public string account { get; set; }
        public DateTime trans_date { get; set; }
        public string pat_name { get; set; }
        public string fin_code { get; set; }
        public double UnbilledAmount { get; set; }

    }
}
