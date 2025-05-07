using System.ComponentModel;

namespace DashboardTrilhasEsporte.Enums
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
