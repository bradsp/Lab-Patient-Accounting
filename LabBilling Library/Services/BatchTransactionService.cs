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
    private readonly IAppEnvironment _appEnvironment;
    private readonly AccountService _accountService;

    public BatchTransactionService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
        _accountService = new(appEnvironment);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chkBatch"></param>
    /// <returns>Batch Number or -1 if error</returns>
    public ChkBatch SavePaymentBatch(ChkBatch chkBatch, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

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

    public List<ChkBatch> GetOpenPaymentBatches(IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        List<ChkBatch> chkBatches = uow.ChkBatchRepository.GetOpenBatches();

        return chkBatches;
    }

    public ChkBatch GetPaymentBatchById(int batchNo, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var batch = uow.ChkBatchRepository.GetById(batchNo);
        batch.ChkBatchDetails = uow.ChkBatchDetailRepository.GetByBatch(batchNo);

        batch.ChkBatchDetails.ForEach(x =>
        {
            if (!string.IsNullOrEmpty(x.AccountNo))
            {
                var acc = _accountService.GetAccountMinimal(x.AccountNo);
                x.PatientName = acc?.PatFullName;
                x.Balance = _accountService.GetBalance(x.AccountNo, uow);
            }
        }
);
        return batch;
    }

    public bool DeletePaymentBatch(int batchNo, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
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

    public void PostBatchPayments(int batchNo, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

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
                Source = detail.Source,
                Status = detail.Status,
            };

            chks.Add(chk);
        });

        AddBatch(chks);

        uow.ChkBatchRepository.UpdatePostedDate(batchNo, DateTime.Now);
        uow.Commit();

    }

    public void PostBatchPayments(IList<Chk> chks, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        try
        {
            AddBatch(chks.ToList());
            uow.Commit();
        }
        catch(Exception ex) 
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error adding payment records. Records have been rolled back.", ex);
        }
    }

    public ChkBatchDetail SavePaymentBatchDetail(ChkBatchDetail detail, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        var result = uow.ChkBatchDetailRepository.Save(detail);
        uow.Commit();
        return result;
    }

    public bool DeletePaymentBatchDetail(int detailId, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        if (uow.ChkBatchDetailRepository.Delete(detailId))
        {
            uow.Commit();
            return true;
        }

        return false;
    }

    public bool AddBatch(List<Chk> chks, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        try
        {
            // Some transactional DB work
            foreach (Chk chk in chks)
            {
                uow.ChkRepository.Add(chk);
            }
        }
        catch (Exception e)
        {
            Log.Instance.Fatal(e, $"Exception adding chk record");
            throw new ApplicationException("Exception encountered posting batch. Records have been rolled back.", e);
        }

        return true;

    }

    public decimal GetNextBatchNumber(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.NumberRepository.GetNumber("batch");
    }
}
