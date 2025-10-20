using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Data.Entities;
using System.Text;
using System.Globalization;


// Essa classe é responsável por construir a estrutura representação (DTO) do ResumoFinanceiro
// É uma junção de dados que vem do SkuMarketplace e Vendas

namespace DashboardTrilhaEsporte.Domain.DTOs
{

    public class ResumoFinanceiroDTO
    {
        public String? skuId { get; set; }

        public String? marketplace { get; set; }

        // Numero do pedido
        public String? codigoPedido { get; set; }
        public DateTime? dataPedido { get; set; }
        public Decimal valorTotalProdutos { get; set; }
        public Decimal? comissaoEsperada { get; set; }
        public Decimal valorRecebido { get; set; }
        public Decimal valorAReceber { get; set; }
        public Decimal valorDescontado { get; set; }
        public Decimal descontoFrete { get; set; }

        public StatusPagamento situacaoPagamento { get; set; }

        public ErrosPagamento situacaoFinal { get; set; }


        public ResumoFinanceiroDTO()
        {
            
        }


        // Método responsável por montar o resumo financeiro
        public static List<ResumoFinanceiroDTO> MontarResumoFinanceiro(List<SkuMarketplaceDTO> skuMarketplaces, List<Vendas> vendas)
        {
            // Remover duplicatas da lista de vendas
            vendas = vendas.Distinct().ToList();
            // Criar um dicionário para armazenar o valor de venda por skuMarketplaceId
           
            var vendaDict = vendas
            .GroupBy(v => v.skuMarketplaceId)
            // trata as dublicações de skuMarketplaceId (pega o primeiro valor)
            .ToDictionary(g => g.Key, g => g.First().valorVenda);

            // Agrupar os skuMarketplaces por numeroPedido e marketplace
            var agrupados = skuMarketplaces
                .GroupBy(x => new { x.skuMarketplace.numeroPedido, x.skuMarketplace.marketplace })
                // Usa função auxiliar para criar o ResumoFinanceiroDTO
                .Select(grupo => CriarResumoFinanceiro(grupo, vendaDict))
                .Where(resumo => resumo.valorTotalProdutos != 0m)
                .ToList();


            return agrupados;
        }

        // Método auxiliar para criar o ResumoFinanceiroDTO
        private static ResumoFinanceiroDTO CriarResumoFinanceiro(IGrouping<object, SkuMarketplaceDTO> grupo, Dictionary<string, decimal> vendaDict)
        {

            // Extrair Informações Gerais do Grupo 
            var primeiro = grupo.First();

            var descontoFrete = CalcularDescontoFrete(grupo);
            var valorDescontado = CalcularValorDescontado(grupo);


            var skuId = primeiro.skuMarketplace.skuMarketplaceId;

            // dataPedido é a data do primeiro evento do grupo
            var dataPedido = grupo.Min(g => g.skuMarketplace.dataEvento);

            // valorTotal é o valor total dos produtos vendidos que tem o mesmo skuId
            var valorTotal = vendaDict.ContainsKey(skuId) ? vendaDict[skuId] : 0m;

            // comissaoEsperada é a maior comissão entre os eventos do grupo
            var comissaoEsperada = grupo.Select(g => g.skuMarketplace.comissao).Max();

            // valor a Receber é o valor total menos a comissão esperada
            DateTime inicioMesEspecial = new DateTime(2021, 8, 1);
            DateTime fimMesEspecial = new DateTime(2021, 9, 30); // setembro tem 30 dias

            var AReceber = 5m;

            var dataRepasse =  grupo.Min(g => g.skuMarketplace.dataCiclo);

            if (dataRepasse >= inicioMesEspecial && dataRepasse <= fimMesEspecial)
            {
                DateTime limiteMesAgosto = new DateTime(2021, 8, 31);

                List<string> pedidosEspeciais = new List<string> { "93205770001", "93209860701" };

                string? numeroPedido = primeiro.skuMarketplace.numeroPedido?.Trim();

                if (dataRepasse <= limiteMesAgosto || (numeroPedido != null && pedidosEspeciais.Contains(numeroPedido)))
                {
                    AReceber = valorTotal + descontoFrete;
                }
                else
                {
                     AReceber = valorTotal - comissaoEsperada;
                }
            }

            else
               AReceber = valorTotal - comissaoEsperada;
            


            // valor recebido é valor final para tipo evento "repasse normal" 
            // ou zero para os outros tipos de evento
            var Recebido = CalcularValorRecebido(grupo);

            // Extria as informações do grupo

            var situacaoPagamento = CalcularSituacaoPagamento(Recebido, AReceber);
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


        // Método responsável por calcular a situação do pagamento

        private static StatusPagamento CalcularSituacaoPagamento(decimal valorRecebido, decimal valorAReceber)
        {
            // Se o valor recebido for igual ao valor a receber, retorna Pago
            // Se o valor recebido for maior que o valor a receber, retorna PagoAMais
            // Se o valor recebido for menor que o valor a receber, retorna PagoAMenos
            // Se o valor recebido for zero, retorna NãoPago

            var diferenca = valorRecebido - valorAReceber;
            if (Math.Abs(diferenca) < 0.05m) return StatusPagamento.Pago;
            if (diferenca > 0) return StatusPagamento.PagoAMais;
            if (valorRecebido > 0) return StatusPagamento.PagoAMenos;
            return StatusPagamento.NaoPago;
        }

        // Método responsável por calcular o valor descontado

        private static decimal CalcularValorDescontado(IEnumerable<SkuMarketplaceDTO> grupo)

        {

            //  Caso o evento seja "DescontarHoveHouve" ou "DescontarRetroativo",
            //  o valor descontado é o valor final do evento "DescontarHoveHouve" ou o valor final do evento "DescontarRetroativo"

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


        // Método responsável por calcular o desconto do frete
        private static decimal CalcularDescontoFrete(IEnumerable<SkuMarketplaceDTO> grupo)
        {
            // Caso o evento seja "DescontarReversaCentauroEnvios", o desconto do frete é o valor final do evento "DescontarReversaCentauroEnvios"
            return grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.DescontarReversaCentauroEnvios)
                .Sum(g => g.skuMarketplace.valorFinal);
        }


        // Método responsável por verificar se houve erro na devolução
        private static bool VerificarErroDevolucao(IEnumerable<SkuMarketplaceDTO> grupo, decimal valorTotal)
        {
            // Caso o evento seja "DescontarHoveHouve", o erro de devolução é verdadeiro 
            // se o valor final do evento "DescontarHoveHouve" for diferente do valor total

            var valorHove = grupo
                .Where(g => g.skuMarketplace.tipoEventoNormalizado == Eventos.DescontarHoveHouve)
                .Select(g => g.skuMarketplace.valorFinal)
                .DefaultIfEmpty(0)
                .Max();

            return Math.Abs(valorHove) > 0 && Math.Abs(valorHove) != Math.Abs(valorTotal);
        }


        // Método responsável por calcular a situação final do pagamento
        private static ErrosPagamento CalcularSituacaoFinal(StatusPagamento situacaoPagamento, decimal diferenca, bool erroDevolucao)
        {
            // Se a diferença for menor que 0.01 e não houver erro de devolução, retorna Correto
            if (Math.Abs(diferenca) < 0.01m && !erroDevolucao)
                return ErrosPagamento.PagamentoCorreto;
            if (erroDevolucao)
                return ErrosPagamento.ErroDevolucao;

            return ErrosPagamento.ErroNoPagamento;

        }

        public override string ToString()
        {
            var culture = new CultureInfo("pt-BR");

            return $"{skuId};" +
                  $"{marketplace};" +
                  $"{codigoPedido};" +
                  $"{dataPedido?.ToString("dd/MM/yyyy")};" +
                  $"{valorTotalProdutos.ToString("N2", culture)};" +
                  $"{comissaoEsperada?.ToString("N2", culture)};" +
                  $"{valorRecebido.ToString("N2", culture)};" +
                  $"{valorAReceber.ToString("N2", culture)};" +
                  $"{valorDescontado.ToString("N2", culture)};" +
                  $"{descontoFrete.ToString("N2", culture)};" +
                  $"{situacaoPagamento.GetDescription()};";

        }

    }

}