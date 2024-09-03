
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Tattoo.Management;
using Tattoo.Management.Helpers;
using Tattoo.Management.Services;

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
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IConfigValueService, ConfigValueService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>(); // Asegúrate de que esta línea esté habilitada
builder.Services.AddSingleton<TooltipService>();
builder.Services.AddSingleton<DialogHelper>();
builder.Services.AddSingleton<GlobalState>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddApiCallerClients();
//builder.Services.AddRadzenComponents();
await builder.Build().RunAsync();
