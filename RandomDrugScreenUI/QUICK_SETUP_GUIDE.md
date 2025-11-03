# Quick Setup Guide - Authentication Fix

## Prerequisites
- IIS with Windows Authentication enabled
- SQL Server access to create logins
- Access to appsettings.json files

## Step-by-Step Setup

### Step 1: Create SQL Server Login (5 minutes)

Run this in SQL Server Management Studio:

```sql
-- 1. Create login
CREATE LOGIN [labpa_service] WITH PASSWORD = 'CHANGE_THIS_PASSWORD_123!';

-- 2. Switch to your database
USE [LabBillingProd];  -- or LabBillingTest for dev

-- 3. Create user
CREATE USER [labpa_service] FOR LOGIN [labpa_service];

-- 4. Grant permissions
ALTER ROLE [db_datareader] ADD MEMBER [labpa_service];
ALTER ROLE [db_datawriter] ADD MEMBER [labpa_service];
ALTER ROLE [db_ddladmin] ADD MEMBER [labpa_service];

-- 5. Verify
SELECT name, type_desc FROM sys.database_principals WHERE name = 'labpa_service';
```

### Step 2: Update Configuration Files (2 minutes)

**For Development** (`appsettings.json`):
```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingTest",
    "ServerName": "wth014",
    "LogDatabaseName": "NLog",
    "AuthenticationMode": "SqlServer",
    "DatabaseUsername": "labpa_service",
  "DatabasePassword": "CHANGE_THIS_PASSWORD_123!"
  }
}
```

**For Production** (`appsettings.Production.json`):
```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingProd",
    "ServerName": "wth014",
    "LogDatabaseName": "NLog",
    "AuthenticationMode": "SqlServer",
    "DatabaseUsername": "labpa_service",
    "DatabasePassword": "PRODUCTION_PASSWORD_HERE!"
  }
}
```

### Step 3: Verify IIS Configuration (3 minutes)

1. Open IIS Manager
2. Select your application
3. Double-click "Authentication"
4. Verify:
   - ? **Windows Authentication: Enabled**
   - ? **Anonymous Authentication: Disabled**
5. Click "Application Pools" ? Select your pool
6. Note: Pool identity can remain as `ApplicationPoolIdentity` (recommended)

### Step 4: Deploy and Test (2 minutes)

1. Build the solution
2. Publish to IIS
3. Browse to application
4. Navigate to `/auth-diagnostics`
5. Verify you see:
 ```
   HttpContext User Identity Name: DOMAIN\YourUsername
   Blazor AuthenticationState: ? Authenticated
   User Name: YourUsername
   ```

## Troubleshooting

### Error: "Login failed for user 'DOMAIN\SERVERNAME$'"
**Fix**: Make sure `AuthenticationMode` is set to `"SqlServer"` in appsettings.json

### Error: "DatabaseUsername must be configured"
**Fix**: Add `DatabaseUsername` and `DatabasePassword` to appsettings.json

### Error: "Login failed for user 'labpa_service'"
**Fix**: Verify SQL login was created correctly and password matches

### User shows as "Not Authorized"
**Fix**: Check `dict.user_account` table - Windows username must exist with proper access level

### HttpContext shows "NULL"
**Fix**: Verify Windows Authentication is enabled in IIS

## Verification Checklist

- [ ] SQL Server login `labpa_service` created
- [ ] User `labpa_service` granted database permissions
- [ ] `appsettings.json` updated with SqlServer mode
- [ ] `DatabaseUsername` and `DatabasePassword` configured
- [ ] IIS Windows Authentication enabled
- [ ] IIS Anonymous Authentication disabled
- [ ] Application deployed to IIS
- [ ] `/auth-diagnostics` shows authenticated user
- [ ] No SQL connection errors in logs

## Support

See detailed documentation:
- `AUTHENTICATION_CONFIG.md` - Complete configuration guide
- `AUTHENTICATION_FIX_SUMMARY.md` - Technical details and explanation

## Security Reminder

?? **IMPORTANT**: 
- Use strong passwords for SQL service account
- Consider using Azure Key Vault or similar for password storage in production
- Rotate passwords regularly
- Monitor failed login attempts
