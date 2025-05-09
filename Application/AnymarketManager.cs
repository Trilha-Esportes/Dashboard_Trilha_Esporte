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

        public static List<AnymarketDTO> SubtrairPorId(
        List<SkuMarketplaceDTO> origemSkuMarketplaces,
        List<AnymarketDTO> destinoAnymarket,
        Func<SkuMarketplaceDTO, String> seletorIdOrigem,
        Func<AnymarketDTO, String> seletorIdDestino)
    {
        var idsOrigem = origemSkuMarketplaces
            .Select(seletorIdOrigem)
            .ToHashSet();


        return destinoAnymarket
            .Where(item => idsOrigem.Contains(seletorIdDestino(item)))
            .ToList();
    }


    }
}