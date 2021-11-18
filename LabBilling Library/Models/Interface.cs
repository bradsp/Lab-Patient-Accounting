using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("infce.messages_inbound")]
    [PrimaryKey("systemMsgId", AutoIncrement=true)]
    public class MessagesInbound : IBaseEntity
    {
        public string account_cerner { get; set; } // varchar(50), null
        public decimal? sourceMsgId { get; set; } // numeric(18,0), null
        public string sourceInfce { get; set; } // varchar(50), null
        public string msgType { get; set; } // varchar(20), null
        public DateTime? msgDate { get; set; } // datetime, null
        public string msgContent { get; set; } // varchar(max), null
        public string processFlag { get; set; } // varchar(5), null
        public string processStatusMsg { get; set; } // varchar(250), null
        public decimal systemMsgId { get; set; } // numeric(18,0), not null
        public bool? dx_processed { get; set; } // bit, null
        public DateTime? mod_date { get; set; } // datetime, null
        public string order_pat_id { get; set; } // varchar(50), null
        public string order_visit_id { get; set; } // varchar(50), null
        public DateTime? DOS { get; set; } // datetime, null
        public string dx_processed_method { get; set; } // varchar(50), null
        public string ins_fin_code { get; set; } // varchar(50), null
        public string HL7Message { get; set; } // varchar(max), null
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

    [TableName("infce.messages_inbound_adt")]
    [PrimaryKey("systemMsgId", AutoIncrement = true)]
    public class MessagesInboundAdt : IBaseEntity
    {
        public string account_cerner { get; set; } // varchar(50), null
        public decimal? sourceMsgId { get; set; } // numeric(18,0), null
        public string sourceInfce { get; set; } // varchar(50), null
        public string msgType { get; set; } // varchar(20), null
        public DateTime? msgDate { get; set; } // datetime, null
        public string msgContent { get; set; } // varchar(max), null
        public string processFlag { get; set; } // varchar(5), null
        public string processStatusMsg { get; set; } // varchar(250), null
        public decimal systemMsgId { get; set; } // numeric(18,0), not null
        public bool? dx_processed { get; set; } // bit, null
        public DateTime? mod_date { get; set; } // datetime, null
        public string order_pat_id { get; set; } // varchar(50), null
        public string order_visit_id { get; set; } // varchar(50), null
        public DateTime? DOS { get; set; } // datetime, null
        public string dx_processed_method { get; set; } // varchar(50), null
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

    [TableName("infce.messages_inbound_webconnect")]
    [PrimaryKey("systemMsgId", AutoIncrement = true)]
    public class MessagesInboundWebconnect : IBaseEntity
    {
        public string account_cerner { get; set; } // varchar(50), null
        public decimal? sourceMsgId { get; set; } // numeric(18,0), null
        public string sourceInfce { get; set; } // varchar(50), null
        public string msgType { get; set; } // varchar(20), null
        public DateTime? msgDate { get; set; } // datetime, null
        public string msgContent { get; set; } // varchar(max), null
        public string processFlag { get; set; } // varchar(5), null
        public string processStatusMsg { get; set; } // varchar(250), null
        public decimal systemMsgId { get; set; } // numeric(18,0), not null
        public bool? dx_processed { get; set; } // bit, null
        public DateTime? mod_date { get; set; } // datetime, null
        public string order_pat_id { get; set; } // varchar(50), null
        public string order_visit_id { get; set; } // varchar(50), null
        public DateTime? DOS { get; set; } // datetime, null
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

    [TableName("infce.messages_outbound")]
    [PrimaryKey("systemMsgId", AutoIncrement = true)]
    public class MessagesOutbound : IBaseEntity
    {
        public string account_cerner { get; set; } // varchar(50), null
        public decimal? sourceMsgId { get; set; } // numeric(18,0), null
        public string sourceInfce { get; set; } // varchar(50), null
        public string msgType { get; set; } // varchar(20), null
        public DateTime? msgDate { get; set; } // datetime, null
        public string msgContent { get; set; } // varchar(max), null
        public string processFlag { get; set; } // varchar(5), null
        public string processStatusMsg { get; set; } // varchar(250), null
        public decimal systemMsgId { get; set; } // numeric(18,0), not null
        public bool? dx_processed { get; set; } // bit, null
        public DateTime? mod_date { get; set; } // datetime, null
        public string order_pat_id { get; set; } // varchar(50), null
        public string order_visit_id { get; set; } // varchar(50), null
        public DateTime? DOS { get; set; } // datetime, null
        public string dx_processed_method { get; set; } // varchar(50), null
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

    [TableName("infce.patient_demographics")]
    [PrimaryKey("systemMsgId", AutoIncrement = true)]
    public class PatientDemographic : IBaseEntity
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
        public DateTime? mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}

