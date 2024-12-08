using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Mihcelle.Hwavmvid;
using Mihcelle.Hwavmvid.Alerts;
using Mihcelle.Hwavmvid.Client;
using Mihcelle.Hwavmvid.Cookies;
using Mihcelle.Hwavmvid.Fileupload;
using Mihcelle.Hwavmvid.Modal;
using Mihcelle.Hwavmvid.Notifications;
using Mihcelle.Hwavmvid.Pager;
using Mihcelle.Hwavmvid.Shared.Models;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

// mihcelle.hwavmvid
builder.Services.AddScoped<Applicationprovider, Applicationprovider>();
builder.Services.AddScoped<AlertsService, AlertsService>();

// mihcelle.hwavmvid
builder.Services.AddScoped<Modalservice, Modalservice>();
builder.Services.AddScoped<Fileuploadservice, Fileuploadservice>();
builder.Services.AddScoped<Pagerservice<Applicationpage>, Pagerservice<Applicationpage>>();
builder.Services.AddScoped<Pagerservice<Applicationtask>, Pagerservice<Applicationtask>>();
builder.Services.AddScoped<Pagerservice<Applicationmediafile>, Pagerservice<Applicationmediafile>>();
builder.Services.AddScoped<Applicationmodulesettingsservice, Applicationmodulesettingsservice>();
builder.Services.AddScoped<NotificationsService, NotificationsService>();

// mihcelle.hwavmvid
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// mihcelle.hwavmvid
builder.Services.AddScoped<AuthenticationStateProvider, Applicationauthenticationstateprovider>();

// mihcelle.hwavmvid
builder.Services.AddHttpClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated",
    client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api"); });

// mihcelle.hwavmvid
var configclient = new HttpClient() { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddScoped(serviceprovider => configclient);
using var response = await configclient.GetAsync("tobaccoindustries.json");
using var stream = await response.Content.ReadAsStreamAsync();
builder.Configuration.AddJsonStream(stream);



await builder.Build().RunAsync();
