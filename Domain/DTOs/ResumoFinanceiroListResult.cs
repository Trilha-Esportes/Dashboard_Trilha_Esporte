/*
   st.markdown("### Totais")
            colS1, colS2, colS3 = st.columns(3)
            colS1.metric("Total Valor a Receber", f"{total_valor_a_receber:,.2f}")
            colS2.metric("Total Valor Recebido", f"{total_valor_recebido:,.2f}")
            colS3.metric("Diferença", f"{diferenca:,.2f}")
*/

namespace DashboardTrilhasEsporte.Domain
{
    public class ResumoFinanceiroListResultDTO
    {
        public List<ResumoFinanceiroDTO> resumoFinanceiroDTOs { get; set; }

        public Decimal valorTotalReceber { get; set; }

        public Decimal valorTotalRecebido { get; set; }

        public Decimal valorTotalDiferenca { get; set; }

        public ResumoFinanceiroListResultDTO(List<ResumoFinanceiroDTO> resumoFinanceiroDTOs)
        {
            this.resumoFinanceiroDTOs = resumoFinanceiroDTOs;
            CalcularEstatiticas();
        }

        public void CalcularEstatiticas()
        {
            this.valorTotalReceber = resumoFinanceiroDTOs.Sum(r => r.valorAReceber);
            this.valorTotalRecebido = resumoFinanceiroDTOs.Sum(r => r.valorRecebido);
            this.valorTotalDiferenca = valorTotalRecebido - valorTotalReceber;
        }




    }
}