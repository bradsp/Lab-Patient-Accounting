using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

/// <summary>
/// Service for handling requisition form printing operations.
/// Supports both modern browser-based printing and legacy dot-matrix PCL5 printing.
/// </summary>
public class RequisitionPrintingService
{
    private readonly IAppEnvironment _appEnvironment;
    private readonly DotMatrixRequisitionService _dotMatrixService;

    public RequisitionPrintingService(IAppEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;
        _dotMatrixService = new DotMatrixRequisitionService();
    }

    /// <summary>
    /// Form types available for printing
    /// </summary>
    public enum FormType
    {
        CLIREQ,     // Client Requisition Forms
        PTHREQ,     // Path Requisition Forms
        CYTREQ,     // Cytology Requisition Forms
        CUSTODY,    // Chain of Custody Forms
        LABOFFICE,  // Lab Office Forms (TOX LAB)
        EDLAB,      // ED Lab Forms
        EHS// EHS Forms
    }

    /// <summary>
    /// Records a print job in the audit trail
    /// </summary>
    public async Task<bool> RecordPrintJobAsync(
        string clientMnemonic,
        string clientName,
   FormType formType,
        int quantity,
        string printerName,
        string userName,
   string applicationName = "LabOutreachUI")
    {
        try
        {
            using var uow = new UnitOfWorkMain(_appEnvironment);
            var repository = uow.GetRepository<RequisitionPrintTrackRepository>(true);

            var printTrack = new RequisitionPrintTrack
            {
                ClientName = clientName,
                FormPrinted = formType.ToString(),
                QuantityPrinted = quantity,
                PrinterName = printerName,
                ModUser = userName,
                ModApp = applicationName
            };

            await repository.AddAsync(printTrack);
            uow.Commit();

            Log.Instance.Info($"Print job recorded: {clientName} - {formType} - Qty: {quantity}");
            return true;
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error recording print job: {ex.Message}", ex);
            return false;
        }
    }

    /// <summary>
    /// Gets available printers from the system
    /// </summary>
    public List<string> GetAvailablePrinters()
    {
        try
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            return printers.Cast<string>().OrderBy(p => p).ToList();
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error getting printers: {ex.Message}", ex);
            return new List<string>();
        }
    }

    /// <summary>
    /// Gets the default printer
    /// </summary>
    public string GetDefaultPrinter()
    {
        try
        {
            var printerSettings = new System.Drawing.Printing.PrinterSettings();
            return printerSettings.PrinterName;
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error getting default printer: {ex.Message}", ex);
            return string.Empty;
        }
    }

    /// <summary>
    /// Validates client information before printing
    /// </summary>
    public async Task<(bool isValid, List<string> errors)> ValidateClientForPrinting(string clientMnemonic, IUnitOfWork uow = null)
    {
        var errors = new List<string>();
        bool disposeUow = false;

        try
        {
            if (uow == null)
            {
                uow = new UnitOfWorkMain(_appEnvironment);
                disposeUow = true;
            }

            var client = uow.ClientRepository.GetClient(clientMnemonic);

            if (client == null)
            {
                errors.Add("Client not found");
                return (false, errors);
            }

            if (client.IsDeleted)
            {
                errors.Add("Client is inactive");
            }

            if (string.IsNullOrWhiteSpace(client.Name))
            {
                errors.Add("Client name is missing");
            }

            if (string.IsNullOrWhiteSpace(client.StreetAddress1) &&
        string.IsNullOrWhiteSpace(client.City) &&
      string.IsNullOrWhiteSpace(client.State))
            {
                errors.Add("Client address information is incomplete");
            }

            return (errors.Count == 0, errors);
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error validating client: {ex.Message}", ex);
            errors.Add($"Validation error: {ex.Message}");
            return (false, errors);
        }
        finally
        {
            if (disposeUow && uow != null)
            {
                uow.Dispose();
            }
        }
    }

    /// <summary>
    /// Prints requisition forms directly to a dot-matrix printer using PCL5 commands.
    /// Optimized for 3-ply pin-fed forms. Bypasses browser/GDI for precise control.
    /// </summary>
    /// <param name="clientMnemonic">Client mnemonic code</param>
    /// <param name="printerName">Name of dot-matrix printer</param>
    /// <param name="formType">Type of form to print</param>
    /// <param name="copies">Number of copies to print</param>
    /// <param name="userName">User requesting the print job</param>
    /// <returns>Success status and any error messages</returns>
    public async Task<(bool success, string message)> PrintToDotMatrixAsync(
  string clientMnemonic,
        string printerName,
        FormType formType = FormType.CLIREQ,
        int copies = 1,
        string userName = "System")
    {
        try
        {
          // Validate client
     using var uow = new UnitOfWorkMain(_appEnvironment);
            var (isValid, errors) = await ValidateClientForPrinting(clientMnemonic, uow);
      
            if (!isValid)
   {
    string errorMsg = string.Join("; ", errors);
     Log.Instance.Warn($"Client validation failed for {clientMnemonic}: {errorMsg}");
       return (false, $"Validation failed: {errorMsg}");
   }

        // Get client data
            var client = uow.ClientRepository.GetClient(clientMnemonic);
     if (client == null)
          {
 return (false, "Client not found");
            }

    // Generate PCL5 formatted data
    string pcl5Data = _dotMatrixService.FormatRequisition(client, formType.ToString());

            // Convert to bytes for raw printing
  byte[] pcl5Bytes = Encoding.ASCII.GetBytes(pcl5Data);

     // Send directly to printer
     string documentName = $"{formType} - {client.ClientMnem}";
   bool printSuccess = RawPrinterHelper.SendBytesToPrinter(printerName, pcl5Bytes, documentName);

    if (!printSuccess)
            {
     string errorMsg = RawPrinterHelper.GetLastErrorMessage();
           Log.Instance.Error($"Failed to print to {printerName}: {errorMsg}");
     return (false, $"Print failed: {errorMsg}");
}

          // Record print job
   await RecordPrintJobAsync(
     client.ClientMnem,
        client.Name,
    formType,
                copies,
         printerName,
           userName,
              "RequisitionPrintingService");

            Log.Instance.Info($"Successfully printed {copies} cop{(copies == 1 ? "y" : "ies")} of {formType} for {client.ClientMnem} to {printerName}");
            return (true, $"Successfully printed {copies} requisition(s)");
      }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error printing to dot-matrix printer: {ex.Message}", ex);
            return (false, $"Error: {ex.Message}");
    }
    }

    /// <summary>
    /// Prints requisitions for multiple clients (batch printing).
    /// </summary>
    /// <param name="clientMnemonics">Collection of client codes</param>
    /// <param name="printerName">Dot-matrix printer name</param>
    /// <param name="formType">Form type</param>
 /// <param name="copiesPerClient">Copies per client</param>
    /// <param name="userName">User name</param>
    /// <returns>Summary of print results</returns>
    public async Task<(int successCount, int failCount, List<string> errors)> PrintBatchToDotMatrixAsync(
        IEnumerable<string> clientMnemonics,
    string printerName,
        FormType formType = FormType.CLIREQ,
        int copiesPerClient = 1,
        string userName = "System")
    {
        int successCount = 0;
        int failCount = 0;
        List<string> errors = new List<string>();

        foreach (var clientMnem in clientMnemonics)
        {
          var (success, message) = await PrintToDotMatrixAsync(clientMnem, printerName, formType, copiesPerClient, userName);
         
      if (success)
{
      successCount++;
            }
        else
       {
       failCount++;
          errors.Add($"{clientMnem}: {message}");
    }
      }

        Log.Instance.Info($"Batch print completed: {successCount} successful, {failCount} failed");
 return (successCount, failCount, errors);
    }

 /// <summary>
    /// Prints an alignment test pattern to verify pin-fed form positioning.
    /// Useful for initial printer setup and troubleshooting.
    /// </summary>
    /// <param name="printerName">Dot-matrix printer name</param>
    /// <returns>Success status</returns>
    public bool PrintAlignmentTest(string printerName)
    {
        try
{
          string testPattern = _dotMatrixService.GenerateAlignmentTestPattern();
  byte[] testBytes = Encoding.ASCII.GetBytes(testPattern);
    
  bool success = RawPrinterHelper.SendBytesToPrinter(printerName, testBytes, "Alignment Test");
   
          if (success)
        {
             Log.Instance.Info($"Alignment test printed successfully to {printerName}");
       }
       else
            {
                Log.Instance.Error($"Alignment test failed: {RawPrinterHelper.GetLastErrorMessage()}");
 }
            
return success;
        }
        catch (Exception ex)
    {
         Log.Instance.Error($"Error printing alignment test: {ex.Message}", ex);
     return false;
        }
    }
}
