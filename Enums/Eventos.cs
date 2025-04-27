using System.ComponentModel;

namespace DashboardTrilhasEsporte.Enums
{
    public enum Eventos
    {
        [Description("Repasse Normal")]
        RepasseNormal = 0,

        [Description("Descontar Houve")]
        DescontarHoveHouve = 1,

        [Description("Descontar Reversa Centauro Envios")]
        DescontarReversaCentauroEnvios = 2,

        [Description("Ajuste de Ciclo")]
        AjusteDeCiclo = 3,

        [Description("Descontar Retroativo")]
        DescontarRetroativo = 4,

        [Description("Evento nao Reconhecido")]
        Outros = 5,

        [Description("Não informado")]
        Desconhecido = 5

    }



}
