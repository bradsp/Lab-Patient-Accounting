using LabBilling.Core.Models;
using System.Collections.Generic;

namespace LabBilling.Core.Services;
public interface IBatchTransactionService
{
    bool DeletePaymentBatch(int batchNo);
    bool DeletePaymentBatchDetail(int detailId);
    decimal GetNextBatchNumber();
    List<ChkBatch> GetOpenPaymentBatches();
    ChkBatch GetPaymentBatchById(int batchNo);
    void PostBatchPayments(IList<Chk> chks);
    void PostBatchPayments(int batchNo);
    ChkBatch SavePaymentBatch(ChkBatch chkBatch);
    ChkBatchDetail SavePaymentBatchDetail(ChkBatchDetail detail);
}