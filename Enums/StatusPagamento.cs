using System.ComponentModel;

// Class responsável por Padronizar os erros retornados pelo DTO do Resumo Financeiro


namespace DashboardTrilhaEsporte.Enums
{
    public enum StatusPagamento
    {
        [Description("Pago corretamente")]
        Pago,

        [Description("Pago a mais")]
        PagoAMais,

        [Description("Pago a menos")]
        PagoAMenos,

        [Description("Não pago")]
        NaoPago,



    }

}
