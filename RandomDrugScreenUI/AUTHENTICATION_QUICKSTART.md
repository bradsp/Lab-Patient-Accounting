# Quick Start - Authentication Configuration

## Choose Your Authentication Mode

### Option 1: Windows Authentication (Production - Recommended)

**appsettings.Production.json:**
```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingProd",
    "ServerName": "your-production-server",
    "LogDatabaseName": "NLog",
    "AuthenticationMode": "Integrated"
  }
}
```

**IIS Configuration:**
- Enable Windows Authentication
- Disable Anonymous Authentication
- Application Pool: Use appropriate domain account

**User Setup:**
- Windows username must match entry in `emp` table
- Set appropriate `access` level (VIEW, ENTER/EDIT, etc.)
- Ensure `access` is NOT "NONE"

### Option 2: SQL Server Authentication (Development/Testing)

**appsettings.Development.json:**
```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingTest",
    "ServerName": "your-dev-server",
    "LogDatabaseName": "NLog",
 "AuthenticationMode": "SqlServer",
    "DatabaseUsername": "app_user",
    "DatabasePassword": "secure_password"
  }
}
```

**User Setup:**
1. Add user to `emp` table
2. Set password (SHA256 hash)
3. Set `access` level
4. Navigate to `/login` and enter credentials

## Testing Your Setup

### Test Integrated Auth:
1. Run app with `AuthenticationMode: "Integrated"`
2. App should auto-login with your Windows username
3. If login fails, check:
   - Windows username matches `emp.name`
   - `emp.access` is not "NONE"
   - SQL Server allows Windows auth

### Test SQL Auth:
1. Run app with `AuthenticationMode: "SqlServer"`
2. Navigate to `/login`
3. Enter username/password from `emp` table
4. If login fails, check:
   - Password is correctly hashed
   - `emp.access` is not "NONE"
   - Database credentials are correct

## Common Issues

**"User not authorized"**
? Check `emp.access` field is set to "VIEW" or "ENTER/EDIT"

**"Invalid username or password"**
? Verify password hash in database matches SHA256 hash

**Connection fails**
? Check server name, database name, and network connectivity

**Windows auth not working in IIS**
? Enable Windows Authentication in IIS, disable Anonymous

## Next Steps

See [AUTHENTICATION_SETUP.md](AUTHENTICATION_SETUP.md) for complete documentation.
