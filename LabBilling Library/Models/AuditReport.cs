using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models;

[TableName("dictionary.Monthly_Reports")]
[PrimaryKey("id", AutoIncrement = true)]
public class AuditReport : IBaseEntity
{
    [Column("mi_name")]
    public string ReportName { get; set; }
    [Column("sql_code")]
    public string ReportCode { get; set; }
    [Column("report_title")]
    public string ReportTitle { get; set; }
    [Column("comments")]
    public string Comments { get; set; }
    [Column("button")]
    public string Button { get; set; }
    [Column("child_button")]
    public bool IsChildButton { get; set; }
    [Column("id")]
    public int Id { get; set; }

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
