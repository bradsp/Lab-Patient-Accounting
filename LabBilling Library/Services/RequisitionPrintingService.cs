using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

/// <summary>
/// Service for handling requisition form printing operations
/// </summary>
public class RequisitionPrintingService
{
    private readonly IAppEnvironment _appEnvironment;

    public RequisitionPrintingService(IAppEnvironment appEnvironment)
    {
      _appEnvironment = appEnvironment;
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
}
