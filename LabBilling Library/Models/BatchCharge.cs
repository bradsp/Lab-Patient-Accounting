namespace LabBilling.Core.Models;

public sealed class BatchCharge
{
    public string AccountNo { get; set; }
    public string CDM { get; set; }
    public string ChargeDescription { get; set; }
    public int Qty { get; set; }
    //public string Comment { get; set; }
}
