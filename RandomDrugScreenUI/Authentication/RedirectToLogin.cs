using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace RandomDrugScreenUI.Authentication;

/// <summary>
/// Redirects anonymous users to the login page.
/// Use this component on pages that require authentication.
/// </summary>
public class RedirectToLogin : ComponentBase
{
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    [CascadingParameter]
    protected Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    private bool hasCheckedAuth = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !hasCheckedAuth && AuthenticationStateTask != null)
        {
            hasCheckedAuth = true;

            var authState = await AuthenticationStateTask;
            var user = authState.User;

            Console.WriteLine($"[RedirectToLogin] Checking authentication...");
            Console.WriteLine($"[RedirectToLogin] User: {user?.Identity?.Name ?? "NULL"}");
            Console.WriteLine($"[RedirectToLogin] IsAuthenticated: {user?.Identity?.IsAuthenticated}");

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                Console.WriteLine($"[RedirectToLogin] User not authenticated, redirecting to /login");
                var returnUrl = Uri.EscapeDataString(NavigationManager.Uri);
                NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}", forceLoad: true);
            }
            else
            {
                Console.WriteLine($"[RedirectToLogin] User is authenticated, no redirect needed");
            }
        }
    }
}
