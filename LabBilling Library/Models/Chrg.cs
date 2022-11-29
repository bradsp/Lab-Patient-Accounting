using System;
using System.Collections.Generic;
using System.Security.Policy;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("chrg")]
    [PrimaryKey("chrg_num", AutoIncrement = true)]
    public class Chrg : IBaseEntity
    {
        [Column("credited")]
        public bool IsCredited { get; set; }
        [Column("chrg_num")]
        public int ChrgId { get; set; }
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("service_date")]
        public DateTime? ServiceDate { get; set; }
        [Column("hist_date")]
        public DateTime? HistoryDate { get; set; }
        [Column("cdm")]
        public string CDMCode { get; set; }
        [Column("qty")]
        public int Quantity { get; set; }
        [Column("net_amt")]
        public double NetAmount { get; set; }
        [Column("comment")]
        public string Comment { get; set; }
        [Column("invoice")]
        public string Invoice { get; set; }
        [Column("fin_type")]
        public string FinancialType { get; set; }
        [Column("mt_req_no")]
        public string LISReqNo { get; set; }
        [Column("post_date")]
        public DateTime? PostingDate { get; set; }
        [Column("fin_code")]
        public string FinCode { get; set; }
        [Column("performing_site")]
        public string PerformingSite { get; set; }
        [Column("bill_method")]
        public string BillMethod { get; set; }
        [Column("post_file")]
        public string PostingFile { get; set; }
        [Column("lname")]
        public string PatLastName { get; set; }
        [Column("fname")]
        public string PatFirstName { get; set; }
        [Column("mname")]
        public string PatMiddleName { get; set; }
        [Column("name_suffix")]
        public string PatNameSuffix { get; set; }
        [Column("name_prefix")]
        public string PatNamePrefix { get; set; }
        [Column("pat_name")]
        public string PatFullName { get; set; }
        [Column("order_site")]
        public string OrderingSite { get; set; }
        [Column("pat_ssn")]
        public string PatSocSecNo { get; set; }
        [Column("unitno")]
        public string UnitNo { get; set; }
        [Column("location")]
        public string Location { get; set; }
        [Column("responsiblephy")]
        public string ResponsibleProvider { get; set; }
        [Column("mt_mnem")]
        public string OrderMnem { get; set; }
        [Column("action")]
        public string Action { get; set; }
        [Column("facility")]
        public string Facility { get; set; }
        [Column("referencereq")]
        public string ReferenceReq { get; set; }
        [Column("pat_dob")]
        public DateTime? PatBirthDate { get; set; }
        [Column("chrg_err")]
        public string ChrgError { get; set; }
        [Column("istemp")]
        public string IsTemp { get; set; }

        [ResultColumn]
        [Column("age_on_date_of_service")]
        public int AgeOnDateOfService { get; set; }
        [Column("retail")]
        public double RetailAmount { get; set; }
        [Column("inp_price")]
        public double HospAmount { get; set; }
        [ResultColumn]
        [Column("calc_amt")]
        public double CalculatedAmount { get; set; }

        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        public Guid rowguid { get; set; }

        [Ignore]
        public string CptList 
        { 
            get
            {
                if (ChrgDetails != null)
                {
                    string cptList = string.Empty;
                    ChrgDetails.ForEach(x => cptList += x.Cpt4 + ", ");

                    cptList = cptList.Remove(cptList.Length - 1);

                    return cptList;
                }

                return string.Empty;
            }
        }

        [Ignore]
        public string CdmDescription
        {
            get
            {
                if(this.Cdm != null)
                    return this.Cdm.Description;
                
                return string.Empty;
            }
        }
    
        [Ignore]        
        public List<ChrgDetail> ChrgDetails { get; set; } = new List<ChrgDetail>();

        [Ignore]
        public Cdm Cdm { get; set; } = new Cdm();
    }

    [TableName("InvoiceChargeView")]
    public class InvoiceChargeView : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }
        [Column("qty")]
        public int Quantity { get; set; }
        [Column("inp_amt")]
        public double HospAmount { get; set; }
        [Column("retail")]
        public double RetailAmount { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
        [Column("cdm")]
        public string ChargeItemId { get; set; }
        [Column("descript")]
        public string ChargeDescription { get; set; }
        [Column("cptList")]
        public string CptList { get; set; }

        [Ignore]
        public DateTime mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

    [TableName("StatementChargeView")]
    public class StatementChargeView : IBaseEntity
    {

        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }


        [Ignore]
        public DateTime mod_date { get; set; }
        [Ignore] 
        public string mod_user { get; set; }
        [Ignore] 
        public string mod_prg { get; set; }
        [Ignore] 
        public string mod_host { get; set; }
        [Ignore] 
        public Guid rowguid { get; set; }
    }

    public class ChrgChrgDetailRelator
    {
        public Chrg current;

        public Chrg MapIt(Chrg chrg, ChrgDetail a)
        {
            if (chrg == null)
                return current;

            if(current != null && current.ChrgId == chrg.ChrgId)
            {
                current.ChrgDetails.Add(a);

                return null;
            }

            var prev = current;

            current = chrg;
            current.ChrgDetails = new List<ChrgDetail>
            {
                a
            };

            return prev;
        }
    }
}
