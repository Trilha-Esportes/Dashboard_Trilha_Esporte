using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.Entities;


namespace DashboardTrilhaEsporte.Domain.DTOs
{

    public class ResumoFinanceiroDTO
    {
        public String? marketplace { get; set; }
        public String skuId { get; set; }

        public String codigoPedido { get; set; }
        public DateTime? dataPedido { get; set; }
        public Decimal valorTotalProdutos { get; set; }
        public Decimal? comissaoEsperada { get; set; }
        public Decimal valorRecebido { get; set; }
        public Decimal valorAReceber { get; set; }
        public Decimal valorDescontado { get; set; }
        public Decimal descontoFrete { get; set; }

        public StatusPagamento situacaoPagamento { get; set; }

        public StatusPagamento situacaoFinal { get; set; }


        public static List<ResumoFinanceiroDTO> MontarAnymarketDTO(List<SkuMarketplaceDTO> skuMarketplaces, List<Vendas> vendas)
        {

            vendas = vendas.ToList();

            var vendaDict = vendas
     .GroupBy(v => v.skuMarketplaceId)
     .ToDictionary(g => g.Key, g => g.First().valorVenda);




            var agrupados = skuMarketplaces
        .GroupBy(x => new { x.skuMarketplace.numeroPedido, x.skuMarketplace.marketplace })
        .Select(grupo => CriarResumoFinanceiro(grupo, vendaDict))
        .Where(resumo => resumo.valorRecebido != 0m)
        .ToList();


            return agrupados;
        }

        private static ResumoFinanceiroDTO CriarResumoFinanceiro(IGrouping<object, SkuMarketplaceDTO> grupo, Dictionary<string, decimal> vendaDict)
        {

            // Extrair Informações Gerais do Grupo 
            var primeiro = grupo.First();
            var skuId = primeiro.skuMarketplace.skuMarketplaceId;


            var dataPedido = grupo.Min(g => g.skuMarketplace.dataEvento);

            var valorTotal = vendaDict.ContainsKey(skuId) ? vendaDict[skuId] : 0m;

            var comissaoEsperada = grupo.Select(g => g.skuMarketplace.comissao).Max();

            var AReceber = valorTotal - comissaoEsperada;

            //Calculo dos Parametros 
            var Recebido = CalcularValorRecebido(grupo);

            var situacaoPagamento = CalcularSituacaoPagamento(Recebido, AReceber);
            var valorDescontado = CalcularValorDescontado(grupo);
            var descontoFrete = CalcularDescontoFrete(grupo);
            var erroDevolucao = VerificarErroDevolucao(grupo, valorTotal);
            var situacaoFinal = CalcularSituacaoFinal(situacaoPagamento, Recebido - AReceber, erroDevolucao);


            return new ResumoFinanceiroDTO
            {
                skuId = primeiro.skuMarketplace.skuMarketplaceId,
                marketplace = primeiro.skuMarketplace.marketplace,
                codigoPedido = primeiro.skuMarketplace.numeroPedido,
                dataPedido = dataPedido,
                valorTotalProdutos = valorTotal,
                comissaoEsperada = comissaoEsperada,
                valorAReceber = AReceber,
                valorRecebido = Recebido,
                valorDescontado = valorDescontado,
                descontoFrete = descontoFrete,
                situacaoPagamento = situacaoPagamento,
                situacaoFinal = situacaoFinal
            };
        }

        private static decimal CalcularValorRecebido(IEnumerable<SkuMarketplaceDTO> grupo)
        {


            decimal v = grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.RepasseNormal)
                .Select(g => g.skuMarketplace.valorFinal)
                .DefaultIfEmpty()
                .Max();

            return v;
        }


        private static StatusPagamento CalcularSituacaoPagamento(decimal valorRecebido, decimal valorAReceber)
        {
            var diferenca = valorRecebido - valorAReceber;
            if (Math.Abs(diferenca) < 0.05m) return StatusPagamento.Pago;
            if (diferenca > 0) return StatusPagamento.PagoAMais;
            if (valorRecebido > 0) return StatusPagamento.PagoAMenos;
            return StatusPagamento.NaoPago;
        }

        private static decimal CalcularValorDescontado(IEnumerable<SkuMarketplaceDTO> grupo)
        {
            var valorHove = grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.DescontarHoveHouve)
                .Select(g => g.skuMarketplace.valorFinal)
                .DefaultIfEmpty(0)
                .Max();

            var valorRetro = grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.DescontarRetroativo)
                .Sum(g => g.skuMarketplace.valorFinal);

            return valorHove + valorRetro;
        }

        private static decimal CalcularDescontoFrete(IEnumerable<SkuMarketplaceDTO> grupo)
        {
            return grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.DescontarReversaCentauroEnvios)
                .Sum(g => g.skuMarketplace.valorFinal);
        }

        private static bool VerificarErroDevolucao(IEnumerable<SkuMarketplaceDTO> grupo, decimal valorTotal)
        {
            var valorHove = grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.DescontarHoveHouve)
                .Select(g => g.skuMarketplace.valorFinal)
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