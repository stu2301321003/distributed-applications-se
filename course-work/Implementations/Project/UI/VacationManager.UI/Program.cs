using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VacationManager.UI;
using VacationManager.UI.Api.Models;
using VacationManager.UI.Api.Services;
using VacationManager.UI.Api.Services.Abstractions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var apiConfig = builder.Configuration
    .GetSection("Api")
    .Get<ApiConfig>();

builder.Services.AddSingleton(apiConfig);
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();

await builder.Build().RunAsync();
