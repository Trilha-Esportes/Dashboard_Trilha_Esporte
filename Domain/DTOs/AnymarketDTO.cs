
using DashboardTrilhaEsporte.Enums;
using System.Globalization;

using DashboardTrilhaEsporte.Data.Entities;



// Essa classe é responsável por construir a estrutura representação (DTO) do AnymarketDTO
// É uma junção de dados que vem do SkuMarketplace e Vendas


namespace DashboardTrilhaEsporte.Domain.DTOs
{
    public class AnymarketDTO
    {
        public String? skuId { get; set; }
        public String? numeroPedido { get; set; }
        public Decimal valorSkumarketplace { get; set; }
        public Decimal valorVenda { get; set; }
        public Eventos tipoEventoNormalizado { get; set; }
        public AnymarketErros Erros { get; set; }


        // Método responsável por montar o Anymarket
        public static List<AnymarketDTO> MontarAnymarketDTO(List<SkuMarketplaceDTO> skuMarketplaces, List<Vendas> vendas)
        {

            // Remover duplicatas da lista de vendas
            vendas = vendas.Distinct().ToList();

            // Criar um dicionário para armazenar o valor de venda por skuMarketplaceId
            var vendaDict = vendas
            .GroupBy(v => v.skuMarketplaceId)
            .ToDictionary(g => g.Key, g => g.First().valorVenda);


            var agrupados = skuMarketplaces.Distinct()
             .GroupBy(x => new { x.skuMarketplace.numeroPedido, x.skuMarketplace.tipoEventoNormalizado })
             // Usa função auxiliar para criar o AnymarketDTO  
             .Select(grupo => CriarAnymarketDTO(grupo, vendaDict))
             .ToList();

            return agrupados;
        }

        // Método auxiliar para criar o AnymarketDTO
        private static AnymarketDTO CriarAnymarketDTO(IGrouping<dynamic, SkuMarketplaceDTO> grupo, Dictionary<string, decimal> vendaDict)
        {
            // Extrair Informações Gerais do Grupo
            var primeiro = grupo.First();
            var skuId = primeiro.skuMarketplace.skuMarketplaceId;
            var eventoNormalizado = primeiro.skuMarketplace.tipoEventoNormalizado;
            // Pega o primeiro valor de venda do dicionário, se não existir retorna 0
            var valorVendaAtual = vendaDict.ContainsKey(skuId) ? vendaDict[skuId] : 0m;

            // Pega o primeiro valor de venda do dicionário, se não existir retorna 0
            var valorLiquido = grupo.Select(g => g.skuMarketplace.valorLiquido).DefaultIfEmpty(0m).Max();

            var erros = ChecarErrosAnymarket(valorVendaAtual, valorLiquido, eventoNormalizado);

            return new AnymarketDTO
            {
                numeroPedido = primeiro.skuMarketplace.numeroPedido,
                valorSkumarketplace = primeiro.skuMarketplace.valorLiquido,
                valorVenda = valorVendaAtual,
                tipoEventoNormalizado = eventoNormalizado,
                Erros = erros,
                skuId = skuId
            };
        }

        // Método responsável por verificar os erros do Anymarket
        public static AnymarketErros ChecarErrosAnymarket(decimal valorVenda, decimal valorLiquido, Eventos tipoEventoNormalizado)
        {
            if (valorVenda == 0m)
            {
                return AnymarketErros.ErroVendaNaoEncontrada;
            }

            if (tipoEventoNormalizado == Eventos.RepasseNormal && valorLiquido != valorVenda)
            {
                return AnymarketErros.ErroValoresDivergentes;
            }

            return AnymarketErros.SemErros;
        }

        public override string ToString()
        {
            var culture = new CultureInfo("pt-BR");

            return $"{skuId};" +
                $"{numeroPedido};" +
                $"{valorSkumarketplace.ToString("N2", culture)};" +
                $"{valorVenda.ToString("N2", culture)};" +
                $"{tipoEventoNormalizado.GetDescription()};" +
                $"{Erros.GetDescription()};";
        }

    }
}
