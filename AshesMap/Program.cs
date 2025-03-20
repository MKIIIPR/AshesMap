using AshesMap;
using AshesMapBib.MapHandling;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddScoped<MapHandler>();
builder.Services.AddScoped<TileDownloader>();
builder.Services.AddHttpClient("PrimaryApi", client =>
{
    client.BaseAddress = new Uri("https://primaryapi.example.com/");
});
builder.Services.AddHttpClient("BackupApi", client =>
{
    client.BaseAddress = new Uri("https://backupapi.example.com/");
});
builder.Services.AddScoped<GenericMultiApiService>();

await builder.Build().RunAsync();
