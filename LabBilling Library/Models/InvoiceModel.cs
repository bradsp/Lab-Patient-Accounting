using System;
using System.Collections.Generic;

namespace LabBilling.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    public class InvoiceModel
    {
        //Invoice header information
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
        public double BalanceForward { get; set; }
        public double BalanceDue { get; set; }

        public string BillingCompanyName { get; set; }
        public string BillingCompanyAddress { get; set; }
        public string BillingCompanyCity { get; set; }
        public string BillingCompanyState { get; set; }
        public string BillingCompanyZipCode { get; set; }
        public string BillingCompanyPhone { get; set; }

        public List<InvoiceDetailModel> InvoiceDetails;
    }

    /// <summary>
    /// 
    /// </summary>
    public class InvoiceDetailModel
    {
        public string Account { get; set; }
        public string PatientName { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string CDM { get; set; }
        public string CPT { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public double Amount { get; set; }
    }

    public class UnbilledClient
    {
        public bool SelectForInvoice { get; set; }
        public string ClientMnem { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public double UnbilledAmount { get; set; }
    }
}
