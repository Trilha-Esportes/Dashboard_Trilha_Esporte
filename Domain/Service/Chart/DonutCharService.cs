
using Microsoft.AspNetCore.Components;

namespace DashboardTrilhaEsporte.Domain.Service{
    public class DonutCharService : ComponentBase
    {

        [Parameter] public double[] Data { get; set; } = Array.Empty<double>();
        [Parameter] public string[] Labels { get; set; } = Array.Empty<string>();
        [Parameter] public string Width { get; set; } = "300px";
        [Parameter] public string Height { get; set; } = "300px";
        [Parameter] public string Title { get; set; } = "Total";

    }
}