using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace LabOutreachUI.Pages
{
   public class AuthDiagModel : PageModel
{
        public bool IsAuthenticated { get; set; }
  public string? Username { get; set; }
        public string? AuthenticationType { get; set; }
   public bool HttpContextAvailable { get; set; }
 public Dictionary<string, string?> ServerVariables { get; set; } = new();

        public void OnGet()
 {
     HttpContextAvailable = HttpContext != null;

   if (HttpContext != null)
     {
  IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
     Username = User?.Identity?.Name;
 AuthenticationType = User?.Identity?.AuthenticationType;

         // Capture relevant server variables
          var relevantVars = new[] {
"AUTH_TYPE",
      "AUTH_USER",
   "LOGON_USER",
"REMOTE_USER",
      "SERVER_NAME",
     "SERVER_PORT",
 "SERVER_SOFTWARE",
       "HTTP_USER_AGENT",
       "REMOTE_ADDR",
 "REMOTE_HOST"
   };

       foreach (var varName in relevantVars)
        {
    var value = HttpContext.Request.Headers[varName].FirstOrDefault()
         ?? HttpContext.Connection.RemoteIpAddress?.ToString();
     
    ServerVariables[varName] = value ?? "Not Available";
       }

   // Try to get from server variables if available
     try
    {
  var serverVars = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IServerVariablesFeature>();
     if (serverVars != null)
    {
               foreach (var varName in relevantVars)
          {
          ServerVariables[varName] = serverVars[varName] ?? ServerVariables.GetValueOrDefault(varName, "Not Available");
       }
        }
 }
    catch
                {
      // Server variables feature not available
     }
         }
 }
    }
}
