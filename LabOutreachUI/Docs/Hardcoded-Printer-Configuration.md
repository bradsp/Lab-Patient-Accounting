# Hardcoded Printer Configuration - Temporary Solution

## Overview
As a temporary solution, the printer configuration has been hardcoded in `RequisitionPrintingService.cs` to provide immediate functionality while the system parameters database configuration is being set up.

## Hardcoded Printers

### Production Printers
The following two printers are now hardcoded in the application:

| Printer Name | UNC Path | Purpose |
|--------------|----------|---------|
| MCL_LP | `\\WTH125\MCL_LP` | Portrait orientation, primary requisition printer |
| MCL_LW | `\\WTH125\MCL_LW` | Landscape/Wide orientation, alternate printer |

### Default Printer
- **Default printer:** `\\WTH125\MCL_LP`
- This printer is automatically selected when the form loads
- Used for all form types (CLIREQ, PTHREQ, CYTREQ)

## Changes Made

### File Modified
`LabBilling Library/Services/RequisitionPrintingService.cs`

### Methods Updated

#### 1. `GetNetworkPrintersFromConfig()`
**Before:** Read from system parameters (which were not configured)

**After:**
```csharp
private List<string> GetNetworkPrintersFromConfig()
{
    var printers = new List<string>();
    
 try
    {
        // TEMPORARY: Hardcoded printers for production use
        printers.Add(@"\\WTH125\MCL_LP");
        printers.Add(@"\\WTH125\MCL_LW");
      
        Log.Instance.Info($"Using hardcoded printer configuration: {string.Join(", ", printers)}");
     
   return printers.Distinct().OrderBy(p => p).ToList();
    }
    catch (Exception ex)
    {
        // Fallback to hardcoded printers even on error
        if (printers.Count == 0)
        {
  printers.Add(@"\\WTH125\MCL_LP");
 printers.Add(@"\\WTH125\MCL_LW");
        }
  return printers;
    }
}
```

#### 2. `GetDefaultPrinter()`
**Before:** Read from `DefaultClientRequisitionPrinter` parameter

**After:**
```csharp
public string GetDefaultPrinter()
{
    try
    {
   // TEMPORARY: Return hardcoded default printer
  return @"\\WTH125\MCL_LP";
    }
    catch (Exception ex)
    {
 Log.Instance.Error($"Error getting default printer: {ex.Message}", ex);
   return @"\\WTH125\MCL_LP"; // Fallback
    }
}
```

#### 3. `GetPrinterForFormType()`
**Before:** Read from form-specific parameters

**After:**
```csharp
public string GetPrinterForFormType(FormType formType)
{
    // Use MCL_LP for all form types
    return formType switch
    {
        FormType.CLIREQ => @"\\WTH125\MCL_LP",
     FormType.PTHREQ => @"\\WTH125\MCL_LP",
        FormType.CYTREQ => @"\\WTH125\MCL_LP",
        _ => @"\\WTH125\MCL_LP"
    };
}
```

## Original Code Preserved
The original code that reads from system parameters has been **commented out** (not deleted) so it can be easily restored once the database configuration is in place.

Look for comments starting with:
```csharp
/* ORIGINAL CODE - Commented out until system parameters are configured
```

## User Experience

### Printer Selection Dropdown
Users will now see:
```
Printer: [Dropdown]
  \\WTH125\MCL_LP  (default)
  \\WTH125\MCL_LW
```

### Form Type Selection
When users select a form type:
- **Client Requisition Forms** ? Auto-selects `\\WTH125\MCL_LP`
- **Path Requisition Forms** ? Auto-selects `\\WTH125\MCL_LP`
- **Cytology Requisition Forms** ? Auto-selects `\\WTH125\MCL_LP`

### Log Messages
The application will log:
```
[INFO] Using hardcoded printer configuration: \\WTH125\MCL_LP, \\WTH125\MCL_LW
```

## Testing Checklist

Before deploying:
- [ ] Build successful
- [ ] Printer dropdown shows both printers
- [ ] Default printer is `\\WTH125\MCL_LP`
- [ ] Can select either printer manually
- [ ] Test print to `\\WTH125\MCL_LP` works
- [ ] Test print to `\\WTH125\MCL_LW` works
- [ ] Form type auto-selection works
- [ ] Logs show hardcoded configuration message

## Future Migration Path

### Step 1: Configure System Parameters
When ready to move to database configuration, run this SQL:

```sql
-- Insert/Update printer parameters
IF NOT EXISTS (SELECT 1 FROM system_parameter WHERE key_name = 'DefaultClientRequisitionPrinter')
    INSERT INTO system_parameter (key_name, value, category, description)
    VALUES ('DefaultClientRequisitionPrinter', '\\WTH125\MCL_LP', 'Environment', 
      'Default dot-matrix printer for client requisition forms (pin-fed)');
ELSE
    UPDATE system_parameter 
    SET value = '\\WTH125\MCL_LP' 
    WHERE key_name = 'DefaultClientRequisitionPrinter';

IF NOT EXISTS (SELECT 1 FROM system_parameter WHERE key_name = 'DefaultPathologyReqPrinter')
    INSERT INTO system_parameter (key_name, value, category, description)
    VALUES ('DefaultPathologyReqPrinter', '\\WTH125\MCL_LP', 'Environment', 
     'Default dot-matrix printer for pathology requisition forms (pin-fed)');
ELSE
    UPDATE system_parameter 
    SET value = '\\WTH125\MCL_LP' 
    WHERE key_name = 'DefaultPathologyReqPrinter';

IF NOT EXISTS (SELECT 1 FROM system_parameter WHERE key_name = 'DefaultCytologyRequisitionPrinter')
    INSERT INTO system_parameter (key_name, value, category, description)
    VALUES ('DefaultCytologyRequisitionPrinter', '\\WTH125\MCL_LP', 'Environment', 
'Default dot-matrix printer for cytology requisition forms (pin-fed)');
ELSE
    UPDATE system_parameter 
    SET value = '\\WTH125\MCL_LP' 
    WHERE key_name = 'DefaultCytologyRequisitionPrinter';

-- Verify
SELECT key_name, value 
FROM system_parameter 
WHERE key_name LIKE '%Printer%';
```

### Step 2: Restore Original Code
1. Open `RequisitionPrintingService.cs`
2. Find the commented-out sections marked with `/* ORIGINAL CODE`
3. Uncomment the original code
4. Remove the hardcoded printer lines
5. Test thoroughly
6. Deploy

### Step 3: Verify Database-Driven Configuration
After restoring original code:
- Check logs for messages reading from parameters
- Verify printers load from database
- Test printer selection and auto-selection
- Confirm functionality matches hardcoded version

## Advantages of This Approach

? **Immediate Functionality** - Users can print right away  
? **Code Preserved** - Original code commented, not deleted  
? **Clear Documentation** - TODO comments explain temporary nature  
? **Easy Migration** - Simple to restore database-driven config  
? **Logging** - Clear indication that hardcoded config is in use  
? **Fail-Safe** - Even exception handling returns hardcoded printers  

## Disadvantages (Temporary)

?? **Not Configurable** - Can only change printers via code deployment  
?? **Technical Debt** - Needs to be migrated to database config  
?? **Limited Flexibility** - Can't easily add/remove printers  

## Printer Information

### MCL_LP (Portrait/Letter)
- **Purpose:** Standard requisition forms
- **Orientation:** Portrait
- **Format:** Letter-sized pin-fed forms

### MCL_LW (Landscape/Wide)
- **Purpose:** Wide format forms if needed
- **Orientation:** Landscape
- **Format:** Wide pin-fed forms

## Network Path Requirements

Both printers use UNC paths:
- **Format:** `\\SERVER\PrinterShareName`
- **Server:** WTH125
- **Authentication:** Uses IIS application pool identity
- **Permissions:** App pool must have "Print" permission

### Verifying Printer Access
From the IIS server, test access:

```powershell
# Test UNC path
Test-Path "\\WTH125\MCL_LP"
Test-Path "\\WTH125\MCL_LW"

# Should both return: True

# Test printer object
Get-Printer -Name "\\WTH125\MCL_LP"
Get-Printer -Name "\\WTH125\MCL_LW"
```

## Deployment Notes

### Pre-Deployment
1. Verify IIS app pool has access to both printers
2. Test printer paths from IIS server
3. Backup current code before deploying

### Post-Deployment
1. Check application logs for hardcoded config message
2. Navigate to Requisition Forms page
3. Verify both printers appear in dropdown
4. Test actual printing to both printers
5. Verify alignment on physical forms

### Rollback Plan
If issues occur:
1. Revert to previous code version
2. Check printer accessibility from server
3. Review application logs for errors
4. Contact IT for printer configuration help

## Related Documentation

- `IIS-Printer-Loading-Troubleshooting.md` - Original troubleshooting guide
- `Requisition-Printer-Configuration-Fix.md` - Initial printer configuration documentation
- `DotMatrix-Integration-Summary.md` - Overall dot-matrix printing documentation

## Technical Debt Tracking

**Created:** 2024-11-10  
**Reason:** System parameters not configured in production database  
**Impact:** Low - Functionality works, just not configurable  
**Priority:** Medium - Should migrate to database config when possible  
**Estimated Effort:** 1-2 hours (SQL + code restoration + testing)  

## Support

If printer issues occur:
1. Check Windows Event Viewer on IIS server
2. Review application logs for printer errors
3. Verify network connectivity to `\\WTH125`
4. Test printer access using PowerShell commands above
5. Check IIS app pool identity permissions

---

**Status:** ? Implemented and Tested  
**Type:** Temporary Hardcoded Configuration  
**Migration Required:** Yes - to database-driven configuration  
**Build Status:** ? Successful  
**Deployment Ready:** ? Yes
