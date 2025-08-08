using DashboardTrilhaEsporte.Data.Repository;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Data.Entities;

namespace DashboardTrilhaEsporte.Application
{
    public class SkuMarketplaceDevolucaoManager
    {
        private readonly ProdutoEstornadoRepository _produtoEstornadoRepository;
        private readonly SkuMarketplaceRepository _skuMarketplaceRepository;

        public Boolean _dadosCarregados { get; private set; } = false;

        public List<SkuMarketplaceDevolucaoDTO>? skuMarketplaceDevolucaoDTO { get; private set; }

        public SkuMarketplaceDevolucaoManager(ProdutoEstornadoRepository produtoEstornadoRepository, SkuMarketplaceRepository skuMarketplaceRepository)
        {
            this._produtoEstornadoRepository = produtoEstornadoRepository;
            this._skuMarketplaceRepository = skuMarketplaceRepository;
        }

        public async Task CarregarDadosAsync()
        {
            if (this._dadosCarregados)
            {
                return;
            }
            else
            {
                List<ProdutoEstornado> listaDevolucoes = await _produtoEstornadoRepository.ObterListaDevolucoesAsync();

                List<SkuMarketplace> listaPedidos = await _skuMarketplaceRepository.ObterListaMarketplaceAsync();

                listaPedidos = listaPedidos
                               .Where(p => p.tipoEventoNormalizado == Enums.Eventos.DescontarHoveHouve)
                              .ToList();


                skuMarketplaceDevolucaoDTO = (from devolucao in listaDevolucoes
                                                        join pedido in listaPedidos
                                                             on new { devolucao.marketplaceId, devolucao.numeroPedido }
                                                             equals new { pedido.marketplaceId, pedido.numeroPedido }
                                                        select new SkuMarketplaceDevolucaoDTO(pedido, devolucao))
                                                       .ToList();

                this._dadosCarregados = true;

            }
        }

    }
}