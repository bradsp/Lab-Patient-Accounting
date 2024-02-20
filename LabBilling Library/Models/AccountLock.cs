using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models;

[TableName("acc_lock")]
[PrimaryKey("id", AutoIncrement = true)]
public class AccountLock : IBaseEntity
{
    public int id { get; set; }
    public string AccountNo { get; set; }
    public DateTime LockDateTime { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedUser { get; set; }
    public string UpdatedApp { get; set; }
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
}
