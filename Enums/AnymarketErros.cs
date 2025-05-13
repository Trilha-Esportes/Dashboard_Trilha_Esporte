using System.ComponentModel;

// Class responsável por Padronizar os erros retornados pelo DTO do Anymarket

namespace DashboardTrilhaEsporte.Enums
{
    public enum AnymarketErros
    {
        [Description("Sem erros")]
        SemErros= 0,

        [Description("Venda Não Encontrada")]
        ErroVendaNaoEncontrada = 1,

        [Description("Valores Divergentes")]
        ErroValoresDivergentes = 2,

    }



}
