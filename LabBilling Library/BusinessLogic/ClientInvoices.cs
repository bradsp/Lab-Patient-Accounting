using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace LabBilling.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientInvoices
    {
        private string _connection;
        private readonly PetaPoco.Database dbConnection;

        public ClientInvoices(string connection)
        {
            if(connection == "" || connection == null)
            {
                throw new ArgumentException("Must have a valid connection string", "connection");
            }
            _connection = connection;
            dbConnection = new PetaPoco.Database(connection, new SqlServerDatabaseProvider());
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

        /// <summary>
        /// Generate an invoice for a single client.
        /// </summary>
        /// <param name="clientMnemonic"></param>
        /// <param name="throughDate"></param>
        public void GenerateInvoice(string clientMnemonic, DateTime throughDate)
        {
            Log.Instance.Trace("Entering");
            if (clientMnemonic == null || throughDate == null)
                throw new ArgumentNullException();
            SystemParametersRepository systemdb = new SystemParametersRepository(_connection);
            AccountRepository accdb = new AccountRepository(_connection);
            Account acc = accdb.GetByAccount(clientMnemonic);
            if (acc == null)
                NewClient(clientMnemonic);

            InvoiceModel invoiceModel = new InvoiceModel();

            invoiceModel.BillingCompanyName = systemdb.GetByKey("invoice_company_name") ?? "Medical Center Laboratory";
            invoiceModel.BillingCompanyAddress = systemdb.GetByKey("invoice_company_address") ?? "PO Box 3099";
            invoiceModel.BillingCompanyCity = systemdb.GetByKey("invoice_company_city") ?? "Jackson";
            invoiceModel.BillingCompanyState = systemdb.GetByKey("invoice_company_state") ?? "TN";
            invoiceModel.BillingCompanyZipCode = systemdb.GetByKey("invoice_company_zipcode") ?? "38303";
            invoiceModel.BillingCompanyPhone = systemdb.GetByKey("invoice_company_zipcode") ?? "731-541-7300 / 866-396-8537";
            invoiceModel.ImageFilePath = systemdb.GetByKey("invoice_logo_image_path") ?? @"../../../ViewerClientBill/MCLleft.jpg";

            ClientRepository clientdb = new ClientRepository(_connection);
            NumberRepository numberdb = new NumberRepository(_connection);

            Client client = clientdb.GetClient(clientMnemonic);
            invoiceModel.ClientName = client.Name;
            invoiceModel.Address1 = client.StreetAddress1;
            invoiceModel.Address2 = client.StreetAddress2;
            invoiceModel.City = client.City;
            invoiceModel.State = client.State;
            invoiceModel.ZipCode = client.ZipCode;
            invoiceModel.InvoiceDate = DateTime.Today;
            invoiceModel.InvoiceNo = numberdb.GetNumber("invoice").ToString();

            double balanceForward = clientdb.Balance(clientMnemonic);
            invoiceModel.BalanceForward = balanceForward;
            double payments = 0.00;
            double adjustments = 0.00;
            invoiceModel.InvoiceDetails = new List<InvoiceDetailModel>();

            //get payments since last invoice
            ChkRepository chkdb = new ChkRepository(_connection);
            var chks = chkdb.GetByAccount(clientMnemonic, false);

            foreach(Chk chk in chks)
            {
                invoiceModel.InvoiceDetails.Add(new InvoiceDetailModel()
                {
                    ServiceDate = chk.DateReceived,
                    Description = string.Format("Payment Received - {0}", chk.Source),
                    Amount = chk.PaidAmount + chk.WriteOffAmount + chk.ContractualAmount
                });
                payments += chk.PaidAmount + chk.WriteOffAmount + chk.ContractualAmount;
                //update chk record with new invoice number
                chk.Invoice = invoiceModel.InvoiceNo;
                chkdb.Update(chk);
            }

            //get adjustments since last invoice
            ChrgRepository chrgdb = new ChrgRepository(_connection);
            var chrgs = chrgdb.GetInvoiceCharges(clientMnemonic);

            foreach(InvoiceChargeView chrg in chrgs)
            {
                invoiceModel.InvoiceDetails.Add(new InvoiceDetailModel()
                {
                    ServiceDate = chrg.TransactionDate,
                    Description = chrg.ChargeDescription,
                    Qty = chrg.Quantity,
                    CDM = chrg.ChargeItemId,
                    Amount = chrg.Amount
                });
                adjustments += chrg.Amount;
            }

            chrgdb.SetChargeInvoiceStatus(clientMnemonic, invoiceModel.InvoiceNo);

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

            //store the invoice data as xml for storage in invoice history
            XmlSerializer x = new XmlSerializer(invoiceModel.GetType());
            StringWriter textWriter = new StringWriter();

            x.Serialize(textWriter, invoiceModel);

            //write client invoice history record
            InvoiceHistory invoiceHistory = new InvoiceHistory();
            InvoiceHistoryRepository historyRepository = new InvoiceHistoryRepository(_connection);

            invoiceHistory.cl_mnem = clientMnemonic;
            invoiceHistory.balance_due = balanceForward + invoiceAmountTotal;
            invoiceHistory.bal_forward = balanceForward;
            invoiceHistory.discount = discountTotal;
            invoiceHistory.invoice = invoiceModel.InvoiceNo;
            invoiceHistory.payments = payments;
            invoiceHistory.thru_date = throughDate;
            invoiceHistory.total_chrg = invoiceAmountTotal;
            invoiceHistory.true_balance_due = balanceForward + invoiceAmountTotal;
            invoiceHistory.cbill_html = textWriter.ToString();

            historyRepository.Add(invoiceHistory);

            invoiceModel.BalanceDue = balanceForward + invoiceAmountTotal;

            InvoicePrint.CreatePDF(invoiceModel, $"c:\\temp\\{invoiceModel.InvoiceNo}.pdf");

        }

        /// <summary>
        /// Adds new client account if it does not exist
        /// </summary>
        private void NewClient(string clientMnem)
        {
            //check to see if client is valid and client exists
            ClientRepository clientdb = new ClientRepository(_connection);
            Client client = clientdb.GetClient(clientMnem);

            if (client == null)
            {
                throw new ArgumentException("Client mnemonic is not found in client table","clientMnem");
            }

            Account account;

            //check to see if client account exists
            AccountRepository accdb = new AccountRepository(_connection);
            account = accdb.GetByAccount(clientMnem);

            if(account == null)
            {
                //account does not exist - add the account
                account = new Account();
                account.AccountNo = clientMnem;
                account.PatFullName = client.Name;
                account.MeditechAccount = clientMnem;
                account.FinCode = "CLIENT";
                account.TransactionDate = DateTime.Today;
                account.ClientMnem = clientMnem;

                accdb.Add(account);
            }

        }

        public List<UnbilledClient> GetUnbilledClients(DateTime thruDate)
        {
            var cmd = PetaPoco.Sql.Builder
                .Append("select vbs.cl_mnem as 'ClientMnem', client.cli_nme as 'ClientName', dictionary.clienttype.description as 'ClientType', ")
                .Append("sum(dbo.GetAccBalByDate(vbs.account, GETDATE())) as 'UnbilledAmount' ")
                .Append("from vw_cbill_select vbs join client on vbs.cl_mnem = client.cli_mnem ")
                .Append("join dictionary.clienttype on client.[type] = dictionary.clienttype.[type] ")
                .Append("where trans_date <= @0", thruDate.ToShortDateString())
                .Append("group by vbs.cl_mnem, client.cli_nme, dictionary.clienttype.description ")
                .Append("order by vbs.cl_mnem ");

            return dbConnection.Fetch<UnbilledClient>(cmd);
        }

        public List<UnbilledAccounts> GetUnbilledAccounts(string clientMnem, DateTime thruDate)
        {
            var cmd = PetaPoco.Sql.Builder
                .Append("select vbs.cl_mnem, vbs.account, vbs.trans_date, vbs.pat_name, vbs.fin_code, ")
                .Append("dbo.GetAccBalByDate(vbs.account, GETDATE()) as 'UnbilledAmount' ")
                .Append("from vw_cbill_select vbs ")
                .Append("where cl_mnem = @0 ", clientMnem)
                .Append("and trans_date <= @0 ", thruDate);

            return dbConnection.Fetch<UnbilledAccounts>(cmd);

        }
    }

}
