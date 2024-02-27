using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabBilling.Core.Services;

public class BatchTransactionService
{
    private IAppEnvironment appEnvironment;
    private AccountService accountService;

    public BatchTransactionService(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
        accountService = new(appEnvironment);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chkBatch"></param>
    /// <returns>Batch Number or -1 if error</returns>
    public ChkBatch SavePaymentBatch(ChkBatch chkBatch)
    {
        Log.Instance.Trace("Entering");
        using UnitOfWorkMain uow = new(appEnvironment, true);

        if (chkBatch.BatchNo > 0)
        {
            chkBatch = uow.ChkBatchRepository.Update(chkBatch);
            chkBatch.ChkBatchDetails.ForEach(d => uow.ChkBatchDetailRepository.Update(d));
            //save the chk details
            chkBatch.ChkBatchDetails.ForEach(x => x.Batch = chkBatch.BatchNo);
            chkBatch.ChkBatchDetails.ForEach(x => uow.ChkBatchDetailRepository.Save(x));
            uow.Commit();
            return chkBatch;
        }
        else
        {
            chkBatch = uow.ChkBatchRepository.Add(chkBatch);
            //save the chk details
            chkBatch.ChkBatchDetails.ForEach(x => x.Batch = chkBatch.BatchNo);
            chkBatch.ChkBatchDetails.ForEach(x => uow.ChkBatchDetailRepository.Save(x));
            uow.Commit();
            return chkBatch;
        }

    }

    public List<ChkBatch> GetOpenPaymentBatches()
    {
        using UnitOfWorkMain unitOfWork = new(appEnvironment);
        List<ChkBatch> chkBatches = unitOfWork.ChkBatchRepository.GetOpenBatches();

        return chkBatches;
    }

    public ChkBatch GetPaymentBatchById(int batchNo)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        var batch = uow.ChkBatchRepository.GetById(batchNo);
        batch.ChkBatchDetails = uow.ChkBatchDetailRepository.GetByBatch(batchNo);

        batch.ChkBatchDetails.ForEach(x =>
        {
            if (!string.IsNullOrEmpty(x.AccountNo))
            {
                var acc = accountService.GetAccount(x.AccountNo, true);
                x.PatientName = acc.PatFullName;
                x.Balance = acc.Balance;
            }
            x.Balance = accountService.GetBalance(x.AccountNo);
        }
);
        return batch;
    }

    public bool DeletePaymentBatch(int batchNo)
    {
        using UnitOfWorkMain uow = new(appEnvironment, true);
        var chkBatch = GetPaymentBatchById(batchNo);

        if (chkBatch != null || chkBatch.BatchNo <= 0)
        {
            uow.ChkBatchRepository.Delete(chkBatch);
            uow.ChkBatchDetailRepository.DeleteBatch(batchNo);
            uow.Commit();
            return true;
        }

        return false;
    }

    public void PostBatchPayments(int batchNo)
    {
        using ChkBatchUnitOfWork uow = new(appEnvironment, true);

        var chkBatch = GetPaymentBatchById(batchNo);
        List<Chk> chks = new();
        chkBatch.ChkBatchDetails.ForEach(detail =>
        {
            Chk chk = new()
            {
                AccountNo = detail.AccountNo,
                Batch = detail.Batch,
                PaidAmount = detail.AmtPaid,
                ChkDate = detail.CheckDate,
                DateReceived = detail.DateReceived,
                CheckNo = detail.CheckNo,
                Comment = detail.Comment,
                ContractualAmount = detail.Contractual,
                WriteOffAmount = detail.WriteOffAmount,
                WriteOffCode = detail.WriteOffCode,
                WriteOffDate = detail.WriteOffDate,
                Source = detail.Source
            };

            chks.Add(chk);
        });

        uow.AddBatch(chks);

        uow.ChkBatchRepository.UpdatePostedDate(batchNo, DateTime.Now);
        uow.Commit();

    }

    public void PostBatchPayments(IList<Chk> chks)
    {
        using ChkBatchUnitOfWork uow = new(appEnvironment, true);
        try
        {
            uow.AddBatch(chks.ToList());
            uow.Commit();
        }
        catch(Exception ex) 
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error adding payment records. Records have been rolled back.", ex);
        }
    }

    public ChkBatchDetail SavePaymentBatchDetail(ChkBatchDetail detail)
    {
        using ChkBatchUnitOfWork uow = new(appEnvironment, true);
        var result = uow.ChkBatchDetailRepository.Save(detail);
        uow.Commit();
        return result;
    }

    public bool DeletePaymentBatchDetail(int detailId)
    {
        using ChkBatchUnitOfWork uow = new(appEnvironment, true);

        if (uow.ChkBatchDetailRepository.Delete(detailId))
        {
            uow.Commit();
            return true;
        }

        return false;
    }

    public decimal GetNextBatchNumber()
    {
        using ChkBatchUnitOfWork uow = new(appEnvironment);

        return uow.NumberRepository.GetNumber("batch");
    }
}
