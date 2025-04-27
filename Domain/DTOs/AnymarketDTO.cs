/*
"numero_pedido": "Número do Pedido",
            "valor_liquido": "Valor (sku_marketplace/vendasDF)",
            "valor_vendas": "Valor (vendas)",
            "tipo_evento_normalizado": "Tipo de Evento",
            "erros_anymarket": "Erros Anymarket"
*/

using DashboardTrilhasEsporte.Enums;

namespace DashboardTrilhasEsporte.Domain
{
    public class AnymarketDTO
    {
        public String numeroPedido { get; set; }
        public Decimal valorSkumarketplace { get; set; }
        public Eventos tipoEventoNormalizado { get; set; }
        public AnymarketErros Erros { get; set; }

        public static List<AnymarketDTO> MontarResumo(List<SkuMarketplace> geral, List<Vendas> vendas)
        {
            var vendaDict = vendas.ToDictionary(v => v.skuMarketplaceId, v => v.valorVenda);

            var agrupados = geral
             .GroupBy(x => new { x.numeroPedido, x.tipoEventoNormalizado }) // Agrupamento único
             .Select(grupo => CriarAnymarketDTO(grupo, vendaDict))
             .ToList();

            return agrupados;
        }

        private static AnymarketDTO CriarAnymarketDTO(IGrouping<dynamic, SkuMarketplace> grupo, Dictionary<int, decimal> vendaDict)
        {
            var primeiro = grupo.First();
            var skuId = primeiro.skuMarketplaceId;
            var eventoNormalizado = primeiro.tipoEventoNormalizado;
            var valorVenda = vendaDict.ContainsKey(skuId) ? vendaDict[skuId] : 0m;
            var valorLiquido = grupo.Select(g => g.valorLiquido).DefaultIfEmpty(0m).Max();

            var erros = ChecarErrosAnymarket(valorVenda, valorLiquido, eventoNormalizado);

            return new AnymarketDTO
            {
                numeroPedido = primeiro.numeroPedido,
                valorSkumarketplace = valorVenda,
                tipoEventoNormalizado = eventoNormalizado,
                Erros = erros
            };
        }

        public static AnymarketErros ChecarErrosAnymarket(decimal valorVenda, decimal valorLiquido, Eventos tipoEventoNormalizado)
        {
            if (valorVenda == 0m)
            {
                return AnymarketErros.erroVendaNaoEncontrada;
            }

            if (tipoEventoNormalizado == Eventos.RepasseNormal && valorLiquido != valorVenda)
            {
                return AnymarketErros.ErroValoresDivergentes;
            }

            return AnymarketErros.SemErros;
        }
    }
}
