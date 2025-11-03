# Authentication Configuration for Random Drug Screen UI

## Overview

This Blazor Server application supports two authentication modes:
1. **Windows/Integrated Authentication** (recommended for production)
2. **SQL Server Authentication** (for development/testing)

The authentication system integrates with the existing `AuthenticationService` and `UserAccountRepository` without modifying their functionality, ensuring compatibility with other applications.

## Authentication Modes

### 1. Windows/Integrated Authentication (Production)

**Benefits:**
- More secure (no credentials in configuration files)
- Seamless user experience (automatic login using Windows credentials)
- Leverages Active Directory for user management
- No password management required

**Configuration:**

In `appsettings.json` or `appsettings.Production.json`:

```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingProd",
    "ServerName": "your-server-name",
 "LogDatabaseName": "NLog",
    "AuthenticationMode": "Integrated"
  }
}
```

**How it works:**
1. Application reads the Windows username from `Environment.UserName`
2. Calls `AuthenticationService.AuthenticateIntegrated(username)`
3. Validates user exists in the `emp` table with appropriate access level
4. User is automatically logged in if authorized

### 2. SQL Server Authentication (Development)

**Benefits:**
- Useful for development environments without domain authentication
- Allows testing with different user credentials
- More flexible for testing scenarios

**Configuration:**

In `appsettings.Development.json`:

```json
{
  "AppSettings": {
    "DatabaseName": "LabBillingTest",
    "ServerName": "your-dev-server",
    "LogDatabaseName": "NLog",
    "AuthenticationMode": "SqlServer",
 "DatabaseUsername": "your_db_user",
    "DatabasePassword": "your_db_password"
  }
}
```

**How it works:**
1. User is redirected to `/login` page
2. User enters their application username and password
3. Calls `AuthenticationService.Authenticate(username, password)`
4. Password is hashed using SHA256 and compared to stored hash
5. User is authenticated if credentials match and access level is valid

## User Authorization

User access is controlled through the `emp` table with the following fields:

- `name` - Username (primary key)
- `full_name` - Display name
- `access` - Access level (None, View, EnterEdit, etc.)
- `password` - SHA256 hashed password (for SQL auth mode)
- `reserve4` (IsAdministrator) - Admin flag
- `access_edit_dictionary` (CanEditDictionary) - Dictionary editing permission

## Application Components

### 1. CustomAuthenticationStateProvider
Location: `RandomDrugScreenUI/Authentication/CustomAuthenticationStateProvider.cs`

**Responsibilities:**
- Manages authentication state for Blazor
- Interfaces with existing `AuthenticationService`
- Stores user session information in protected browser storage
- Provides claims-based authorization

**Key Methods:**
- `AuthenticateIntegrated(username)` - Windows authentication
- `Authenticate(username, password)` - SQL authentication
- `Logout()` - Clears session and logs out user
- `GetCurrentUser()` - Retrieves current authenticated user

### 2. Login Page
Location: `RandomDrugScreenUI/Pages/Login.razor`

A standard login form with:
- Username and password fields
- Form validation
- Error message display
- Loading state during authentication
- Return URL support for deep linking

### 3. AuthenticationWrapper
Location: `RandomDrugScreenUI/Shared/AuthenticationWrapper.razor`

**Responsibilities:**
- Checks authentication on app startup
- Handles auto-login for Windows auth mode
- Redirects unauthenticated users to login page
- Shows loading indicator during auth check

### 4. Updated MainLayout
Location: `RandomDrugScreenUI/Shared/MainLayout.razor`

**Additions:**
- Displays current user's name
- Logout button
- Uses `<AuthorizeView>` for conditional rendering

## Security Considerations

### Session Management
- User session data stored in `ProtectedSessionStorage` (encrypted)
- Only minimal user info stored (username, full name, permissions)
- Full user account retrieved from database on demand

### Database Connection
- **Integrated Auth**: Uses Windows credentials, no passwords stored
- **SQL Auth**: Database credentials in config file (use environment variables or Azure Key Vault in production)
- Connection string built securely using `SqlConnectionStringBuilder`

### Password Storage
- Passwords hashed using SHA256
- Hashing performed by existing `AuthenticationService.EncryptPassword()`
- No plaintext passwords stored or transmitted

## Environment-Specific Configuration

### Development
```json
{
  "AppSettings": {
    "AuthenticationMode": "SqlServer",
    "DatabaseUsername": "dev_user",
    "DatabasePassword": "dev_password"
  }
}
```

### Production
```json
{
  "AppSettings": {
  "AuthenticationMode": "Integrated"
  }
}
```

**Best Practice:** Use Azure App Service Configuration or Environment Variables for sensitive settings in production.

## IIS Configuration (Production Deployment)

For Windows Authentication to work in IIS:

1. **Enable Windows Authentication** in IIS:
   - Open IIS Manager
 - Select your application
   - Double-click "Authentication"
   - Enable "Windows Authentication"
   - Disable "Anonymous Authentication"

2. **Configure Application Pool**:
   - Identity: Use ApplicationPoolIdentity or a specific service account
   - Ensure the identity has appropriate database permissions

3. **Web.config** (auto-generated, but verify):
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

## Testing Different Authentication Modes

### Test Windows Authentication
1. Set `AuthenticationMode` to "Integrated"
2. Ensure your Windows user exists in the `emp` table
3. Run the application - you should be auto-logged in

### Test SQL Authentication
1. Set `AuthenticationMode` to "SqlServer"
2. Provide database credentials in config
3. Run the application - you should see the login page
4. Log in with a username/password from the `emp` table

## Protecting Specific Pages

To require authentication on a page:

```razor
@page "/candidates"
@attribute [Authorize]

<h3>Candidate Management</h3>
...
```

To require specific roles:

```razor
@attribute [Authorize(Roles = "Administrator")]
```

To check permissions in code:

```razor
<AuthorizeView Policy="EditDictionary">
    <Authorized>
        <button>Edit Dictionary</button>
    </Authorized>
    <NotAuthorized>
        <p>You don't have permission to edit the dictionary.</p>
  </NotAuthorized>
</AuthorizeView>
```

## Troubleshooting

### Issue: User not authenticated in production
**Solution:** Verify Windows Authentication is enabled in IIS and the user's Windows username matches an entry in the `emp` table.

### Issue: SQL authentication fails
**Solution:** 
- Verify database credentials are correct
- Check that the user exists in the `emp` table
- Ensure password is properly hashed in the database
- Check `access` field is not set to "None"

### Issue: Application can't connect to database
**Solution:**
- Verify SQL Server is accessible from the web server
- Check firewall rules
- Ensure connection string is correct
- Verify database user has appropriate permissions

## Migration from Development to Production

1. Update `appsettings.Production.json`:
   - Set `AuthenticationMode` to "Integrated"
 - Remove `DatabaseUsername` and `DatabasePassword`
   - Update server and database names

2. Configure IIS for Windows Authentication (see above)

3. Ensure all users have Windows usernames that match their `emp` table entries

4. Test authentication with various user accounts

## Future Enhancements

Potential improvements to consider:

1. **Azure AD Integration**: Add support for Azure Active Directory authentication
2. **Multi-Factor Authentication**: Implement MFA for additional security
3. **Password Reset**: Add self-service password reset functionality (for SQL auth mode)
4. **Audit Logging**: Log authentication attempts and user actions
5. **Session Timeout**: Implement automatic logout after inactivity
6. **Role-Based Policies**: Create custom authorization policies based on user permissions

## Support

For questions or issues with authentication, contact your system administrator or development team.
