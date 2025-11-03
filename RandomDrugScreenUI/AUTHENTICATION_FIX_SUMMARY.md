# Authentication Fix Summary

## Problem Identified

The application was failing to authenticate users when hosted in IIS due to a fundamental limitation of Windows Integrated Authentication in web applications:

### Root Cause
When using `IntegratedAuthentication = true` in the SQL Server connection string:
- The database connection attempts to use the **IIS Application Pool identity** (machine account: `WTHMC\WTH014$`)
- The Windows authenticated user credentials (`WTHMC\bpowers`) **do not automatically flow through** to SQL Server connections
- Result: `Login failed for user 'WTHMC\WTH014$'`

### Log Evidence
```
2025-11-03 09:01:12.7855|INFO|...|Windows user from HttpContext: WTHMC\bpowers
2025-11-03 09:01:13.1121|ERROR|...|Login failed for user 'WTHMC\WTH014$'.
```

The app correctly identifies the user as `WTHMC\bpowers`, but SQL Server sees the machine account `WTHMC\WTH014$`.

## Solution Implemented

Changed the authentication strategy to use:
1. **Windows Authentication** for user identification (unchanged)
2. **SQL Server Authentication** for database connections (NEW)

### Changes Made

#### 1. Configuration Files Updated

**appsettings.json** and **appsettings.Production.json**:
```json
{
  "AppSettings": {
    "AuthenticationMode": "SqlServer",  // Changed from "Integrated"
    "DatabaseUsername": "labpa_service",
    "DatabasePassword": ""// Add secure password here
  }
}
```

#### 2. Program.cs Enhanced

Added better error handling and logging for authentication mode configuration:
```csharp
// For SQL Server authentication, set credentials from configuration
if (!useIntegratedAuth)
{
    appEnv.UserName = config.GetValue<string>("AppSettings:DatabaseUsername") ?? "";
    appEnv.Password = config.GetValue<string>("AppSettings:DatabasePassword") ?? "";
    
    if (string.IsNullOrEmpty(appEnv.UserName))
    {
        log.LogError("[AppEnvironment] DatabaseUsername not configured for SqlServer authentication mode");
 throw new InvalidOperationException("DatabaseUsername must be configured when AuthenticationMode is SqlServer");
    }
}
```

#### 3. CustomAuthenticationStateProvider Enhanced

Added fallback to `UserCircuitHandler` to handle the case where HttpContext is no longer available after the Blazor circuit is established:

```csharp
// First attempt: HttpContext (available during initial connection)
if (_httpContextAccessor.HttpContext?.User?.Identity?.Name != null)
{
    windowsUsername = _httpContextAccessor.HttpContext.User.Identity.Name;
}
else
{
 // Second attempt: Try to get from UserCircuitHandler (after circuit is established)
  var circuitHandler = _serviceProvider.GetServices<CircuitHandler>()
  .OfType<UserCircuitHandler>()
        .FirstOrDefault();
    
    if (circuitHandler?.WindowsUsername != null)
    {
        windowsUsername = circuitHandler.WindowsUsername;
    }
}
```

#### 4. Documentation Added

- **AUTHENTICATION_CONFIG.md**: Comprehensive guide for setup and troubleshooting

## SQL Server Setup Required

You must create a SQL Server login for the service account:

```sql
-- Create the login
CREATE LOGIN [labpa_service] WITH PASSWORD = 'YourSecurePassword123!';

-- Grant access to your database
USE [LabBillingProd];
CREATE USER [labpa_service] FOR LOGIN [labpa_service];

-- Grant necessary permissions
ALTER ROLE [db_datareader] ADD MEMBER [labpa_service];
ALTER ROLE [db_datawriter] ADD MEMBER [labpa_service];
ALTER ROLE [db_ddladmin] ADD MEMBER [labpa_service];
```

## Configuration Steps

1. **Create SQL Server login** (see above)
2. **Update appsettings.json**:
   - Set `AuthenticationMode` to `"SqlServer"`
   - Set `DatabaseUsername` to `"labpa_service"`
   - Set `DatabasePassword` to your secure password
3. **Ensure IIS Windows Authentication is enabled**
4. **Deploy and test**

## How It Works Now

```
User Access Flow:
???????????????
?   Browser   ?
???????????????
   ? Windows Auth (WTHMC\bpowers)
  ?
????????????????????
?   IIS / Kestrel  ? ??? Captures Windows identity
????????????????????
 ? HttpContext.User.Identity.Name = "WTHMC\bpowers"
         ?
???????????????????????????
?  Blazor Server Circuit  ? ??? UserCircuitHandler preserves identity
???????????????????????????
    ? Windows Username: "WTHMC\bpowers"
         ?
????????????????????????????
? CustomAuthStateProvider  ? ??? Looks up user in database
????????????????????????????
    ? SQL Connection with labpa_service account
         ?
????????????????????????????
? SQL Server Database    ?
?   dict.user_account      ? ??? Query: WHERE username = 'WTHMC\bpowers'
?   Connection: labpa_service
????????????????????????????
```

## Key Points

1. **Windows username is preserved** in `AppEnvironment.User` for audit logging
2. **SQL service account** is used for all database operations
3. **User permissions** are managed in the `dict.user_account` table
4. **HttpContext loses user after circuit establishment** - this is normal Blazor Server behavior, handled by `UserCircuitHandler`

## Testing

After applying changes:
1. Access the application through IIS
2. Check `/auth-diagnostics` page
3. Verify:
   - HttpContext shows your Windows username
   - Blazor authentication state shows "Authenticated"
 - No SQL connection errors in logs
   - AppEnvironment.User shows your Windows username

## Security Notes

- The Windows username determines **who** is using the application (audit trail)
- The SQL service account determines **what** the application can do in the database (permissions)
- Store the SQL password securely (consider Azure Key Vault, AWS Secrets Manager, or DPAPI)
- Use strong passwords for the SQL service account
- Rotate passwords regularly

## Rollback

If you need to rollback to Integrated Authentication (not recommended for IIS):
```json
{
  "AppSettings": {
    "AuthenticationMode": "Integrated",
    "DatabaseUsername": "",
    "DatabasePassword": ""
  }
}
```

Note: This only works if the IIS Application Pool has a SQL Server login or if the application runs under a user account.
