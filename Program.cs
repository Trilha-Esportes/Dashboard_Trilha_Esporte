using MudBlazor.Services;
using Blazored;
using DashboardTrilhasEsporte.Components;
using DashboardTrilhasEsporte.Data;
using DashboardTrilhasEsporte.Application;
using DashboardTrilhasEsporte.Domain.Entities;
var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();


string? conexao = builder.Configuration.GetConnectionString("DefaultConnection");

// Usa string vazia apenas se 'conexao' for nula ou em branco
string conexaoValida = string.IsNullOrWhiteSpace(conexao) ? "" : conexao;

builder.Services.AddSingleton<DBContext>(new DBContext(conexaoValida));

builder.Services.AddScoped<SkuMarketplaceRepository>();

builder.Services.AddScoped<VendasRepository>();



// Registra o Manager, também com escopo
builder.Services.AddScoped<SkuMarketplaceManager>();
builder.Services.AddScoped<AnymarketManager>();
builder.Services.AddScoped<ResumoFinanceiroManager>();



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
