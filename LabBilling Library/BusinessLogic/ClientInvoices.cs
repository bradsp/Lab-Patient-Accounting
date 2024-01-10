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

namespace LabBilling.Core.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClientInvoices : DataAccess.Database
    {
        //private string _connection;
        private readonly ChkRepository chkdb;
        private readonly ChrgRepository chrgdb;
        private readonly ClientRepository clientdb;
        private readonly SystemParametersRepository systemdb;
        private readonly AccountRepository accdb;
        private readonly InvoiceHistoryRepository invoiceHistoryRepository;
        private string fileSavePath = null;

        private IAppEnvironment _appEnvironment;
        public event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceGenerated;
        public event EventHandler<ClientInvoiceGeneratedEventArgs> InvoiceRunCompleted;

        public ClientInvoices(IAppEnvironment appEnvironment) : base(appEnvironment.ConnectionString)
        {
            if(appEnvironment == null) throw new ArgumentNullException(nameof(appEnvironment));
            if (!appEnvironment.EnvironmentValid) throw new ArgumentException("App Environment is not valid.");

            _appEnvironment = appEnvironment;

            chrgdb = new ChrgRepository(_appEnvironment);
            chkdb = new ChkRepository(_appEnvironment);
            clientdb = new ClientRepository(_appEnvironment);
            systemdb = new SystemParametersRepository(_appEnvironment);
            accdb = new AccountRepository(_appEnvironment);
            invoiceHistoryRepository = new InvoiceHistoryRepository(_appEnvironment);

            //this.fileSavePath = systemdb.GetByKey("invoice_file_location") ?? string.Empty;
            this.fileSavePath = appEnvironment.ApplicationParameters.InvoiceFileLocation;

            if (string.IsNullOrEmpty(fileSavePath))
            {
                throw new InvalidParameterValueException(nameof(ApplicationParameters.InvoiceFileLocation));
            }
        }

        public void Compile(DateTime thruDate, IList<UnbilledClient> unbilledClients, IProgress<int> progress)
        {
            int clientCount = unbilledClients.Count;
            Log.Instance.Debug("Begin Transaction");
            dbConnection.BeginTransaction();
            try
            {
                int tempCount = 0;
                foreach (UnbilledClient unbilledClient in unbilledClients)
                {
                    GenerateInvoice(unbilledClient.ClientMnem, thruDate);
                    tempCount++;
                    progress?.Report((tempCount * 100 / clientCount));
                    InvoiceGenerated?.Invoke(this, new ClientInvoiceGeneratedEventArgs()
                    {
                        ClientMnem = unbilledClient.ClientMnem,
                        Progress = tempCount * 100 / clientCount,
                        InvoiceCount = clientCount,
                        InvoicesGenerated = tempCount
                    });
                    Log.Instance.Info($"Invoice for {unbilledClient.ClientMnem} generated.");
                }
                Log.Instance.Debug("Complete Transaction");
                dbConnection.CompleteTransaction();
                Log.Instance.Info($"Client Invoice Run Complete: {tempCount} invoices generated.");
                InvoiceRunCompleted?.Invoke(this, new ClientInvoiceGeneratedEventArgs()
                {
                    ClientMnem = "",
                    Progress = tempCount * 100 / clientCount,
                    InvoiceCount = clientCount,
                    InvoicesGenerated = tempCount
                });
                return;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error encountered during invoice run. Process is aborted and transactions rolled back.", ex);
                Log.Instance.Debug("Abort Transaction");
                dbConnection.AbortTransaction();
                return;
            }
        }

        public string GenerateStatement(string clientMnemonic, DateTime asOfDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnemonic}");

            if (clientMnemonic == null)
                throw new ArgumentNullException("clientMnemonic");
            if (asOfDate > DateTime.Today)
                throw new ArgumentOutOfRangeException("asOfDate");

            Account acc = accdb.GetByAccount(clientMnemonic);
            if (acc == null)
                clientdb.NewClient(clientMnemonic);

            InvoiceModel invoiceModel = new InvoiceModel();
            invoiceModel.StatementType = InvoiceModel.StatementTypeEnum.Statement;
            invoiceModel.BillingCompanyName = _appEnvironment.ApplicationParameters.InvoiceCompanyName;
            invoiceModel.BillingCompanyAddress = _appEnvironment.ApplicationParameters.InvoiceCompanyAddress;
            invoiceModel.BillingCompanyCity = _appEnvironment.ApplicationParameters.InvoiceCompanyCity;
            invoiceModel.BillingCompanyState = _appEnvironment.ApplicationParameters.InvoiceCompanyState;
            invoiceModel.BillingCompanyZipCode = _appEnvironment.ApplicationParameters.InvoiceCompanyZipCode;
            invoiceModel.BillingCompanyPhone = _appEnvironment.ApplicationParameters.InvoiceCompanyPhone;
            invoiceModel.ImageFilePath = _appEnvironment.ApplicationParameters.InvoiceLogoImagePath;

            Client client = clientdb.GetClient(clientMnemonic);

            invoiceModel.ClientMnem = clientMnemonic;
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;

            invoiceModel.BalanceForwardDate = asOfDate;
            invoiceModel.BalanceForward = clientdb.Balance(clientMnemonic, asOfDate);
            invoiceModel.BalanceDue = clientdb.Balance(clientMnemonic);
            
            var details = clientdb.GetStatementDetails(clientMnemonic, asOfDate);

            invoiceModel.ClientStatementDetails = details;

            InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            string filename = $"{fileSavePath}\\Statement-{invoiceModel.ClientMnem}-{DateTime.Today.ToString("yyyyMMdd")}.pdf";
            invoicePrint.CreateStatementPdf(invoiceModel, filename);

            //InvoicePrint.CreatePDF(invoiceModel, $"{fileSavePath}\\Statement-{invoiceModel.ClientMnem}-{DateTime.Today.ToString("yyyyMMdd")}.pdf");
            return filename;
        }

        public string PrintInvoice(string invoiceNo)
        {
            InvoiceModel invoiceModel = new InvoiceModel();

            InvoiceHistory invoiceHistory = invoiceHistoryRepository.GetByInvoice(invoiceNo);


            InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            string xml = invoiceHistory.InvoiceData;
            xml = xml.Replace("&#x0;", "");
            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceModel));
            StringReader rdr = new StringReader(xml);
            
            invoiceModel = (InvoiceModel)serializer.Deserialize(rdr);

            invoiceModel.ImageFilePath = _appEnvironment.ApplicationParameters.InvoiceLogoImagePath; //systemdb.GetByKey("invoice_logo_image_path") ?? string.Empty;

            //only print an invoice if there are invoice lines to print.
            if(invoiceModel.InvoiceDetails.Count() > 0)
            {
                string filename = $"{fileSavePath}\\Invoice-{invoiceModel.ClientMnem}-{invoiceModel.InvoiceNo}.pdf";
                invoicePrint.CreateInvoicePdf(invoiceModel, filename);
                return filename;
            }

            return string.Empty;
        }

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
            
            Account acc = accdb.GetByAccount(clientMnemonic);
            if (acc == null)
                clientdb.NewClient(clientMnemonic);

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

            NumberRepository numberdb = new NumberRepository(_appEnvironment);

            Client client = clientdb.GetClient(clientMnemonic);
            invoiceModel.ClientMnem = clientMnemonic;
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;
            invoiceModel.InvoiceNo = numberdb.GetNumber("invoice").ToString();

            invoiceModel.InvoiceDetails = new List<InvoiceDetailModel>();

            invoiceModel.ShowCpt = client.PrintCptOnInvoice;

            List<InvoiceSelect> accounts = accdb.GetInvoiceAccounts(clientMnemonic, throughDate).ToList();

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

                List<InvoiceChargeView> charges = chrgdb.GetInvoiceCharges(account.AccountNo, clientMnemonic);

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

                Chrg accChrg = new Chrg();
                accChrg.AccountNo = account.AccountNo;
                accChrg.CDMCode = "CBILL";
                accChrg.Invoice = invoiceModel.InvoiceNo;
                accChrg.Quantity = amountTotal < 0 ? 1 : -1;
                accChrg.HospAmount = inpTotal;
                accChrg.RetailAmount = retailTotal;
                accChrg.NetAmount = Math.Abs(amountTotal);
                accChrg.FinancialType = "C";
                accChrg.FinCode = account.FinCode;
                accChrg.ServiceDate = account.TransactionDate;
                accChrg.Status = "NEW";
                accChrg.PostingDate = DateTime.Today;
                accChrg.PerformingSite = "";
                accChrg.OrderingSite = "";
                accChrg.IsCredited = false;
                accChrg.ClientMnem = invoiceModel.ClientMnem;
                //accChrg.PatFirstName = "";
                //accChrg.PatLastName = "";
                //accChrg.PatMiddleName = "";
                accChrg.ChrgDetails.Add(new ChrgDetail()
                {
                    Cpt4 = "NONE",
                    Type = "NORM",
                    Amount = Math.Abs(amountTotal)
                });
                chrgdb.AddCharge(accChrg);

                chrgdb.SetChargeInvoiceStatus(account.AccountNo, clientMnemonic, invoiceModel.InvoiceNo);
            }

            //write client invoice transaction on client account

            Chrg invoiceChrg = new Chrg();
            invoiceChrg.AccountNo = clientMnemonic;
            invoiceChrg.CDMCode = "CBILL";
            invoiceChrg.Invoice = invoiceModel.InvoiceNo;
            invoiceChrg.Quantity = invoiceAmountTotal < 0 ? -1 : 1;
            invoiceChrg.HospAmount = invoiceInpTotal;
            invoiceChrg.RetailAmount = invoiceRetailTotal;
            invoiceChrg.NetAmount = Math.Abs(invoiceAmountTotal);
            invoiceChrg.FinancialType = "C";
            invoiceChrg.FinCode = "CLIENT";
            invoiceChrg.ServiceDate = DateTime.Today;
            invoiceChrg.Status = "NEW";
            invoiceChrg.ClientMnem = invoiceModel.ClientMnem;
            invoiceChrg.ChrgDetails.Add(new ChrgDetail()
            {
                Cpt4 = "NONE",
                Type = "NORM",
                Amount = Math.Abs(invoiceAmountTotal)
            });
            chrgdb.AddCharge(invoiceChrg);
            invoiceModel.InvoiceTotal = invoiceAmountTotal;
            invoiceModel.DiscountTotal = discountTotal;

            SaveInvoiceHistory(invoiceModel);

            //InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            //invoicePrint.CreateInvoicePdf(invoiceModel, $"{fileSavePath}\\Invoice-{invoiceModel.ClientMnem}-{invoiceModel.InvoiceNo}.pdf");

        }

        private void SaveInvoiceHistory(InvoiceModel invoiceModel)
        {
            //store the invoice data as xml for storage in invoice history
            XmlSerializer x = new XmlSerializer(invoiceModel.GetType());
            StringWriter textWriter = new StringWriter();

            x.Serialize(textWriter, invoiceModel);

            //write client invoice history record
            InvoiceHistory invoiceHistory = new InvoiceHistory();

            invoiceHistory.ClientMnem = invoiceModel.ClientMnem;
            invoiceHistory.Discount = invoiceModel.DiscountTotal;
            invoiceHistory.InvoiceNo = invoiceModel.InvoiceNo;
            invoiceHistory.ThroughDate = invoiceModel.ThroughDate;
            invoiceHistory.TotalCharges = invoiceModel.InvoiceTotal;
            invoiceHistory.InvoiceData = textWriter.ToString();

            //invoiceHistory.balance_due = balanceForward + invoiceAmountTotal;
            //invoiceHistory.bal_forward = balanceForward;
            //invoiceHistory.payments = payments;
            //invoiceHistory.true_balance_due = balanceForward + invoiceAmountTotal;

            invoiceHistoryRepository.Add(invoiceHistory);
        }

        public bool UndoInvoice(string invoiceNo)
        {
            throw new NotImplementedException();
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
