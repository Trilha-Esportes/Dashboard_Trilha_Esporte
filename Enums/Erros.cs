using System.ComponentModel;

// Class responsável por Padronizar os erros retornados pelo DTO do skuMarkerplace


namespace DashboardTrilhaEsporte.Enums
{
    public enum Erros
    {
        [Description("Erro no valor da Comissão")]
        ErroComissao = 0,

        [Description("Valor Final Negativo")]
        ValorFinalNegativo = 1,

        [Description("Falta o valor na Comisao")]
        FaltaDeComisao = 2,

        [Description("Falta  a Data da Comissão")]
        FaltaDataComissao = 3,

        [Description("Erro de Devolução")]
        ErroDevolucao = 4,

       

    }



}
