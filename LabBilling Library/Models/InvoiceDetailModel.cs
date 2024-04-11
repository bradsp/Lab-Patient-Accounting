using System;
using System.Collections.Generic;

namespace LabBilling.Core.Models;

/// <summary>
/// 
/// </summary>
public sealed class InvoiceDetailModel
{
    public string Account { get; set; }
    public string PatientName { get; set; }
    public DateTime ServiceDate { get; set; }
    public double AccountTotal { get; set; }

    public List<InvoiceDetailLinesModel> InvoiceDetailLines { get; set; } = new List<InvoiceDetailLinesModel>();
}
