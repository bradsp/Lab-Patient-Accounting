using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Security.Claims;

namespace LabOutreachUI.Services;

/// <summary>
/// Captures the user's Windows authentication from the initial HTTP request
/// and makes it available throughout the Blazor circuit lifetime
/// </summary>
public class UserCircuitHandler : CircuitHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserCircuitHandler(IHttpContextAccessor httpContextAccessor)
    {
    _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User { get; private set; }
    public string? WindowsUsername { get; private set; }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // Capture the user from the initial HTTP request
        var httpContext = _httpContextAccessor.HttpContext;
 
        if (httpContext?.User != null)
        {
            User = httpContext.User;
      WindowsUsername = httpContext.User.Identity?.Name;
    
         Console.WriteLine($"[UserCircuitHandler] Captured Windows user: {WindowsUsername ?? "NULL"}");
    Console.WriteLine($"[UserCircuitHandler] IsAuthenticated: {httpContext.User.Identity?.IsAuthenticated}");
        }
        else
        {
       Console.WriteLine($"[UserCircuitHandler] No HttpContext or User available");
   }

        return base.OnConnectionUpAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
   // Clean up
        User = null;
        WindowsUsername = null;
        return base.OnConnectionDownAsync(circuit, cancellationToken);
    }
}
