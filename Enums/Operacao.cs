using System.ComponentModel;

namespace DashboardTrilhaEsporte.Enums
{
    public enum Operacao
    {
        [Description("Indenização")]
        Indenizacao = 0,

        [Description("Devolução Parcial")]
        DevolucaoParcial = 1,

        [Description("Recusa")]
        Recusa = 2,

        [Description("Devolução")]
        Devolucao = 3,

        [Description("Cancelamento")]
        Cancelamento = 4,

        [Description("Vale Compras")]
        ValeCompras = 5,

        [Description("Reintegração")]
        Reintegracao = 6,

        [Description("Defeito")]
        Defeito = 7,

        [Description("Congelado")]
        Congelado = 8,

        [Description("Devolução/Troca")]
        DevolucaoTroca = 9,

        [Description("Troca")]
        Troca = 10,

        [Description("Desconhecido")]
        Desconhecido =999
    }
}