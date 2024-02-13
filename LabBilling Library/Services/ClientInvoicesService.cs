using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using PetaPoco;
using Utilities;
using LabBilling.Core.UnitOfWork;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LabBilling.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClientInvoicesService
    {
        private IAppEnvironment appEnvironment;
        public event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceGenerated;
        public event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceRunCompleted;
        private const string invoiceCdm = "CBILL";

        public ClientInvoicesService(IAppEnvironment appEnvironment)
        {
            if(appEnvironment == null) throw new ArgumentNullException(nameof(appEnvironment));
            if (!appEnvironment.EnvironmentValid) throw new ArgumentException("App Environment is not valid.");

            this.appEnvironment = appEnvironment;
        }

        public void Compile(DateTime thruDate, IList<UnbilledClient> unbilledClients, IProgress<int> progress)
        {
            Log.Instance.Trace("Begin");
            using UnitOfWorkMain unitOfWork = new(appEnvironment, true);
            int clientCount = unbilledClients.Count;
            try
            {
                int tempCount = 0;
                foreach (UnbilledClient unbilledClient in unbilledClients)
                {
                    GenerateInvoice(unbilledClient.ClientMnem, thruDate);
                    tempCount++;
                    progress?.Report(HelperExtensions.ComputePercentage(tempCount, clientCount));
                    InvoiceGenerated?.Invoke(this, new ClientInvoiceGeneratedEventArgs()
                    {
                        ClientMnem = unbilledClient.ClientMnem,
                        Progress = HelperExtensions.ComputePercentage(tempCount, clientCount),
                        InvoiceCount = clientCount,
                        InvoicesGenerated = tempCount
                    });
                    Log.Instance.Info($"Invoice for {unbilledClient.ClientMnem} generated.");
                }
                Log.Instance.Debug("Complete Transaction");
                unitOfWork.Commit();
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

        public InvoiceModel GenerateStatement(string clientMnemonic, DateTime asOfDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnemonic}");
            using UnitOfWorkMain unitOfWork = new(appEnvironment, true);

            if (clientMnemonic == null)
                throw new ArgumentNullException(nameof(clientMnemonic));
            if (asOfDate > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(asOfDate));

            Account acc = unitOfWork.AccountRepository.GetByAccount(clientMnemonic);

            if (acc == null)
                unitOfWork.ClientRepository.NewClient(clientMnemonic);

            InvoiceModel invoiceModel = new()
            {
                StatementType = InvoiceModel.StatementTypeEnum.Statement,
                BillingCompanyName = appEnvironment.ApplicationParameters.InvoiceCompanyName,
                BillingCompanyAddress = appEnvironment.ApplicationParameters.InvoiceCompanyAddress,
                BillingCompanyCity = appEnvironment.ApplicationParameters.InvoiceCompanyCity,
                BillingCompanyState = appEnvironment.ApplicationParameters.InvoiceCompanyState,
                BillingCompanyZipCode = appEnvironment.ApplicationParameters.InvoiceCompanyZipCode,
                BillingCompanyPhone = appEnvironment.ApplicationParameters.InvoiceCompanyPhone,
                ImageFilePath = appEnvironment.ApplicationParameters.InvoiceLogoImagePath
            };

            Client client = unitOfWork.ClientRepository.GetClient(clientMnemonic);

            invoiceModel.ClientMnem = clientMnemonic;
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;

            invoiceModel.BalanceForwardDate = asOfDate;
            invoiceModel.BalanceForward = unitOfWork.ClientRepository.Balance(clientMnemonic, asOfDate);
            invoiceModel.BalanceDue = unitOfWork.ClientRepository.Balance(clientMnemonic);
            
            var details = GetStatementDetails(clientMnemonic, asOfDate);

            invoiceModel.ClientStatementDetails = details;

            //InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            //string filename = $"{fileSavePath}\\Statement-{invoiceModel.ClientMnem}-{DateTime.Today.ToString("yyyyMMdd")}.pdf";
            //invoicePrint.CreateStatementPdf(invoiceModel, filename);

            return invoiceModel;
        }

        //public string PrintInvoice(string invoiceNo)
        //{
        //    InvoiceModel invoiceModel = new InvoiceModel();

        //    InvoiceHistory invoiceHistory = invoiceHistoryRepository.GetByInvoice(invoiceNo);


        //    InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

        //    string xml = invoiceHistory.InvoiceData;
        //    xml = xml.Replace("&#x0;", "");
        //    XmlSerializer serializer = new XmlSerializer(typeof(InvoiceModel));
        //    StringReader rdr = new StringReader(xml);
            
        //    invoiceModel = (InvoiceModel)serializer.Deserialize(rdr);

        //    invoiceModel.ImageFilePath = _appEnvironment.ApplicationParameters.InvoiceLogoImagePath; //systemdb.GetByKey("invoice_logo_image_path") ?? string.Empty;

        //    //only print an invoice if there are invoice lines to print.
        //    if(invoiceModel.InvoiceDetails.Count() > 0)
        //    {
        //        string filename = $"{fileSavePath}\\Invoice-{invoiceModel.ClientMnem}-{invoiceModel.InvoiceNo}.pdf";
        //        invoicePrint.CreateInvoicePdf(invoiceModel, filename);
        //        return filename;
        //    }

        //    return string.Empty;
        //}

        /// <summary>
        /// Generate an invoice for a single client.
        /// </summary>
        /// <param name="clientMnemonic"></param>
        /// <param name="throughDate"></param>
        public void GenerateInvoice(string clientMnemonic, DateTime throughDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnemonic} thrudate {throughDate}");
            if (clientMnemonic == null)
                throw new ArgumentNullException(nameof(clientMnemonic));
            AccountService accountService = new(appEnvironment);
            using UnitOfWorkMain unitOfWork = new(appEnvironment, true);

            Account acc = unitOfWork.AccountRepository.GetByAccount(clientMnemonic);
            if (acc == null)
                unitOfWork.ClientRepository.NewClient(clientMnemonic);

            InvoiceModel invoiceModel = new()
            {
                StatementType = InvoiceModel.StatementTypeEnum.Invoice,
                ThroughDate = throughDate,

                BillingCompanyName = appEnvironment.ApplicationParameters.InvoiceCompanyName,
                BillingCompanyAddress = appEnvironment.ApplicationParameters.InvoiceCompanyAddress,
                BillingCompanyCity = appEnvironment.ApplicationParameters.InvoiceCompanyCity,
                BillingCompanyState = appEnvironment.ApplicationParameters.InvoiceCompanyCity,
                BillingCompanyZipCode = appEnvironment.ApplicationParameters.InvoiceCompanyZipCode,
                BillingCompanyPhone = appEnvironment.ApplicationParameters.InvoiceCompanyPhone,
                ImageFilePath = appEnvironment.ApplicationParameters.InvoiceLogoImagePath
            };

            Client client = unitOfWork.ClientRepository.GetClient(clientMnemonic);
            invoiceModel.ClientMnem = clientMnemonic;
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;
            invoiceModel.InvoiceNo = unitOfWork.NumberRepository.GetNumber("invoice").ToString();

            invoiceModel.InvoiceDetails = new List<InvoiceDetailModel>();

            invoiceModel.ShowCpt = client.PrintCptOnInvoice;

            List<InvoiceSelect> accounts = accountService.GetInvoiceAccounts(clientMnemonic, throughDate).ToList();

            if (client.PrintInvoiceInDateOrder)
                accounts = accounts.OrderBy(x => x.TransactionDate).ThenBy(y => y.AccountNo).ToList();
            else
                accounts = accounts.OrderBy(x => x.AccountNo).ToList();

            double invoiceAmountTotal = 0.0;
            double invoiceRetailTotal = 0.0;
            double invoiceInpTotal = 0.0;
            double discountTotal = 0.0;

            //loop through accounts - write details to POCO
            foreach (InvoiceSelect account in accounts)
            {
                double amountTotal = 0.0;
                double inpTotal = 0.0;
                double retailTotal = 0.0;

                List<InvoiceChargeView> charges = unitOfWork.ChrgRepository.GetInvoiceCharges(account.AccountNo, clientMnemonic);

                InvoiceDetailModel invoiceDetail = new()
                {
                    Account = account.AccountNo,
                    PatientName = account.PatientName,
                    ServiceDate = account.TransactionDate

                };
                double accountTotal = 0.0;

                foreach(InvoiceChargeView chrg in charges)
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
                    CDMCode = "CBILL",
                    Invoice = invoiceModel.InvoiceNo,
                    Quantity = amountTotal < 0 ? 1 : -1,
                    HospAmount = inpTotal,
                    RetailAmount = retailTotal,
                    NetAmount = Math.Abs(amountTotal),
                    FinancialType = "C",
                    FinCode = account.FinCode,
                    ServiceDate = account.TransactionDate,
                    Status = "NEW",
                    PostingDate = DateTime.Today,
                    PerformingSite = "",
                    OrderingSite = "",
                    IsCredited = false,
                    ClientMnem = invoiceModel.ClientMnem
                };

                accChrg.ChrgDetails.Add(new ChrgDetail()
                {
                    Cpt4 = "NONE",
                    Type = "NORM",
                    Amount = Math.Abs(amountTotal)
                });
                unitOfWork.ChrgRepository.AddCharge(accChrg);

                unitOfWork.ChrgRepository.SetChargeInvoiceStatus(account.AccountNo, clientMnemonic, invoiceModel.InvoiceNo);
            }

            //write client invoice transaction on client account

            Chrg invoiceChrg = new()
            {
                AccountNo = clientMnemonic,
                CDMCode = "CBILL",
                Invoice = invoiceModel.InvoiceNo,
                Quantity = invoiceAmountTotal < 0 ? -1 : 1,
                HospAmount = invoiceInpTotal,
                RetailAmount = invoiceRetailTotal,
                NetAmount = Math.Abs(invoiceAmountTotal),
                FinancialType = "C",
                FinCode = "CLIENT",
                ServiceDate = DateTime.Today,
                Status = "NEW",
                ClientMnem = invoiceModel.ClientMnem
            };
            invoiceChrg.ChrgDetails.Add(new ChrgDetail()
            {
                Cpt4 = "NONE",
                Type = "NORM",
                Amount = Math.Abs(invoiceAmountTotal)
            });
            unitOfWork.ChrgRepository.AddCharge(invoiceChrg);
            invoiceModel.InvoiceTotal = invoiceAmountTotal;
            invoiceModel.DiscountTotal = discountTotal;

            SaveInvoiceHistory(invoiceModel);

            unitOfWork.Commit();
        }

        public List<InvoiceHistory> GetInvoiceHistory(string clientMnem = null, DateTime? fromDate = null, DateTime? thruDate = null, string invoice = null)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var retval = uow.InvoiceHistoryRepository.GetWithSort(clientMnem, fromDate, thruDate, invoice);

            return retval.ToList();
        }

        private void SaveInvoiceHistory(InvoiceModel invoiceModel)
        {
            //store the invoice data as xml for storage in invoice history
            XmlSerializer x = new XmlSerializer(invoiceModel.GetType());
            StringWriter textWriter = new StringWriter();

            x.Serialize(textWriter, invoiceModel);

            using UnitOfWorkMain unitOfWork = new(appEnvironment, true);
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

            unitOfWork.InvoiceHistoryRepository.Add(invoiceHistory);
            unitOfWork.Commit();
        }

        public bool UndoInvoice(string invoiceNo)
        {
            throw new NotImplementedException();
        }

        public List<ClientStatementDetailModel> GetStatementDetails(string clientMnem, DateTime asOfDate)
        {
            using UnitOfWorkMain unitOfWork = new(appEnvironment, true);

            var charges = unitOfWork.ChrgRepository.GetByAccount(clientMnem, true, true, asOfDate, false);

            var payments = unitOfWork.ChkRepository.GetByAccount(clientMnem, asOfDate);

            List<ClientStatementDetailModel> statementDetails = new();

            foreach (var chrg in charges)
            {
                if (chrg.NetAmount == 0 && chrg.CDMCode == invoiceCdm)
                    continue;

                var statementDetail = new ClientStatementDetailModel
                {
                    ServiceDate = chrg.ServiceDate == null ? DateTime.MinValue : (DateTime)chrg.ServiceDate,
                    Account = chrg.AccountNo,
                    Invoice = chrg.Invoice,
                    Amount = chrg.NetAmount * chrg.Quantity
                };
                if (chrg.CDMCode == invoiceCdm)
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
            }

            foreach (var chk in payments)
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
            }

            statementDetails.Sort((x, y) => DateTime.Compare(x.ServiceDate, y.ServiceDate));

            return statementDetails;
        }

        public List<UnbilledClient> GetUnbilledClients(DateTime throughDate, IProgress<int> progress)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var retval = uow.ClientRepository.GetUnbilledClients(throughDate, progress);

            return retval;
        }

        public async Task<List<UnbilledClient>> GetUnbilledClientsAsync(DateTime throughDate, IProgress<int> progress)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var retval = await uow.ClientRepository.GetUnbilledClientsAsync(throughDate, progress);

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

}
