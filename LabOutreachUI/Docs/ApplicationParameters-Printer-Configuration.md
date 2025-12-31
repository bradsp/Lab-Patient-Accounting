# Using ApplicationParameters for Printer Configuration

## Overview
The application is now properly configured to automatically use printer settings from the `ApplicationParameters` object, which loads values from the `system_parms` database table.

## Configured Parameters

The following printer parameters are already defined in `ApplicationParameters-Environment.cs`:

| Parameter Name | Description | Form Type | Database Key |
|----------------|-------------|-----------|--------------|
| `DefaultClientRequisitionPrinter` | Dot-matrix printer for client requisition forms (pin-fed) | CLIREQ | `DefaultClientRequisitionPrinter` |
| `DefaultPathologyReqPrinter` | Dot-matrix printer for pathology requisition forms (pin-fed) | PTHREQ | `DefaultPathologyReqPrinter` |
| `DefaultCytologyRequisitionPrinter` | Dot-matrix printer for cytology requisition forms (pin-fed) | CYTREQ | `DefaultCytologyRequisitionPrinter` |

## How It Works

### 1. Database Configuration
Set the printer UNC paths in the `system_parms` table:

```sql
-- Configure network printers
UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-01\DotMatrixReq'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-02\PathPrinter'
WHERE key_name = 'DefaultPathologyReqPrinter';

UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-03\CytoPrinter'
WHERE key_name = 'DefaultCytologyRequisitionPrinter';

-- Verify configuration
SELECT key_name, parm_value, description
FROM dbo.system_parms
WHERE key_name LIKE '%Requisition%Printer';
```

### 2. Application Startup
On application startup, the `SystemParametersRepository` loads these values:

```csharp
// In SystemParametersRepository.LoadParameters()
var appParams = new ApplicationParameters();
// Loads all parameters from database, including:
appParams.DefaultClientRequisitionPrinter = value from database
appParams.DefaultPathologyReqPrinter = value from database
appParams.DefaultCytologyRequisitionPrinter = value from database
```

### 3. Service Layer Usage
The `RequisitionPrintingService` automatically uses these values:

```csharp
// Gets available printers from configuration
public List<string> GetAvailablePrinters()
{
    var appParams = _appEnvironment.ApplicationParameters;
    
    // Returns printers from parameters
    if (!string.IsNullOrEmpty(appParams.DefaultClientRequisitionPrinter))
        printers.Add(appParams.DefaultClientRequisitionPrinter);
    
    if (!string.IsNullOrEmpty(appParams.DefaultPathologyReqPrinter))
     printers.Add(appParams.DefaultPathologyReqPrinter);
        
    if (!string.IsNullOrEmpty(appParams.DefaultCytologyRequisitionPrinter))
        printers.Add(appParams.DefaultCytologyRequisitionPrinter);
    
    return printers;
}

// Gets form-specific printer
public string GetPrinterForFormType(FormType formType)
{
    return formType switch
    {
  FormType.CLIREQ => appParams.DefaultClientRequisitionPrinter,
        FormType.PTHREQ => appParams.DefaultPathologyReqPrinter,
     FormType.CYTREQ => appParams.DefaultCytologyRequisitionPrinter,
        _ => GetDefaultPrinter()
    };
}
```

### 4. UI Integration
The `AddressRequisitionPrint.razor` component now:

#### **Automatically Selects Printer Based on Form Type**
When user selects a form type, the corresponding printer is automatically selected:

```razor
<select class="form-select" @bind="selectedFormType" @bind:after="OnFormTypeChanged">
```

```csharp
private void OnFormTypeChanged()
{
    if (Enum.TryParse<RequisitionPrintingService.FormType>(selectedFormType, out var formType))
    {
    // Auto-select form-specific printer
        string printerForType = PrintingService.GetPrinterForFormType(formType);
        if (!string.IsNullOrEmpty(printerForType))
        {
   selectedPrinter = printerForType;
        }
    }
}
```

#### **Shows Default Printer for Selected Form**
Displays which printer will be used:

```razor
@if (!string.IsNullOrEmpty(selectedFormType))
{
    <small class="text-muted">
        Default printer: <code>@(PrintingService.GetPrinterForFormType(currentType) ?? "Not configured")</code>
    </small>
}
```

#### **Warns When No Printers Configured**
Shows helpful message if no printers are set up:

```razor
@if (availablePrinters.Count == 0)
{
    <div class="alert alert-warning">
        <strong>No printers configured.</strong> Please configure network printer paths in System Parameters:
        <ul>
       <li><code>DefaultClientRequisitionPrinter</code> - for CLIREQ forms</li>
      <li><code>DefaultPathologyReqPrinter</code> - for PTHREQ forms</li>
            <li><code>DefaultCytologyRequisitionPrinter</code> - for CYTREQ forms</li>
        </ul>
        Use UNC paths like: <code>\\COMPUTER-NAME\PrinterShare</code>
    </div>
}
```

## Configuration Workflow

### Step 1: Share Printer on Client PC
```powershell
# On client PC with dot-matrix printer
# 1. Share the printer: Settings ? Devices ? Printers ? Share this printer
# 2. Set share name (e.g., "DotMatrixReq")
# 3. Note the UNC path: \\CLIENT-PC-NAME\DotMatrixReq
```

### Step 2: Update Database
```sql
-- Update the appropriate parameter based on form type
UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-NAME\DotMatrixReq'
WHERE key_name = 'DefaultClientRequisitionPrinter';  -- For CLIREQ forms
```

### Step 3: Restart Application
```powershell
# Restart IIS app pool to load new parameters
Restart-WebAppPool -Name "LabOutreachUI"

# Or restart the application if running standalone
```

### Step 4: Verify in UI
1. Navigate to Print Requisition Forms
2. Select form type (e.g., CLIREQ)
3. Verify printer dropdown shows configured UNC path
4. Printer should be auto-selected
5. See "Default printer: \\CLIENT-PC\DotMatrixReq" message

## User Experience

### Form Type Selection Flow
1. User selects "CLIREQ" form type
2. Application automatically selects `DefaultClientRequisitionPrinter`
3. User sees confirmation: "Default printer: \\PC-01\DotMatrixReq"
4. User can override by manually selecting different printer
5. Click "Print to Dot-Matrix" to send job

### Example Scenarios

#### Scenario 1: Single Location
```sql
-- All forms use same printer
UPDATE dbo.system_parms
SET parm_value = '\\FRONT-DESK\DotMatrix'
WHERE key_name IN (
    'DefaultClientRequisitionPrinter',
    'DefaultPathologyReqPrinter',
    'DefaultCytologyRequisitionPrinter'
);
```

#### Scenario 2: Multiple Locations
```sql
-- Different printers for different departments
UPDATE dbo.system_parms SET parm_value = '\\LAB-PC\ClientReqPrinter'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parms SET parm_value = '\\PATH-PC\PathReqPrinter'
WHERE key_name = 'DefaultPathologyReqPrinter';

UPDATE dbo.system_parms SET parm_value = '\\CYTO-PC\CytoReqPrinter'
WHERE key_name = 'DefaultCytologyRequisitionPrinter';
```

#### Scenario 3: Print Server
```sql
-- Centralized print server
UPDATE dbo.system_parms SET parm_value = '\\PRINTSERVER\ClientRequisitions'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parms SET parm_value = '\\PRINTSERVER\PathRequisitions'
WHERE key_name = 'DefaultPathologyReqPrinter';

UPDATE dbo.system_parms SET parm_value = '\\PRINTSERVER\CytoRequisitions'
WHERE key_name = 'DefaultCytologyRequisitionPrinter';
```

## Troubleshooting

### Printer Not Appearing in Dropdown

**Check 1: Database Configuration**
```sql
SELECT key_name, parm_value
FROM dbo.system_parms
WHERE key_name LIKE '%Printer%';
```

**Check 2: Application Parameters Loaded**
- Parameters load at application startup
- Restart application after database changes
- Check logs for parameter loading errors

**Check 3: Empty Values**
```sql
-- Make sure values are not NULL or empty
UPDATE dbo.system_parms
SET parm_value = '\\YOUR-PRINTER-PATH'
WHERE key_name = 'DefaultClientRequisitionPrinter'
AND (parm_value IS NULL OR parm_value = '');
```

### Printer Auto-Selection Not Working

**Verify Service Method:**
```csharp
// Test in admin UI or debug
var printer = PrintingService.GetPrinterForFormType(FormType.CLIREQ);
// Should return configured UNC path
```

**Check JavaScript Console:**
- F12 Developer Tools ? Console
- Look for any Blazor binding errors

### Parameters Not Loading

**Check SystemParametersRepository:**
```csharp
// In LoadParameters() method
// Verify ApplicationParameters properties are being set
Log.Instance.Debug($"Loaded printer: {appParams.DefaultClientRequisitionPrinter}");
```

**Verify DataType in Database:**
```sql
SELECT key_name, parm_value, dataType
FROM dbo.system_parms
WHERE key_name LIKE '%Printer%';

-- dataType should be 'String' for printer parameters
```

## Testing

### Test Form-Specific Printer Selection
1. Open Print Requisition Forms
2. Select "CLIREQ" ? Should auto-select `DefaultClientRequisitionPrinter`
3. Select "PTHREQ" ? Should auto-select `DefaultPathologyReqPrinter`
4. Select "CYTREQ" ? Should auto-select `DefaultCytologyRequisitionPrinter`

### Test Network Printer Access
1. Navigate to `/admin/printer-configuration`
2. Click "Test" for each printer
3. Click "Alignment Test" to verify connectivity
4. Check printed output for positioning

### Test End-to-End Print
1. Select form type
2. Verify correct printer auto-selected
3. Click "Print to Dot-Matrix"
4. Verify form prints correctly
5. Check `rpt_track` table for audit record

## Benefits

? **Centralized Configuration** - All printer settings in database  
? **Automatic Selection** - Correct printer auto-selected based on form type  
? **User Friendly** - Shows default printer for each form type  
? **Flexible** - Users can override auto-selection if needed  
? **Network Ready** - Supports UNC paths for remote printers  
? **Audit Trail** - All print jobs tracked in database  
? **IIS Compatible** - Works with server-side deployment  

## Related Documentation

- [Network-Printer-Configuration-IIS.md](./Network-Printer-Configuration-IIS.md) - Complete setup guide
- [DotMatrix-Requisition-Testing-Guide.md](./DotMatrix-Requisition-Testing-Guide.md) - Testing procedures
- [IIS-Network-Printer-Implementation-Summary.md](./IIS-Network-Printer-Implementation-Summary.md) - Technical details

---
**Document Version:** 1.0  
**Last Updated:** {Current Date}  
**Status:** Production Ready
