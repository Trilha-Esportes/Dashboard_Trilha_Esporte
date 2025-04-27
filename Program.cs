using MudBlazor.Services;
using Blazored;
using DashboardTrilhasEsporte.Components;
using DashboardTrilhasEsporte.Data;
using DashboardTrilhasEsporte.Domain;
var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();



// Registra serviço com variaveis de ambiente (Conexão com o banco de dados )
builder.Services.AddSingleton<DBContext>(new DBContext(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<SkuMarketplaceRepository>();


// Registra o Manager, também com escopo
builder.Services.AddScoped<SkuMarketplaceManager>();

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
