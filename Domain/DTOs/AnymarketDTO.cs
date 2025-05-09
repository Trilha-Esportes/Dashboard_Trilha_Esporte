
using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.Entities;

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

        public static List<AnymarketDTO> MontarAnymarketDTO(List<SkuMarketplaceDTO> skuMarketplaces, List<Vendas> vendas)
        {

            vendas = vendas.Distinct().ToList();
    
            var vendaDict = vendas
    .GroupBy(v => v.skuMarketplaceId)
    .ToDictionary(g => g.Key, g => g.First().valorVenda);


            var agrupados = skuMarketplaces.Distinct()
             .GroupBy(x => new { x.skuMarketplace.numeroPedido, x.skuMarketplace.tipoEventoNormalizado })
             .Select(grupo => CriarAnymarketDTO(grupo, vendaDict))
             .ToList();

            return agrupados;
        }

        private static AnymarketDTO CriarAnymarketDTO(IGrouping<dynamic, SkuMarketplaceDTO> grupo, Dictionary<string, decimal> vendaDict)
        {
            var primeiro = grupo.First();
            var skuId = primeiro.skuMarketplace.skuMarketplaceId;
            var eventoNormalizado = primeiro.skuMarketplace.tipoEventoNormalizado;
            var valorVendaAtual = vendaDict.ContainsKey(skuId) ? vendaDict[skuId] : 0m;
            var valorLiquido = grupo.Select(g => g.skuMarketplace.valorLiquido).DefaultIfEmpty(0m).Max();

            var erros = ChecarErrosAnymarket(valorVendaAtual, valorLiquido, eventoNormalizado);

            return new AnymarketDTO
            {
                numeroPedido = primeiro.skuMarketplace.numeroPedido,
                valorSkumarketplace = primeiro.skuMarketplace.valorLiquido,
                valorVenda= valorVendaAtual,
                tipoEventoNormalizado = eventoNormalizado,
                Erros = erros,
                skuId = skuId
            };
        }

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
    }
}
