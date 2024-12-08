using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Mihcelle.Hwavmvid.Components;
using Mihcelle.Hwavmvid.Shared.Constants;
using Mihcelle.Hwavmvid.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Mihcelle.Hwavmvid;
using Mihcelle.Hwavmvid.Alerts;
using Mihcelle.Hwavmvid.Client;
using Mihcelle.Hwavmvid.Cookies;
using Mihcelle.Hwavmvid.Fileupload;
using Mihcelle.Hwavmvid.Modal;
using Mihcelle.Hwavmvid.Notifications;
using Mihcelle.Hwavmvid.Pager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


// mihcelle.hwavmvid
builder.Services.AddHttpClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated",
    client => { client.BaseAddress = new Uri(builder.Environment.ContentRootPath + "api"); });






builder.Services.AddScoped<Cookiesprovider, Cookiesprovider>();





// mihcelle.hwavmvid
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var installed = !string.IsNullOrEmpty(connectionString);

if (installed == false)
{
    var configpath = string.Concat(builder.Environment.ContentRootPath, "\\wwwroot\\", "tobaccoindustries.json");
    var jsonconfig = System.IO.File.ReadAllText(configpath);
    var deserializedconfig = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonconfig);
    if (deserializedconfig != null)
    {
        deserializedconfig["installation"] = new { createdon = string.Empty };
        var updatedconfigfile = JsonSerializer.Serialize(deserializedconfig, new JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(configpath, updatedconfigfile);
    }
}

try
{
    builder.Services.AddDbContext<Mihcelle.Hwavmvid.Server.Data.Applicationdbcontext>(options => options.UseSqlServer(connectionString));
    builder.Services.AddIdentity<Applicationuser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 2;
    })
        .AddEntityFrameworkStores<Mihcelle.Hwavmvid.Server.Data.Applicationdbcontext>();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.Name = Authentication.Authcookiename;
        options.Cookie.HttpOnly = false;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });
}
catch (Exception exception) { Console.WriteLine(exception.Message); }







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Mihcelle.Hwavmvid.Client._Imports).Assembly);

app.Run();
