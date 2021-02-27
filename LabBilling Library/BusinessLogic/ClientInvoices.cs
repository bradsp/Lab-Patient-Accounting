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
            _connection = connection;
            dbConnection = new PetaPoco.Database(connection, new CustomSqlServerDatabaseProvider());
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
            invoiceModel.ClientName = client.cli_nme;
            invoiceModel.Address1 = client.addr_1;
            invoiceModel.Address2 = client.addr_2;
            invoiceModel.City = client.city;
            invoiceModel.State = client.st;
            invoiceModel.ZipCode = client.zip;
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
                    ServiceDate = chk.date_rec,
                    Description = string.Format("Payment Received - {0}", chk.source),
                    Amount = chk.amt_paid + chk.write_off + chk.contractual
                });
                payments += chk.amt_paid + chk.write_off + chk.contractual;
                //update chk record with new invoice number
                chk.invoice = invoiceModel.InvoiceNo;
                chkdb.Update(chk);
            }

            //get adjustments since last invoice
            ChrgRepository chrgdb = new ChrgRepository(_connection);
            var chrgs = chrgdb.GetInvoiceCharges(clientMnemonic);

            foreach(InvoiceChargeView chrg in chrgs)
            {
                invoiceModel.InvoiceDetails.Add(new InvoiceDetailModel()
                {
                    ServiceDate = chrg.trans_date,
                    Description = chrg.descript,
                    Qty = chrg.qty,
                    CDM = chrg.cdm,
                    Amount = chrg.amount
                });
                adjustments += chrg.amount;
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
                        ServiceDate = chrg.trans_date,
                        CDM = chrg.cdm,
                        CPT = "",
                        Description = chrg.descript,
                        Qty = chrg.qty,
                        Amount = chrg.amount
                    }) ;
                    amountTotal += chrg.amount;
                    inpTotal += chrg.inp_amt;
                    retailTotal += chrg.retail;
                    discountTotal += chrg.retail - chrg.amount;
                }
                //write cbill transfer chrg to account
                invoiceAmountTotal += amountTotal;
                invoiceInpTotal += inpTotal;
                invoiceRetailTotal += retailTotal;

                Chrg accChrg = new Chrg();
                accChrg.account = account.account;
                accChrg.cdm = "CBILL";
                accChrg.invoice = invoiceModel.InvoiceNo;
                accChrg.qty = -1;
                accChrg.inp_price = inpTotal;
                accChrg.retail = retailTotal;
                accChrg.net_amt = amountTotal;
                accChrg.fin_type = "C";
                accChrg.fin_code = account.fin_code;
                accChrg.service_date = DateTime.Today;
                accChrg.ChrgDetails.Add(new Amt()
                {
                    cpt4 = "NONE",
                    type = "NORM",
                    amount = amountTotal
                });
                chrgdb.AddCharge(accChrg);

                chrgdb.SetChargeInvoiceStatus(account.account, invoiceModel.InvoiceNo);
            }

            //write client invoice transaction on client account

            Chrg invoiceChrg = new Chrg();
            invoiceChrg.account = clientMnemonic;
            invoiceChrg.cdm = "CBILL";
            invoiceChrg.invoice = invoiceModel.InvoiceNo;
            invoiceChrg.qty = 1;
            invoiceChrg.inp_price = invoiceInpTotal;
            invoiceChrg.retail = invoiceRetailTotal;
            invoiceChrg.net_amt = invoiceAmountTotal;
            invoiceChrg.fin_type = "C";
            invoiceChrg.fin_code = "CLIENT";
            invoiceChrg.service_date = DateTime.Today;
            invoiceChrg.ChrgDetails.Add(new Amt()
            {
                cpt4 = "NONE",
                type = "NORM",
                amount = invoiceAmountTotal
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

            return;
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
                account.account = clientMnem;
                account.pat_name = client.cli_nme;
                account.meditech_account = clientMnem;
                account.fin_code = "CLIENT";
                account.trans_date = DateTime.Today;
                account.cl_mnem = clientMnem;

                accdb.Add(account);
            }

            return;

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
