using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Collections.Concurrent;

namespace LabOutreachUI.Services;

/// <summary>
/// Captures the user's Windows authentication from the initial HTTP request
/// and makes it available throughout the Blazor circuit lifetime.
/// Uses a static cache to persist user data across service scope changes.
/// </summary>
public class UserCircuitHandler : CircuitHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserCircuitHandler> _logger;
 
    // Static cache to persist user info across scopes
  private static readonly ConcurrentDictionary<string, CircuitUserInfo> _circuitUsers = new();

    public UserCircuitHandler(IHttpContextAccessor httpContextAccessor, ILogger<UserCircuitHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public ClaimsPrincipal? User { get; private set; }
    public string? WindowsUsername { get; private set; }
    public string? CircuitId { get; private set; }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        CircuitId = circuit.Id;
    
   // Capture the user from the initial HTTP request
        var httpContext = _httpContextAccessor.HttpContext;

     _logger.LogInformation("[UserCircuitHandler] OnConnectionUpAsync called for circuit {CircuitId}. HttpContext available: {Available}, User authenticated: {Auth}", 
      circuit.Id, 
            httpContext != null,
      httpContext?.User?.Identity?.IsAuthenticated ?? false);
 
     if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
 User = httpContext.User;
    WindowsUsername = httpContext.User.Identity.Name;
    
         // Store in static cache
            _circuitUsers[circuit.Id] = new CircuitUserInfo
      {
            WindowsUsername = WindowsUsername,
       User = httpContext.User,
                CapturedAt = DateTime.UtcNow
            };

            _logger.LogInformation("[UserCircuitHandler] ? Captured and cached Windows user for circuit {CircuitId}: {Username}", 
         circuit.Id, WindowsUsername);
        }
        else
     {
         _logger.LogWarning("[UserCircuitHandler] ? No authenticated user available during OnConnectionUpAsync for circuit {CircuitId}. HttpContext: {HC}, User: {User}, IsAuth: {IsAuth}", 
        circuit.Id,
    httpContext != null ? "Available" : "NULL",
    httpContext?.User != null ? "Available" : "NULL",
       httpContext?.User?.Identity?.IsAuthenticated ?? false);
    }

      return base.OnConnectionUpAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[UserCircuitHandler] Connection down for circuit {CircuitId}, clearing user context", circuit.Id);
      
     // Remove from cache
        _circuitUsers.TryRemove(circuit.Id, out _);
        
        // Clean up instance
        User = null;
      WindowsUsername = null;
        CircuitId = null;
        
        return base.OnConnectionDownAsync(circuit, cancellationToken);
    }

  /// <summary>
    /// Gets the Windows username for a specific circuit from the static cache
    /// </summary>
    public static string? GetWindowsUsernameForCircuit(string circuitId)
    {
        if (_circuitUsers.TryGetValue(circuitId, out var userInfo))
        {
        return userInfo.WindowsUsername;
      }
        return null;
  }

    /// <summary>
    /// Gets the ClaimsPrincipal for a specific circuit from the static cache
    /// </summary>
    public static ClaimsPrincipal? GetUserForCircuit(string circuitId)
    {
 if (_circuitUsers.TryGetValue(circuitId, out var userInfo))
      {
            return userInfo.User;
     }
    return null;
    }

    /// <summary>
    /// Gets all active circuit user information (for debugging)
    /// </summary>
    public static Dictionary<string, string?> GetAllCircuitUsers()
    {
        return _circuitUsers.ToDictionary(
       kvp => kvp.Key,
  kvp => kvp.Value.WindowsUsername
        );
    }
}

internal class CircuitUserInfo
{
    public string? WindowsUsername { get; set; }
    public ClaimsPrincipal? User { get; set; }
    public DateTime CapturedAt { get; set; }
}
