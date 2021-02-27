using System;

namespace LabBilling.Core.Models
{
    public interface IBaseEntity
    {
        //tables based on this interface will implement audit fields
        DateTime? mod_date { get; set; }
        string mod_user { get; set; }
        string mod_prg { get; set; }
        string mod_host { get; set; }
        Guid rowguid { get; set; }
    }
}
