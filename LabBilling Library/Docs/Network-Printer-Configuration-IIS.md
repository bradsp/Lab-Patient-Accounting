# Network Printer Configuration for IIS Deployment

## Overview
When the Lab Patient Accounting application runs on an IIS server, it cannot access printers installed on client PCs directly. This guide explains how to configure network printers for dot-matrix requisition printing.

## The Problem
- **Client-side printers**: Not accessible from server-side Blazor code
- **Browser limitations**: Cannot enumerate or send raw data to local printers
- **IIS execution context**: Code runs on server, not client browser

## The Solution: Network Printer Shares

Configure client PC printers as **network shares** that the IIS server can access via UNC paths.

## Step-by-Step Configuration

### 1. Share the Dot-Matrix Printer on Client PC

#### On Windows 10/11 Client PC:

1. **Open Settings** ? **Devices** ? **Printers & scanners**

2. **Select your dot-matrix printer** ? **Manage** ? **Printer properties**

3. **Go to Sharing tab**:
   - ? Check "Share this printer"
   - Set share name (e.g., `DotMatrixReq`)
   - ? Check "Render print jobs on client computers"

4. **Click Apply**

5. **Note the UNC path**: `\\CLIENT-PC-NAME\DotMatrixReq`

#### Alternative: PowerShell Method
```powershell
# Run as Administrator
Set-Printer -Name "YourPrinterName" -Shared $true -ShareName "DotMatrixReq"
```

### 2. Grant Server Access to Shared Printer

#### Option A: Domain Environment (Recommended)
1. IIS Application Pool should run under a **domain service account**
2. Grant this account **Print** permission on the shared printer:
   - Printer Properties ? Security tab
   - Add the IIS app pool identity (e.g., `DOMAIN\IISAppPoolAccount`)
   - Grant "Print" permission

#### Option B: Workgroup Environment
1. Create matching local account on client PC and server
2. Configure IIS app pool to use this account:
   ```
 IIS Manager ? Application Pools ? LabOutreachUI ? Advanced Settings
   ? Identity ? Custom account ? Set credentials
   ```

### 3. Configure System Parameters

#### Add Network Printer to Database:

```sql
-- Insert/Update network printer configuration
UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-01\DotMatrixReq'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-02\PathReqPrinter'
WHERE key_name = 'DefaultPathologyReqPrinter';

UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC-03\CytoReqPrinter'
WHERE key_name = 'DefaultCytologyRequisitionPrinter';

-- Verify configuration
SELECT key_name, parm_value
FROM dbo.system_parms
WHERE key_name LIKE '%Printer%';
```

#### Or via Application UI:
1. Go to **System Settings** ? **Parameters**
2. Find "Environment" category
3. Update printer parameters with UNC paths:
   - `DefaultClientRequisitionPrinter`: `\\CLIENT-PC\DotMatrixReq`
   - `DefaultPathologyReqPrinter`: `\\OTHER-PC\PathPrinter`
   - `DefaultCytologyRequisitionPrinter`: `\\THIRD-PC\CytoPrinter`

### 4. Test Network Printer Access

#### From IIS Server (Command Prompt):
```cmd
:: Test printer connectivity
net use \\CLIENT-PC\DotMatrixReq

:: List shared printers on remote PC
net view \\CLIENT-PC
```

#### From Application:
1. Navigate to **Print Requisition Forms**
2. Select **Dot-Matrix** mode
3. Printer dropdown should show network printers from configuration
4. Click **Test Alignment** to verify connectivity

### 5. Firewall Configuration

Ensure firewall allows File and Printer Sharing:

#### On Client PC (Windows Firewall):
```powershell
# Enable File and Printer Sharing
Set-NetFirewallRule -DisplayGroup "File And Printer Sharing" -Enabled True -Profile Domain

# Or for specific inbound rule:
New-NetFirewallRule -DisplayName "Allow Print Spooler" `
    -Direction Inbound -Protocol TCP -LocalPort 445 -Action Allow
```

#### Required Ports:
- **TCP 445**: SMB/CIFS file sharing
- **TCP 139**: NetBIOS Session Service (legacy)
- **UDP 137**: NetBIOS Name Service

## UNC Path Formats

### Correct Formats:
```
\\HOSTNAME\PrinterShareName       ? Recommended
\\192.168.1.100\PrinterShareName     ? IP address (if DNS issues)
\\FQDN.domain.com\PrinterShareName   ? Fully qualified domain name
```

### Incorrect Formats:
```
\\HOSTNAME\C$\Printer                ? Don't use admin shares
USB001          ? Local port name won't work
LPT1:        ? Local port won't work
```

## Multiple Printer Locations Scenario

If you have multiple client PCs with dot-matrix printers:

### Approach 1: Location-Based Configuration
```sql
-- Add custom parameters for each location
INSERT INTO dbo.system_parms (key_name, parm_value, category, description)
VALUES 
    ('Printer_Location_Building1', '\\PC-BLDG1\DotMatrix', 'Environment', 'Building 1 Requisition Printer'),
    ('Printer_Location_Building2', '\\PC-BLDG2\DotMatrix', 'Environment', 'Building 2 Requisition Printer'),
    ('Printer_Location_Building3', '\\PC-BLDG3\DotMatrix', 'Environment', 'Building 3 Requisition Printer');
```

Then users select location-specific printer from dropdown.

### Approach 2: User-Based Defaults
Store preferred printer per user in database:
```sql
CREATE TABLE dbo.user_printer_preferences (
    user_id VARCHAR(50) NOT NULL,
    form_type VARCHAR(20) NOT NULL,
    printer_path VARCHAR(255) NOT NULL,
    CONSTRAINT PK_user_printer_pref PRIMARY KEY (user_id, form_type)
);
```

## Troubleshooting

### Error: "Access Denied" or "Printer Not Found"

**Check:**
1. ? IIS App Pool identity has Print permission on shared printer
2. ? Client PC firewall allows File and Printer Sharing
3. ? Network path is correct (test with `net use`)
4. ? Print Spooler service running on client PC

**Test Manually:**
```cmd
:: From IIS server command prompt (run as IIS app pool identity)
net use \\CLIENT-PC\DotMatrixReq
dir \\CLIENT-PC\DotMatrixReq
```

### Error: "The network path was not found"

**Solutions:**
1. **Check Network Connectivity**:
   ```cmd
   ping CLIENT-PC
   ```

2. **Verify SMB is enabled**:
   ```powershell
   Get-WindowsOptionalFeature -Online -FeatureName SMB1Protocol
 Enable-WindowsOptionalFeature -Online -FeatureName SMB1Protocol
   ```

3. **Use IP address instead of hostname**:
   ```
   \\192.168.1.100\DotMatrixReq
   ```

### Error: "Invalid Printer Name" in Event Log

**Fix:**
1. Check printer share name has no spaces or special characters
2. Restart Print Spooler on both client and server:
   ```cmd
   net stop spooler
   net start spooler
   ```

### Print Jobs Stuck in Queue

**Resolution:**
1. Clear print queue on client PC
2. Check client printer is online and ready
3. Verify ribbon/paper in dot-matrix printer
4. Restart Print Spooler service

## Security Considerations

### Principle of Least Privilege
- Grant only **Print** permission (not Manage Printers or Manage Documents)
- Use dedicated service account for IIS app pool
- Don't use domain admin accounts

### Audit Trail
All print jobs are logged in `dbo.rpt_track` table:
```sql
SELECT TOP 100 
    mod_date,
    mod_user,
    cli_nme,
    form_printed,
    qty_printed,
    printer_name
FROM dbo.rpt_track
ORDER BY mod_date DESC;
```

### Network Isolation
Consider placing dot-matrix printers on isolated VLAN if handling sensitive data.

## Alternative: Print Server Approach

For large deployments, consider a dedicated print server:

1. **Install Print Server role** on Windows Server
2. **Add all dot-matrix printers** to print server
3. **Share printers** from print server
4. **Configure system parameters** to point to print server:
   ```
   \\PRINTSERVER\ClientRequisitionPrinter
   \\PRINTSERVER\PathRequisitionPrinter
   \\PRINTSERVER\CytoRequisitionPrinter
   ```

**Benefits:**
- Centralized management
- Better monitoring
- Easier to secure
- Print queue visibility

## Validation Checklist

Before going live:

- [ ] Client PC printer shared with appropriate name
- [ ] Firewall allows File and Printer Sharing
- [ ] IIS app pool identity has Print permission
- [ ] Network path tested from server (`net use`)
- [ ] System parameters updated with UNC paths
- [ ] Test alignment printed successfully
- [ ] Actual requisition printed correctly
- [ ] Print job recorded in `rpt_track` table
- [ ] Multiple copies tested
- [ ] 3-ply carbon quality verified
- [ ] Form positioning accurate (3 lines, 50 chars)

## Performance Tips

1. **Pre-authenticate connections**: Configure persistent drive mapping
   ```cmd
   net use \\CLIENT-PC\DotMatrixReq /persistent:yes
   ```

2. **Monitor network latency**: Large print jobs may timeout on slow networks

3. **Use wired connections**: Wireless can cause intermittent failures

4. **Keep printers powered on**: Configure power settings to prevent sleep

## Support Contacts

- **IT Help Desk**: For printer sharing and network access issues
- **Application Support**: For system parameter configuration
- **Development Team**: For code-level troubleshooting

---
**Document Version:** 1.0  
**Last Updated:** {Current Date}  
**Applies To:** IIS-deployed Blazor applications with server-side printing
