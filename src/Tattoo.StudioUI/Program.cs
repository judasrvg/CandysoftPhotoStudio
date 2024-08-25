using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Tattoo.StudioUI;
using Tattoo.StudioUI.Helpers;
using Tattoo.StudioUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

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
builder.Services.AddApiCallerClients();
//builder.Services.AddRadzenComponents();
await builder.Build().RunAsync();
