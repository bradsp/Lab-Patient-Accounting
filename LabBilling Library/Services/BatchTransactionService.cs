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
    private IUnitOfWork _uow;

    public BatchTransactionService(IAppEnvironment appEnvironment, IUnitOfWork uow)
    {
        this._appEnvironment = appEnvironment;
        _accountService = new(appEnvironment, uow);
        _uow = uow;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chkBatch"></param>
    /// <returns>Batch Number or -1 if error</returns>
    public ChkBatch SavePaymentBatch(ChkBatch chkBatch)
    {
        Log.Instance.Trace("Entering");
        _uow.StartTransaction();

        if (chkBatch.BatchNo > 0)
        {
            chkBatch = _uow.ChkBatchRepository.Update(chkBatch);
            chkBatch.ChkBatchDetails.ForEach(d => _uow.ChkBatchDetailRepository.Update(d));
            //save the chk details
            chkBatch.ChkBatchDetails.ForEach(x => x.Batch = chkBatch.BatchNo);
            chkBatch.ChkBatchDetails.ForEach(x => _uow.ChkBatchDetailRepository.Save(x));
            _uow.Commit();
            return chkBatch;
        }
        else
        {
            chkBatch = _uow.ChkBatchRepository.Add(chkBatch);
            //save the chk details
            chkBatch.ChkBatchDetails.ForEach(x => x.Batch = chkBatch.BatchNo);
            chkBatch.ChkBatchDetails.ForEach(x => _uow.ChkBatchDetailRepository.Save(x));
            _uow.Commit();
            return chkBatch;
        }

    }

    public List<ChkBatch> GetOpenPaymentBatches()
    {
        List<ChkBatch> chkBatches = _uow.ChkBatchRepository.GetOpenBatches();

        return chkBatches;
    }

    public ChkBatch GetPaymentBatchById(int batchNo)
    {
        var batch = _uow.ChkBatchRepository.GetById(batchNo);
        batch.ChkBatchDetails = _uow.ChkBatchDetailRepository.GetByBatch(batchNo);

        batch.ChkBatchDetails.ForEach(x =>
        {
            if (!string.IsNullOrEmpty(x.AccountNo))
            {
                var acc = _accountService.GetAccountMinimal(x.AccountNo);
                x.PatientName = acc?.PatFullName;
                x.Balance = _accountService.GetBalance(x.AccountNo);
            }
        }
);
        return batch;
    }

    public bool DeletePaymentBatch(int batchNo)
    {
        _uow.StartTransaction();
        var chkBatch = GetPaymentBatchById(batchNo);

        if (chkBatch != null || chkBatch.BatchNo <= 0)
        {
            _uow.ChkBatchRepository.Delete(chkBatch);
            _uow.ChkBatchDetailRepository.DeleteBatch(batchNo);
            _uow.Commit();
            return true;
        }

        return false;
    }

    public void PostBatchPayments(int batchNo)
    {
        _uow.StartTransaction();

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

        _uow.ChkBatchRepository.UpdatePostedDate(batchNo, DateTime.Now);
        _uow.Commit();

    }

    public void PostBatchPayments(IList<Chk> chks)
    {
        _uow.StartTransaction();
        try
        {
            AddBatch(chks.ToList());
            _uow.Commit();
        }
        catch(Exception ex) 
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error adding payment records. Records have been rolled back.", ex);
        }
    }

    public ChkBatchDetail SavePaymentBatchDetail(ChkBatchDetail detail)
    {
        _uow.StartTransaction();
        var result = _uow.ChkBatchDetailRepository.Save(detail);
        _uow.Commit();
        return result;
    }

    public bool DeletePaymentBatchDetail(int detailId)
    {
        _uow.StartTransaction();

        if (_uow.ChkBatchDetailRepository.Delete(detailId))
        {
            _uow.Commit();
            return true;
        }

        return false;
    }

    public bool AddBatch(List<Chk> chks)
    {
        Log.Instance.Trace("Entering");

        _uow.StartTransaction();
        try
        {
            // Some transactional DB work
            foreach (Chk chk in chks)
            {
                _uow.ChkRepository.Add(chk);
            }
        }
        catch (Exception e)
        {
            Log.Instance.Fatal(e, $"Exception adding chk record");
            throw new ApplicationException("Exception encountered posting batch. Records have been rolled back.", e);
        }

        return true;

    }

    public decimal GetNextBatchNumber()
    {

        return _uow.NumberRepository.GetNumber("batch");
    }
}
