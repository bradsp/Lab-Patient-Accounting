using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Core.Models;

[TableName("remittance_file")]
[PrimaryKey("Id", AutoIncrement = true)]
public class RemittanceFile : IBaseEntity
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public DateTime ProcessedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }
    public List<RemittanceClaim> Claims { get; set; } = new List<RemittanceClaim>();
    [Ignore]
    public Guid rowguid { get; set; }
}
