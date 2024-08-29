using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Globalization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tattoo.InkVibesTattoo;
using Tattoo.InkVibesTattoo.Helpers;
using Tattoo.InkVibesTattoo.Services;
using BlazorStrap;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var ImagesBaseUrl = builder.Configuration.GetSection("ImagesBaseUrl").Get<string>();
if (ImagesBaseUrl != null)
{
    builder.Services.AddSingleton(ImagesBaseUrl);
}
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<ITattooService, TattooService>();
builder.Services.AddScoped<IConfigValueService, ConfigValueService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>(); // Asegúrate de que esta línea esté habilitada
builder.Services.AddSingleton<TooltipService>(); // Cambiar a Scoped
builder.Services.AddSingleton<DialogHelper>(); // Cambiar a Scoped
builder.Services.AddSingleton<GlobalState>(); // Cambiar a Scoped
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<LocalizationService>();
builder.Services.AddApiCallerClients();
builder.Services.AddBlazorStrap();


// Add localization
builder.Services.AddLocalization();
//builder.Services.AddRadzenComponents();
var host = builder.Build();
//await host.SetDefaultCulture();
await host.RunAsync();
