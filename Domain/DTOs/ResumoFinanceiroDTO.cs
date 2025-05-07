using DashboardTrilhasEsporte.Enums;
using DashboardTrilhasEsporte.Domain.Entities;


namespace DashboardTrilhasEsporte.Domain.DTOs
{

    public class ResumoFinanceiroDTO
    {
        public String? marketplace { get; set; }
        public String? codigoPedido { get; set; }
        public DateTime? dataPedido { get; set; }
        public Decimal valorTotalProdutos { get; set; }
        public Decimal? comissaoEsperada { get; set; }
        public Decimal valorRecebido { get; set; }
        public Decimal valorAReceber { get; set; }
        public Decimal valorDescontado { get; set; }
        public Decimal descontoFrete { get; set; }

        public StatusPagamento situacaoPagamento { get; set; }

        public StatusPagamento situacaoFinal { get; set; }


        public static List<ResumoFinanceiroDTO> MontarAnymarketDTO(List<SkuMarketplace> skuMarketplaces, List<Vendas> vendas)
        {

            var vendaDict = vendas.Distinct().ToDictionary(v => v.skuMarketplaceId, v => v.valorVenda);

            var agrupados = skuMarketplaces.Distinct()
             .GroupBy(x => new { x.numeroPedido, x.tipoEventoNormalizado })
             .Select(grupo => CriarResumoFinanceiro(grupo, vendaDict))
             .ToList();

            return agrupados;
        }

        private static ResumoFinanceiroDTO CriarResumoFinanceiro(IGrouping<object, SkuMarketplace> grupo, Dictionary<int, decimal> vendaDict)
        {

            // Extrair Informações Gerais do Grupo 
            var primeiro = grupo.First();
            var skuId = primeiro.skuMarketplaceId;


            var dataPedido = grupo.Min(g => g.dataEvento);
            var valorTotal = vendaDict.ContainsKey(skuId) ? vendaDict[skuId] : 0m;
            var comissaoEsperada = grupo.Select(g => g.comissao).Max();
            var valorAReceber = valorTotal - comissaoEsperada;

            //Calculo dos Parametros 
            var valorRecebido = CalcularValorRecebido(grupo);
            var situacaoPagamento = CalcularSituacaoPagamento(valorRecebido, valorAReceber);
            var valorDescontado = CalcularValorDescontado(grupo);
            var descontoFrete = CalcularDescontoFrete(grupo);
            var erroDevolucao = VerificarErroDevolucao(grupo, valorTotal);
            var situacaoFinal = CalcularSituacaoFinal(situacaoPagamento, valorRecebido - valorAReceber, erroDevolucao);

            return new ResumoFinanceiroDTO
            {
                marketplace = primeiro.marketplace,
                codigoPedido = primeiro.numeroPedido,
                dataPedido = dataPedido,
                valorTotalProdutos = valorTotal,
                comissaoEsperada = comissaoEsperada,
                valorAReceber = valorAReceber,
                valorRecebido = valorRecebido,
                valorDescontado = valorDescontado,
                descontoFrete = descontoFrete,
                situacaoPagamento = situacaoPagamento,
                situacaoFinal = situacaoFinal
            };
        }

        private static decimal CalcularValorRecebido(IEnumerable<SkuMarketplace> grupo)
        {
            return grupo
                .Where(g => g.tipoEventoNormalizado == Eventos.RepasseNormal)
                .Select(g => g.valorFinal)
                .DefaultIfEmpty(0)
                .Max();
        }

        private static StatusPagamento CalcularSituacaoPagamento(decimal valorRecebido, decimal valorAReceber)
        {
            var diferenca = valorRecebido - valorAReceber;
            if (Math.Abs(diferenca) < 0.05m) return StatusPagamento.Pago;
            if (diferenca > 0) return StatusPagamento.PagoAMais;
            if (valorRecebido > 0) return StatusPagamento.PagoAMenos;
            return StatusPagamento.NaoPago;
        }

        private static decimal CalcularValorDescontado(IEnumerable<SkuMarketplace> grupo)
        {
            var valorHove = grupo
                .Where(g => g.tipoEventoNormalizado == Eventos.DescontarHoveHouve)
                .Select(g => g.valorFinal)
                .DefaultIfEmpty(0)
                .Max();

            var valorRetro = grupo
                .Where(g => g.tipoEventoNormalizado == Eventos.DescontarRetroativo)
                .Sum(g => g.valorFinal);

            return valorHove + valorRetro;
        }

        private static decimal CalcularDescontoFrete(IEnumerable<SkuMarketplace> grupo)
        {
            return grupo
                .Where(g => g.tipoEventoNormalizado == Eventos.DescontarReversaCentauroEnvios)
                .Sum(g => g.valorFinal);
        }

        private static bool VerificarErroDevolucao(IEnumerable<SkuMarketplace> grupo, decimal valorTotal)
        {
            var valorHove = grupo
                .Where(g => g.tipoEventoNormalizado == Eventos.DescontarHoveHouve)
                .Select(g => g.valorFinal)
                .DefaultIfEmpty(0)
                .Max();

            return Math.Abs(valorHove) > 0 && Math.Abs(valorHove) != Math.Abs(valorTotal);
        }

        private static StatusPagamento CalcularSituacaoFinal(StatusPagamento situacaoPagamento, decimal diferenca, bool erroDevolucao)
        {
            if (Math.Abs(diferenca) < 0.01m && !erroDevolucao)
                return StatusPagamento.Correto;
            if (erroDevolucao)
                return StatusPagamento.ErroDevolucao;
            return situacaoPagamento;
        }
    }

}