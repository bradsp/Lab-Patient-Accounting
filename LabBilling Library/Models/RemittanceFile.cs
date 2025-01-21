using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Core.Models;

[TableName("remittance_file")]
[PrimaryKey("RemittanceId", AutoIncrement = true)]
public class RemittanceFile : IBaseEntity
{
    public int RemittanceId { get; set; }
    public string FileName { get; set; }
    public DateTime ProcessedDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public string Payer { get; set; }
    public string TransactionTraceNumber { get; set; }
    public int ClaimCount { get; set; }
    public decimal TotalChargeAmount { get; set; }
    public decimal TotalPaymentAmount { get; set; }
    public decimal TotalPatientResponsibilityAmount { get; set; }
    public decimal TotalPaidAmount { get; set; }
    public decimal TotalAllowedAmount { get; set; }
    public string RemittanceData { get; set; }

    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }

    public string PostingUser { get; set; }
    public string PostingHost { get; set; }

    [Ignore]
    public List<RemittanceClaim> Claims { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
       


    public RemittanceFile()
    {
        Claims = new List<RemittanceClaim>();
    }
}
