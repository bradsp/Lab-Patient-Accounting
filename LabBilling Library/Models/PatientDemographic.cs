﻿using PetaPoco;
using System;

namespace LabBilling.Core.Models;


[TableName("infce.patient_demographics")]
[PrimaryKey("systemMsgId", AutoIncrement = true)]
public sealed class PatientDemographic : IBaseEntity
{
    public string enctr_sys { get; set; } // varchar(25), null
    public string client { get; set; } // varchar(max), null
    public string account { get; set; } // varchar(16), null
    public string service_date { get; set; } // varchar(8), null
    public string acc_outreach { get; set; } // varchar(50), null
    public string acc_fin_nbr { get; set; } // varchar(50), null
    public string systemMsgId { get; set; } // varchar(36), not null
    public string hne { get; set; } // varchar(25), null
    public string mrn { get; set; } // varchar(25), null
    public string mrn_outreach { get; set; } // varchar(50), null
    public string outreach_personID { get; set; } // varchar(50), null
    public string PATIENT { get; set; } // varchar(133), null
    public string pat_dob { get; set; } // varchar(8), null
    public string visit_id { get; set; } // varchar(40), null
    public string xmlContents { get; set; } // XML(.), null
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

