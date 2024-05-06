using LabBilling.Core.Models;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.Services;
public interface IHL7ProcessorService
{
    List<MessageInbound> GetMessages(DateTime fromDate, DateTime thruDate);
    void ProcessMessage(int systemMessageId);
    void ProcessMessages();
    MessageInbound SetMessageDoNotProcess(int systemMessageId, string statusMessage);
}