using DashboardTrilhasEsporte.Enums;

namespace DashboardTrilhasEsporte.Domain.DTOs{

    public class AnymarketDadosDTO{

        public List<AnymarketDTO> anymarketDTOs {get; set;}
        public int totalVendasNaoEncontada {get; set;}
        public int totalVendasValorDivergente {get; set;}

        public AnymarketDadosDTO (List<AnymarketDTO> anymarketDTOs){
            this.anymarketDTOs = anymarketDTOs;
        }

        public void CalcularEstatiticas()
        {
            this.anymarketDTOs.ForEach(vendas =>
            {
                if (vendas.Erros == AnymarketErros.ErroVendaNaoEncontrada)
                {
                    this.totalVendasNaoEncontada++;
                }
                else if (vendas.Erros == AnymarketErros.ErroValoresDivergentes)
                {
                    this.totalVendasValorDivergente++;
                }
            });
        }

        
    }

}

