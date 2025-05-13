using DashboardTrilhaEsporte.Data;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Entities;

namespace DashboardTrilhaEsporte.Application
{
    public class AnymarketManager
    {
        private readonly VendasRepository _repo;

        private Boolean _dadosCarregados = false;

        public AnymarketDadosDTO? anymarketDadosDTO { get; set; }

        public AnymarketManager(VendasRepository repository)
        {
            _repo = repository;
        }

        public async Task CarregarDadosAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            if (this._dadosCarregados)
            {
                return;
            }
            else
            {
                List<Vendas> listaVenda = await _repo.ObterlistaVendas();
                List<AnymarketDTO> anymarketDTOs = AnymarketDTO.MontarAnymarketDTO(skuMarketplaces, listaVenda);
                this.anymarketDadosDTO = new AnymarketDadosDTO(anymarketDTOs);
                this._dadosCarregados = true;
            }
        }

         public async Task<List<AnymarketDTO>> AtualizarListaAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            List<Vendas> lista = await _repo._listaVendas!;
            return AnymarketDTO.MontarAnymarketDTO(skuMarketplaces, lista);
        }


    }
}