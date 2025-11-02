using LabBilling.Core.DataAccess;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RandomDrugScreenUI.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure HSTS options for production
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

// Add authentication services
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

// Configure AppEnvironment
builder.Services.AddSingleton<IAppEnvironment>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    
    var authMode = config.GetValue<string>("AppSettings:AuthenticationMode") ?? "Integrated";
    var useIntegratedAuth = authMode.Equals("Integrated", StringComparison.OrdinalIgnoreCase);
    
    var appEnv = new AppEnvironment
    {
        DatabaseName = config.GetValue<string>("AppSettings:DatabaseName") ?? "LabBillingProd",
        ServerName = config.GetValue<string>("AppSettings:ServerName") ?? "localhost",
        LogDatabaseName = config.GetValue<string>("AppSettings:LogDatabaseName") ?? "LabBillingLogs",
        IntegratedAuthentication = useIntegratedAuth,
        User = Environment.UserName
    };

    // For SQL Server authentication, set credentials
    if (!useIntegratedAuth)
    {
        appEnv.UserName = config.GetValue<string>("AppSettings:DatabaseUsername") ?? "";
        appEnv.Password = config.GetValue<string>("AppSettings:DatabasePassword") ?? "";
    }

    // Initialize with default empty parameters first to allow ConnectionString to work
    appEnv.ApplicationParameters = new LabBilling.Core.Models.ApplicationParameters();

    try
    {
        // Now try to load real parameters from database
     using var uow = new UnitOfWorkSystem(appEnv);
        var systemService = new SystemService(appEnv, uow);
        appEnv.ApplicationParameters = systemService.LoadSystemParameters();
    }
    catch (Exception ex)
    {
        // Log the error but continue with default parameters
        Console.WriteLine($"Warning: Could not load system parameters from database: {ex.Message}");
        Console.WriteLine("Continuing with default application parameters.");
 // ApplicationParameters already initialized above with defaults
    }

    return appEnv;
});

// Register UnitOfWorkSystem for authentication (uses system-level connection)
builder.Services.AddScoped<IUnitOfWorkSystem>(sp =>
{
    var appEnv = sp.GetRequiredService<IAppEnvironment>();
    return new UnitOfWorkSystem(appEnv);
});

// Register services
builder.Services.AddScoped<IRandomDrugScreenService, RandomDrugScreenService>();
builder.Services.AddScoped<DictionaryService>();
builder.Services.AddScoped<IUnitOfWork>(sp => 
{
    var appEnv = sp.GetRequiredService<IAppEnvironment>();
    return new UnitOfWorkMain(appEnv);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

// Enforce HTTPS redirection
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Optionally auto-login with Windows authentication in production
if (app.Environment.IsProduction())
{
var config = app.Services.GetRequiredService<IConfiguration>();
    var authMode = config.GetValue<string>("AppSettings:AuthenticationMode") ?? "Integrated";
    
  if (authMode.Equals("Integrated", StringComparison.OrdinalIgnoreCase))
    {
  Console.WriteLine("Production mode with Integrated Authentication enabled.");
        Console.WriteLine("Users will be automatically authenticated using Windows credentials.");
    }
}

app.Run();
