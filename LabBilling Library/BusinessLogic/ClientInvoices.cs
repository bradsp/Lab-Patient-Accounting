using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientInvoices
    {
        private string _connection;
        private readonly PetaPoco.Database dbConnection;
        private readonly ChkRepository chkdb;
        private readonly ChrgRepository chrgdb;
        private readonly ClientRepository clientdb;

        public ClientInvoices(string connection)
        {
            if(connection == "" || connection == null)
            {
                throw new ArgumentException("Must have a valid connection string", "connection");
            }
            _connection = connection;
            dbConnection = new PetaPoco.Database(connection, new SqlServerDatabaseProvider());

            chrgdb = new ChrgRepository(dbConnection);
            chkdb = new ChkRepository(dbConnection);
            clientdb = new ClientRepository(dbConnection);
        }

        public int Compile(DateTime thruDate, IList<UnbilledClient> unbilledClients, IProgress<int> progress)
        {
            if (thruDate == null)
                throw new ArgumentNullException();

            //var unbilledClients = GetUnbilledClients(thruDate);

            int clientCount = unbilledClients.Count;

            int tempCount = 0;
            foreach (UnbilledClient unbilledClient in unbilledClients)
            {
                GenerateInvoice(unbilledClient.ClientMnem, thruDate);
                tempCount++;
                if (progress != null)
                {
                    progress.Report((tempCount * 100 / clientCount));
                }
            }

            return tempCount;
        }

        public void GenerateStatement(string clientMnemonic, DateTime asOfDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnemonic}");

            if (clientMnemonic == null)
                throw new ArgumentNullException("clientMnemonic");
            if (asOfDate > DateTime.Today)
                throw new ArgumentOutOfRangeException("asOfDate");

            SystemParametersRepository systemdb = new SystemParametersRepository(_connection);
            AccountRepository accdb = new AccountRepository(_connection);
            Account acc = accdb.GetByAccount(clientMnemonic);
            if (acc == null)
                clientdb.NewClient(clientMnemonic);

            InvoiceModel invoiceModel = new InvoiceModel();

            invoiceModel.StatementType = InvoiceModel.StatementTypeEnum.Statement;
            invoiceModel.BillingCompanyName = systemdb.GetByKey("invoice_company_name") ?? string.Empty;
            invoiceModel.BillingCompanyAddress = systemdb.GetByKey("invoice_company_address") ?? string.Empty;
            invoiceModel.BillingCompanyCity = systemdb.GetByKey("invoice_company_city") ?? string.Empty;
            invoiceModel.BillingCompanyState = systemdb.GetByKey("invoice_company_state") ?? string.Empty;
            invoiceModel.BillingCompanyZipCode = systemdb.GetByKey("invoice_company_zipcode") ?? string.Empty;
            invoiceModel.BillingCompanyPhone = systemdb.GetByKey("invoice_company_zipcode") ?? string.Empty;
            invoiceModel.ImageFilePath = systemdb.GetByKey("invoice_logo_image_path") ?? string.Empty;

            Client client = clientdb.GetClient(clientMnemonic);

            invoiceModel.ClientMnem = clientMnemonic;
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;

            double balanceForward = clientdb.Balance(clientMnemonic); ;

            invoiceModel.BalanceForward = balanceForward;
            
            invoiceModel.InvoiceDetails = new List<InvoiceDetailModel>();

            var details = clientdb.GetStatementDetails(clientMnemonic, asOfDate);

            InvoicePrint.CreatePDF(invoiceModel, $"d:\\temp\\Statement-{invoiceModel.ClientMnem}-{DateTime.Today.ToString("yyyyMMdd")}.pdf");

        }


        /// <summary>
        /// Generate an invoice for a single client.
        /// </summary>
        /// <param name="clientMnemonic"></param>
        /// <param name="throughDate"></param>
        public void GenerateInvoice(string clientMnemonic, DateTime throughDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnemonic} thrudate {throughDate}");
            if (clientMnemonic == null || throughDate == null)
                throw new ArgumentNullException();
            SystemParametersRepository systemdb = new SystemParametersRepository(_connection);
            AccountRepository accdb = new AccountRepository(_connection);
            Account acc = accdb.GetByAccount(clientMnemonic);
            if (acc == null)
                clientdb.NewClient(clientMnemonic);

            InvoiceModel invoiceModel = new InvoiceModel();
            invoiceModel.StatementType = InvoiceModel.StatementTypeEnum.Invoice;
            invoiceModel.ThroughDate = throughDate;

            invoiceModel.BillingCompanyName = systemdb.GetByKey("invoice_company_name") ?? string.Empty;
            invoiceModel.BillingCompanyAddress = systemdb.GetByKey("invoice_company_address") ?? string.Empty;
            invoiceModel.BillingCompanyCity = systemdb.GetByKey("invoice_company_city") ?? string.Empty;
            invoiceModel.BillingCompanyState = systemdb.GetByKey("invoice_company_state") ?? string.Empty;
            invoiceModel.BillingCompanyZipCode = systemdb.GetByKey("invoice_company_zipcode") ?? string.Empty;
            invoiceModel.BillingCompanyPhone = systemdb.GetByKey("invoice_company_zipcode") ?? string.Empty;
            invoiceModel.ImageFilePath = systemdb.GetByKey("invoice_logo_image_path") ?? string.Empty;

            NumberRepository numberdb = new NumberRepository(_connection);

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

            //double balanceForward = clientdb.Balance(clientMnemonic);
            //invoiceModel.BalanceForward = balanceForward;
            //double payments = 0.00;
            //double adjustments = 0.00;
            invoiceModel.InvoiceDetails = new List<InvoiceDetailModel>();



            ////get payments since last invoice
            //var chks = chkdb.GetByAccount(clientMnemonic, false);

            //foreach(Chk chk in chks)
            //{
            //    invoiceModel.InvoiceDetails.Add(new InvoiceDetailModel()
            //    {
            //        ServiceDate = chk.DateReceived,
            //        Description = string.Format("Payment Received - {0}", chk.Source),
            //        Amount = chk.PaidAmount + chk.WriteOffAmount + chk.ContractualAmount
            //    });
            //    payments += chk.PaidAmount + chk.WriteOffAmount + chk.ContractualAmount;
            //    //update chk record with new invoice number
            //    chk.Invoice = invoiceModel.InvoiceNo;
            //    chkdb.Update(chk);
            //}

            //get adjustments since last invoice
            //var chrgs = chrgdb.GetInvoiceCharges(clientMnemonic);

            //foreach(InvoiceChargeView chrg in chrgs)
            //{
            //    invoiceModel.InvoiceDetails.Add(new InvoiceDetailModel()
            //    {
            //        ServiceDate = chrg.TransactionDate,
            //        Description = chrg.ChargeDescription,
            //        Qty = chrg.Quantity,
            //        CDM = chrg.ChargeItemId,
            //        Amount = chrg.Amount
            //    });
            //    adjustments += chrg.Amount;
            //}

            //chrgdb.SetChargeInvoiceStatus(clientMnemonic, invoiceModel.InvoiceNo);

            //compile accounts/charges for this client

            List<InvoiceSelect> accounts = accdb.GetInvoiceAccounts(clientMnemonic, throughDate).ToList();

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

                List<InvoiceChargeView> charges = chrgdb.GetInvoiceCharges(account.account);
                foreach(InvoiceChargeView chrg in charges)
                {
                    invoiceModel.InvoiceDetails.Add(new InvoiceDetailModel()
                    {
                        Account = account.account,
                        PatientName = account.pat_name,
                        ServiceDate = chrg.TransactionDate,
                        CDM = chrg.ChargeItemId,
                        CPT = "",
                        Description = chrg.ChargeDescription,
                        Qty = chrg.Quantity,
                        Amount = chrg.Amount
                    }) ;
                    amountTotal += chrg.Amount;
                    inpTotal += chrg.HospAmount;
                    retailTotal += chrg.RetailAmount;
                    discountTotal += chrg.RetailAmount - chrg.Amount;
                }

                //write cbill transfer chrg to account
                invoiceAmountTotal += amountTotal;
                invoiceInpTotal += inpTotal;
                invoiceRetailTotal += retailTotal;

                Chrg accChrg = new Chrg();
                accChrg.AccountNo = account.account;
                accChrg.CDMCode = "CBILL";
                accChrg.Invoice = invoiceModel.InvoiceNo;
                accChrg.Quantity = -1;
                accChrg.HospAmount = inpTotal;
                accChrg.RetailAmount = retailTotal;
                accChrg.NetAmount = amountTotal;
                accChrg.FinancialType = "C";
                accChrg.FinCode = account.fin_code;
                accChrg.ServiceDate = DateTime.Today;
                accChrg.ChrgDetails.Add(new ChrgDetail()
                {
                    Cpt4 = "NONE",
                    Type = "NORM",
                    Amount = amountTotal
                });
                chrgdb.AddCharge(accChrg);

                chrgdb.SetChargeInvoiceStatus(account.account, invoiceModel.InvoiceNo);
            }

            //write client invoice transaction on client account

            Chrg invoiceChrg = new Chrg();
            invoiceChrg.AccountNo = clientMnemonic;
            invoiceChrg.CDMCode = "CBILL";
            invoiceChrg.Invoice = invoiceModel.InvoiceNo;
            invoiceChrg.Quantity = 1;
            invoiceChrg.HospAmount = invoiceInpTotal;
            invoiceChrg.RetailAmount = invoiceRetailTotal;
            invoiceChrg.NetAmount = invoiceAmountTotal;
            invoiceChrg.FinancialType = "C";
            invoiceChrg.FinCode = "CLIENT";
            invoiceChrg.ServiceDate = DateTime.Today;
            invoiceChrg.ChrgDetails.Add(new ChrgDetail()
            {
                Cpt4 = "NONE",
                Type = "NORM",
                Amount = invoiceAmountTotal
            });
            chrgdb.AddCharge(invoiceChrg);
            invoiceModel.InvoiceTotal = invoiceAmountTotal;
            invoiceModel.DiscountTotal = discountTotal;

            SaveInvoiceHistory(invoiceModel);


            //invoiceModel.BalanceDue = balanceForward + invoiceAmountTotal;

            InvoicePrint.CreatePDF(invoiceModel, $"c:\\temp\\Invoice-{invoiceModel.ClientMnem}-{invoiceModel.InvoiceNo}.pdf");



        }

        private void SaveInvoiceHistory(InvoiceModel invoiceModel)
        {
            //store the invoice data as xml for storage in invoice history
            XmlSerializer x = new XmlSerializer(invoiceModel.GetType());
            StringWriter textWriter = new StringWriter();

            x.Serialize(textWriter, invoiceModel);

            //write client invoice history record
            InvoiceHistory invoiceHistory = new InvoiceHistory();
            InvoiceHistoryRepository historyRepository = new InvoiceHistoryRepository(_connection);

            invoiceHistory.cl_mnem = invoiceModel.ClientMnem;
            invoiceHistory.discount = invoiceModel.DiscountTotal;
            invoiceHistory.invoice = invoiceModel.InvoiceNo;
            invoiceHistory.thru_date = invoiceModel.ThroughDate;
            invoiceHistory.total_chrg = invoiceModel.InvoiceTotal;
            invoiceHistory.cbill_html = textWriter.ToString();

            //invoiceHistory.balance_due = balanceForward + invoiceAmountTotal;
            //invoiceHistory.bal_forward = balanceForward;
            //invoiceHistory.payments = payments;
            //invoiceHistory.true_balance_due = balanceForward + invoiceAmountTotal;

            historyRepository.Add(invoiceHistory);
        }


        public bool UndoInvoice(string invoiceNo)
        {
            throw new NotImplementedException();
        }

    }

}
