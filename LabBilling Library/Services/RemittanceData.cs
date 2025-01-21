using System;
using System.Collections.Generic;
using System.Linq;

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
    public string PaidAmount
    {
        get
        {
            return Loop2000s.Sum(x => x.Loop2100s.Sum(y => Convert.ToDouble(y.PaidAmount))).ToString();
        }
    }
    public string AllowedAmount
    {
        get
        {
            return Loop2000s.Sum(x => x.Loop2100s.Sum(y => Convert.ToDouble(y.AllowedAmount))).ToString();
        }
    }
    public string InterchangeControlNumber { get; set; }
    public string GroupControlNumber { get; set; }
    public string TransactionSetControlNumber { get; set; }
    public string CurrentTransactionTraceNumber { get; set; }
    public DateTime? PaymentDate { get; set; }


    public string PayerContactName { get; set; }
    public string PayerContactPhone { get; set; }
    public string PayerContactEmail { get; set; }

}
