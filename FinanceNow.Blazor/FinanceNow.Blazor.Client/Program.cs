using FinanceNow.Blazor.Client.Modelos;
using FinanceNow.Blazor.Client.Response;
using FinanceNow.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7143/Api/") });
builder.Services.AddScoped<Transacao>();
builder.Services.AddScoped<ApiResponse<Transacao>>();
builder.Services.AddScoped<ApiService>();


await builder.Build().RunAsync();
