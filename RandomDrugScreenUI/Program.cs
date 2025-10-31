using LabBilling.Core.DataAccess;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RandomDrugScreenUI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure AppEnvironment
builder.Services.AddSingleton<IAppEnvironment>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var appEnv = new AppEnvironment
    {
        DatabaseName = config.GetValue<string>("AppSettings:DatabaseName") ?? "LabBillingProd",
        ServerName = config.GetValue<string>("AppSettings:ServerName") ?? "localhost",
        LogDatabaseName = config.GetValue<string>("AppSettings:LogDatabaseName") ?? "LabBillingLogs",
        IntegratedAuthentication = true,
        User = Environment.UserName
    };

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

// Register services
builder.Services.AddScoped<IRandomDrugScreenService, RandomDrugScreenService>();
builder.Services.AddScoped<DictionaryService>();
builder.Services.AddScoped<IUnitOfWork>(sp => 
{
    var appEnv = sp.GetRequiredService<IAppEnvironment>();
    return new UnitOfWorkMain(appEnv);
});

// Keep sample service for now
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
