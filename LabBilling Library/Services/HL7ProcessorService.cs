using System;
using System.Collections.Generic;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using j4jayant.HL7.Parser;
using Utilities;
using LabBilling.Logging;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.Services;

public sealed class HL7ProcessorService
{
    j4jayant.HL7.Parser.Message hl7Message;
    private MessageInbound currentMessage;
    private Account accountRecord = new();
    private Phy phy = new();
    private string MFNeventCode;
    private List<MessageInbound> messagesInbound = new List<MessageInbound>();

    private List<ChargeTransaction> chargeTransactions = new();
    private readonly IAppEnvironment appEnvironment;

    public HL7ProcessorService(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
    }

    private class ChargeTransaction
    {
        public string Account { get; set; }
        public string Cdm { get; set; }
        public int Qty { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Comment { get; set; } = null;
        public string RefNumber { get; set; } = null;
    }

    public enum Status
    {
        NotProcessed,
        Processed,
        Failed,
        DoNotProcess
    }

    public static string StatusToString(Status status)
    {
        string processStatus = null;
        processStatus = status switch
        {
            Status.NotProcessed => "N",
            Status.Processed => "P",
            Status.Failed => "F",
            Status.DoNotProcess => "DNP",
            _ => String.Empty,
        };
        return processStatus;
    }

    public static string StatusToString(string status)
    {
        string processStatus = null;

        processStatus = status switch
        {
            "NotProcessed" => "N",
            "Processed" => "P",
            "Failed" => "F",
            "DoNotProcess" => "DNP",
            _ => string.Empty,
        };
        return processStatus;
    }

    public List<MessageInbound> GetMessages(DateTime fromDate, DateTime thruDate)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.MessagesInboundRepository.GetByDateRange(fromDate, thruDate);
    }

    public void ProcessMessage(int systemMessageId)
    {
        Log.Instance.Trace($"Entering");
        using UnitOfWorkMain unitOfWork = new(appEnvironment);

        currentMessage = unitOfWork.MessagesInboundRepository.GetById(systemMessageId);
        if (currentMessage != null)
            ProcessMessage();

    }

    private void ProcessMessage()
    {
        Log.Instance.Debug($"Processing {currentMessage.MessageType} for account {currentMessage.SourceAccount}");
        Console.WriteLine($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffffffK} - Processing {currentMessage.MessageType} for account {currentMessage.SourceAccount}");
        UnitOfWorkMain unitOfWork = new(appEnvironment);
        try
        {
            var result = ParseHL7(currentMessage.HL7Message);

            currentMessage.ProcessFlag = StatusToString(result.status);
            currentMessage.ProcessStatusMsg = result.statusText;
            currentMessage.Errors = result.errors.ToString();

            unitOfWork.MessagesInboundRepository.Update(currentMessage, new[]
            {
                nameof(MessageInbound.ProcessFlag),
                nameof(MessageInbound.ProcessStatusMsg),
                nameof(MessageInbound.Errors)
            });
            Console.WriteLine($"Processing {currentMessage.MessageType} for account {currentMessage.SourceAccount} complete.");
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex.Message);

            currentMessage.ProcessFlag = StatusToString(Status.Failed);
            currentMessage.ProcessStatusMsg = "Exception encountered during process.";
            currentMessage.Errors = ex.Message + "\n" + ex.StackTrace;

            Console.WriteLine($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffffffK} - Exception encountered during process.");
            unitOfWork.MessagesInboundRepository.Update(currentMessage, new[]
            {
                nameof(MessageInbound.ProcessFlag),
                nameof(MessageInbound.ProcessStatusMsg),
                nameof(MessageInbound.Errors)
            });
        }
    }

    public void ProcessMessages()
    {
        Log.Instance.Trace($"Entering - Querying messages to process");
        UnitOfWorkMain unitOfWork = new(appEnvironment);
        var msgsToProcess = unitOfWork.MessagesInboundRepository.GetUnprocessedMessages();

        foreach (var msg in msgsToProcess)
        {
            currentMessage = msg;
            ProcessMessage();
        }
    }

    private (Status status, string statusText, StringBuilder errors) ParseHL7(string message)
    {
        Log.Instance.Debug($"Parsing message: {message}");
        hl7Message = new j4jayant.HL7.Parser.Message(message);
        StringBuilder errors = new();
        Status processStatus;

        bool isParsed = false;
        accountRecord = new Account();

        try
        {
            isParsed = hl7Message.ParseMessage();
        }
        catch (Exception ex)
        {

        }

        if (isParsed)
        {
            string statusText;
            //process ADT message
            if (hl7Message.MessageStructure == "ADT_A04" || hl7Message.MessageStructure == "ADT_A08")
            {
                Log.Instance.Trace($"Message type: {hl7Message.MessageStructure} - Control ID: {hl7Message.MessageControlID}");
                var result = ProcessADTMessage();
                errors = result.errors;
                statusText = result.statusText;
                processStatus = result.status;
            }
            else if (hl7Message.MessageStructure == "ADT_A03")
            {
                Log.Instance.Trace($"Message type: {hl7Message.MessageStructure} - Control ID: {hl7Message.MessageControlID}");
                statusText = "Skipping A03";
                processStatus = Status.DoNotProcess;
            }
            //process Charge message
            else if (hl7Message.MessageStructure == "DFT_P03")
            {
                Log.Instance.Trace($"Message type: {hl7Message.MessageStructure} - Control ID: {hl7Message.MessageControlID}");
                var result = ProcessDFTMessage();
                errors = result.errors;
                statusText = result.statusText;
                processStatus = result.status;
            }
            //process MFN message
            else if (hl7Message.MessageStructure == "MFN_M02")
            {
                Log.Instance.Trace($"Message type: {hl7Message.MessageStructure} - Control ID: {hl7Message.MessageControlID}");
                var result = ProcessMFNMessage();
                errors = result.errors;
                statusText = result.statusText;
                processStatus = result.status;

            }
            else
            {
                Log.Instance.Trace($"Message type: {hl7Message.MessageStructure} - Control ID: {hl7Message.MessageControlID}");
                statusText = "Invalid message type. Not processed.";
                processStatus = Status.DoNotProcess;
            }

            return (processStatus, statusText, errors);
        }
        else
        {
            Log.Instance.Error("Message format error. Unable to parse.");
            errors.AppendLine("Message format error. Unable to parse.");
            return (Status.Processed, "Message format error. Unable to parse.", errors);
        }
    }

    private (Status status, string statusText, StringBuilder errors) ProcessADTMessage()
    {
        Log.Instance.Trace("Entering");
        bool canFile = true;
        StringBuilder errors = new();
        UnitOfWorkMain unitOfWork = new(appEnvironment);
        AccountService accountService = new(appEnvironment);
        string[] invalidFacilities = new string[] { "001", "005", "007", "008", "009", "010", "080", "800", "850", "900" };

        string facility = hl7Message.GetValue("MSH.4");

        string statusText;
        if (invalidFacilities.Contains(facility))
        {
            //invalid facility - do not file
            Log.Instance.Debug($"Invalid facility {facility}");
            statusText = "Not a valid facility for Lab outreach";
            return (Status.DoNotProcess, statusText, errors);
        }

        ParsePID();
        ParsePV1();
        ParseDG1();
        ParseGT1();
        ParseIN1IN2();

        //perform error checking
        accountRecord.Client = unitOfWork.ClientRepository.GetClient(accountRecord.ClientMnem);
        if (accountRecord.Client == null)
        {
            //error - invalid client
            Log.Instance.Error($"[ERROR] Invalid client {accountRecord.ClientMnem}");
            errors.AppendLine($"[ERROR] Invalid client {accountRecord.ClientMnem}");
            canFile = false;
        }
        else
        {
            accountRecord.ClientName = accountRecord.Client.Name;
        }

        foreach (var ins in accountRecord.Insurances)
        {
            var insc = unitOfWork.InsCompanyRepository.GetByCode(ins.InsCode);
            if (insc == null)
            {
                Log.Instance.Warn($"[WARNING] Insurance code not valid {ins.InsCode}");
                errors.AppendLine($"[WARNING] Insurance code not valid {ins.InsCode}");
                canFile = true;
            }
            else
            {
                ins.FinCode = insc.FinancialCode;
            }
        }
        if (accountRecord.Client != null)
        {
            switch (accountRecord.Client.BillMethod)
            {
                case "INVOICE":
                    accountRecord.FinCode = "Y";
                    break;
                case "PATIENT":
                    if (accountRecord.Insurances.Count > 0)
                    {
                        if (accountRecord.FinCode != accountRecord.Insurances[0].FinCode)
                        {
                            accountRecord.FinCode = accountRecord.Insurances[0].FinCode;
                        }
                    }
                    if (accountRecord.FinCode == "Y")
                    {
                        accountRecord.FinCode = "K";
                    }
                    break;
                case "PER ACCOUNT":
                    if (accountRecord.FinCode != "Y")
                    {
                        if (accountRecord.Insurances.Count > 0)
                        {
                            if (accountRecord.FinCode != accountRecord.Insurances[0].FinCode)
                            {
                                accountRecord.FinCode = accountRecord.Insurances[0].FinCode;
                            }
                        }
                    }
                    break;
                default:
                    if (accountRecord.FinCode != "Y")
                    {
                        if (accountRecord.Insurances.Count > 0)
                        {
                            if (accountRecord.FinCode != accountRecord.Insurances[0].FinCode)
                            {
                                accountRecord.FinCode = accountRecord.Insurances[0].FinCode;
                            }
                        }
                    }
                    break;
            }
        }

        if (string.IsNullOrEmpty(accountRecord.FinCode))
        {
            Log.Instance.Error($"[ERROR] No fin code");
            accountRecord.FinCode = "K";
        }

        foreach (var dx in accountRecord.Pat.Diagnoses)
        {
            var dxRecord = unitOfWork.DictDxRepository.GetByCode(dx.Code, (DateTime)accountRecord.TransactionDate);

            if (dxRecord == null)
            {
                Log.Instance.Warn($"[WARN] Diagnosis {dx.Code} not found.");
                errors.AppendLine($"[WARN] Diagnosis {dx.Code} not found.");

            }
        }

        try
        {
            //check for existing account
            var existingAccount = unitOfWork.AccountRepository.GetByAccount(accountRecord.AccountNo);
            if (existingAccount != null)
            {
                //compare and update existingAccount for update -
                //need to decide when to update and when to not update - maybe update until past initial hold period

                //if fin_code or client are different - determine if charges need to be reprocessed (after the update)
                bool reprocessCharges = false;

                accountRecord.Fin = unitOfWork.FinRepository.GetFin(accountRecord.FinCode);

                if (existingAccount.Fin.FinClass != accountRecord.Fin.FinClass)
                    reprocessCharges = true;

                if (accountRecord.Fin.FinClass == "C" && existingAccount.ClientMnem != accountRecord.ClientMnem)
                    reprocessCharges = true;

                //loop through all properties and determine which ones are different

                if (canFile)
                {
                    // add account
                    unitOfWork.AccountRepository.Update(accountRecord);
                    unitOfWork.PatRepository.Save(accountRecord.Pat);
                    foreach (var ins in accountRecord.Insurances)
                    {
                        if (ins.Account != accountRecord.AccountNo)
                            ins.Account = accountRecord.AccountNo;

                        unitOfWork.InsRepository.Save(ins);
                    }

                    statusText = $"{accountRecord.AccountNo} updated.";

                    if(reprocessCharges)
                    {
                        accountService.ReprocessCharges(accountRecord.AccountNo, "Client or FinClass change.");
                    }

                    return (Status.Processed, statusText, errors);
                }
                else
                {
                    return (Status.Failed, "Required information missing. See errors.", errors);
                }
            }
            else
            {
                if (canFile)
                {
                    // add account
                    unitOfWork.AccountRepository.Add(accountRecord);
                    statusText = $"{accountRecord.AccountNo} added.";
                    return (Status.Processed, statusText, errors);
                }
                else
                {
                    return (Status.Failed, "Required information missing. See errors.", errors);
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException.Message.Contains("truncated"))
            {
                errors.Append("Data is longer than a database field. Record not written.");
                Log.Instance.Error(ex.InnerException, "Data is longer than a database field. Record not written.");
            }
            else
            {
                errors.Append(ex.Message);
                if (ex.InnerException != null)
                {
                    errors.Append(ex.Message);
                }
            }
            return (Status.Failed, "Database error", errors);
        }
    }

    private (Status status, string statusText, StringBuilder errors) ProcessDFTMessage()
    {
        Log.Instance.Trace("Entering");
        bool canFile = true;
        StringBuilder errors = new();
        chargeTransactions = new List<ChargeTransaction>();

        UnitOfWorkMain unitOfWork = new(appEnvironment);
        AccountService accountService = new(appEnvironment);

        ParsePID();
        ParsePV1();
        ParseFT1();

        //make sure account exists - if not, create it from the PID segment
        var existingAccount = unitOfWork.AccountRepository.GetByAccount(accountRecord.AccountNo);

        if (existingAccount == null)
        {
            accountRecord.Client = unitOfWork.ClientRepository.GetClient(accountRecord.ClientMnem);
            if (accountRecord.Client == null)
            {
                //error - invalid client
                Log.Instance.Error($"[ERROR] Invalid client {accountRecord.ClientMnem}");
                errors.AppendLine($"[ERROR] Invalid client {accountRecord.ClientMnem}");
                canFile = false;
            }
            else
            {
                accountRecord.ClientName = accountRecord.Client.Name;
            }

            if (accountRecord.Client != null)
            {
                switch (accountRecord.Client.BillMethod)
                {
                    case "INVOICE":
                        accountRecord.FinCode = "Y";
                        break;
                    case "PATIENT":
                        break;
                    case "PER ACCOUNT":
                        if (accountRecord.FinCode != "Y")
                        {
                            accountRecord.FinCode = "K";
                        }
                        break;
                    default:
                        accountRecord.FinCode = "K";
                        break;
                }
            }
            if (string.IsNullOrEmpty(accountRecord.FinCode))
            {
                Log.Instance.Error($"[ERROR] No fin code");
                accountRecord.FinCode = "K";
            }

            if(canFile)
            {
                unitOfWork.AccountRepository.Add(accountRecord);
                unitOfWork.PatRepository.Save(accountRecord.Pat);
                existingAccount = unitOfWork.AccountRepository.GetByAccount(accountRecord.AccountNo);
            }
        }

        if (canFile)
        {
            foreach (var transaction in chargeTransactions)
            {
                //if the account has no previous charges, make sure the account transaction date matches the charge service date
                if(accountRecord.Charges.Count == 0)
                {
                    if (accountRecord.TransactionDate != transaction.ServiceDate)
                    {
                        Log.Instance.Debug($"Account {accountRecord.AccountNo} transaction date {accountRecord.TransactionDate} does not match charge service date {transaction.ServiceDate}. Account updated.");
                        accountRecord.TransactionDate = transaction.ServiceDate;

                        unitOfWork.AccountRepository.Update(accountRecord, new string[] { nameof(Account.TransactionDate) });
                    }
                }

                transaction.Account = accountRecord.AccountNo;
                transaction.Comment = $"MSG ID: {currentMessage.SystemMsgId}";
                Log.Instance.Debug($"Adding charge {transaction.Account},{transaction.Cdm},{transaction.Qty},{transaction.ServiceDate},{transaction.Comment},{transaction.RefNumber}");
                try
                {
                    accountService.AddCharge(existingAccount, transaction.Cdm, transaction.Qty, transaction.ServiceDate, transaction.Comment, transaction.RefNumber);
                }
                catch (CdmNotFoundException cdmex)
                {
                    errors.AppendLine($"[WARN] {cdmex.Message} for {transaction.Cdm} on {existingAccount.AccountNo}. Charge not posted.");
                    return (Status.Failed, $"{accountRecord.AccountNo} - charges not posted.", errors);
                }
                catch(InvalidClientException cliex)
                {
                    errors.AppendLine($"[ERROR] {cliex.Message} for {existingAccount.ClientMnem} on {existingAccount.AccountNo}. Charge not posted.");
                    Log.Instance.Error(cliex);
                    return (Status.Failed, $"{accountRecord.AccountNo} - charges not posted.", errors);
                }
                catch(Exception ex)
                {
                    errors.AppendLine($"[ERROR] {ex.Message} for {existingAccount.ClientMnem} on {existingAccount.AccountNo}. Charge not posted.");
                    Log.Instance.Error(ex, $"[ERROR] {ex.Message} for {existingAccount.ClientMnem} on {existingAccount.AccountNo}. Charge not posted.");
                    return (Status.Failed, $"{accountRecord.AccountNo} - charges not posted.", errors);
                }

            }
            return (Status.Processed, $"{accountRecord.AccountNo} - charges posted.", errors);
        }
        else
        {
            return (Status.Failed, "Unable to process charges. See errors.", errors);
        }

    }

    private (Status status, string statusText, StringBuilder errors) ProcessMFNMessage()
    {
        Log.Instance.Trace("Entering");
        //bool canFile = true;
        StringBuilder errors = new StringBuilder();
        string statusText = string.Empty;
        UnitOfWorkMain unitOfWork = new(appEnvironment);
        string eventCode = ParseMFE();
        ParseSTF();
        ParsePRA();

        //MUP = update
        //MAD = add
        //MDL = delete
        //MDC = deactivate
        //MAC = reactivate
        //MSU = suspend

        Phy existingPhy = unitOfWork.PhyRepository.GetByNPI(phy.NpiId);

        if (existingPhy == null)
        {
            Log.Instance.Debug("Provider added");
            unitOfWork.PhyRepository.Add(phy);
            return (Status.Processed, $"Provider added.", errors);
        }
        else
        {
            phy.uri = existingPhy.uri;
            phy.rowguid = existingPhy.rowguid;

            if (eventCode == "MDL" || eventCode == "MDC")
            {
                phy.IsDeleted = true;
                Log.Instance.Debug("Provider deleted");
            }
            else
            {
                phy.IsDeleted = false;
                Log.Instance.Debug("Provider updated");
            }
            unitOfWork.PhyRepository.Update(phy);
            return (Status.Processed, $"Provider updated.", errors);
        }
    }

    private void ParseMSH()
    {

    }

    private static string ValidateZipCode(string value)
    {
        Regex validateZipRegex = new Regex("^[0-9]{5}(?:-[0-9]{4})?$");

        if (!validateZipRegex.IsMatch(value))
        {
            return "";
        }
        else
        {
            return value;
        }
    }

    private void ParsePID()
    {
        //Segment segPID = hl7Message.DefaultSegment("PID");
        accountRecord.EMPINumber = hl7Message.GetValue("PID.2.1");

        //if (hl7Message.HasRepetitions("PID.4"))
        //{
        //    List<Field> repList = hl7Message.Segments("PID")[0].Fields(4).Repetitions();
        //    foreach(var field in repList)
        //    {
        //        string mrn = field.Components(1).Value;
        //        string mrntype = field.Components(4).Value;
        //    }
        //}
        //else
        //{
        //    if (hl7Message.Segments("PID")[0].Fields(4).IsComponentized)
        //        accountRecord.MRN = hl7Message.GetValue("PID.4.1");
        //    else
        //        accountRecord.MRN = hl7Message.GetValue("PID.4");
        //}

        if (hl7Message.HasRepetitions("PID.5"))
        {
            List<Field> repList = hl7Message.Segments("PID")[0].Fields(5).Repetitions();

            accountRecord.PatLastName = repList[0].Components(1).Value;
            accountRecord.PatFirstName = repList[0].Components(2).Value;
            accountRecord.PatMiddleName = repList[0].Components(3).Value;
            accountRecord.PatNameSuffix = repList[0].Components(4).Value;
        }
        else
        {
            accountRecord.PatLastName = hl7Message.GetValue("PID.5.1");
            accountRecord.PatFirstName = hl7Message.GetValue("PID.5.2");
            accountRecord.PatMiddleName = hl7Message.GetValue("PID.5.3");
            accountRecord.PatNameSuffix = hl7Message.GetValue("PID.5.4");
        }

        accountRecord.BirthDate = new DateTime().ParseHL7Date(hl7Message.GetValue("PID.7").Left(8));
        accountRecord.Sex = hl7Message.GetValue("PID.8");
        if(!Dictionaries.sexSource.ContainsKey(accountRecord.Sex))
        {
            accountRecord.Sex = "U";
        }
        if(hl7Message.HasRepetitions("PID.10"))
        {
            List<Field> repList = hl7Message.Segments("PID")[0].Fields(10).Repetitions();
            accountRecord.Pat.Race = repList[0].Components(1).Value;
        }
        else
        {
            accountRecord.Pat.Race = hl7Message.GetValue("PID.10.1");
        }
        if (hl7Message.HasRepetitions("PID.11"))
        {
            List<Field> repList = hl7Message.Segments("PID")[0].Fields(11).Repetitions();

            accountRecord.Pat.Address1 = repList[0].Components(1).Value;
            accountRecord.Pat.Address2 = repList[0].Components(2).Value;
            accountRecord.Pat.City = repList[0].Components(3).Value;
            accountRecord.Pat.State = repList[0].Components(4).Value;
            accountRecord.Pat.ZipCode = ValidateZipCode(repList[0].Components(5).Value);

            accountRecord.Pat.EmailAddress = repList[1].Components(1).Value;
        }
        else
        {
            accountRecord.Pat.Address1 = hl7Message.GetValue("PID.11.1");
            accountRecord.Pat.Address2 = hl7Message.GetValue("PID.11.2");
            accountRecord.Pat.City = hl7Message.GetValue("PID.11.3");
            accountRecord.Pat.State = hl7Message.GetValue("PID.11.4");
            accountRecord.Pat.ZipCode = ValidateZipCode(hl7Message.GetValue("PID.11.5"));
        }

        if (hl7Message.HasRepetitions("PID.13"))
        {
            List<Field> repList = hl7Message.Segments("PID")[0].Fields(13).Repetitions();
            accountRecord.Pat.PrimaryPhone = repList[0].Components(1).Value;
        }
        accountRecord.Pat.MaritalStatus = hl7Message.GetValue("PID.16");
        if(!Dictionaries.maritalSource.ContainsKey(accountRecord.Pat.MaritalStatus))
        {
            accountRecord.Pat.MaritalStatus = "U";
        }
        accountRecord.AccountNo = "L" + hl7Message.GetValue("PID.18.1");
        accountRecord.MeditechAccount = accountRecord.AccountNo;
        accountRecord.Pat.AccountNo = accountRecord.AccountNo;
        accountRecord.SocSecNo = hl7Message.GetValue("PID.19").Replace("-", string.Empty);

    }

    private void ParsePV1()
    {
        UnitOfWorkMain unitOfWork = new(appEnvironment);

        accountRecord.ClientMnem = string.IsNullOrEmpty(hl7Message.GetValue("PV1.3.1")) 
            ? hl7Message.GetValue("PV1.3.1") 
            : unitOfWork.MappingRepository.GetMappedValue("CLIENT", "CERNER", hl7Message.GetValue("PV1.3.1"));

        if(string.IsNullOrEmpty(accountRecord.ClientMnem))
        {
            accountRecord.ClientMnem = string.IsNullOrEmpty(hl7Message.GetValue("PV1.6.1")) 
                ? hl7Message.GetValue("PV1.6.1") 
                : unitOfWork.MappingRepository.GetMappedValue("CLIENT", "CERNER", hl7Message.GetValue("PV1.6.1"));
        }

        if (string.IsNullOrEmpty(accountRecord.ClientMnem))
        {
            accountRecord.ClientMnem = string.IsNullOrEmpty(hl7Message.GetValue("PV1.3.4")) 
                ? hl7Message.GetValue("PV1.3.4")
                : unitOfWork.MappingRepository.GetMappedValue("CLIENT", "CERNER", hl7Message.GetValue("PV1.3.4"));
        }
        accountRecord.FinCode = string.IsNullOrEmpty(hl7Message.GetValue("PV1.20"))
            ? hl7Message.GetValue("PV1.20")
            : unitOfWork.MappingRepository.GetMappedValue("FIN_CODE", "CERNER", hl7Message.GetValue("PV1.20"));
        accountRecord.OriginalFinCode = accountRecord.FinCode;
        accountRecord.TransactionDate = new DateTime().ParseHL7Date(hl7Message.GetValue("PV1.44"));
        if(accountRecord.TransactionDate == DateTime.MinValue)
        {
            //default transaction date to today
            accountRecord.TransactionDate = DateTime.Today;
        }

        accountRecord.Pat.ProviderId = string.IsNullOrEmpty(hl7Message.GetValue("PV1.17.1"))
           ? hl7Message.GetValue("PV1.17.1")
           : unitOfWork.MappingRepository.GetMappedValue("PHY_ID", "CERNER", hl7Message.GetValue("PV1.17.1"));

        accountRecord.Pat.Physician = unitOfWork.PhyRepository.GetByNPI(accountRecord.Pat.ProviderId);

    }

    private void ParseDG1()
    {
        List<Segment> dg1Segments = hl7Message.Segments("DG1");

        foreach (var dx in dg1Segments)
        {
            PatDiag patDiag = new()
            {
                Code = dx.Fields(3).Value.Replace(".", "")
            };

            accountRecord.Pat.Diagnoses.Add(patDiag);
        }
    }

    private void ParseGT1()
    {
        UnitOfWorkMain unitOfWork = new(appEnvironment);
        if (hl7Message.Segments("GT1").Count > 0)
        {
            accountRecord.Pat.GuarantorLastName = hl7Message.GetValue("GT1.3.1");
            accountRecord.Pat.GuarantorFirstName = hl7Message.GetValue("GT1.3.2");

            if (hl7Message.HasRepetitions("GT1.5"))
            {
                List<Field> repList = hl7Message.Segments("GT1")[0].Fields(5).Repetitions();
                accountRecord.Pat.GuarantorAddress = repList[0].Components(1).Value;
                accountRecord.Pat.GuarantorCity = repList[0].Components(3).Value;
                accountRecord.Pat.GuarantorState = repList[0].Components(4).Value;
                accountRecord.Pat.GuarantorZipCode = ValidateZipCode(repList[0].Components(5).Value);
                //email address is field 6
            }
            else
            {
                accountRecord.Pat.GuarantorAddress = hl7Message.GetValue("GT1.5.1");
                accountRecord.Pat.GuarantorCity = hl7Message.GetValue("GT1.5.3");
                accountRecord.Pat.GuarantorState = hl7Message.GetValue("GT1.5.4");
                accountRecord.Pat.GuarantorZipCode = ValidateZipCode(hl7Message.GetValue("GT1.5.5"));
            }

            if (hl7Message.HasRepetitions("GT1.6"))
            {
                List<Field> repList = hl7Message.Segments("GT1")[0].Fields(6).Repetitions();
                accountRecord.Pat.GuarantorPrimaryPhone = repList[0].Value;
            }
            else
            {
                accountRecord.Pat.GuarantorPrimaryPhone = hl7Message.GetValue("GT1.6");
            }
            accountRecord.Pat.GuarRelationToPatient = hl7Message.GetValue("GT1.11") != string.Empty 
                ? unitOfWork.MappingRepository.GetMappedValue("GUAR_REL", "CERNER", hl7Message.GetValue("GT1.11")) 
                : string.Empty;
        }
    }

    private void ParseIN1IN2()
    {
        var segIn1 = hl7Message.Segments("IN1");
        var segIn2 = hl7Message.Segments("IN2");

        UnitOfWorkMain unitOfWork = new(appEnvironment);
        for (int i = 0; i < segIn1.Count; i++)
        {
            Segment in1 = segIn1[i];

            Segment in2 = null;

            if (segIn2.Count >= i + 1)
                in2 = segIn2[i];

            Ins ins = new()
            {
                Account = accountRecord.AccountNo
            };
            if (in1.Fields(1).Value == "1")
                ins.Coverage = InsCoverage.Primary;
            else if (in1.Fields(1).Value == "2")
                ins.Coverage = InsCoverage.Secondary;
            else if (in1.Fields(1).Value == "3")
                ins.Coverage = InsCoverage.Tertiary;
            else
            {
                //this is not a valid insurance coverage type -- disregard
                return;
            }

            ins.InsCode = string.IsNullOrEmpty(in1.Fields(2).Components(1).Value)
                ? in1.Fields(2).Components(1).Value
                : unitOfWork.MappingRepository.GetMappedValue("INS_CODE", "CERNER", in1.Fields(2).Components(1).Value);
            if (ins.InsCode == String.Empty)
            {
                ins.InsCode = in1.Fields(2).Components(1).Value;
            }

            ins.PlanName = in1.Fields(2).Components(2).Value;
            ins.PlanStreetAddress1 = in1.Fields(5).Components(1).Value;
            ins.PlanStreetAddress2 = in1.Fields(5).Components(2).Value;
            ins.PlanCityState = $"{in1.Fields(5).Components(3).Value}, {in1.Fields(5).Components(4).Value} {in1.Fields(5).Components(5).Value}";
            ins.GroupNumber = in1.Fields(8).Value;
            ins.GroupName = in1.Fields(11).Value;

            ins.HolderLastName = in1.Fields(16).Components(1).Value;
            ins.HolderFirstName = in1.Fields(16).Components(2).Value;
            ins.HolderMiddleName = in1.Fields(16).Components(3).Value;
            ins.HolderSex = in1.Fields(43).Value;

            if (ins.HolderSex != "M" && ins.HolderSex != "F")
                ins.HolderSex = String.Empty;

            ins.Relation = string.IsNullOrEmpty(in1.Fields(17).Value)
                ? in1.Fields(17).Value
                : unitOfWork.MappingRepository.GetMappedValue("GUAR_REL", "CERNER", in1.Fields(17).Value);

            if (!string.IsNullOrEmpty(in1.Fields(18).Value))
            {
                ins.HolderBirthDate = new DateTime().ParseHL7Date(in1.Fields(18).Value);
            }
            else
            {
                ins.HolderBirthDate = null;
            }

            List<Field> repList = in1.Fields(19).Repetitions();
            if (repList != null)
            {
                ins.HolderStreetAddress = repList[0].Components(1).Value;
                ins.HolderCity = repList[0].Components(3).Value;
                ins.HolderState = repList[0].Components(4).Value;
                ins.HolderZip = repList[0].Components(5).Value;
            }
            ins.Employer = in1.Fields(11).Value; // or in2.Fields(3).Component(2).Value

            ins.EmployerCityState = $"{in1.Fields(44).Components(3).Value}, {in1.Fields(44).Components(4).Value} {in1.Fields(44).Components(5).Value}";

            ins.PolicyNumber = in1.Fields(49).Value;

            if (in2 != null)
            {
                ins.CertSSN = in2.Fields(2).Value;

                if (string.IsNullOrEmpty(ins.Relation))
                {
                    ins.Relation = string.IsNullOrEmpty(in2.Fields(72).Value)
                        ? in2.Fields(72).Value
                        : unitOfWork.MappingRepository.GetMappedValue("GUAR_REL", "CERNER", in2.Fields(72).Value);
                }
            }

            accountRecord.Insurances.Add(ins);
        }
    }

    private void ParseFT1()
    {
        var segFT1 = hl7Message.Segments("FT1");

        foreach (var seg in segFT1)
        {
            ChargeTransaction transaction = new()
            {
                //string transactionId = hl7Message.GetValue("FT1.2");
                //string transactionBatchId = hl7Message.GetValue("FT1.3");

                ServiceDate = new DateTime().ParseHL7Date(seg.Fields(4).Value.Left(8)),
                Cdm = seg.Fields(7).Components(1).Value
            };

            //string cdmDescription = hl7Message.GetValue("FT1.7.2");

            if (seg.Fields(6).Value == "C")
                transaction.Qty = Convert.ToInt16(seg.Fields(10).Value) * -1;
            else
                transaction.Qty = Convert.ToInt16(seg.Fields(10).Value);

            //ordering doctor - repeating FT1.21.1

            transaction.RefNumber = seg.Fields(23).Components(1).Value;

            //string cpt = hl7Message.GetValue("FT1.25.1");
            //string abn = hl7Message.GetValue("FT1.27");

            chargeTransactions.Add(transaction);
        }
        //parse PR1 here, if needed
    }


    private void ParseSTF()
    {
        MFNeventCode = hl7Message.GetValue("MFE.2");

        phy.LastName = hl7Message.GetValue("STF.3.1");
        phy.FirstName = hl7Message.GetValue("STF.3.2");
        phy.MiddleInitial = hl7Message.GetValue("STF.3.3");
        if (phy.MiddleInitial.Length > 1)
            phy.MiddleInitial = phy.MiddleInitial.Left(1);
        phy.Credentials = hl7Message.GetValue("STF.3.4");

        //STF-10 - phone repeating
        if (hl7Message.HasRepetitions("STF.10"))
        {
            List<Field> repList = hl7Message.Segments("STF")[0].Fields(10).Repetitions();

            phy.Phone = repList[0].Components(1).Value;
        }

        phy.Address1 = hl7Message.GetValue("STF.11.1");
        phy.Address2 = hl7Message.GetValue("STF.11.2");
        phy.City = hl7Message.GetValue("STF.11.3");
        phy.State = hl7Message.GetValue("STF.11.4");
        phy.ZipCode = hl7Message.GetValue("STF.11.5");
    }

    private void ParsePRA()
    {
        string drnum = hl7Message.GetValue("PRA.1.1");
        string group = hl7Message.GetValue("PRA.2");
        string speciality = hl7Message.GetValue("PRA.5");

        phy.DoctorNumber = drnum;

        //practioner ids - PRA.6 repeating
        if (hl7Message.HasRepetitions("PRA.6"))
        {
            List<Field> repList = hl7Message.Segments("PRA")[0].Fields(6).Repetitions();
            foreach (var field in repList)
            {
                string code = field.Components(1).Value;
                string codeType = field.Components(2).Value;

                if (codeType == "NPI")
                {
                    phy.NpiId = code;
                    phy.BillingNpi = code;
                }

                if (codeType == "UPIN")
                    phy.Upin = code;
            }
        }
    }

    private string ParseMFE()
    {
        //MFE-1
        string eventCode = hl7Message.GetValue("MFE.1");

        string identifier = hl7Message.GetValue("MFE.4.1");
        string identifierType = hl7Message.GetValue("MFE.4.3");

        return eventCode;
    }


    public MessageInbound SetMessageDoNotProcess(int systemMessageId, string statusMessage)
    {
        UnitOfWorkMain uow = new(appEnvironment);
        var message = uow.MessagesInboundRepository.GetById(systemMessageId);

        message.ProcessFlag = StatusToString(Status.DoNotProcess);
        message.ProcessStatusMsg = statusMessage;

        var result = uow.MessagesInboundRepository.Update(message, new[]
        {
            nameof(MessageInbound.ProcessFlag),
            nameof(MessageInbound.ProcessStatusMsg)
        });

        return result;
    }
}
