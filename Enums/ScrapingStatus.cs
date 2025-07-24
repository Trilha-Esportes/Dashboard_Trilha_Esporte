using System.ComponentModel;

namespace DashboardTrilhaEsporte.Enums
{
    public enum ScrapingStatus
    {
        [Description("Link Ativo")]
        Ativo = 0,

        [Description("Desativado")]
        DESATIVADO = 1,

        [Description("Sem Estoque")]
        SEMESTOQUE = 2,
        
        [Description("Disponível")]
        DISPONIVEL = 3,

    }
}