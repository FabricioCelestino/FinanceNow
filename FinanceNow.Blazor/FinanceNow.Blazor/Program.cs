using FinanceNow.Blazor.Client.Modelos;
using FinanceNow.Blazor.Client.Pages;
using FinanceNow.Blazor.Client.Response;
using FinanceNow.Blazor.Client.Services;
using FinanceNow.Blazor.Components;
using Microsoft.JSInterop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7143/Api/") });
builder.Services.AddScoped<Transacao>();
builder.Services.AddScoped<ApiResponse<Transacao>>();
builder.Services.AddScoped<ApiService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();


app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FinanceNow.Blazor.Client._Imports).Assembly);

app.Run();
