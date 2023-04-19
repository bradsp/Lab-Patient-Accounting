namespace LabBilling.Core.Models
{
    public sealed class UnbilledClient
    {
        public bool SelectForInvoice { get; set; }
        public string ClientMnem { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public double UnbilledAmount { get; set; }
        public double PriorBalance { get; set; }
    }
}
