using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("infce.messages_inbound")]
[PrimaryKey("systemMsgId", AutoIncrement = true)]
public sealed class MessageInbound : IBaseEntity
{

    [Column("account_cerner")]
    public string SourceAccount { get; set; } // varchar(50), null

    [Column("sourceMsgId")]
    public double SourceMsgId { get; set; } // numeric(18,0), null

    [Column("sourceInfce")]
    public string SourceInfce { get; set; } // varchar(50), null

    [Column("msgType")]
    public string MessageType { get; set; } // varchar(20), null

    [Column("msgDate")]
    public DateTime MessageDate { get; set; } // datetime, null

    [Column("msgContent")]
    public string MessageContent { get; set; } // varchar(max), null

    [Column("processFlag")]
    public string ProcessFlag { get; set; } // varchar(5), null

    [Column("processStatusMsg")]
    public string ProcessStatusMsg { get; set; } // varchar(250), null

    [Column("systemMsgId")]
    public double SystemMsgId { get; set; } // numeric(18,0), not null

    [Column("dx_processed")]
    public bool DxProcessed { get; set; } // bit, null

    [Column("mod_date")]
    public DateTime UpdatedDate { get; set; } // datetime, null

    [Column("order_pat_id")]
    public string OrderPatId { get; set; } // varchar(50), null

    [Column("order_visit_id")]
    public string OrderVisitId { get; set; } // varchar(50), null

    [Column("DOS")]
    public DateTime DateOfService { get; set; } // datetime, null

    [Column("dx_processed_method")]
    public string DxProcessedMethod { get; set; } // varchar(50), null

    [Column("ins_fin_code")]
    public string InsFinCode { get; set; } // varchar(50), null

    [Column("HL7Message")]
    public string HL7Message { get; set; } // varchar(max), null

    [Column("errors")]
    public string Errors { get; set; } // varchar(max), null


    [Column("mod_user")]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    public string UpdatedHost { get; set; }
    [Ignore]
    public Guid rowguid { get; set; }
}

public sealed class MessageQueueCount
{
    public string MessageType { get; set; }
    public int QueueCount { get; set; }
}

