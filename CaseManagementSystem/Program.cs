using CaseManagementSystem.Data.AgencyTypes;
using CaseManagementSystem.Data.CaseProfiles;
using CaseManagementSystem.Data.Cases;
using CaseManagementSystem.Data.CaseTypes;
using CaseManagementSystem.Data.Companies;
using CaseManagementSystem.Data.Auth;
using CaseManagementSystem.Data.Subjects;
using CaseManagementSystem.Data.TimeLimits;
using CaseManagementSystem.Data.TitlePrefixes;
using CaseManagementSystem.Data.TraceLevels;
using CaseManagementSystem.Data.TraceReason;
using CaseManagementSystem.Data.TraceResults;
using CaseManagementSystem.Data.AddressType;
using CaseManagementSystem.Data.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using CaseManagementSystem.Data.AgencyTypeCaseProfile;
using CaseManagementSystem.Data.Documents;
using Blazor.Component.Maps;
using CaseManagementSystem.Emails;
using CaseManagementSystem.Emails.Templates;
using CMS.DL.Model;
using Microsoft.Extensions.Configuration;
using CaseManagementSystem.Helpers;
using CaseManagementSystem.Data.Country;
using CaseManagementSystem.Data.CompaniesAddress;
using CaseManagementSystem.Data.Scheduler;
using Hangfire;
using Hangfire.SqlServer;
using System.Diagnostics;
using CaseManagementSystem.Data.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Logging.Configuration;
using CaseManagementSystem.Data.UsersDocuments;
using CaseManagementSystem.Data.Dashboard;
using Microsoft.AspNetCore.Http.Features;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Your token validation parameters (issuer, audience, etc.)
            };
        });

// Configure authorization policies if needed
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("role", "InternalAdmin"));
    // Add more policies as needed for different roles or claims
});
builder.Services.AddLogging(builder =>
{
    builder.SetMinimumLevel(LogLevel.Debug); 
    builder.AddConsole();
    builder.AddDebug();  
});

// Register custom service
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AddressService>();
builder.Services.AddSingleton<AddressTypeService>();
builder.Services.AddSingleton<CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<UserDocumentsService>();
builder.Services.AddSingleton<CompaniesService>();
builder.Services.AddSingleton<CompaniesAddressService>();
builder.Services.AddSingleton<CountryService>();
builder.Services.AddSingleton<AgencyTypeService>();
builder.Services.AddSingleton<CaseTypeService>();
builder.Services.AddSingleton<CaseService>();
builder.Services.AddSingleton<TraceLevelService>();
builder.Services.AddSingleton<TraceReasonService>();
builder.Services.AddSingleton<SubjectService>();
builder.Services.AddSingleton<CaseProfileService>();
builder.Services.AddSingleton<TimeLimitService>();
builder.Services.AddSingleton<TitlePrefixeService>();
builder.Services.AddSingleton<TraceResultsService>();
builder.Services.AddSingleton<AgencyTypeCaseProfileService>();
builder.Services.AddSingleton<DocumentService>();
builder.Services.AddSingleton<CaseWatchersService>();
builder.Services.AddSingleton<SchedulerServices>();
builder.Services.AddSingleton<DashboardService>();
builder.Services.AddSingleton<ICryptoService, CryptoService>();
builder.Services.AddSingleton<IEmailService>(new EmailService(
    builder.Configuration.GetValue<string>("EMailConfiguration:AdminEmail"), 
    builder.Configuration.GetValue<string>("EMailConfiguration:From"),
    builder.Configuration.GetValue<string>("EMailConfiguration:SmtpServer"),
    builder.Configuration.GetValue<int>("EMailConfiguration:Port"),
    builder.Configuration.GetValue<string>("EMailConfiguration:Username"),
    builder.Configuration.GetValue<string>("EMailConfiguration:Password"),
    builder.Configuration.GetValue<string>("EMailConfiguration:ccEmail")
));

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

//builder.Services.AddHangfire(c => c.UseMemoryStorage());
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage("Server=tcp:esarisk.database.windows.net,1433;Initial Catalog=elvis_new;Persist Security Info=False;User ID=elvis;Password=esarisk@123%;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    //config.UseSqlServerStorage("Data Source=DESKTOP-GOKCDVO;Initial Catalog=elivis-local;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;");
    //config.UseSqlServerStorage("Server=DESKTOP-GOKCDVO;Database=elvis-local;Integrated Security=True;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");
});
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IRecurringJobManager, RecurringJobManager>();
builder.Services.AddSingleton<ILogger<SchedulerServices>, Logger<SchedulerServices>>();
builder.Services.AddScoped<SchedulerServices>();

var serviceProvider = builder.Services.BuildServiceProvider();
var schedulerServices = serviceProvider.GetService<SchedulerServices>();
RecurringJob.AddOrUpdate<SchedulerServices>("GetUsersPendingDays", x => schedulerServices.GetUsersPendingDays(), Cron.Daily);
builder.Services.AddHangfireServer();

// Mud blazor UI
builder.Services.AddMudServices();
builder.Services.AddMapsServices();
var app = builder.Build();

/*
EmailService es = new EmailService(
    builder.Configuration.GetValue<string>("EMailConfiguration:AdminEmail"),
    builder.Configuration.GetValue<string>("EMailConfiguration:From"),
    builder.Configuration.GetValue<string>("EMailConfiguration:SmtpServer"),
    builder.Configuration.GetValue<int>("EMailConfiguration:Port"),
    builder.Configuration.GetValue<string>("EMailConfiguration:Username"),
    builder.Configuration.GetValue<string>("EMailConfiguration:Password")
);

CaseAssigned emailNotifCaseCreatedClient = new CaseAssigned(string.Empty, string.Empty, "AA");
await es.SendEmailNotificationAsync(emailNotifCaseCreatedClient, "asdf", null);
*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // Add console logger
    builder.AddDebug();  // Add debug logger
                         // Add any other loggers as needed (e.g., AddFile, AddEventLog)
});

TraceSource ts = new TraceSource("TraceTest");
ts.TraceEvent(TraceEventType.Error, 1, "Error message.");

// OR using the constructor with both string connection and IConfiguration parameters

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseHangfireDashboard("/hangfire");
app.UseHangfireServer();
app.Run();
