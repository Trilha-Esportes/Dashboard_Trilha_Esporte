using MudBlazor.Services;
using Blazored;
using DashboardTrilhaEsporte.Components;
using DashboardTrilhaEsporte.Data;
using DashboardTrilhaEsporte.Application;
using DashboardTrilhaEsporte.Domain.Service;
using System.Globalization;


DotNetEnv.Env.Load();

var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Montar a connection string a partir das variáveis de ambiente
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");

var connectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={dbName}";

// Add MudBlazor services
builder.Services.AddMudServices();



builder.Services.AddSingleton<DBContext>(new DBContext(connectionString));
builder.Services.AddScoped<SkuMarketplaceRepository>();
builder.Services.AddScoped<VendasRepository>();
builder.Services.AddScoped<SkuMarketplaceManager>();
builder.Services.AddScoped<AnymarketManager>();
builder.Services.AddScoped<ResumoFinanceiroManager>();
builder.Services.AddScoped<FiltroStateService>();



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
