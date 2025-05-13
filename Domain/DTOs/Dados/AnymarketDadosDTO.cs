using DashboardTrilhaEsporte.Enums;

// Essa classe amazena as informaçoes estatisticas de lista de AnymarketDTO
// Essa informação é usada para construir o dashboard

namespace DashboardTrilhaEsporte.Domain.DTOs{

    public class AnymarketDadosDTO{

        public List<AnymarketDTO>? anymarketDTOs {get; set;}
        public int totalVendasNaoEncontada {get; set;}
        public int totalVendasValorDivergente {get; set;}

        public AnymarketDadosDTO (){
          
        }
        public AnymarketDadosDTO (List<AnymarketDTO> anymarketDTOs){
            this.anymarketDTOs = anymarketDTOs;
            CalcularEstatiticas();
        }

        // Calcula numero de vendas não encontradas e divergentes
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

