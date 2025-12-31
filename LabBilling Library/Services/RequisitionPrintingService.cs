using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Gets available printers from the system.
    /// When running on IIS server, returns network printers from system parameters
    /// instead of locally installed printers.
    /// </summary>
    public List<string> GetAvailablePrinters()
    {
        try
        {
            Log.Instance.Debug("GetAvailablePrinters() called");

            // First, try to get network printers from system parameters
            var networkPrinters = GetNetworkPrintersFromConfig();

            Log.Instance.Debug($"GetNetworkPrintersFromConfig() returned {networkPrinters.Count} printer(s)");

            if (networkPrinters.Count > 0)
            {
                Log.Instance.Info($"Returning {networkPrinters.Count} network printer(s) from configuration: {string.Join(", ", networkPrinters)}");
                return networkPrinters;
            }

            // Fallback to locally installed printers (for development/local scenarios)
            Log.Instance.Warn("No network printers configured in system parameters, attempting to fall back to local printers");

            try
            {
                var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                var printerList = printers.Cast<string>().OrderBy(p => p).ToList();
                Log.Instance.Debug($"Found {printerList.Count} local printer(s): {string.Join(", ", printerList)}");
                return printerList;
            }
            catch (Exception localEx)
            {
                Log.Instance.Error($"Unable to access local printers (expected on IIS): {localEx.Message}");
                return new List<string>();
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error in GetAvailablePrinters(): {ex.Message}", ex);
            return new List<string>();
        }
    }

    /// <summary>
    /// Gets network printer paths from system parameters.
    /// Looks for parameters: DefaultClientRequisitionPrinter, DefaultPathologyReqPrinter, DefaultCytologyRequisitionPrinter
    /// and any custom printer parameters.
    /// </summary>
    private List<string> GetNetworkPrintersFromConfig()
    {
        var printers = new List<string>();

        try
        {
            // TEMPORARY: Hardcoded printers for production use
            // TODO: Move these to system parameters once they are configured in the database
            printers.Add(@"\\WTH125\MCL_LP");
            printers.Add(@"\\WTH125\MCL_LW");

            Log.Instance.Info($"Using hardcoded printer configuration: {string.Join(", ", printers)}");

            return printers.Distinct().OrderBy(p => p).ToList();

            /* ORIGINAL CODE - Commented out until system parameters are configured
          if (_appEnvironment == null)
     {
         Log.Instance.Error("AppEnvironment is null in GetNetworkPrintersFromConfig()");
        return printers;
   }

              var appParams = _appEnvironment.ApplicationParameters;

   if (appParams == null)
              {
       Log.Instance.Error("ApplicationParameters is null in GetNetworkPrintersFromConfig()");
               return printers;
  }

              Log.Instance.Debug($"Checking printer configuration parameters:");
          Log.Instance.Debug($"  DefaultClientRequisitionPrinter = '{appParams.DefaultClientRequisitionPrinter ?? "(null)"}'");
         Log.Instance.Debug($"  DefaultPathologyReqPrinter = '{appParams.DefaultPathologyReqPrinter ?? "(null)"}'");
              Log.Instance.Debug($"  DefaultCytologyRequisitionPrinter = '{appParams.DefaultCytologyRequisitionPrinter ?? "(null)"}'");

         // Add standard requisition printers if configured
      if (!string.IsNullOrWhiteSpace(appParams.DefaultClientRequisitionPrinter))
           {
      printers.Add(appParams.DefaultClientRequisitionPrinter);
        Log.Instance.Debug($"Added DefaultClientRequisitionPrinter: {appParams.DefaultClientRequisitionPrinter}");
  }

     if (!string.IsNullOrWhiteSpace(appParams.DefaultPathologyReqPrinter))
   {
                  printers.Add(appParams.DefaultPathologyReqPrinter);
    Log.Instance.Debug($"Added DefaultPathologyReqPrinter: {appParams.DefaultPathologyReqPrinter}");
              }

     if (!string.IsNullOrWhiteSpace(appParams.DefaultCytologyRequisitionPrinter))
    {
      printers.Add(appParams.DefaultCytologyRequisitionPrinter);
        Log.Instance.Debug($"Added DefaultCytologyRequisitionPrinter: {appParams.DefaultCytologyRequisitionPrinter}");
        }

        // Remove duplicates and sort
            var distinctPrinters = printers.Distinct().OrderBy(p => p).ToList();

              if (distinctPrinters.Count == 0)
     {
      Log.Instance.Warn("No printer parameters are configured in system parameters. Please configure: DefaultClientRequisitionPrinter, DefaultPathologyReqPrinter, and/or DefaultCytologyRequisitionPrinter");
     }

      return distinctPrinters;
       */
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error loading network printers from configuration: {ex.Message}", ex);

            // Fallback to hardcoded printers even on error
            if (printers.Count == 0)
            {
                printers.Add(@"\\WTH125\MCL_LP");
                printers.Add(@"\\WTH125\MCL_LW");
                Log.Instance.Warn("Exception occurred, using hardcoded printer fallback");
            }

            return printers;
        }
    }

    /// <summary>
    /// Gets the default printer.
    /// When running on IIS, returns the first configured network printer.
    /// </summary>
    public string GetDefaultPrinter()
    {
        try
        {
            // TEMPORARY: Return hardcoded default printer
            // TODO: Update to use system parameters once configured
            return @"\\WTH125\MCL_LP";

            /* ORIGINAL CODE - Commented out until system parameters are configured
                   // Try to get default from application parameters first
           var appParams = _appEnvironment.ApplicationParameters;

        if (!string.IsNullOrEmpty(appParams.DefaultClientRequisitionPrinter))
              return appParams.DefaultClientRequisitionPrinter;

             // Fallback to system default printer (for local scenarios)
       var printerSettings = new System.Drawing.Printing.PrinterSettings();
                   return printerSettings.PrinterName;
             */
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error getting default printer: {ex.Message}", ex);
            return @"\\WTH125\MCL_LP"; // Fallback to hardcoded printer
        }
    }

    /// <summary>
    /// Gets printer for specific form type based on configuration.
    /// </summary>
    public string GetPrinterForFormType(FormType formType)
    {
        // TEMPORARY: Return hardcoded printer based on form type
        // TODO: Update to use system parameters once configured

        // Use MCL_LP (portrait) for most forms, MCL_LW (landscape/wide) could be used for specific forms if needed
        return formType switch
        {
            FormType.CLIREQ => @"\\WTH125\MCL_LP",
            FormType.PTHREQ => @"\\WTH125\MCL_LP",
            FormType.CYTREQ => @"\\WTH125\MCL_LP",
            _ => @"\\WTH125\MCL_LP"
        };

        /* ORIGINAL CODE - Commented out until system parameters are configured
               var appParams = _appEnvironment.ApplicationParameters;

         return formType switch
               {
            FormType.CLIREQ => appParams.DefaultClientRequisitionPrinter ?? GetDefaultPrinter(),
           FormType.PTHREQ => appParams.DefaultPathologyReqPrinter ?? GetDefaultPrinter(),
             FormType.CYTREQ => appParams.DefaultCytologyRequisitionPrinter ?? GetDefaultPrinter(),
          _ => GetDefaultPrinter()
               };
               */
    }

    /// <summary>
    /// Validates that a printer is accessible from the server.
    /// For network printers, verifies the UNC path is reachable.
    /// </summary>
    public (bool isValid, string message) ValidatePrinterAccess(string printerName)
    {
        if (string.IsNullOrEmpty(printerName))
            return (false, "Printer name is empty");

        try
        {
            // Check if it's a network printer (UNC path)
            if (printerName.StartsWith(@"\\"))
            {
                Log.Instance.Debug($"Validating network printer access: {printerName}");

                // Try to open the printer to verify access
                var testData = new byte[] { 0x1B, 0x45 }; // PCL reset command
                bool canAccess = RawPrinterHelper.SendBytesToPrinter(printerName, testData, "Access Test");

                if (!canAccess)
                {
                    string error = RawPrinterHelper.GetLastErrorMessage();
                    return (false, $"Cannot access network printer: {error}");
                }

                return (true, "Printer is accessible");
            }

            // Local printer validation
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            bool exists = printers.Cast<string>().Any(p => p.Equals(printerName, StringComparison.OrdinalIgnoreCase));

            return exists
    ? (true, "Printer found")
          : (false, "Printer not found on server");
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error validating printer access: {ex.Message}", ex);
            return (false, $"Validation error: {ex.Message}");
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
    /// Prints requisition forms directly to a dot-matrix printer using plain text mode.
    /// Optimized for 3-ply pin-fed forms using simple text positioning with spaces.
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

            // Generate plain text formatted data
            string textData = _dotMatrixService.FormatRequisition(client, formType.ToString());

            // Convert to bytes for raw printing
            byte[] textBytes = Encoding.ASCII.GetBytes(textData);

            // Send to printer multiple times based on copies parameter
            string documentName = $"{formType} - {client.ClientMnem}";
            int successfulCopies = 0;

            for (int i = 0; i < copies; i++)
            {
                bool printSuccess = RawPrinterHelper.SendBytesToPrinter(printerName, textBytes, documentName);

                if (!printSuccess)
                {
                    string errorMsg = RawPrinterHelper.GetLastErrorMessage();
                    Log.Instance.Error($"Failed to print copy {i + 1} of {copies} to {printerName}: {errorMsg}");

                    // If first copy fails, return immediately
                    if (i == 0)
                    {
                        return (false, $"Print failed: {errorMsg}");
                    }

                    // If subsequent copy fails, report partial success
                    return (false, $"Printed {successfulCopies} of {copies} copies. Last error: {errorMsg}");
                }

                successfulCopies++;
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

    /// <summary>
    /// Prints requisition to file emulator for development/testing without physical printer.
    /// Creates plain text file, text preview, and visual layout.
    /// </summary>
    /// <param name="clientMnemonic">Client mnemonic code</param>
    /// <param name="formType">Type of form to print</param>
    /// <param name="copies">Number of copies (for logging purposes)</param>
    /// <param name="userName">User requesting the print job</param>
    /// <returns>Success status, message, and file path</returns>
    public async Task<(bool success, string message, string filePath)> PrintToEmulatorAsync(
        string clientMnemonic,
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
                return (false, $"Validation failed: {errorMsg}", null);
            }

            // Get client data
            var client = uow.ClientRepository.GetClient(clientMnemonic);
            if (client == null)
            {
                return (false, "Client not found", null);
            }

            // Generate plain text formatted data
            string textData = _dotMatrixService.FormatRequisition(client, formType.ToString());

            // Write to emulator - create one file per copy
            var emulator = new PCL5FileEmulatorService();
            var createdFiles = new List<string>();

            for (int i = 0; i < copies; i++)
            {
                string fileName = $"{formType}_{client.ClientMnem}_{DateTime.Now:yyyyMMdd_HHmmss}_copy{i + 1}.txt";
                string filePath = emulator.WritePCL5ToFile(textData, fileName);
                createdFiles.Add(fileName);
            }

            // Record print job (mark as emulator)
            await RecordPrintJobAsync(
         client.ClientMnem,
          client.Name,
          formType,
         copies,
           "DOTMATRIX_EMULATOR",
             userName,
              "EmulatorMode");

            string fileList = string.Join("\n  � ", createdFiles);
            string message = $"Dot-matrix emulation successful!\n\n" +
             $"Created {copies} cop{(copies == 1 ? "y" : "ies")} in:\n{emulator.GetOutputDirectory()}\n\n" +
               $"Files created:\n  � {fileList}\n\n" +
              $"Each file has corresponding .txt preview and _layout.txt files.\n" +
             $"Open any _layout.txt file to verify column positioning (should be at column 55).";

            Log.Instance.Info($"Emulated print for {client.ClientMnem} - {copies} copies created");
            return (true, message, createdFiles.FirstOrDefault());
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error in emulator print: {ex.Message}", ex);
            return (false, $"Emulator error: {ex.Message}", null);
        }
    }

    /// <summary>
    /// Prints alignment test to emulator for development/testing.
    /// </summary>
    /// <returns>Success status, message, and file path</returns>
    public (bool success, string message, string filePath) PrintAlignmentTestToEmulator()
    {
        try
        {
            string testPattern = _dotMatrixService.GenerateAlignmentTestPattern();

            var emulator = new PCL5FileEmulatorService();
            string fileName = $"AlignmentTest_{DateTime.Now:yyyyMMdd_HHmmss}.pcl";
            string filePath = emulator.WritePCL5ToFile(testPattern, fileName);

            string message = $"Alignment test pattern created!\n\n" +
            $"Files created in:\n{emulator.GetOutputDirectory()}\n\n" +
                       $"Check the _layout.txt file to verify the grid appears at the correct position.";

            Log.Instance.Info($"Emulated alignment test created: {filePath}");
            return (true, message, filePath);
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error creating emulated alignment test: {ex.Message}", ex);
            return (false, $"Error: {ex.Message}", null);
        }
    }
}
