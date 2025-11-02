using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Core.DataAccess;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace RandomDrugScreenUI.Authentication;

/// <summary>
/// Custom authentication state provider for the Random Drug Screen application.
/// Integrates with existing AuthenticationService without modifying it.
/// </summary>
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly IServiceProvider _serviceProvider;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(
        ProtectedSessionStorage sessionStorage,
        IServiceProvider serviceProvider)
    {
    _sessionStorage = sessionStorage;
        _serviceProvider = serviceProvider;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
     try
        {
       // Try to get user from session storage
            var userSessionResult = await _sessionStorage.GetAsync<UserSessionInfo>("UserSession");
            
            if (userSessionResult.Success && userSessionResult.Value != null)
            {
             var userSession = userSessionResult.Value;
   var claims = CreateClaimsFromUser(userSession);
       var identity = new ClaimsIdentity(claims, "CustomAuth");
         var claimsPrincipal = new ClaimsPrincipal(identity);
           return await Task.FromResult(new AuthenticationState(claimsPrincipal));
          }
        }
        catch
 {
            // Session storage not available yet or error reading it
        }

      return await Task.FromResult(new AuthenticationState(_anonymous));
    }

    /// <summary>
    /// Authenticates a user using Windows/Integrated authentication
 /// </summary>
    public async Task<bool> AuthenticateIntegrated(string username)
  {
        try
        {
         using var scope = _serviceProvider.CreateScope();
      var uowSystem = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSystem>();
          var authService = new AuthenticationService(uowSystem);

          var user = authService.AuthenticateIntegrated(username);

        if (user == null || user.Access == UserStatus.None)
            {
        return false;
   }

     await SetAuthenticatedUser(user);
   return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Authenticates a user using username and password
    /// </summary>
    public async Task<(bool success, string errorMessage)> Authenticate(string username, string password)
    {
        try
 {
            using var scope = _serviceProvider.CreateScope();
     var uowSystem = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSystem>();
     var authService = new AuthenticationService(uowSystem);

            var (isAuthenticated, user) = authService.Authenticate(username, password);

          if (!isAuthenticated || user == null)
            {
     return (false, "Invalid username or password");
            }

        if (user.Access == UserStatus.None)
            {
       return (false, "User account is not authorized");
          }

  await SetAuthenticatedUser(user);
         return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Authentication error: {ex.Message}");
        }
    }

  /// <summary>
    /// Logs out the current user
    /// </summary>
 public async Task Logout()
    {
    await _sessionStorage.DeleteAsync("UserSession");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    /// <summary>
    /// Gets the current authenticated user account
/// </summary>
    public async Task<UserAccount?> GetCurrentUser()
    {
        try
        {
  var userSessionResult = await _sessionStorage.GetAsync<UserSessionInfo>("UserSession");
            
    if (userSessionResult.Success && userSessionResult.Value != null)
   {
     var userSession = userSessionResult.Value;
    
    // Retrieve full user account from database
                using var scope = _serviceProvider.CreateScope();
       var uowSystem = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSystem>();
        return uowSystem.UserAccountRepository.GetByUsername(userSession.UserName);
            }
  }
   catch
        {
      // Error retrieving user
     }

  return null;
 }

    private async Task SetAuthenticatedUser(UserAccount user)
    {
        var userSession = new UserSessionInfo
      {
            UserName = user.UserName,
        FullName = user.FullName,
     IsAdministrator = user.IsAdministrator,
          CanEditDictionary = user.CanEditDictionary,
  Access = user.Access
   };

        await _sessionStorage.SetAsync("UserSession", userSession);

   var claims = CreateClaimsFromUser(userSession);
  var identity = new ClaimsIdentity(claims, "CustomAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private List<Claim> CreateClaimsFromUser(UserSessionInfo userSession)
    {
 var claims = new List<Claim>
    {
            new Claim(ClaimTypes.Name, userSession.UserName),
  new Claim(ClaimTypes.GivenName, userSession.FullName ?? userSession.UserName),
     new Claim("Access", userSession.Access ?? UserStatus.None)
        };

        if (userSession.IsAdministrator)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
        }

        if (userSession.CanEditDictionary)
        {
            claims.Add(new Claim("Permission", "EditDictionary"));
        }

  return claims;
    }
}

/// <summary>
/// Minimal user session information stored in browser session storage
/// </summary>
public class UserSessionInfo
{
    public string UserName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public bool IsAdministrator { get; set; }
    public bool CanEditDictionary { get; set; }
    public string? Access { get; set; }
}
