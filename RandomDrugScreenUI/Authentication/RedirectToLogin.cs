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

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationStateTask != null)
        {
         var authState = await AuthenticationStateTask;
    var user = authState.User;

            if (!user.Identity?.IsAuthenticated ?? true)
          {
        var returnUrl = Uri.EscapeDataString(NavigationManager.Uri);
           NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
            }
        }
    }
}
