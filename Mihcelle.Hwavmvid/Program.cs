using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mihcelle.Hwavmvid.Components;
using Mihcelle.Hwavmvid.Data;
using Mihcelle.Hwavmvid.Shared.Constants;
using Mihcelle.Hwavmvid.Shared.Models;
using Mihcelle.Hwavmvid;
using Mihcelle.Hwavmvid.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using Mihcelle.Hwavmvid.Alerts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

try
{

    // mihcelle.hwavmvid
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<Applicationdbcontext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddIdentity<Applicationuser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 2;
    })
    .AddEntityFrameworkStores<Mihcelle.Hwavmvid.Data.Applicationdbcontext>();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.Name = Authentication.Authcookiename;
        options.Cookie.HttpOnly = false;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

}
catch (Exception exception) { Console.WriteLine(exception.Message); }


// mihcelle.hwavmvid
builder.Services.AddHttpClient();
builder.Services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = false;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.DefaultBufferSize = 4096;
                options.JsonSerializerOptions.MaxDepth = 41;
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

builder.Services.AddControllersWithViews();

// mihcelle.hwavmvid
builder.Services.AddCors(option =>
{
    option.AddPolicy("mihcellehwavmvidcorspolicy", (builder) =>
    {
        builder.SetIsOriginAllowedToAllowWildcardSubdomains()
               .SetIsOriginAllowed(isOriginAllowed => true)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// mihcelle.hwavmvid
builder.Services.AddScoped<AuthenticationStateProvider, Applicationauthenticationstateprovider>();
builder.Services.AddScoped<Applicationprovider, Applicationprovider>();
builder.Services.AddScoped<AlertsService, AlertsService>();
builder.Services.AddScoped<Cookiesprovider, Cookiesprovider>();

// mihcelle.hwavmvid
builder.Services.AddSignalR()
    .AddHubOptions<Applicationhub>(options =>
    {
        options.EnableDetailedErrors = true;
        options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.MaximumReceiveMessageSize = Int64.MaxValue;
        options.StreamBufferCapacity = Int32.MaxValue;
    })
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.WriteIndented = false;
        options.PayloadSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        options.PayloadSerializerOptions.AllowTrailingCommas = true;
        options.PayloadSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
        options.PayloadSerializerOptions.DefaultBufferSize = 4096;
        options.PayloadSerializerOptions.MaxDepth = 41;
        options.PayloadSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// mihcelle.hwavmvid
app.UseRouting();
app.UseCors("mihcellehwavmvidcorspolicy");

// mihcelle.hwamvid
app.UseAuthentication();
app.UseAuthorization();

// mihcelle.hwavmvid
app.MapHub<Applicationhub>("/api/applicationhub", options =>
{
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
    options.ApplicationMaxBufferSize = long.MaxValue;
    options.TransportMaxBufferSize = long.MaxValue;
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(10);
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// mihcelle.hwavmvid
app.MapControllers();

app.Run();
