using DashboardTrilhasEsporte.Data;
using DashboardTrilhasEsporte.Domain.DTOs;
using DashboardTrilhasEsporte.Domain.Entities;

namespace DashboardTrilhasEsporte.Application
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

        public static List<SkuMarketplaceDTO> SubtrairPorId(
                List<SkuMarketplaceDTO> listaA,
                List<AnymarketDTO> listaB,
                Func<SkuMarketplaceDTO, int> seletorIdA,
                Func<AnymarketDTO, int> seletorIdB)
        {
            var idsParaRemover = listaB.Select(seletorIdB).ToHashSet();
            return listaA.Where(item => !idsParaRemover.Contains(seletorIdA(item))).ToList();
        }

    }
}