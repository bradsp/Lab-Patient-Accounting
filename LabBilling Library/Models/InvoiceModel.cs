using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace LabBilling.Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InvoiceModel
    {
        public StatementTypeEnum StatementType { get; set; }

        //Invoice header information
        public string ClientMnem { get; set; }
        public string ImageFilePath { get; set; }
        public string ClientName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double InvoiceTotal { get; set; }
        public DateTime BalanceForwardDate { get; set; }
        public double BalanceForward { get; set; }
        public double BalanceDue { get; set; }
        public double DiscountTotal { get; set; }
        public DateTime ThroughDate { get; set; }

        public string BillingCompanyName { get; set; }
        public string BillingCompanyAddress { get; set; }
        public string BillingCompanyCity { get; set; }
        public string BillingCompanyState { get; set; }
        public string BillingCompanyZipCode { get; set; }
        public string BillingCompanyPhone { get; set; }

        public List<InvoiceDetailModel> InvoiceDetails;

        public List<ClientStatementDetailModel> ClientStatementDetails;

        public enum StatementTypeEnum
        {
            Invoice,
            Statement
        }

        public bool ShowCpt { get; set; }

       
    }
}
