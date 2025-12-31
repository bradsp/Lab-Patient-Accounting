# IIS Printer Loading Troubleshooting Guide

## Issue
When running the Blazor application on IIS, the printer selection dropdown does not show any printers.

## Root Causes

### 1. **System Parameters Not Configured**
The most common cause - the printer paths are not configured in the system parameters table.

**Solution:**
1. Access the database directly or through admin interface
2. Add/update these system parameters:

```sql
-- Check current values
SELECT key_name, value 
FROM dbo.system_parameter 
WHERE key_name IN (
    'DefaultClientRequisitionPrinter',
    'DefaultPathologyReqPrinter',
    'DefaultCytologyRequisitionPrinter'
);

-- Insert/Update printer paths
UPDATE dbo.system_parameter 
SET value = '\\PRINT-SERVER\DotMatrixClient'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parameter 
SET value = '\\PRINT-SERVER\DotMatrixPath'
WHERE key_name = 'DefaultPathologyReqPrinter';

UPDATE dbo.system_parameter 
SET value = '\\PRINT-SERVER\DotMatrixCyto'
WHERE key_name = 'DefaultCytologyRequisitionPrinter';
```

### 2. **Application Parameters Not Loading**
The ApplicationParameters object might not be properly initialized.

**Check:**
- Review application startup logs
- Verify database connection is working
- Ensure system parameters are being loaded into ApplicationParameters

### 3. **IIS Application Pool Identity Lacks Permissions**
The IIS app pool identity might not have permissions to access network printers.

**Solution:**
```
1. Open IIS Manager
2. Navigate to Application Pools
3. Find your application pool
4. Click "Advanced Settings"
5. Under "Process Model" ? "Identity"
6. Change to a domain account with network printer access
7. Restart the application pool
```

### 4. **Network Printer Paths Incorrect**
UNC paths might be incorrectly formatted or inaccessible.

**Valid Formats:**
```
? \\SERVER-NAME\PrinterShareName
? \\192.168.1.100\DotMatrix1
? \\PRINT-SRV01\RequisitionPrinter

? LPT1 (local port)
? Z:\Printer (mapped drive)
? http://printer/ (URL)
```

## Enhanced Logging

The updated code now includes comprehensive logging to help diagnose the issue.

### Log Messages to Look For

#### Successful Printer Loading
```
[DEBUG] GetAvailablePrinters() called
[DEBUG] GetNetworkPrintersFromConfig() returned 3 printer(s)
[DEBUG] Checking printer configuration parameters:
[DEBUG]   DefaultClientRequisitionPrinter = '\\SERVER\DotMatrix1'
[DEBUG]   DefaultPathologyReqPrinter = '\\SERVER\DotMatrix2'
[DEBUG]DefaultCytologyRequisitionPrinter = '\\SERVER\DotMatrix3'
[DEBUG] Added DefaultClientRequisitionPrinter: \\SERVER\DotMatrix1
[DEBUG] Added DefaultPathologyReqPrinter: \\SERVER\DotMatrix2
[DEBUG] Added DefaultCytologyRequisitionPrinter: \\SERVER\DotMatrix3
[INFO] Returning 3 network printer(s) from configuration: \\SERVER\DotMatrix1, \\SERVER\DotMatrix2, \\SERVER\DotMatrix3
[INFO] LoadPrinters() received 3 printer(s) from PrintingService
[DEBUG] Available printers: \\SERVER\DotMatrix1, \\SERVER\DotMatrix2, \\SERVER\DotMatrix3
```

#### No Printers Configured
```
[DEBUG] GetAvailablePrinters() called
[DEBUG] Checking printer configuration parameters:
[DEBUG]   DefaultClientRequisitionPrinter = '(null)'
[DEBUG]   DefaultPathologyReqPrinter = '(null)'
[DEBUG]   DefaultCytologyRequisitionPrinter = '(null)'
[WARN] No printer parameters are configured in system parameters. Please configure: DefaultClientRequisitionPrinter, DefaultPathologyReqPrinter, and/or DefaultCytologyRequisitionPrinter
[DEBUG] GetNetworkPrintersFromConfig() returned 0 printer(s)
[WARN] No network printers configured in system parameters, attempting to fall back to local printers
[ERROR] Unable to access local printers (expected on IIS): Access denied
[WARN] No printers returned from PrintingService.GetAvailablePrinters()
```

#### Configuration Error
```
[ERROR] AppEnvironment is null in GetNetworkPrintersFromConfig()
[ERROR] ApplicationParameters is null in GetNetworkPrintersFromConfig()
[ERROR] Error loading network printers from configuration: Object reference not set to an instance of an object
```

## Diagnostic Steps

### Step 1: Check Application Logs

**Location:**
- Windows Event Viewer ? Application logs
- Application log files (if file logging is configured)
- IIS logs in `C:\inetpub\logs\LogFiles`

**Look for:**
- Log messages starting with "GetAvailablePrinters()"
- Any ERROR or WARN messages related to printers
- Stack traces indicating configuration issues

### Step 2: Verify System Parameters

**Run this query:**
```sql
SELECT 
    key_name,
    value,
    CASE 
     WHEN value IS NULL THEN 'NULL'
        WHEN LEN(RTRIM(value)) = 0 THEN 'EMPTY STRING'
 WHEN value NOT LIKE '\\%' THEN 'INVALID FORMAT (not UNC path)'
    ELSE 'OK'
    END AS status
FROM dbo.system_parameter
WHERE key_name IN (
    'DefaultClientRequisitionPrinter',
    'DefaultPathologyReqPrinter',
    'DefaultCytologyRequisitionPrinter'
);
```

**Expected Output:**
```
key_name      | value      | status
------------------------------------|------------------------|-------
DefaultClientRequisitionPrinter     | \\SERVER\DotMatrix1   | OK
DefaultPathologyReqPrinter   | \\SERVER\DotMatrix2   | OK
DefaultCytologyRequisitionPrinter   | \\SERVER\DotMatrix3   | OK
```

### Step 3: Test Network Printer Access

**From IIS Server:**

1. **Test UNC path access:**
   ```cmd
   net use \\PRINT-SERVER\DotMatrix1
   ```

2. **Test from PowerShell:**
   ```powershell
   Test-Path "\\PRINT-SERVER\DotMatrix1"
   ```

3. **Test printer connection:**
   ```powershell
   $printer = "\\PRINT-SERVER\DotMatrix1"
   Get-Printer -Name $printer
   ```

### Step 4: Verify IIS Configuration

**Check Application Pool Identity:**
```powershell
Import-Module WebAdministration
Get-ItemProperty "IIS:\AppPools\YourAppPoolName" -Name processModel
```

**Test identity permissions:**
1. Log in as the app pool identity
2. Try to access `\\PRINT-SERVER\DotMatrix1`
3. Verify "Print" permission is granted

### Step 5: Test in Browser Developer Tools

1. Open browser Developer Tools (F12)
2. Navigate to the Requisition Forms page
3. Open Console tab
4. Look for any JavaScript errors
5. Check Network tab for failed API calls

## Common Error Patterns

### Error: "No printers returned from PrintingService.GetAvailablePrinters()"

**Cause:** System parameters are empty or null

**Fix:**
```sql
-- Verify parameters exist
SELECT COUNT(*) FROM dbo.system_parameter 
WHERE key_name LIKE '%Printer%';

-- If count is 0, parameters need to be inserted
INSERT INTO dbo.system_parameter (key_name, value, category, description)
VALUES 
('DefaultClientRequisitionPrinter', '\\YOUR-PRINT-SERVER\DotMatrix1', 'Environment', 'Default dot-matrix printer for client requisition forms (pin-fed)'),
('DefaultPathologyReqPrinter', '\\YOUR-PRINT-SERVER\DotMatrix2', 'Environment', 'Default dot-matrix printer for pathology requisition forms (pin-fed)'),
('DefaultCytologyRequisitionPrinter', '\\YOUR-PRINT-SERVER\DotMatrix3', 'Environment', 'Default dot-matrix printer for cytology requisition forms (pin-fed)');
```

### Error: "ApplicationParameters is null"

**Cause:** Application startup issue, parameters not loaded

**Fix:**
1. Check application startup code
2. Verify `IAppEnvironment` is properly injected
3. Review dependency injection configuration in `Program.cs` or `Startup.cs`
4. Check for exceptions during application initialization

### Error: "Access denied" when accessing printers

**Cause:** IIS app pool identity lacks permissions

**Fix:**
1. Grant the app pool identity access to printer shares:
   ```
   - Open Print Management on print server
   - Right-click printer ? Printer Properties
   - Security tab ? Add the app pool identity
   - Grant "Print" permission
   ```

2. Or use a domain service account:
   ```
   - IIS Manager ? Application Pools
   - Select pool ? Advanced Settings
   - Identity ? Custom account
   - Enter domain\serviceaccount credentials
   ```

## Testing Checklist

- [ ] System parameters are configured with valid UNC paths
- [ ] Application startup logs show parameters loading successfully
- [ ] IIS app pool identity has network printer access
- [ ] UNC paths are accessible from IIS server
- [ ] Application logs show printers being loaded
- [ ] Browser console shows no JavaScript errors
- [ ] Printer dropdown populates with configured printers

## Quick Test Script

Run this on the IIS server to test configuration:

```powershell
# Test script for printer configuration
$printers = @(
  "\\PRINT-SERVER\DotMatrix1",
    "\\PRINT-SERVER\DotMatrix2",
    "\\PRINT-SERVER\DotMatrix3"
)

Write-Host "Testing printer configuration..." -ForegroundColor Cyan

foreach ($printer in $printers) {
  Write-Host "`nTesting: $printer" -ForegroundColor Yellow
    
    # Test UNC path
    if (Test-Path $printer) {
 Write-Host "  ? UNC path accessible" -ForegroundColor Green
    } else {
        Write-Host "  ? UNC path NOT accessible" -ForegroundColor Red
    }
    
    # Test printer object
    try {
        $p = Get-Printer -Name $printer -ErrorAction Stop
        Write-Host "  ? Printer object found: $($p.Name)" -ForegroundColor Green
        Write-Host "    Status: $($p.PrinterStatus)" -ForegroundColor Gray
        Write-Host "    Driver: $($p.DriverName)" -ForegroundColor Gray
    } catch {
 Write-Host "? Cannot access printer object: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`nTest complete." -ForegroundColor Cyan
```

## Resolution Steps

### Immediate Fix (Add Printers to Configuration)

1. **Connect to database:**
   ```sql
   USE LabBilling; -- or your database name
   ```

2. **Insert printer configurations:**
   ```sql
   -- Replace PRINT-SERVER and printer names with your actual values
   
   IF NOT EXISTS (SELECT 1 FROM system_parameter WHERE key_name = 'DefaultClientRequisitionPrinter')
       INSERT INTO system_parameter (key_name, value, category, description)
       VALUES ('DefaultClientRequisitionPrinter', '\\PRINT-SERVER\ClientReqPrinter', 'Environment', 
               'Default dot-matrix printer for client requisition forms (pin-fed)');
 ELSE
       UPDATE system_parameter 
       SET value = '\\PRINT-SERVER\ClientReqPrinter' 
       WHERE key_name = 'DefaultClientRequisitionPrinter';

   IF NOT EXISTS (SELECT 1 FROM system_parameter WHERE key_name = 'DefaultPathologyReqPrinter')
       INSERT INTO system_parameter (key_name, value, category, description)
VALUES ('DefaultPathologyReqPrinter', '\\PRINT-SERVER\PathReqPrinter', 'Environment', 
       'Default dot-matrix printer for pathology requisition forms (pin-fed)');
 ELSE
       UPDATE system_parameter 
       SET value = '\\PRINT-SERVER\PathReqPrinter' 
       WHERE key_name = 'DefaultPathologyReqPrinter';

   IF NOT EXISTS (SELECT 1 FROM system_parameter WHERE key_name = 'DefaultCytologyRequisitionPrinter')
       INSERT INTO system_parameter (key_name, value, category, description)
       VALUES ('DefaultCytologyRequisitionPrinter', '\\PRINT-SERVER\CytoReqPrinter', 'Environment', 
 'Default dot-matrix printer for cytology requisition forms (pin-fed)');
   ELSE
       UPDATE system_parameter 
       SET value = '\\PRINT-SERVER\CytoReqPrinter' 
       WHERE key_name = 'DefaultCytologyRequisitionPrinter';
   ```

3. **Restart the application:**
   ```cmd
   iisreset /noforce
   ```
   Or restart just the app pool:
   ```powershell
   Restart-WebAppPool -Name "YourAppPoolName"
   ```

4. **Test the application:**
   - Navigate to Requisition Forms page
   - Verify printers appear in dropdown
   - Check application logs for success messages

### Long-term Fix (Proper Configuration Management)

1. **Create printer configuration admin interface**
2. **Implement configuration validation**
3. **Add health checks for printer accessibility**
4. **Set up monitoring/alerts for printer issues**

## Files Modified

### Enhanced Logging Added To:
- ?? `LabBilling Library/Services/RequisitionPrintingService.cs`
  - Added detailed logging in `GetAvailablePrinters()`
  - Added detailed logging in `GetNetworkPrintersFromConfig()`
  - Added null checks and error messages

- ?? `LabOutreachUI/Components/Clients/AddressRequisitionPrint.razor`
  - Added logging in `LoadPrinters()` method
  - Added null check for PrintingService
  - Added printer list logging

## Next Steps After Resolution

Once printers are loading correctly:

1. **Document the configuration:**
   - Record which printers are configured for which form types
   - Document the UNC paths used
   - Note any special printer settings or requirements

2. **Test printing:**
   - Select each form type
   - Verify correct printer auto-selection
   - Test actual print jobs
 - Check alignment on physical forms

3. **Monitor logs:**
   - Watch for any permission issues
   - Check for print job failures
   - Monitor printer availability

4. **Plan for high availability:**
   - Consider printer failover configuration
   - Document printer maintenance procedures
   - Set up monitoring/alerting

---

**Status**: Enhanced logging implemented
**Build**: ? Successful  
**Next Action**: Deploy to IIS and check logs to identify specific issue
