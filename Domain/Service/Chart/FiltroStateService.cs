namespace DashboardTrilhaEsporte.Domain.Service{

public class FiltroStateService
{
    public event Action? OnFiltroToggled;
    private bool _mostrarFiltro;

    public bool MostrarFiltro => _mostrarFiltro;

    public void ToggleFiltro()
    {
        _mostrarFiltro = !_mostrarFiltro;
        OnFiltroToggled?.Invoke();

    }
}

}
