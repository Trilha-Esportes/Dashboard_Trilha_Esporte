

// Essa classe amazena as informaçoes estatisticas de lista de ResumoFinanceiroDTO
// Essa informação é usada para construir o dashboard

using DashboardTrilhaEsporte.Enums;

namespace DashboardTrilhaEsporte.Domain.DTOs
{
    public class ResumoFinanceiroDadosDTO
    {
        public List<ResumoFinanceiroDTO> resumoFinanceiroDTOs { get; set; }

        public Decimal valorTotalReceber { get; set; }

        public Decimal valorTotalRecebido { get; set; }

        public Decimal valorTotalDiferenca { get; set; }

        public ResumoFinanceiroDadosDTO()
        {
            resumoFinanceiroDTOs = new List<ResumoFinanceiroDTO>();
            valorTotalReceber = 0;
            valorTotalRecebido = 0;
            valorTotalDiferenca = 0;
        }

        public ResumoFinanceiroDadosDTO(List<ResumoFinanceiroDTO> resumoFinanceiroDTOs)
        {
            this.resumoFinanceiroDTOs = resumoFinanceiroDTOs;
            CalcularEstatiticas();
        }

        // Somatorio dos valores a receber e recebidos
        public void CalcularEstatiticas()
        {
            this.valorTotalReceber = resumoFinanceiroDTOs.Sum(r => r.valorAReceber);
            this.valorTotalRecebido = resumoFinanceiroDTOs.Sum(r => r.valorRecebido);
            this.valorTotalDiferenca = valorTotalRecebido - valorTotalReceber;
        }

        // Método responsável por obter a distribuição dos Tipos de Pagamento
    
        // Essa informação é para cosntrir graficos
        public Dictionary<string, double> ObterDistribuicaoPagamento()
        {
            var tipoPagamentos = new List<StatusPagamento>();

            foreach (var status in Enum.GetValues<StatusPagamento>()) {
                tipoPagamentos.Add(status);
            }


            var resultado = new Dictionary<string, double>();

            foreach (var status in tipoPagamentos)
            {
                if (resumoFinanceiroDTOs != null && resumoFinanceiroDTOs.Any())
                {
                    var count = resumoFinanceiroDTOs.Count(r => r.situacaoPagamento == status);
                    resultado.Add(status.GetDescription(), count);
                }
                else
                {
                    resultado.Add(status.GetDescription(), 0);
                }
            }
          

            return resultado;
        }



  

        public Dictionary<string, double> ObterDistribuicaoErrosPagamento()
        {
            var tipoPagamentos = new List<ErrosPagamento>();

            foreach (var erro in Enum.GetValues<ErrosPagamento>()) {
                tipoPagamentos.Add(erro);
            }


            var resultado = new Dictionary<string, double>();

            foreach (var erros in tipoPagamentos)
            {
                if (resumoFinanceiroDTOs != null && resumoFinanceiroDTOs.Any())
                {
                    var count = resumoFinanceiroDTOs.Count(r => r.situacaoFinal == erros);
                    resultado.Add(erros.GetDescription(), count);
                }
                else
                {
                    resultado.Add(erros.GetDescription(), 0);
                }
            }
          

            return resultado;
        }



    }
}