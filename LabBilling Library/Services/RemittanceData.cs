using System;
using System.Collections.Generic;

namespace LabBilling.Core.Services;

public class RemittanceData
{
    public List<Loop2000> Loop2000s { get; set; } = new List<Loop2000>();

    public string TransactionHandlingCode { get; set; }
    public string TotalPremiumPaymentAmount { get; set; }
    public string CreditorDebitFlagCode { get; set; }
    public string PayerName { get; set; }
    public string PayeeName { get; set; }
    public string BankIdentifier { get; set; }
    public string PayerAddress { get; set; }
    public string PayerAddress2 { get; set; }
    public string PayeeAddress { get; set; }
    public string PayerCity { get; set; }
    public string PayerState { get; set; }
    public string PayerZip { get; set; }
    public string PayeeCity { get; set; }
    public string PayeeState { get; set; }
    public string PayeeZip { get; set; }
    public string PaidAmount { get; set; }
    public string AllowedAmount { get; set; }
    public string InterchangeControlNumber { get; set; }
    public string GroupControlNumber { get; set; }
    public string TransactionSetControlNumber { get; set; }
    public DateTime? PaymentDate { get; set; }

}
