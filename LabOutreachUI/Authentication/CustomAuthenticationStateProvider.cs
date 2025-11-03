using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Core.DataAccess;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using LabOutreachUI.Services;

namespace LabOutreachUI.Authentication;

/// <summary>
/// Custom authentication state provider for the Random Drug Screen application.
/// Integrates with existing AuthenticationService without modifying it.
/// Supports both Windows Authentication (automatic) and username/password login.
/// </summary>
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private Task<AuthenticationState>? _authenticationStateTask;
    private bool _hasAttemptedAuth = false;

    public CustomAuthenticationStateProvider(
        ProtectedSessionStorage sessionStorage,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CustomAuthenticationStateProvider> logger)
    {
        _sessionStorage = sessionStorage;
        _serviceProvider = serviceProvider;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogDebug("GetAuthenticationStateAsync called");

        // If we already have an authenticated state cached, return it
        if (_authenticationStateTask != null)
        {
            _logger.LogDebug("Returning cached authentication state");
            return await _authenticationStateTask;
        }

        try
        {
            // First, try to get from session storage (for post-render requests)
            if (_hasAttemptedAuth)
            {
                try
                {
                    var userSessionResult = await _sessionStorage.GetAsync<UserSessionInfo>("UserSession");

                    if (userSessionResult.Success && userSessionResult.Value != null)
                    {
                        var userSession = userSessionResult.Value;
                        _logger.LogInformation("Restored from session: {UserName}", userSession.UserName);

                        var claims = CreateClaimsFromUser(userSession);
                        var identity = new ClaimsIdentity(claims, "Session");
                        var claimsPrincipal = new ClaimsPrincipal(identity);
                        var authState = new AuthenticationState(claimsPrincipal);

                        _authenticationStateTask = Task.FromResult(authState);
                        return authState;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogDebug(ex, "Session storage not available (pre-render phase)");
                }
            }

            // Try to get Windows user from HttpContext or UserCircuitHandler
            string? windowsUsername = null;

            // First attempt: HttpContext (available during initial connection)
            if (_httpContextAccessor.HttpContext?.User?.Identity?.Name != null)
            {
                windowsUsername = _httpContextAccessor.HttpContext.User.Identity.Name;
                _logger.LogInformation("Windows user from HttpContext: {UserName}", windowsUsername);
            }
            else
            {
                // Second attempt: Try to get from UserCircuitHandler (after circuit is established)
                try
                {
                    var circuitHandler = _serviceProvider.GetServices<CircuitHandler>()
       .OfType<UserCircuitHandler>()
           .FirstOrDefault();

                    if (circuitHandler?.WindowsUsername != null)
                    {
                        windowsUsername = circuitHandler.WindowsUsername;
                        _logger.LogInformation("Windows user from CircuitHandler: {UserName}", windowsUsername);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Could not retrieve user from CircuitHandler");
                }
            }

            if (!string.IsNullOrEmpty(windowsUsername))
            {
                // Try to get user from database
                using var scope = _serviceProvider.CreateScope();
                var uowSystem = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSystem>();
                var authService = new AuthenticationService(uowSystem);

                var user = authService.AuthenticateIntegrated(windowsUsername);

                if (user != null && user.Access != UserStatus.None)
                {
                    _logger.LogInformation("User found in database: {UserName}, Access: {Access}",
                        user.UserName, user.Access);

                    // Create claims directly from database user
                    var userSession = new UserSessionInfo
                    {
                        UserName = user.UserName,
                        FullName = user.FullName,
                        IsAdministrator = user.IsAdministrator,
                        CanEditDictionary = user.CanEditDictionary,
                        Access = user.Access
                    };

                    var claims = CreateClaimsFromUser(userSession);
                    var identity = new ClaimsIdentity(claims, "Windows");
                    var claimsPrincipal = new ClaimsPrincipal(identity);

                    // Save to session storage for subsequent requests
                    try
                    {
                        await _sessionStorage.SetAsync("UserSession", userSession);
                        _logger.LogInformation("Session saved for {UserName}", user.UserName);
                        _hasAttemptedAuth = true;
                    }
                    catch (InvalidOperationException ex)
                    {
                        _logger.LogDebug(ex, "Could not save session (pre-render phase)");
                    }

                    var authState = new AuthenticationState(claimsPrincipal);
                    _authenticationStateTask = Task.FromResult(authState);
                    return authState;
                }
                else
                {
                    _logger.LogWarning("User not found or no access: {UserName}", windowsUsername);
                }
            }
            else
            {
                _logger.LogWarning("No Windows user available from HttpContext or CircuitHandler");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAuthenticationStateAsync");
        }

        _logger.LogWarning("Returning anonymous authentication state");
        return await Task.FromResult(new AuthenticationState(_anonymous));
    }

    /// <summary>
    /// Authenticates a user using Windows/Integrated authentication
    /// </summary>
    public async Task<bool> AuthenticateIntegrated(string username)
    {
        _logger.LogInformation("AuthenticateIntegrated called for {UserName}", username);
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var uowSystem = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSystem>();
            var authService = new AuthenticationService(uowSystem);

            var user = authService.AuthenticateIntegrated(username);

            if (user == null || user.Access == UserStatus.None)
            {
                _logger.LogWarning("AuthenticateIntegrated failed: User not found or no access for {UserName}", username);
                return false;
            }

            await SetAuthenticatedUser(user);
            _logger.LogInformation("AuthenticateIntegrated succeeded for {UserName}", username);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AuthenticateIntegrated for {UserName}", username);
            return false;
        }
    }

    /// <summary>
    /// Authenticates a user using username and password
    /// </summary>
    public async Task<(bool success, string errorMessage)> Authenticate(string username, string password)
    {
        _logger.LogInformation("Authenticate called for {UserName}", username);
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var uowSystem = scope.ServiceProvider.GetRequiredService<IUnitOfWorkSystem>();
            var authService = new AuthenticationService(uowSystem);

            var (isAuthenticated, user) = authService.Authenticate(username, password);

            if (!isAuthenticated || user == null)
            {
                _logger.LogWarning("Authentication failed for {UserName}", username);
                return (false, "Invalid username or password");
            }

            if (user.Access == UserStatus.None)
            {
                _logger.LogWarning("User {UserName} has no access", username);
                return (false, "User account is not authorized");
            }

            await SetAuthenticatedUser(user);
            _logger.LogInformation("Authentication succeeded for {UserName}", username);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating {UserName}", username);
            return (false, $"Authentication error: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs out the current user
    /// </summary>
    public async Task Logout()
    {
        _logger.LogInformation("Logout called");
        await _sessionStorage.DeleteAsync("UserSession");
        _authenticationStateTask = null;
        _hasAttemptedAuth = false;
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
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
        _hasAttemptedAuth = true;

        var claims = CreateClaimsFromUser(userSession);
        var identity = new ClaimsIdentity(claims, "CustomAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var authState = new AuthenticationState(claimsPrincipal);
        _authenticationStateTask = Task.FromResult(authState);
        NotifyAuthenticationStateChanged(_authenticationStateTask);
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
