using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZasNetWebClient;
using ZasNetWebClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7203") });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();
