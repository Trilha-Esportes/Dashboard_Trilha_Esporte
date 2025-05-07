

namespace DashboardTrilhasEsporte.Domain.DTOs
{
    public class ResumoFinanceiroDadosDTO
    {
        public List<ResumoFinanceiroDTO> resumoFinanceiroDTOs { get; set; }

        public Decimal valorTotalReceber { get; set; }

        public Decimal valorTotalRecebido { get; set; }

        public Decimal valorTotalDiferenca { get; set; }

        public ResumoFinanceiroDadosDTO(List<ResumoFinanceiroDTO> resumoFinanceiroDTOs)
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