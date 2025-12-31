using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabOutreachUI.Pages;

public class AccessDeniedPageModel : PageModel
{
    public bool IsAuthenticated { get; set; }
    public string? Username { get; set; }
    public string? UnauthorizedReason { get; set; }

    public void OnGet()
    {
   IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
        Username = User?.Identity?.Name ?? "Unknown";
  UnauthorizedReason = User?.FindFirst("UnauthorizedReason")?.Value;
    }
}
