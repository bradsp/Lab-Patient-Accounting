using LabBilling.Core.Models;
using System;

namespace LabBilling.Core.Services;

public class AccountEventArgs : EventArgs
{
    public string AccountNo { get; set; }
    public string UpdateMessage { get; set; }
    public Account Account { get; set; }
}
