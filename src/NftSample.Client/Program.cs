using NftSample.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var services = builder.Services;

services.AddSingleton(sp => new HttpClient { BaseAddress = new (builder.HostEnvironment.BaseAddress) });
services.AddMudServices();
services.AddAuthServices();
services.AddApiService(builder.HostEnvironment.BaseAddress);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.Build().RunAsync();
