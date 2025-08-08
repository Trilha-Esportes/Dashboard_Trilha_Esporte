using System.ComponentModel;
using MudBlazor.Extensions;

namespace DashboardTrilhaEsporte.Enums
{
    public enum MotivoDevolucao
    {
        [Description("Produto Errado")]
        ProdutoErrado = 0,

        [Description("Arrependimento")]
        Arrependimento = 1,

        [Description("Desistência")]
        Desistencia = 2,

        [Description("Recusa")]
        Recusa = 3,

        [Description("Cancelamento")]
        Cancelamento = 4,

        [Description("Defeito")]
        Defeito = 5,

        [Description("Fraude")]
        Fraude = 6,


        [Description("Observação")]
        Observacao = 8,

        [Description("Ao Remetente")]
        AoRemetente = 9,

        [Description("Não Informado")]
        NaoInformado = 10,

        [Description("Não apto (ML)")]
        NaoAptoML = 11,

        [Description("Recusada a Devolução")]
        RecusadaADevolucao = 12,

        [Description("Não Serviu")]
        NaoServiu = 13,

        [Description("Desconhecido")]
        Desconhecido =999
}
}