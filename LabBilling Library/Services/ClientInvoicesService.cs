﻿using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utilities;

namespace LabBilling.Core.Services;

/// <summary>
/// 
/// </summary>
public sealed class ClientInvoicesService
{
    private readonly IAppEnvironment _appEnvironment;
    public event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceGenerated;
    public event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceRunCompleted;
    private readonly DictionaryService _dictionaryService;
    private readonly AccountService _accountService;

    public event EventHandler<ClientInvoiceProgressEventArgs> ReportProgress;

    public ClientInvoicesService(IAppEnvironment appEnvironment)
    {
        if (appEnvironment == null) throw new ArgumentNullException(nameof(appEnvironment));
        if (!appEnvironment.EnvironmentValid) throw new ArgumentException("App Environment is not valid.");

        this._appEnvironment = appEnvironment;
        _dictionaryService = new(appEnvironment);
        _accountService = new(appEnvironment);
    }

    public async Task CompileAsync(DateTime thruDate, IList<UnbilledClient> unbilledClients, CancellationToken token, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Begin");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        int clientCount = unbilledClients.Count;
        try
        {
            int tempCount = 0;
            foreach (UnbilledClient unbilledClient in unbilledClients)
            {
                if (token.IsCancellationRequested)
                {
                    Log.Instance.Error("Cancellation requested by user.");
                    return;
                }
                ReportProgress?.Invoke(this, new ClientInvoiceProgressEventArgs()
                {
                    Client = unbilledClient.ClientName,
                    ClientsTotal = clientCount,
                    ClientsProcessed = tempCount,
                    ReportingClient = true,
                });
                await GenerateInvoiceAsync(unbilledClient.ClientMnem, thruDate, token);
                tempCount++;
                InvoiceGenerated?.Invoke(this, new ClientInvoiceGeneratedEventArgs()
                {
                    ClientMnem = unbilledClient.ClientMnem,
                    Progress = HelperExtensions.ComputePercentage(tempCount, clientCount),
                    InvoiceCount = clientCount,
                    InvoicesGenerated = tempCount
                });
                ReportProgress?.Invoke(this, new ClientInvoiceProgressEventArgs()
                {
                    Client = unbilledClient.ClientName,
                    ClientsTotal = clientCount,
                    ClientsProcessed = tempCount,
                    ReportingClient = true
                });
                Log.Instance.Info($"Invoice for {unbilledClient.ClientMnem} generated.");
            }
            Log.Instance.Debug("Complete Transaction");
            uow.Commit();
            Log.Instance.Info($"Client Invoice Run Complete: {tempCount} invoices generated.");
            InvoiceRunCompleted?.Invoke(this, new ClientInvoiceGeneratedEventArgs()
            {
                ClientMnem = "",
                Progress = HelperExtensions.ComputePercentage(tempCount, clientCount),
                InvoiceCount = clientCount,
                InvoicesGenerated = tempCount
            });
            return;
        }
        catch (Exception ex)
        {
            Log.Instance.Error("Error encountered during invoice run. Process is aborted and transactions rolled back.", ex);
            Log.Instance.Debug("Abort Transaction");
            return;
        }
    }

    public InvoiceModel GenerateStatement(string clientMnemonic, DateTime asOfDate, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - client {clientMnemonic}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        ArgumentNullException.ThrowIfNull(clientMnemonic);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(asOfDate, DateTime.Today);

        Account acc = uow.AccountRepository.GetByAccount(clientMnemonic);

        if (acc == null)
            _dictionaryService.AddClientAccount(uow, clientMnemonic);

        InvoiceModel invoiceModel = new()
        {
            StatementType = InvoiceModel.StatementTypeEnum.Statement,
            BillingCompanyName = _appEnvironment.ApplicationParameters.InvoiceCompanyName,
            BillingCompanyAddress = _appEnvironment.ApplicationParameters.InvoiceCompanyAddress,
            BillingCompanyCity = _appEnvironment.ApplicationParameters.InvoiceCompanyCity,
            BillingCompanyState = _appEnvironment.ApplicationParameters.InvoiceCompanyState,
            BillingCompanyZipCode = _appEnvironment.ApplicationParameters.InvoiceCompanyZipCode,
            BillingCompanyPhone = _appEnvironment.ApplicationParameters.InvoiceCompanyPhone,
            ImageFilePath = _appEnvironment.ApplicationParameters.InvoiceLogoImagePath
        };

        Client client = uow.ClientRepository.GetClient(clientMnemonic);

        invoiceModel.ClientMnem = clientMnemonic;
        invoiceModel.ClientName = client.Name;
        invoiceModel.Address1 = client.StreetAddress1;
        invoiceModel.Address2 = client.StreetAddress2;
        invoiceModel.City = client.City;
        invoiceModel.State = client.State;
        invoiceModel.ZipCode = client.ZipCode;
        invoiceModel.InvoiceDate = DateTime.Today;

        invoiceModel.BalanceForwardDate = asOfDate;
        invoiceModel.BalanceForward = uow.ClientRepository.Balance(clientMnemonic, asOfDate);
        invoiceModel.BalanceDue = uow.ClientRepository.Balance(clientMnemonic);

        var details = GetStatementDetails(clientMnemonic, asOfDate);

        invoiceModel.ClientStatementDetails = details;
        uow.Commit();
        return invoiceModel;
    }

    public async Task GenerateInvoiceAsync(string clientMnemonic, DateTime throughDate, CancellationToken token, IUnitOfWork uow = null) => await Task.Run(() => GenerateInvoice(clientMnemonic, throughDate, token, uow));

    /// <summary>
    /// Generate an invoice for a single client.
    /// </summary>
    /// <param name="clientMnemonic"></param>
    /// <param name="throughDate"></param>
    public void GenerateInvoice(string clientMnemonic, DateTime throughDate, CancellationToken token, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - client {clientMnemonic} thrudate {throughDate}");
        ArgumentNullException.ThrowIfNull(clientMnemonic);
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        try
        {
            Account acc = uow.AccountRepository.GetByAccount(clientMnemonic);
            if (acc == null)
                _dictionaryService.AddClientAccount(uow, clientMnemonic);

            InvoiceModel invoiceModel = new()
            {
                StatementType = InvoiceModel.StatementTypeEnum.Invoice,
                ThroughDate = throughDate,

                BillingCompanyName = _appEnvironment.ApplicationParameters.InvoiceCompanyName,
                BillingCompanyAddress = _appEnvironment.ApplicationParameters.InvoiceCompanyAddress,
                BillingCompanyCity = _appEnvironment.ApplicationParameters.InvoiceCompanyCity,
                BillingCompanyState = _appEnvironment.ApplicationParameters.InvoiceCompanyCity,
                BillingCompanyZipCode = _appEnvironment.ApplicationParameters.InvoiceCompanyZipCode,
                BillingCompanyPhone = _appEnvironment.ApplicationParameters.InvoiceCompanyPhone,
                ImageFilePath = _appEnvironment.ApplicationParameters.InvoiceLogoImagePath
            };

            Client client = uow.ClientRepository.GetClient(clientMnemonic);
            invoiceModel.ClientMnem = clientMnemonic;
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;
            invoiceModel.InvoiceNo = uow.NumberRepository.GetNumber("invoice").ToString();

            invoiceModel.InvoiceDetails = new List<InvoiceDetailModel>();

            invoiceModel.ShowCpt = client.PrintCptOnInvoice;

            List<InvoiceSelect> accounts = _accountService.GetInvoiceAccounts(clientMnemonic, throughDate, uow).ToList();

            if (client.PrintInvoiceInDateOrder)
                accounts = accounts.OrderBy(x => x.TransactionDate).ThenBy(y => y.AccountNo).ToList();
            else
                accounts = accounts.OrderBy(x => x.AccountNo).ToList();

            double invoiceAmountTotal = 0.0;
            double invoiceRetailTotal = 0.0;
            double invoiceInpTotal = 0.0;
            double discountTotal = 0.0;
            int processed = 0;
            //loop through accounts - write details to POCO
            foreach (InvoiceSelect account in accounts)
            {
                if (token.IsCancellationRequested)
                {
                    Log.Instance.Error("Cancellation requested by user.");
                    return;
                }

                double amountTotal = 0.0;
                double inpTotal = 0.0;
                double retailTotal = 0.0;

                List<InvoiceChargeView> charges = uow.ChrgRepository.GetInvoiceCharges(account.AccountNo, clientMnemonic);

                InvoiceDetailModel invoiceDetail = new()
                {
                    Account = account.AccountNo,
                    PatientName = account.PatientName,
                    ServiceDate = account.TransactionDate

                };
                double accountTotal = 0.0;

                foreach (InvoiceChargeView chrg in charges)
                {
                    invoiceDetail.InvoiceDetailLines.Add(new InvoiceDetailLinesModel()
                    {
                        CDM = chrg.ChargeItemId,
                        CPT = chrg.CptList,
                        Description = chrg.ChargeDescription,
                        Qty = chrg.Quantity,
                        Amount = chrg.Amount
                    });
                    accountTotal += chrg.Amount;
                    amountTotal += chrg.Amount;
                    inpTotal += chrg.HospAmount;
                    retailTotal += chrg.RetailAmount;
                    discountTotal += chrg.RetailAmount - chrg.Amount;
                }

                invoiceDetail.AccountTotal = accountTotal;

                invoiceModel.InvoiceDetails.Add(invoiceDetail);

                //write cbill transfer chrg to account
                invoiceAmountTotal += amountTotal;
                invoiceInpTotal += inpTotal;
                invoiceRetailTotal += retailTotal;

                Chrg accChrg = new()
                {
                    AccountNo = account.AccountNo,
                    CDMCode = _appEnvironment.ApplicationParameters.ClientInvoiceCdm,
                    Invoice = invoiceModel.InvoiceNo,
                    Quantity = amountTotal < 0 ? 1 : -1,
                    HospAmount = inpTotal,
                    RetailAmount = retailTotal,
                    NetAmount = Math.Abs(amountTotal),
                    FinancialType = _appEnvironment.ApplicationParameters.ClientFinancialTypeCode,
                    FinCode = account.FinCode,
                    ServiceDate = account.TransactionDate,
                    Status = _appEnvironment.ApplicationParameters.NewChargeStatus,
                    PostingDate = DateTime.Today,
                    PerformingSite = "",
                    OrderingSite = "",
                    IsCredited = false,
                    ClientMnem = invoiceModel.ClientMnem
                };

                var detail = new ChrgDetail()
                {
                    Cpt4 = "NONE",
                    Type = "NORM",
                    Amount = Math.Abs(amountTotal),
                };

                var rchrg = uow.ChrgRepository.Add(accChrg);
                detail.ChrgNo = rchrg.ChrgId;

                uow.ChrgDetailRepository.Add(detail);

                uow.ChrgRepository.SetChargeInvoiceStatus(account.AccountNo, clientMnemonic, invoiceModel.InvoiceNo);
                ReportProgress?.Invoke(this, new ClientInvoiceProgressEventArgs()
                {
                    AccountsTotal = accounts.Count,
                    AccountsProcessed = ++processed,
                    ReportingAccount = true
                });
            }

            //write client invoice transaction on client account

            Chrg invoiceChrg = new()
            {
                AccountNo = clientMnemonic,
                CDMCode = _appEnvironment.ApplicationParameters.ClientInvoiceCdm,
                Invoice = invoiceModel.InvoiceNo,
                Quantity = invoiceAmountTotal < 0 ? -1 : 1,
                HospAmount = invoiceInpTotal,
                RetailAmount = invoiceRetailTotal,
                NetAmount = Math.Abs(invoiceAmountTotal),
                FinancialType = _appEnvironment.ApplicationParameters.ClientFinancialTypeCode,
                FinCode = _appEnvironment.ApplicationParameters.ClientAccountFinCode,
                ServiceDate = DateTime.Today,
                Status = _appEnvironment.ApplicationParameters.NewChargeStatus,
                ClientMnem = invoiceModel.ClientMnem
            };
            var invdetail = new ChrgDetail()
            {
                Cpt4 = "NONE",
                Type = "NORM",
                Amount = Math.Abs(invoiceAmountTotal)
            };

            var retinvchrg = uow.ChrgRepository.Add(invoiceChrg);
            invdetail.ChrgNo = retinvchrg.ChrgId;
            uow.ChrgDetailRepository.Add(invdetail);
            invoiceModel.InvoiceTotal = invoiceAmountTotal;
            invoiceModel.DiscountTotal = discountTotal;

            SaveInvoiceHistory(invoiceModel);

            uow.Commit();
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error encountered generating invoice.", ex);
        }
    }

    public List<InvoiceHistory> GetInvoiceHistory(string clientMnem = null, DateTime? fromDate = null, DateTime? thruDate = null, string invoice = null, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var retval = uow.InvoiceHistoryRepository.GetWithSort(clientMnem, fromDate, thruDate, invoice);

        return retval.ToList();
    }

    private void SaveInvoiceHistory(InvoiceModel invoiceModel, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        //store the invoice data as xml for storage in invoice history
        XmlSerializer x = new(invoiceModel.GetType());
        StringWriter textWriter = new();

        x.Serialize(textWriter, invoiceModel);

        uow.StartTransaction();
        //write client invoice history record
        InvoiceHistory invoiceHistory = new()
        {
            ClientMnem = invoiceModel.ClientMnem,
            Discount = invoiceModel.DiscountTotal,
            InvoiceNo = invoiceModel.InvoiceNo,
            ThroughDate = invoiceModel.ThroughDate,
            TotalCharges = invoiceModel.InvoiceTotal,
            InvoiceData = textWriter.ToString()
        };

        uow.InvoiceHistoryRepository.Add(invoiceHistory);
        uow.Commit();
    }

    public bool UndoInvoice(string invoiceNo)
    {
        throw new NotImplementedException();
    }

    public List<ClientStatementDetailModel> GetStatementDetails(string clientMnem, DateTime asOfDate, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        var charges = uow.ChrgRepository.GetByAccount(clientMnem, true, true, asOfDate, false);
        var payments = uow.ChkRepository.GetByAccount(clientMnem, asOfDate);

        List<ClientStatementDetailModel> statementDetails = new();
        charges.ForEach(chrg =>
        {
            if (chrg.NetAmount == 0 && chrg.CDMCode == _appEnvironment.ApplicationParameters.ClientInvoiceCdm)
                return;

            var statementDetail = new ClientStatementDetailModel
            {
                ServiceDate = chrg.ServiceDate == null ? DateTime.MinValue : (DateTime)chrg.ServiceDate,
                Account = chrg.AccountNo,
                Invoice = chrg.Invoice,
                Amount = chrg.NetAmount * chrg.Quantity
            };
            if (chrg.CDMCode == _appEnvironment.ApplicationParameters.ClientInvoiceCdm)
            {
                statementDetail.Description = $"Invoice {chrg.Invoice}";
                statementDetail.Reference = chrg.Invoice;
            }
            else
            {
                //see if account is in comment and extract it for the line description
                string pattern = "([A-Z_]*)\\[(\\w*)\\]";
                Regex rg = new(pattern);
                Match match = rg.Match(chrg.Comment);
                if (match.Success)
                {
                    string account = match.Groups[1].Value;
                    statementDetail.Description = $"Adjustment: {chrg.CdmDescription} on {account}";
                }
                else
                {
                    statementDetail.Description = $"Adjustment: {chrg.CdmDescription}";
                }
            }

            statementDetails.Add(statementDetail);

        });

        payments.ForEach(chk =>
        {
            var statementDetail = new ClientStatementDetailModel();

            if (chk.PaidAmount > 0)
            {
                statementDetail.Description = $"Payment Received - {chk.Comment}";
            }
            else if (chk.WriteOffAmount > 0)
            {
                statementDetail.Description = $"Adjustment - {chk.Comment}";
            }
            statementDetail.Amount = (chk.PaidAmount + chk.ContractualAmount + chk.WriteOffAmount) * -1;
            statementDetail.ServiceDate = chk.DateReceived == null ? DateTime.Today : (DateTime)chk.DateReceived;
            statementDetail.Reference = chk.CheckNo;
            statementDetails.Add(statementDetail);
        });

        statementDetails.Sort((x, y) => DateTime.Compare(x.ServiceDate, y.ServiceDate));
        uow.Commit();

        return statementDetails;
    }

    public List<UnbilledClient> GetUnbilledClients(DateTime throughDate, IProgress<int> progress, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        List<UnbilledClient> unbilledClients = new();

        var unbilledAccounts = uow.ClientRepository.GetUnbilledAccounts(throughDate, progress);

        var clients = uow.ClientRepository.GetAll(false);

        int total = clients.Count;
        int processed = 0;

        clients.AsParallel().ForAll(client =>
        {
            UnbilledClient unbilledClient = new()
            {
                ClientMnem = client.ClientMnem,
                ClientName = client.Name,
                ClientType = client.ClientType.Description,
                PriorBalance = _accountService.GetBalance(client.ClientMnem),
                UnbilledAccounts = unbilledAccounts.Where(x => x.ClientMnem == client.ClientMnem).ToList()
            };

            if (unbilledClient.PriorBalance != 0 || unbilledClient.UnbilledAccounts.Sum(x => x.UnbilledAmount) != 0)
                unbilledClients.Add(unbilledClient);

            progress?.Report(HelperExtensions.ComputePercentage(++processed, total));
        });

        return unbilledClients;

    }

    public async Task<List<UnbilledClient>> GetUnbilledClientsAsync(DateTime throughDate, IProgress<int> progress, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var retval = await Task.Run(() => GetUnbilledClients(throughDate, progress, uow));
        retval.Sort((x, y) => x.ClientName.CompareTo(y.ClientName));
        return retval;
    }
}

public class ClientInvoiceGeneratedEventArgs : EventArgs
{
    public string ClientMnem { get; set; }
    public int Progress { get; set; }
    public int InvoicesGenerated { get; set; }
    public int InvoiceCount { get; set; }
}
