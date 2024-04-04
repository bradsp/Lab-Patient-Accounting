using System.Collections.Generic;
using System.Linq;

namespace LabBilling.Core.Models;

public sealed class UnbilledClient
{
    public bool SelectForInvoice { get; set; }
    public string ClientMnem { get; set; }
    public string ClientName { get; set; }
    public string ClientType { get; set; }
    public double UnbilledAmount
    {
        get
        {
            return UnbilledAccounts.Sum(x => x.UnbilledAmount);
        }
    }
    public double PriorBalance { get; set; }

    public List<UnbilledAccounts> UnbilledAccounts { get; set; }

}
