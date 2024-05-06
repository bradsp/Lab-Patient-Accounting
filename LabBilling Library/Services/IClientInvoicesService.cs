using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;
public interface IClientInvoicesService
{
    event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceGenerated;
    event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceRunCompleted;
    event EventHandler<ClientInvoiceProgressEventArgs> ReportProgress;

    Task CompileAsync(DateTime thruDate, IList<UnbilledClient> unbilledClients, CancellationToken token);
    void GenerateInvoice(string clientMnemonic, DateTime throughDate, CancellationToken token);
    Task GenerateInvoiceAsync(string clientMnemonic, DateTime throughDate, CancellationToken token);
    InvoiceModel GenerateStatement(string clientMnemonic, DateTime asOfDate);
    List<InvoiceHistory> GetInvoiceHistory(string clientMnem = null, DateTime? fromDate = null, DateTime? thruDate = null, string invoice = null);
    List<ClientStatementDetailModel> GetStatementDetails(string clientMnem, DateTime asOfDate);
    List<UnbilledClient> GetUnbilledClients(DateTime throughDate, IProgress<int> progress);
    Task<List<UnbilledClient>> GetUnbilledClientsAsync(DateTime throughDate, IProgress<int> progress);
    bool UndoInvoice(string invoiceNo);
}