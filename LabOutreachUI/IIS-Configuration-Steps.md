# IIS Configuration for Windows Authentication

## ?? IMPORTANT: ASP.NET Core Configuration Limitation

**ASP.NET Core applications CANNOT configure authentication in web.config** when using in-process hosting.
The authentication settings MUST be configured at the IIS server/application level using one of the methods below.

The `web.config` only contains the ASP.NET Core module configuration with `forwardWindowsAuthToken="true"`.

---

## Required IIS Features
Ensure Windows Authentication is installed on the IIS server:

```powershell
# Run as Administrator
Install-WindowsFeature Web-Windows-Auth
```

## METHOD 1: Configure via IIS Manager (Recommended for Single Server)

1. Open **IIS Manager**
2. Navigate to your application (LabOutreachUI)
3. Double-click **Authentication**
4. **Disable** Anonymous Authentication (right-click ? Disable)
5. **Enable** Windows Authentication (right-click ? Enable)
6. Right-click Windows Authentication ? **Advanced Settings**:
   - ? **Enable Kernel-mode authentication**
   - Extended Protection: **Accept**
7. Right-click Windows Authentication ? **Providers**:
   - Click **Clear** to remove all
   - Add **Negotiate** (click Add, type "Negotiate")
   - Add **NTLM** (click Add, type "NTLM")
   - Ensure order is: **Negotiate**, then **NTLM**

---

## METHOD 2: Configure via PowerShell (Recommended for Automation/Multiple Servers)

```powershell
# Run as Administrator
# Replace 'Default Web Site/LabOutreachUI' with your actual site path
$sitePath = 'IIS:\Sites\Default Web Site\LabOutreachUI'

# Import IIS module
Import-Module WebAdministration

# Disable Anonymous Authentication
Set-WebConfigurationProperty -Filter '/system.webServer/security/authentication/anonymousAuthentication' `
  -Name enabled -Value $false -PSPath $sitePath

# Enable Windows Authentication
Set-WebConfigurationProperty -Filter '/system.webServer/security/authentication/windowsAuthentication' `
 -Name enabled -Value $true -PSPath $sitePath

# Clear existing providers
Clear-WebConfiguration -Filter '/system.webServer/security/authentication/windowsAuthentication/providers' `
    -PSPath $sitePath

# Add Negotiate provider (Kerberos)
Add-WebConfigurationProperty -Filter '/system.webServer/security/authentication/windowsAuthentication/providers' `
    -Name "." -Value @{value='Negotiate'} -PSPath $sitePath

# Add NTLM provider (fallback)
Add-WebConfigurationProperty -Filter '/system.webServer/security/authentication/windowsAuthentication/providers' `
    -Name "." -Value @{value='NTLM'} -PSPath $sitePath

# Enable kernel-mode authentication (recommended for better performance)
Set-WebConfigurationProperty -Filter '/system.webServer/security/authentication/windowsAuthentication' `
    -Name useKernelMode -Value $true -PSPath $sitePath

# Enable Extended Protection
Set-WebConfigurationProperty -Filter '/system.webServer/security/authentication/windowsAuthentication' `
    -Name extendedProtection.tokenChecking -Value Allow -PSPath $sitePath

# Restart the application pool (replace with your app pool name)
Restart-WebAppPool -Name "YourAppPoolName"
```

---

## Configure the Application Pool
The application pool must use ApplicationPoolIdentity or NetworkService:

```powershell
# Set the application pool identity (replace 'YourAppPoolName' with your actual app pool name)
Import-Module WebAdministration
Set-ItemProperty IIS:\AppPools\YourAppPoolName -Name processModel.identityType -Value ApplicationPoolIdentity
```

---

## Browser Configuration

### For Internet Explorer/Edge (IE Mode)
Users should ensure your site is in their **Local Intranet** zone:
1. Internet Options ? Security ? Local Intranet ? Sites ? Advanced
2. Add your site URL (e.g., `http://yourserver` or `https://yourserver`)

### For Chrome/Edge Chromium
Add to the authentication whitelist:
```
--auth-server-whitelist="yourserver.domain.com"
--auth-negotiate-delegate-whitelist="yourserver.domain.com"
```

Or set via Group Policy:
- AuthServerWhitelist: `yourserver.domain.com`
- AuthNegotiateDelegateWhitelist: `yourserver.domain.com`

## Troubleshooting

### If users still get prompted for credentials:

1. **Check DNS/Network Configuration**:
   - Ensure users can access the server using the server name (not IP)
   - The server name must match the Kerberos SPN

2. **Verify SPN (Service Principal Name)**:
   ```powershell
   # Check existing SPNs
   setspn -L SERVERNAME
   
   # Add SPN if missing (replace with your values)
   setspn -S HTTP/servername.domain.com domain\appPoolAccount
   ```

3. **Check Application Pool Identity**:
   - Must have proper permissions to access resources
   - Should NOT be running as a specific user account unless absolutely necessary

4. **Verify web.config deployment**:
   - Ensure the updated web.config is deployed to the server
   - Check that IIS has read the configuration (restart app pool if needed)

5. **Check Windows Event Logs**:
   - Application log for ASP.NET Core errors
   - Security log for authentication failures

6. **Test with Different Browsers**:
   - IE/Edge in IE mode usually works best with Windows Auth
   - Chrome/Firefox need whitelist configuration

## Testing Authentication

Create a simple test page to verify the authenticated user:

Access: `https://yourserver/auth-test`

Expected result: Should show the Windows username without prompting for credentials.
