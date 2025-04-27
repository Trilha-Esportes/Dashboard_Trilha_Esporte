using System.ComponentModel;

namespace DashboardTrilhasEsporte.Enums
{
    public enum StatusPagamento
    {
        [Description("Pago corretamente")]
        Pago,

        [Description("Pago a maior")]
        PagoAMais,

        [Description("Pago a menor")]
        PagoAMenos,

        [Description("Não pago")]
        NaoPago,


        [Description("Pagamento Correto")]
        Correto,

        [Description("Erro de Devolução")]
        ErroDevolucao


    }

}
