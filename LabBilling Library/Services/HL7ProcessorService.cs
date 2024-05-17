using j4jayant.HL7.Parser;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using NPOI.SS.UserModel;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utilities;

namespace LabBilling.Core.Services;

public sealed class HL7ProcessorService
{
    j4jayant.HL7.Parser.Message _hl7Message;
    private MessageInbound _currentMessage;
    private Account _accountRecord = new();
    private Phy _phy = new();
    private string _mFNeventCode;
    private readonly List<MessageInbound> _messagesInbound = new();
    private readonly AccountService _accountService;
    private readonly DictionaryService _dictionaryService;

    private List<ChargeTransaction> _chargeTransactions = new();
    private readonly IAppEnvironment _appEnvironment;

    private const string _accountPrefix = "L";

    public HL7ProcessorService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
        _accountService = new(appEnvironment);
        _dictionaryService = new(appEnvironment);
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

    public List<MessageQueueCount> GetQueueCounts()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        return uow.MessagesInboundRepository.GetQueueCounts();
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
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.MessagesInboundRepository.GetByDateRange(fromDate, thruDate);
    }

    public void ProcessMessage(int systemMessageId)
    {
        Log.Instance.Trace($"Entering");
        using UnitOfWorkMain unitOfWork = new(_appEnvironment);

        _currentMessage = unitOfWork.MessagesInboundRepository.GetById(systemMessageId);
        if (_currentMessage != null)
            ProcessMessage();
    }

    private void ProcessMessage()
    {
        Log.Instance.Debug($"Processing {_currentMessage.MessageType} for account {_currentMessage.SourceAccount}");
        Console.WriteLine($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffffffK} - Processing {_currentMessage.MessageType} for account {_currentMessage.SourceAccount}");
        UnitOfWorkMain uow = new(_appEnvironment, true);
        try
        {
            var (status, statusText, errors) = ParseHL7(_currentMessage.HL7Message);

            _currentMessage.ProcessFlag = StatusToString(status);
            _currentMessage.ProcessStatusMsg = statusText;
            _currentMessage.Errors = errors.ToString();

            uow.MessagesInboundRepository.Update(_currentMessage, new[]
            {
                nameof(MessageInbound.ProcessFlag),
                nameof(MessageInbound.ProcessStatusMsg),
                nameof(MessageInbound.Errors)
            });
            Console.WriteLine($"Processing {_currentMessage.MessageType} for account {_currentMessage.SourceAccount} complete.");
            uow.Commit();
        }
        catch (AccountLockException alex)
        {
            _currentMessage.ProcessFlag = StatusToString(Status.NotProcessed);
            _currentMessage.ProcessStatusMsg = "Account locked. Requeing.";
            _currentMessage.Errors = alex.Message + "\n" + alex.StackTrace;
            Log.Instance.Error(_currentMessage.ProcessStatusMsg, alex.Message);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffffffK} - Account locked. Requeing");
            uow.MessagesInboundRepository.Update(_currentMessage, new[]
{
                nameof(MessageInbound.ProcessFlag),
                nameof(MessageInbound.ProcessStatusMsg),
                nameof(MessageInbound.Errors)
            });
            uow.Commit();
        }
        catch (Exception ex)
        {
            _currentMessage.ProcessFlag = StatusToString(Status.Failed);
            _currentMessage.ProcessStatusMsg = "Exception encountered during process.";
            _currentMessage.Errors = ex.Message + "\n" + ex.StackTrace;
            Log.Instance.Error(_currentMessage.ProcessStatusMsg, ex.Message);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffffffK} - Exception encountered during process.");
            uow.MessagesInboundRepository.Update(_currentMessage, new[]
            {
                nameof(MessageInbound.ProcessFlag),
                nameof(MessageInbound.ProcessStatusMsg),
                nameof(MessageInbound.Errors)
            });
            uow.Commit();
        }
    }

    public void ProcessMessages()
    {
        Log.Instance.Trace($"Entering - Querying messages to process");
        UnitOfWorkMain uow = new(_appEnvironment);
        var msgsToProcess = uow.MessagesInboundRepository.GetUnprocessedMessages();

        foreach (var msg in msgsToProcess)
        {
            _currentMessage = msg;

            if (!_dictionaryService.GetMappingsSendingSystemList().Contains(_currentMessage.SourceInfce))
            {
                _currentMessage.ProcessFlag = StatusToString(Status.Failed);
                _currentMessage.ProcessStatusMsg = $"Interface {_currentMessage.SourceInfce} not defined";

                uow.MessagesInboundRepository.Update(_currentMessage, new[]
                {
                    nameof(MessageInbound.ProcessFlag),
                    nameof(MessageInbound.ProcessStatusMsg),
                    nameof(MessageInbound.Errors)
                });
                Console.WriteLine($"Processing {_currentMessage.MessageType} for account {_currentMessage.SourceAccount} complete.");
                continue;
            }

            ProcessMessage();
        }
    }

    private (Status status, string statusText, StringBuilder errors) ParseHL7(string message)
    {
        Log.Instance.Debug($"Parsing message: {message}");
        _hl7Message = new j4jayant.HL7.Parser.Message(message);
        StringBuilder errors = new();
        Status processStatus;

        bool isParsed = false;
        _accountRecord = new Account();

        //check to see if interface is valid        

        try
        {
            isParsed = _hl7Message.ParseMessage();
        }
        catch (Exception)
        {

        }

        if (isParsed)
        {
            string statusText;
            //process ADT message
            if (_hl7Message.MessageStructure == "ADT_A04" || _hl7Message.MessageStructure == "ADT_A08")
            {
                Log.Instance.Trace($"Message type: {_hl7Message.MessageStructure} - Control ID: {_hl7Message.MessageControlID}");
                var result = ProcessADTMessage();
                errors = result.errors;
                statusText = result.statusText;
                processStatus = result.status;
            }
            else if (_hl7Message.MessageStructure == "ADT_A03")
            {
                Log.Instance.Trace($"Message type: {_hl7Message.MessageStructure} - Control ID: {_hl7Message.MessageControlID}");
                statusText = "Skipping A03";
                processStatus = Status.DoNotProcess;
            }
            //process Charge message
            else if (_hl7Message.MessageStructure == "DFT_P03")
            {
                Log.Instance.Trace($"Message type: {_hl7Message.MessageStructure} - Control ID: {_hl7Message.MessageControlID}");
                var result = ProcessDFTMessage();
                errors = result.errors;
                statusText = result.statusText;
                processStatus = result.status;
            }
            //process MFN message
            else if (_hl7Message.MessageStructure == "MFN_M02")
            {
                Log.Instance.Trace($"Message type: {_hl7Message.MessageStructure} - Control ID: {_hl7Message.MessageControlID}");
                var result = ProcessMFNMessage();
                errors = result.errors;
                statusText = result.statusText;
                processStatus = result.status;

            }
            else
            {
                Log.Instance.Trace($"Message type: {_hl7Message.MessageStructure} - Control ID: {_hl7Message.MessageControlID}");
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
        bool newAccount;
        StringBuilder errors = new();

        string[] invalidFacilities = new string[] { "001", "005", "007", "008", "009", "010", "080", "800", "850", "900" };

        string facility = _hl7Message.GetValue("MSH.4");

        string statusText;
        if (invalidFacilities.Contains(facility))
        {
            //invalid facility - do not file
            Log.Instance.Debug($"Invalid facility {facility}");
            statusText = "Not a valid facility for Lab outreach";
            return (Status.DoNotProcess, statusText, errors);
        }
        string existingFin = string.Empty;
        string existingFinClass = string.Empty;
        string existingClient = string.Empty;
        _accountRecord = _accountService.GetAccount(_accountPrefix + _currentMessage.SourceAccount);
        if (_accountRecord == null)
        {
            _accountRecord = new();
            newAccount = true;
        }
        else
        {
            newAccount = false;
            existingClient = _accountRecord.ClientMnem;
            existingFin = _accountRecord.FinCode;
            existingFinClass = _accountRecord.Fin.FinClass;
        }

        ParsePID();
        ParsePV1();
        ParseDG1();
        ParseGT1();
        ParseIN1IN2();

        //perform error checking
        _accountRecord.Client = _dictionaryService.GetClient(_accountRecord.ClientMnem);
        if (_accountRecord.Client == null)
        {
            //error - invalid client
            Log.Instance.Error($"[ERROR] Invalid client {_accountRecord.ClientMnem}");
            errors.AppendLine($"[ERROR] Invalid client {_accountRecord.ClientMnem}");
            canFile = false;
        }

        foreach (var ins in _accountRecord.Insurances)
        {
            var insc = _dictionaryService.GetInsCompany(ins.InsCode);
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
        if (_accountRecord.Client != null)
        {
            switch (_accountRecord.Client.BillMethod)
            {
                case "INVOICE":
                    _accountRecord.FinCode = _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode;
                    break;
                case "PATIENT":
                    if (_accountRecord.Insurances.Count > 0)
                    {
                        if (_accountRecord.FinCode != _accountRecord.InsurancePrimary.FinCode)
                            _accountRecord.FinCode = _accountRecord.InsurancePrimary.FinCode;
                    }
                    if (_accountRecord.FinCode == _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
                        _accountRecord.FinCode = _appEnvironment.ApplicationParameters.InvalidFinancialCode;
                    break;
                case "PER ACCOUNT":
                    if (_accountRecord.FinCode != _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
                    {
                        if (_accountRecord.Insurances.Count > 0)
                        {
                            if (_accountRecord.FinCode != _accountRecord.InsurancePrimary.FinCode)
                                _accountRecord.FinCode = _accountRecord.InsurancePrimary.FinCode;
                        }
                    }
                    break;
                default:
                    if (_accountRecord.FinCode != _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
                    {
                        if (_accountRecord.Insurances.Count > 0)
                        {
                            if (_accountRecord.FinCode != _accountRecord.InsurancePrimary.FinCode)
                                _accountRecord.FinCode = _accountRecord.InsurancePrimary.FinCode;
                        }
                    }
                    break;
            }
        }

        if (string.IsNullOrEmpty(_accountRecord.FinCode))
        {
            Log.Instance.Error($"[ERROR] No fin code");
            _accountRecord.FinCode = _appEnvironment.ApplicationParameters.InvalidFinancialCode;
        }

        foreach (var dx in _accountRecord.Pat.Diagnoses)
        {
            var dxRecord = _dictionaryService.GetDiagnosis(dx.Code, (DateTime)_accountRecord.TransactionDate);

            if (dxRecord == null)
            {
                Log.Instance.Warn($"[WARN] Diagnosis {dx.Code} not found.");
                errors.AppendLine($"[WARN] Diagnosis {dx.Code} not found.");
            }
        }

        try
        {
            if (!newAccount)
            {
                //compare and update existingAccount for update -
                //need to decide when to update and when to not update - maybe update until past initial hold period

                //if fin_code or client are different - determine if charges need to be reprocessed (after the update)
                bool finCodeChange = false;
                bool clientChange = false;

                _accountRecord.Fin = _dictionaryService.GetFinCode(_accountRecord.FinCode);

                if (existingFinClass != _accountRecord.Fin.FinClass && !string.IsNullOrEmpty(existingFinClass))
                {
                    finCodeChange = true;
                }
                if (_accountRecord.Fin.FinClass == _appEnvironment.ApplicationParameters.ClientFinancialTypeCode 
                    && existingClient != _accountRecord.ClientMnem && !string.IsNullOrEmpty(existingClient))
                {
                    clientChange = true;
                }

                //loop through all properties and determine which ones are different

                if (canFile)
                {
                    //copy new account info to existing account info

                    // add account
                    var addedRecord = _accountService.UpdateAccountDemographics(_accountRecord);

                    foreach (var ins in _accountRecord.Insurances)
                    {
                        _accountService.SaveInsurance(ins);
                    }

                    statusText = $"{_accountRecord.AccountNo} updated.";

                    if (finCodeChange)
                    {
                        string newFin = _accountRecord.FinCode;
                        _accountRecord.FinCode = existingFin;
                        _accountService.ChangeFinancialClass(_accountRecord, newFin);
                    }

                    if (clientChange)
                    {
                        string newClient = _accountRecord.ClientMnem;
                        _accountRecord.ClientMnem = existingClient;
                        _accountService.ChangeClient(_accountRecord, newClient);
                    }
                    _accountService.ClearAccountLock(_accountRecord);
                    return (Status.Processed, statusText, errors);
                }
                else
                {
                    _accountService.ClearAccountLock(_accountRecord);
                    return (Status.Failed, "Required information missing. See errors.", errors);
                }
            }
            else
            {
                if (canFile)
                {
                    // add account
                    _accountService.Add(_accountRecord);
                    statusText = $"{_accountRecord.AccountNo} added.";
                    _accountService.ClearAccountLock(_accountRecord);
                    return (Status.Processed, statusText, errors);
                }
                else
                {
                    _accountService.ClearAccountLock(_accountRecord);
                    return (Status.Failed, "Required information missing. See errors.", errors);
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.Message.Contains("truncated"))
                {
                    errors.Append("Data is longer than a database field. Record not written.");
                    Log.Instance.Error(ex.InnerException, "Data is longer than a database field. Record not written.");
                }
            }
            else
            {
                errors.Append(ex.Message);
                if (ex.InnerException != null)
                {
                    errors.Append(ex.Message);
                }
            }
            _accountService.ClearAccountLock(_accountRecord);
            return (Status.Failed, "Database error", errors);
        }
        finally
        {
            _accountService.ClearAccountLock(_accountRecord);
        }
    }

    private (Status status, string statusText, StringBuilder errors) ProcessDFTMessage()
    {
        Log.Instance.Trace("Entering");
        bool canFile = true;
        StringBuilder errors = new();
        _chargeTransactions = new List<ChargeTransaction>();
        UnitOfWorkMain uow = new(_appEnvironment, true);

        //make sure account exists - if not, create it from the PID segment
        _accountRecord = _accountService.GetAccount(_accountPrefix + _currentMessage.SourceAccount);
        bool accountExists = _accountRecord != null;

        if (!accountExists)
        {
            _accountRecord = new();
        }

        ParsePID();
        ParsePV1();
        ParseFT1();

        if (!accountExists)
        {
            _accountRecord.Client = uow.ClientRepository.GetClient(_accountRecord.ClientMnem);
            if (_accountRecord.Client == null)
            {
                //error - invalid client
                Log.Instance.Error($"[ERROR] Invalid client {_accountRecord.ClientMnem}");
                errors.AppendLine($"[ERROR] Invalid client {_accountRecord.ClientMnem}");
                canFile = false;
            }

            if (_accountRecord.Client != null)
            {
                switch (_accountRecord.Client.BillMethod)
                {
                    case "INVOICE":
                        _accountRecord.FinCode = _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode;
                        break;
                    case "PATIENT":
                        break;
                    case "PER ACCOUNT":
                        _accountRecord.FinCode = _accountRecord.FinCode != _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode 
                            ? _appEnvironment.ApplicationParameters.InvalidFinancialCode : _accountRecord.FinCode;
                        break;
                    default:
                        _accountRecord.FinCode = _appEnvironment.ApplicationParameters.InvalidFinancialCode;
                        break;
                }
            }
            if (string.IsNullOrEmpty(_accountRecord.FinCode))
            {
                Log.Instance.Error($"[ERROR] No fin code");
                _accountRecord.FinCode = _appEnvironment.ApplicationParameters.InvalidFinancialCode;
            }

            if (canFile)
            {
                _accountRecord = _accountService.Add(_accountRecord);
            }
        }

        if (canFile)
        {
            foreach (var transaction in _chargeTransactions)
            {
                //if the account has no previous charges, make sure the account transaction date matches the charge service date
                if (_accountRecord.Charges.Count == 0)
                {
                    if (_accountRecord.TransactionDate != transaction.ServiceDate)
                    {
                        Log.Instance.Debug($"Account {_accountRecord.AccountNo} transaction date {_accountRecord.TransactionDate} does not match charge service date {transaction.ServiceDate}. Account updated.");
                        _accountRecord.TransactionDate = transaction.ServiceDate;

                        uow.AccountRepository.Update(_accountRecord, new string[] { nameof(Account.TransactionDate) });
                    }
                }

                transaction.Account = _accountRecord.AccountNo;
                transaction.Comment = $"MSG ID: {_currentMessage.SystemMsgId}";
                Log.Instance.Debug($"Adding charge {transaction.Account},{transaction.Cdm},{transaction.Qty},{transaction.ServiceDate},{transaction.Comment},{transaction.RefNumber}");
                try
                {
                    if(transaction.Qty < 0)
                    {
                        // look up existing charge to be credited
                        var existingChrg = uow.ChrgRepository.GetChargeByReferenceAndCdm(transaction.RefNumber, transaction.Cdm);

                        if(existingChrg.Count > 0)
                        {
                            var retValue = _accountService.CreditCharge(existingChrg[0].ChrgId, transaction.Comment);
                        }
                        else
                        {
                            _accountService.AddCharge(_accountRecord, transaction.Cdm, transaction.Qty, transaction.ServiceDate, transaction.Comment, transaction.RefNumber);
                        }
                    }
                    else
                    {
                        _accountService.AddCharge(_accountRecord, transaction.Cdm, transaction.Qty, transaction.ServiceDate, transaction.Comment, transaction.RefNumber);
                    }
                }
                catch (ArgumentOutOfRangeException argex)
                {
                    errors.AppendLine($"[ERROR] {argex.ParamName} is not a valid value. CDM {transaction.Cdm}, Client {_accountRecord.ClientMnem}");
                    _accountService.ClearAccountLock(_accountRecord);
                    uow.Commit();
                    return (Status.Failed, $"{_accountRecord.AccountNo} - charges not posted.", errors);
                }
                catch (CdmNotFoundException cdmex)
                {
                    errors.AppendLine($"[WARN] {cdmex.Message} for {transaction.Cdm} on {_accountRecord.AccountNo}. Charge not posted.");
                    _accountService.ClearAccountLock(_accountRecord);
                    uow.Commit();
                    return (Status.Failed, $"{_accountRecord.AccountNo} - charges not posted.", errors);
                }
                catch (InvalidClientException cliex)
                {
                    errors.AppendLine($"[ERROR] {cliex.Message} for {_accountRecord.ClientMnem} on {_accountRecord.AccountNo}. Charge not posted.");
                    _accountService.ClearAccountLock(_accountRecord);
                    Log.Instance.Error(cliex);
                    uow.Commit();
                    return (Status.Failed, $"{_accountRecord.AccountNo} - charges not posted.", errors);
                }
                catch (ApplicationException apex)
                {
                    errors.AppendLine($"[ERROR] {apex.Message} for {_accountRecord.ClientMnem} on {_accountRecord.AccountNo}. Charge not posted.");
                    _accountService.ClearAccountLock(_accountRecord);
                    Log.Instance.Error(apex);
                    uow.Commit();
                    return (Status.Failed, $"{_accountRecord.AccountNo} - charges not posted.", errors);
                }
                catch (Exception ex)
                {
                    errors.AppendLine($"[ERROR] {ex.Message} for {_accountRecord.ClientMnem} on {_accountRecord.AccountNo}. Charge not posted.");
                    Log.Instance.Error(ex, $"[ERROR] {ex.Message} for {_accountRecord.ClientMnem} on {_accountRecord.AccountNo}. Charge not posted.");
                    _accountService.ClearAccountLock(_accountRecord);
                    uow.Commit();
                    return (Status.Failed, $"{_accountRecord.AccountNo} - charges not posted.", errors);
                }
                finally
                {
                    _accountService.ClearAccountLock(_accountRecord);
                }
            }
            if (accountExists)
                _accountService.ClearAccountLock(_accountRecord);
            uow.Commit();
            return (Status.Processed, $"{_accountRecord.AccountNo} - charges posted.", errors);
        }
        else
        {
            if (accountExists)
                _accountService.ClearAccountLock(_accountRecord);
            uow.Commit();
            return (Status.Failed, "Unable to process charges. See errors.", errors);
        }

    }

    private (Status status, string statusText, StringBuilder errors) ProcessMFNMessage()
    {
        Log.Instance.Trace("Entering");
        //bool canFile = true;
        StringBuilder errors = new();
        string statusText = string.Empty;
        UnitOfWorkMain uow = new(_appEnvironment);
        string eventCode = ParseMFE();
        ParseSTF();
        ParsePRA();

        //MUP = update
        //MAD = add
        //MDL = delete
        //MDC = deactivate
        //MAC = reactivate
        //MSU = suspend

        Phy existingPhy = _dictionaryService.GetProvider(_phy.NpiId);

        if (existingPhy == null)
        {
            Log.Instance.Debug("Provider added");
            uow.PhyRepository.Add(_phy);
            uow.Commit();
            return (Status.Processed, $"Provider added.", errors);
        }
        else
        {
            _phy.uri = existingPhy.uri;
            _phy.rowguid = existingPhy.rowguid;

            if (eventCode == "MDL" || eventCode == "MDC")
            {
                _phy.IsDeleted = true;
                Log.Instance.Debug("Provider deleted");
            }
            else
            {
                _phy.IsDeleted = false;
                Log.Instance.Debug("Provider updated");
            }
            uow.PhyRepository.Update(_phy);
            uow.Commit();
            return (Status.Processed, $"Provider updated.", errors);
        }
    }

    private static string ValidateZipCode(string value)
    {
        Regex validateZipRegex = new("^[0-9]{5}(?:-[0-9]{4})?$");

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
        _accountRecord.EMPINumber = _hl7Message.GetValue("PID.2.1");

        if (_hl7Message.HasRepetitions("PID.3"))
        {
            List<Field> repList = _hl7Message.Segments("PID")[0].Fields(5).Repetitions();
            _accountRecord.MRN = repList[0].Components(1).Value;
        }
        else
        {
            _accountRecord.MRN = _hl7Message.GetValue("PID.3.1");
        }

        if (_hl7Message.HasRepetitions("PID.5"))
        {
            List<Field> repList = _hl7Message.Segments("PID")[0].Fields(5).Repetitions();

            _accountRecord.PatLastName = repList[0].Components(1).Value;
            _accountRecord.PatFirstName = repList[0].Components(2).Value;
            _accountRecord.PatMiddleName = repList[0].Components(3).Value;
            _accountRecord.PatNameSuffix = repList[0].Components(4).Value;
        }
        else
        {
            _accountRecord.PatLastName = _hl7Message.GetValue("PID.5.1");
            _accountRecord.PatFirstName = _hl7Message.GetValue("PID.5.2");
            _accountRecord.PatMiddleName = _hl7Message.GetValue("PID.5.3");
            _accountRecord.PatNameSuffix = _hl7Message.GetValue("PID.5.4");
        }

        _accountRecord.BirthDate = new DateTime().ParseHL7Date(_hl7Message.GetValue("PID.7").Left(8));
        _accountRecord.Sex = _hl7Message.GetValue("PID.8");
        if (!Dictionaries.SexSource.ContainsKey(_accountRecord.Sex))
        {
            _accountRecord.Sex = "U";
        }
        if (_hl7Message.HasRepetitions("PID.10"))
        {
            List<Field> repList = _hl7Message.Segments("PID")[0].Fields(10).Repetitions();
            _accountRecord.Pat.Race = repList[0].Components(1).Value;
        }
        else
        {
            _accountRecord.Pat.Race = _hl7Message.GetValue("PID.10.1");
        }
        if (_hl7Message.HasRepetitions("PID.11"))
        {
            List<Field> repList = _hl7Message.Segments("PID")[0].Fields(11).Repetitions();

            _accountRecord.Pat.Address1 = repList[0].Components(1).Value;
            _accountRecord.Pat.Address2 = repList[0].Components(2).Value;
            _accountRecord.Pat.City = repList[0].Components(3).Value;
            _accountRecord.Pat.State = repList[0].Components(4).Value;
            _accountRecord.Pat.ZipCode = ValidateZipCode(repList[0].Components(5).Value);

            _accountRecord.Pat.EmailAddress = repList[1].Components(1).Value;
        }
        else
        {
            _accountRecord.Pat.Address1 = _hl7Message.GetValue("PID.11.1");
            _accountRecord.Pat.Address2 = _hl7Message.GetValue("PID.11.2");
            _accountRecord.Pat.City = _hl7Message.GetValue("PID.11.3");
            _accountRecord.Pat.State = _hl7Message.GetValue("PID.11.4");
            _accountRecord.Pat.ZipCode = ValidateZipCode(_hl7Message.GetValue("PID.11.5"));
        }

        if (_hl7Message.HasRepetitions("PID.13"))
        {
            List<Field> repList = _hl7Message.Segments("PID")[0].Fields(13).Repetitions();
            _accountRecord.Pat.PrimaryPhone = repList[0].Components(1).Value;
        }
        _accountRecord.Pat.MaritalStatus = _hl7Message.GetValue("PID.16");
        if (!Dictionaries.MaritalSource.ContainsKey(_accountRecord.Pat.MaritalStatus))
        {
            _accountRecord.Pat.MaritalStatus = "U";
        }
        _accountRecord.AccountNo = _accountPrefix + _hl7Message.GetValue("PID.18.1");
        _accountRecord.MeditechAccount = _accountRecord.AccountNo;
        _accountRecord.Pat.AccountNo = _accountRecord.AccountNo;
        _accountRecord.SocSecNo = _hl7Message.GetValue("PID.19").Replace("-", string.Empty);

    }

    private void ParsePV1()
    {
        UnitOfWorkMain unitOfWork = new(_appEnvironment);

        _accountRecord.ClientMnem = string.IsNullOrEmpty(_hl7Message.GetValue("PV1.3.1"))
            ? _hl7Message.GetValue("PV1.3.1")
            : unitOfWork.MappingRepository.GetMappedValue("CLIENT", _currentMessage.SourceInfce, _hl7Message.GetValue("PV1.3.1"));

        if (string.IsNullOrEmpty(_accountRecord.ClientMnem))
        {
            _accountRecord.ClientMnem = string.IsNullOrEmpty(_hl7Message.GetValue("PV1.6.1"))
                ? _hl7Message.GetValue("PV1.6.1")
                : unitOfWork.MappingRepository.GetMappedValue("CLIENT", _currentMessage.SourceInfce, _hl7Message.GetValue("PV1.6.1"));
        }

        if (string.IsNullOrEmpty(_accountRecord.ClientMnem))
        {
            _accountRecord.ClientMnem = string.IsNullOrEmpty(_hl7Message.GetValue("PV1.3.4"))
                ? _hl7Message.GetValue("PV1.3.4")
                : unitOfWork.MappingRepository.GetMappedValue("CLIENT", _currentMessage.SourceInfce, _hl7Message.GetValue("PV1.3.4"));
        }

        string msgFin = _hl7Message.GetValue("PV1.20");
        if (msgFin == "\"\"")
            msgFin = string.Empty;
        if (string.IsNullOrEmpty(_accountRecord.FinCode))
        {
            _accountRecord.FinCode = string.IsNullOrEmpty(msgFin)
                ? string.IsNullOrEmpty(_accountRecord.FinCode) ? msgFin : _accountRecord.FinCode
                : unitOfWork.MappingRepository.GetMappedValue("FIN_CODE", _currentMessage.SourceInfce, msgFin);
        }
        _accountRecord.OriginalFinCode = _accountRecord.FinCode;
        _accountRecord.TransactionDate = new DateTime().ParseHL7Date(_hl7Message.GetValue("PV1.44"));
        if (_accountRecord.TransactionDate == DateTime.MinValue)
        {
            //default transaction date to today
            _accountRecord.TransactionDate = DateTime.Today;
        }

        _accountRecord.Pat.ProviderId = string.IsNullOrEmpty(_hl7Message.GetValue("PV1.17.1"))
           ? _hl7Message.GetValue("PV1.17.1")
           : unitOfWork.MappingRepository.GetMappedValue("PHY_ID", _currentMessage.SourceInfce, _hl7Message.GetValue("PV1.17.1"));

        _accountRecord.Pat.Physician = unitOfWork.PhyRepository.GetByNPI(_accountRecord.Pat.ProviderId);

    }

    private void ParseDG1()
    {
        List<Segment> dg1Segments = _hl7Message.Segments("DG1");

        foreach (var dx in dg1Segments)
        {
            PatDiag patDiag = new()
            {
                Code = dx.Fields(3).Value.Replace(".", "")
            };

            _accountRecord.Pat.Diagnoses.Add(patDiag);
        }
    }

    private void ParseGT1()
    {
        UnitOfWorkMain unitOfWork = new(_appEnvironment);
        if (_hl7Message.Segments("GT1").Count > 0)
        {
            _accountRecord.Pat.GuarantorLastName = _hl7Message.GetValue("GT1.3.1");
            _accountRecord.Pat.GuarantorFirstName = _hl7Message.GetValue("GT1.3.2");

            if (_hl7Message.HasRepetitions("GT1.5"))
            {
                List<Field> repList = _hl7Message.Segments("GT1")[0].Fields(5).Repetitions();
                _accountRecord.Pat.GuarantorAddress = repList[0].Components(1).Value;
                _accountRecord.Pat.GuarantorCity = repList[0].Components(3).Value;
                _accountRecord.Pat.GuarantorState = repList[0].Components(4).Value;
                _accountRecord.Pat.GuarantorZipCode = ValidateZipCode(repList[0].Components(5).Value);
                //email address is field 6
            }
            else
            {
                _accountRecord.Pat.GuarantorAddress = _hl7Message.GetValue("GT1.5.1");
                _accountRecord.Pat.GuarantorCity = _hl7Message.GetValue("GT1.5.3");
                _accountRecord.Pat.GuarantorState = _hl7Message.GetValue("GT1.5.4");
                _accountRecord.Pat.GuarantorZipCode = ValidateZipCode(_hl7Message.GetValue("GT1.5.5"));
            }

            if (_hl7Message.HasRepetitions("GT1.6"))
            {
                List<Field> repList = _hl7Message.Segments("GT1")[0].Fields(6).Repetitions();
                _accountRecord.Pat.GuarantorPrimaryPhone = repList[0].Value;
            }
            else
            {
                _accountRecord.Pat.GuarantorPrimaryPhone = _hl7Message.GetValue("GT1.6");
            }
            _accountRecord.Pat.GuarRelationToPatient = _hl7Message.GetValue("GT1.11") != string.Empty
                ? unitOfWork.MappingRepository.GetMappedValue("GUAR_REL", _currentMessage.SourceInfce, _hl7Message.GetValue("GT1.11"))
                : "01";
        }
    }

    private void ParseIN1IN2()
    {
        var segIn1 = _hl7Message.Segments("IN1");
        var segIn2 = _hl7Message.Segments("IN2");

        UnitOfWorkMain unitOfWork = new(_appEnvironment);
        for (int i = 0; i < segIn1.Count; i++)
        {
            Segment in1 = segIn1[i];

            Segment in2 = null;

            if (segIn2.Count >= i + 1)
                in2 = segIn2[i];

            Ins ins;
            InsCoverage insCoverage;
            int existingInsIndex = -1;

            if (in1.Fields(1).Value == "1")
            {
                ins = _accountRecord.Insurances.Find(i => i.Account == _accountRecord.AccountNo && i.Coverage == InsCoverage.Primary);
                insCoverage = InsCoverage.Primary;
            }
            else if (in1.Fields(1).Value == "2")
            {
                ins = _accountRecord.Insurances.Find(i => i.Account == _accountRecord.AccountNo && i.Coverage == InsCoverage.Secondary);
                insCoverage = InsCoverage.Secondary;
            }
            else if (in1.Fields(1).Value == "3")
            {
                ins = _accountRecord.Insurances.Find(i => i.Account == _accountRecord.AccountNo && i.Coverage == InsCoverage.Tertiary);
                insCoverage = InsCoverage.Tertiary;
            }
            else
            {
                //this is not a valid insurance coverage type -- disregard
                return;
            }

            existingInsIndex = _accountRecord.Insurances.IndexOf(ins);
            if (ins == null || ins.rowguid == Guid.Empty)
            {
                ins = new();
                ins.Account = _accountRecord.AccountNo;
                ins.Coverage = insCoverage;
            }

            ins.InsCode = string.IsNullOrEmpty(in1.Fields(2).Components(1).Value)
                ? in1.Fields(2).Components(1).Value
                : unitOfWork.MappingRepository.GetMappedValue("INS_CODE", _currentMessage.SourceInfce, in1.Fields(2).Components(1).Value);
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
                ? "01"
                : unitOfWork.MappingRepository.GetMappedValue("GUAR_REL", _currentMessage.SourceInfce, in1.Fields(17).Value);

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
                        : unitOfWork.MappingRepository.GetMappedValue("GUAR_REL", _currentMessage.SourceInfce, in2.Fields(72).Value);
                }
            }
            if (existingInsIndex < 0)
                _accountRecord.Insurances.Add(ins);
            else
                _accountRecord.Insurances[existingInsIndex] = ins;
        }
    }

    private void ParseFT1()
    {
        var segFT1 = _hl7Message.Segments("FT1");

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

            _chargeTransactions.Add(transaction);
        }
        //parse PR1 here, if needed
    }


    private void ParseSTF()
    {
        _mFNeventCode = _hl7Message.GetValue("MFE.2");

        _phy.LastName = _hl7Message.GetValue("STF.3.1");
        _phy.FirstName = _hl7Message.GetValue("STF.3.2");
        _phy.MiddleInitial = _hl7Message.GetValue("STF.3.3");
        if (_phy.MiddleInitial.Length > 1)
            _phy.MiddleInitial = _phy.MiddleInitial.Left(1);
        _phy.Credentials = _hl7Message.GetValue("STF.3.4");

        //STF-10 - phone repeating
        if (_hl7Message.HasRepetitions("STF.10"))
        {
            List<Field> repList = _hl7Message.Segments("STF")[0].Fields(10).Repetitions();

            _phy.Phone = repList[0].Components(1).Value;
        }

        _phy.Address1 = _hl7Message.GetValue("STF.11.1");
        _phy.Address2 = _hl7Message.GetValue("STF.11.2");
        _phy.City = _hl7Message.GetValue("STF.11.3");
        _phy.State = _hl7Message.GetValue("STF.11.4");
        _phy.ZipCode = _hl7Message.GetValue("STF.11.5");
    }

    private void ParsePRA()
    {
        string drnum = _hl7Message.GetValue("PRA.1.1");
        string group = _hl7Message.GetValue("PRA.2");
        string speciality = _hl7Message.GetValue("PRA.5");

        _phy.DoctorNumber = drnum;

        //practioner ids - PRA.6 repeating
        if (_hl7Message.HasRepetitions("PRA.6"))
        {
            List<Field> repList = _hl7Message.Segments("PRA")[0].Fields(6).Repetitions();
            foreach (var field in repList)
            {
                string code = field.Components(1).Value;
                string codeType = field.Components(2).Value;

                if (codeType == "NPI")
                {
                    _phy.NpiId = code;
                    _phy.BillingNpi = code;
                }

                if (codeType == "UPIN")
                    _phy.Upin = code;
            }
        }
    }

    private string ParseMFE()
    {
        //MFE-1
        string eventCode = _hl7Message.GetValue("MFE.1");

        string identifier = _hl7Message.GetValue("MFE.4.1");
        string identifierType = _hl7Message.GetValue("MFE.4.3");

        return eventCode;
    }


    public MessageInbound SetMessageDoNotProcess(int systemMessageId, string statusMessage)
    {
        UnitOfWorkMain uow = new(_appEnvironment);
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
