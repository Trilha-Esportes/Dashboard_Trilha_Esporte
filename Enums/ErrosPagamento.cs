using System.ComponentModel;

// Class responsável por Padronizar os erros retornados pelo DTO do Resumo Financeiro


namespace DashboardTrilhaEsporte.Enums
{
    public enum ErrosPagamento
    {


        [Description("Sem Erros")]
        PagamentoCorreto,

        [Description("Erro na Devolução")]
        ErroDevolucao,


        [Description("Contem erro(s) no pagamento")]
        ErroNoPagamento,

    }

}
