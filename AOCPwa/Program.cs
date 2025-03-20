using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AOCPwa;
using MudBlazor.Services;
using AshesMapBib.Models;
using AOCPwa.MapHelper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<MapHandler>();

builder.Services.AddMudServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorPWA", policy =>
    {
        policy.WithOrigins("") // PWA-URL hier eintragen
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Falls Cookies oder Auth genutzt werden
    });
});
builder.Services.AddScoped<ResourceApiClient<Node>>(sp =>
    new ResourceApiClient<Node>(
        sp.GetRequiredService<HttpClient>(),
        sp.GetRequiredService<ILogger<ResourceApiClient<Node>>>() // Logger hinzufügen
    )
);

builder.Services.AddScoped<ResourceApiClient<NodePosition>>(sp =>
    new ResourceApiClient<NodePosition>(
        sp.GetRequiredService<HttpClient>(),
        sp.GetRequiredService<ILogger<ResourceApiClient<NodePosition>>>() // Logger hinzufügen
    )
);




await builder.Build().RunAsync();
