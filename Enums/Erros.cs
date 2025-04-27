using System.ComponentModel;

namespace DashboardTrilhasEsporte.Enums
{
    public enum Erros
    {
        [Description("Erro de Comisao")]
        ErroComissao = 0,

        [Description("Valor Final Negativo")]
        ValorFinalNegativo = 1,

        [Description("Falta de Comissão")]
        FaltaDeComisao = 2,

        [Description("Falta de Data de Comissão")]
        FaltaDataComissao = 3,

        [Description("Erro de Devolução")]
        ErroDevolucao = 4,

        [Description("ERRO VENDA NAO ENCONTRADA")]
        VendaNaoEncontrada = 5,

        [Description("Erro Valores Divergentes")]
        ValoresDivergentes = 6,

    }



}
