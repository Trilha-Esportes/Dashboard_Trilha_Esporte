using DashboardTrilhaEsporte.Data.Entities;
using DashboardTrilhaEsporte.Enums;
namespace DashboardTrilhaEsporte.Domain.DTOs
{
    public class SkuMarketplaceDevolucaoDTO
    {

        public long marketplaceId { get; set; }
        public String? numeroPedido { get; set; } = String.Empty;
        public Decimal valorPedido { get; set; }
        public Decimal valorFinal { get; set; }
        public Eventos tipoEvento { get; set; }
        public Decimal diferenca { get; set; }
        public DateTime? dataPedido { get; set; }
        public DateTime? dataCiclo { get; set; }
        public MotivoDevolucao motivoDevolucao { get; set; }
        public Boolean converencia { get; set; }
        public Operacao motivoEntrada { get; set; }

        public SkuMarketplaceDevolucaoDTO()
        {
            
        }
        public SkuMarketplaceDevolucaoDTO(SkuMarketplace pedidos, ProdutoEstornado devolucao)
        {
            this.marketplaceId = pedidos.marketplaceId;
            this.dataCiclo = pedidos.dataCiclo;
            this.numeroPedido = pedidos.numeroPedido;
            this.valorPedido = pedidos.valorLiquido;
            this.valorFinal = pedidos.valorFinal;
            this.tipoEvento = pedidos.tipoEventoNormalizado;
            this.diferenca = pedidos.valorFinal + pedidos.valorLiquido;
            this.dataCiclo = pedidos.dataEvento;
            this.dataPedido = pedidos.dataEvento;
            this.motivoDevolucao = devolucao.motivo;
            this.converencia = devolucao.conferencia;
            this.motivoEntrada = devolucao.operacao;
        } 



    }
}