using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DashboardTrilhasEsporte.Domain.Service;

public class BarChartService : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Gráfico de Barras";
    [Parameter] public string Width { get; set; } = "100%";
    [Parameter] public string Height { get; set; } = "400px";

    [Parameter] public string[] Labels { get; set; } = Array.Empty<string>();
    [Parameter] public List<ChartSeries> Series { get; set; } = new();

    protected ChartOptions _options = new()
    {
        XAxisLines = true,
        YAxisLines = false,
    };
}
