# IIS Deployment: Network Printer Solution - Implementation Summary

## Problem Statement
When the Lab Patient Accounting Blazor application runs on an IIS server (instead of locally), the server-side printing code cannot access printers installed on client PCs. The original implementation used `System.Drawing.Printing.PrinterSettings.InstalledPrinters` which only enumerates printers available to the IIS server.

## Solution Overview
**? RECOMMENDED: Network Printer Configuration via System Parameters**

Configure client PC printers as network shares (UNC paths) and store these paths in the application's `system_parms` database table. The server accesses printers via network paths instead of enumerating local printers.

### Why This Works
- ? Server can access network-shared printers via UNC paths (`\\PC-NAME\PrinterShare`)
- ? No client-side software required
- ? Centralized configuration in database
- ? Works with existing PCL5/raw printing code
- ? Supports multiple locations/printers
- ? Maintains audit trail

## Implementation Changes

### 1. Enhanced `RequisitionPrintingService.cs`

#### Modified Methods:

**`GetAvailablePrinters()`**
- **Before**: Only returned locally installed printers
- **After**: Prioritizes network printers from system parameters, falls back to local printers
- Returns UNC paths like `\\CLIENT-PC\DotMatrixPrinter`

```csharp
public List<string> GetAvailablePrinters()
{
    // Try network printers from config first
    var networkPrinters = GetNetworkPrintersFromConfig();
 if (networkPrinters.Count > 0)
        return networkPrinters;
    
    // Fallback to local printers (development scenario)
    return System.Drawing.Printing.PrinterSettings.InstalledPrinters
        .Cast<string>().OrderBy(p => p).ToList();
}
```

**`GetDefaultPrinter()`**
- **Before**: Returned system default printer
- **After**: Returns printer from `DefaultClientRequisitionPrinter` parameter first
- Falls back to system default only if not configured

**`GetNetworkPrintersFromConfig()` (NEW)**
- Reads printer paths from `ApplicationParameters`
- Extracts:  
  - `DefaultClientRequisitionPrinter`
  - `DefaultPathologyReqPrinter`
  - `DefaultCytologyRequisitionPrinter`
- Returns distinct, sorted list

**`GetPrinterForFormType()` (NEW)**
- Returns appropriate printer based on form type
- Maps:
  - CLIREQ ? `DefaultClientRequisitionPrinter`
  - PTHREQ ? `DefaultPathologyReqPrinter`
  - CYTREQ ? `DefaultCytologyRequisitionPrinter`

**`ValidatePrinterAccess()` (NEW)**
- Tests connectivity to network printers
- Sends test PCL reset command
- Returns success/failure with error details
- Helps diagnose permission/network issues

### 2. Configuration UI: `PrinterConfiguration.razor` (NEW)

Created admin page at `/admin/printer-configuration` with:

**Features:**
- Displays current printer configuration from system parameters
- Shows UNC paths for all configured printers
- Lists available network printers
- **Test Connection** button for each printer
- **Alignment Test** button to print test pattern
- Real-time test results display
- Step-by-step configuration instructions
- Visual indicators (Network UNC vs Local printer badges)

**Purpose:**
- Helps administrators verify printer configuration
- Tests network connectivity before going live
- Provides immediate feedback on printer access issues
- Documents configuration requirements inline

### 3. Documentation Created

#### `Network-Printer-Configuration-IIS.md`
Comprehensive guide covering:
- Network printer sharing setup
- Firewall configuration
- IIS app pool identity permissions
- UNC path formats
- Troubleshooting common issues
- Security considerations
- Multiple location scenarios
- Print server alternative
- Validation checklist

## Configuration Steps

### Database Configuration

```sql
-- Configure network printers in system_parms table
UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-01\DotMatrixReq'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-02\PathPrinter'
WHERE key_name = 'DefaultPathologyReqPrinter';

UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-03\CytoPrinter'
WHERE key_name = 'DefaultCytologyRequisitionPrinter';
```

### Client PC Setup

1. **Share the Printer:**
   - Settings ? Devices ? Printers & scanners
   - Select printer ? Manage ? Printer properties ? Sharing tab
   - ? Share this printer
   - Set share name (e.g., "DotMatrixReq")
   - ? Render print jobs on client computers

2. **Grant Server Access:**
   - Printer Properties ? Security tab
   - Add IIS app pool identity with "Print" permission

3. **Configure Firewall:**
   - Allow File and Printer Sharing
   - Open ports 139, 445 (SMB)

### IIS Configuration

**App Pool Identity:**
```
IIS Manager ? Application Pools ? LabOutreachUI
? Advanced Settings ? Identity ? Custom account
? Set credentials with printer access
```

**Or use domain service account** (recommended for enterprise):
```
DOMAIN\ServiceAccountName
```

## Technical Details

### How It Works

1. **Application Startup:**
   - Loads `ApplicationParameters` from database
   - Reads printer UNC paths from environment parameters

2. **When User Opens Print Form:**
   - UI calls `GetAvailablePrinters()`
   - Service reads network printer paths from config
   - Dropdown shows: `\\CLIENT-PC\DotMatrixReq`

3. **When User Prints:**
   - Service validates printer access
   - Generates PCL5 data via `DotMatrixRequisitionService`
   - Calls `RawPrinterHelper.SendBytesToPrinter(uncPath, pcl5Bytes)`
   - Win32 API opens network printer connection
   - Sends raw PCL5 bytes over network to client PC
   - Client PC's print spooler processes job
   - Dot-matrix printer prints 3-ply form

4. **Network Communication Flow:**
   ```
   IIS Server Process
   ?
   Win32 OpenPrinter("\\CLIENT-PC\DotMatrixReq")
   ?
   SMB/CIFS Connection (Port 445)
   ?
   Client PC Print Spooler Service
   ?
   Dot-Matrix Printer (Pin-Fed Forms)
 ```

### Security Considerations

**Principle of Least Privilege:**
- Grant only **Print** permission (not Manage)
- Use dedicated service account for IIS app pool
- Avoid using domain admin credentials

**Audit Trail:**
All print jobs logged in `rpt_track` table:
```sql
SELECT mod_date, mod_user, cli_nme, form_printed, 
     qty_printed, printer_name
FROM dbo.rpt_track
ORDER BY mod_date DESC;
```

## Benefits of This Solution

### vs. Client-Side Printing
- ? No browser limitations
- ? Full PCL5 control
- ? Precise positioning
- ? Server-side audit trail

### vs. Client-Side Agent
- ? No software installation on client PCs
- ? Simpler architecture
- ? No additional services to maintain
- ? No firewall complexities

### vs. Browser Print
- ? Exact form positioning (3 lines, 50 chars)
- ? Optimized for 3-ply carbon copies
- ? No CSS/browser rendering issues
- ? Direct hardware control

## Testing & Validation

### Pre-Deployment Checklist

- [ ] Client PC printers shared with proper names
- [ ] IIS app pool identity has Print permission
- [ ] Firewall allows File and Printer Sharing
- [ ] UNC paths tested from server: `net use \\CLIENT-PC\PrinterName`
- [ ] System parameters updated in database
- [ ] Application restarted to load new parameters
- [ ] Admin UI shows network printers correctly
- [ ] Test Connection succeeds for all printers
- [ ] Alignment Test prints successfully
- [ ] Actual requisition form prints correctly
- [ ] Form positioning verified (3 lines from top, 50 chars left)
- [ ] 3-ply carbon copies are readable
- [ ] Print job recorded in `rpt_track` table
- [ ] Multiple copies tested
- [ ] Batch printing tested (if applicable)

### Validation Commands

```cmd
:: Test from IIS server (run as app pool identity)
net use \\CLIENT-PC\DotMatrixReq

:: List shares
net view \\CLIENT-PC

:: Test connectivity
ping CLIENT-PC

:: Verify Print Spooler running
sc query spooler
```

## Troubleshooting Guide

### "Access Denied" Error
**Cause:** IIS app pool identity lacks Print permission  
**Fix:** Add app pool identity to printer security with Print permission

### "Network path not found"
**Cause:** SMB/firewall blocking, or PC offline  
**Fix:**
1. Verify client PC is online: `ping CLIENT-PC`
2. Check firewall allows SMB: ports 139, 445
3. Try IP address: `\\192.168.1.100\PrinterName`

### "Printer not ready" or Jobs Stuck
**Cause:** Printer offline, out of paper, or ribbon issue  
**Fix:**
1. Check physical printer status
2. Restart Print Spooler: `net stop spooler && net start spooler`
3. Clear print queue

### Parameters Not Taking Effect
**Cause:** Application hasn't reloaded parameters  
**Fix:** Restart IIS application pool or web application

## Performance Considerations

- **Network Latency**: Print jobs sent over network (typically acceptable)
- **Large Jobs**: May timeout on slow networks (monitor)
- **Concurrent Users**: Network bandwidth consideration for multiple simultaneous prints
- **Printer Availability**: Client PC must be powered on and printer ready

**Optimization Tips:**
1. Use wired network connections for printer PCs
2. Pre-authenticate with persistent connections: `net use /persistent:yes`
3. Monitor print spooler performance on client PCs
4. Configure printers to stay powered on (disable sleep)

## Alternative Scenarios

### Multiple Locations
Store location-specific printers:
```sql
INSERT INTO dbo.system_parms (key_name, parm_value, category)
VALUES 
    ('Printer_Building1', '\\PC-BLDG1\DotMatrix', 'Environment'),
    ('Printer_Building2', '\\PC-BLDG2\DotMatrix', 'Environment');
```

### Print Server Approach
For enterprise deployments:
1. Dedicated Windows Print Server
2. All dot-matrix printers installed on print server
3. Single UNC path: `\\PRINTSERVER\RequisitionPrinter`
4. Centralized management and monitoring

## Files Modified/Created

### Modified:
- `LabBilling Library/Services/RequisitionPrintingService.cs`
  - Enhanced printer enumeration
- Added network printer configuration support
  - Added printer validation methods

### Created:
- `LabBilling Library/Docs/Network-Printer-Configuration-IIS.md`
  - Complete configuration guide
  - Troubleshooting reference
  
- `LabOutreachUI/Components/Admin/PrinterConfiguration.razor`
  - Admin UI for testing and validation
  - Live configuration display
  - Connection testing tools

- `LabOutreachUI/Docs/IIS-Network-Printer-Implementation-Summary.md` (this document)

### Existing (Unchanged):
- `LabBilling Library/Services/RawPrinterHelper.cs` - Still uses Win32 APIs
- `LabBilling Library/Services/DotMatrixRequisitionService.cs` - PCL5 generation unchanged
- `LabBilling Library/Services/PCL5FormatterService.cs` - PCL5 commands unchanged
- `LabOutreachUI/Components/Clients/AddressRequisitionPrint.razor` - UI uses new service methods

## Deployment Instructions

1. **Update Database:**
   ```sql
   -- Run configuration script with actual UNC paths
   UPDATE dbo.system_parms 
   SET parm_value = '\\YOUR-CLIENT-PC\YOUR-PRINTER-SHARE'
   WHERE key_name IN (
       'DefaultClientRequisitionPrinter',
       'DefaultPathologyReqPrinter',
     'DefaultCytologyRequisitionPrinter'
   );
   ```

2. **Configure Client PCs:**
   - Share printers as documented
   - Set appropriate permissions
   - Test connectivity from server

3. **Deploy Application:**
 - Deploy updated code to IIS
   - Configure app pool identity with printer access
   - Restart application pool

4. **Validate:**
   - Navigate to `/admin/printer-configuration`
   - Test each printer connection
   - Print alignment test
   - Verify actual requisition print

5. **Train Users:**
   - Show printer selection dropdown
   - Explain network printer paths
   - Demonstrate alignment testing

## Support & Maintenance

**Regular Checks:**
- Verify printer shares remain active
- Monitor `rpt_track` for failed print jobs
- Test printer connectivity periodically
- Review Windows Event Log for print spooler errors

**Contact Points:**
- **Network/Printer Issues**: IT Help Desk
- **Configuration Changes**: Database Administrator
- **Application Issues**: Development Team

---
**Document Version:** 1.0  
**Created:** {Current Date}  
**For:** IIS-deployed Blazor applications with server-side PCL5 printing  
**Status:** Production Ready
