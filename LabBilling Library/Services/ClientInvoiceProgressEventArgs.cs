using System;

namespace LabBilling.Core.Services;

public class ClientInvoiceProgressEventArgs : EventArgs
{
    public string Client { get; set; } = string.Empty;
    public int ClientsProcessed { get; set; } = 0;
    public int ClientsTotal { get; set; } = 0;
    public int AccountsProcessed { get; set; } = 0;
    public int AccountsTotal { get; set; } = 0;
    public bool ReportingClient { get; set; } = false;
    public bool ReportingAccount { get; set; } = false;
}
