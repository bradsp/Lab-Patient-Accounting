using System;

namespace LabBilling.Core.Models;

public interface IBaseEntity
{
    //tables based on this interface will implement audit fields
    DateTime UpdatedDate { get; set; }
    string UpdatedUser { get; set; }
    string UpdatedApp { get; set; }
    string UpdatedHost { get; set; }
    Guid rowguid { get; set; }
}
