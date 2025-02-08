using System;

namespace LabBilling.Core.Services;

public class ValidationUpdatedEventArgs : EventArgs
{
    public string AccountNo { get; set; }
    public string ValidationStatus { get; set; }
    public string UpdateMessage { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Processed { get; set; }
    public int TotalItems { get; set; }

}
