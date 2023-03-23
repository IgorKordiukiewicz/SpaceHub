using Hangfire;
using Microsoft.Extensions.Options;
using SpaceHub.Application;
using SpaceHub.Infrastructure;
using SpaceHub.Web.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var infrastructureSettingsSection = builder.Configuration.GetSection("InfrastructureSettings");
builder.Services.Configure<InfrastructureSettings>(infrastructureSettingsSection);
var infrastructureSettings = infrastructureSettingsSection.Get<InfrastructureSettings>();

builder.Services.ConfigureInfrastructureServices(infrastructureSettings);
builder.Services.ConfigureApplicationServices();

builder.Services.ConfigureHangfire(infrastructureSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.AddEndpoints();

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

infrastructureSettings = app.Services.GetRequiredService<IOptions<InfrastructureSettings>>().Value;
if(infrastructureSettings.HangfireEnabled)
{
    app.UseHangfireDashboard();
    app.AddJobs();
}


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }
