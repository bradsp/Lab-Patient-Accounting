namespace LabBilling.Core.Models
{
    public sealed class InvoiceDetailLinesModel
    {
        public string CDM { get; set; }
        public string CPT { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public double Amount { get; set; }

    }
}
