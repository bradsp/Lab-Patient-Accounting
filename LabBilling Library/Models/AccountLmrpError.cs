﻿using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("ACC_LMRP")]
[PrimaryKey("uri", AutoIncrement = true)]
public sealed class AccountLmrpError : IBaseEntity
{
    [Column("account")]
    public string AccountNo { get; set; }

    [Column("dos")]
    public DateTime DateOfService { get; set; }

    [Column("fin_code")]
    public string FinancialCode { get; set; }

    [Column("cl_mnem")]
    public string ClientMnem { get; set; }

    [Column("erorr")]
    public string Error { get; set; }
    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }

    public int uri { get; set; }

    [Ignore]
    public Guid rowguid { get; set; }
}
