using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using System;

namespace LabBilling.Core.Services;

public class AddChargeParameters
{
    public Account Account { get; set; }
    public string AccountNumber { get; set; }
    public string Cdm { get; set; }
    public int Quantity { get; set; } = 1;
    public DateTime ServiceDate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string RefNumber { get; set; } = string.Empty;
    public double MiscAmount { get; set; } = 0.00;
    public IUnitOfWork Uow { get; set; } = null;
}