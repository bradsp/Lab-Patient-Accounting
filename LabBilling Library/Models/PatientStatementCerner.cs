using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("dbo.pat_statements_cerner")]
public sealed class PatientStatementCerner : IBaseEntity
{
    [Column("account")]
    public string Account { get; set; }
    [Column("batch_id")]
    public string BatchId { get; set; }
    [Column("statement_type")]
    public string StatementType { get; set; }
    [Column("statement_type_id")]
    public string StatementTypeId { get; set; }
    [Column("statement_text")]
    public string StatementText { get; set; }

    [Ignore]
    public DateTime UpdatedDate { get; set; }
    [Ignore]
    public string UpdatedUser { get; set; }
    [Ignore]
    public string UpdatedApp { get; set; }
    [Ignore]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
}
