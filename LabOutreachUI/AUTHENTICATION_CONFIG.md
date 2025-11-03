# Authentication Configuration Guide

## Overview
The Random Drug Screen UI application uses Windows Authentication for user identification combined with SQL Server service account for database connections.

## How It Works

### 1. Windows Authentication
- IIS captures the Windows authenticated user (e.g., `DOMAIN\username`)
- This username is stored in `AppEnvironment.User` for audit purposes
- The application validates this username against the `dict.user_account` table

### 2. Database Authentication
The application uses **SQL Server Authentication** with a service account for database connections:

```json
{
  "AppSettings": {
    "AuthenticationMode": "SqlServer",
    "DatabaseUsername": "labpa_service",
    "DatabasePassword": "your_secure_password_here"
  }
}
```

## Why Not Integrated Authentication?

When using `IntegratedAuthentication = true` in a web application hosted in IIS:
- The database connection uses the **IIS Application Pool identity** (machine account like `DOMAIN\SERVERNAME$`)
- The Windows authenticated user credentials **do not flow through** to SQL Server
- This causes "Login failed for user 'DOMAIN\SERVERNAME$'" errors

## Setup Instructions

### 1. SQL Server Setup

Create a SQL Server login for the service account:

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

### 2. Configuration File Setup

Update `appsettings.json` (for development) and `appsettings.Production.json` (for production):

```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingProd",
    "ServerName": "your_server_name",
    "LogDatabaseName": "NLog",
    "AuthenticationMode": "SqlServer",
    "DatabaseUsername": "labpa_service",
    "DatabasePassword": "YourSecurePassword123!"
  }
}
```

**Security Note**: Consider using Azure Key Vault, AWS Secrets Manager, or Windows DPAPI to encrypt the password in production.

### 3. IIS Setup

1. **Enable Windows Authentication** in IIS:
   - Open IIS Manager
   - Select your application
   - Click "Authentication"
   - Enable "Windows Authentication"
   - Disable "Anonymous Authentication"

2. **Application Pool Identity**:
   - The app pool can run under `ApplicationPoolIdentity` (recommended)
   - No need to change it since SQL Server authentication is being used

3. **web.config** should have:
   ```xml
   <system.webServer>
   <security>
       <authentication>
         <windowsAuthentication enabled="true" />
         <anonymousAuthentication enabled="false" />
       </authentication>
     </security>
   </system.webServer>
   ```

## Authentication Flow

1. **User accesses application** ? IIS captures Windows username
2. **Initial HTTP request** ? `HttpContext.User.Identity.Name` contains `DOMAIN\username`
3. **Circuit established** ? `UserCircuitHandler` captures the username
4. **Database lookup** ? Uses SQL service account to query `dict.user_account` table for the Windows username
5. **Session stored** ? User information cached in browser session storage
6. **Subsequent requests** ? Authentication retrieved from session storage

## Troubleshooting

### Issue: "Login failed for user 'DOMAIN\SERVERNAME$'"
**Cause**: Application is using Integrated Authentication mode  
**Solution**: Change `AuthenticationMode` to `SqlServer` in appsettings.json

### Issue: "User not found or no access"
**Cause**: Windows username not in `dict.user_account` table  
**Solution**: Add user to database with appropriate access level

### Issue: "No Windows user available from HttpContext"
**Cause**: Anonymous authentication enabled or Windows auth not configured
**Solution**: Check IIS Windows Authentication settings

### Issue: HttpContext loses user after circuit establishment
**Cause**: Normal Blazor Server behavior - HttpContext only available during initial connection  
**Solution**: Application uses `UserCircuitHandler` to preserve the username throughout the circuit lifetime

## Configuration Options

### AuthenticationMode
- `SqlServer`: Use SQL Server authentication (recommended for IIS hosting)
- `Integrated`: Use Windows integrated authentication (only works when app runs as the user, e.g., desktop apps)

### Switching Between Modes

**Development (IIS Express)**:
```json
"AuthenticationMode": "SqlServer"
```

**Production (IIS)**:
```json
"AuthenticationMode": "SqlServer"
```

## Security Best Practices

1. **Use strong passwords** for the SQL service account
2. **Limit SQL permissions** - grant only what's needed
3. **Encrypt configuration** files or use secret management
4. **Rotate passwords** regularly
5. **Monitor failed login attempts** in both Windows and SQL Server
6. **Use HTTPS** to protect credentials in transit

## Additional Notes

- The Windows username is stored in `AppEnvironment.User` for audit logging
- Database operations use the service account credentials
- User permissions and access levels are managed in the `dict.user_account` table
- The `UserCircuitHandler` preserves the authenticated user identity throughout the Blazor Server circuit lifetime
