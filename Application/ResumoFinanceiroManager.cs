using DashboardTrilhasEsporte.Data;
using DashboardTrilhasEsporte.Domain.DTOs;
using DashboardTrilhasEsporte.Domain.Entities;

namespace DashboardTrilhasEsporte.Application
{
    partial class ResumoFinanceiroManager{

        private readonly VendasRepository _repo;

        private Boolean _dadosCarregados = false;

        public ResumoFinanceiroDadosDTO? FinanceiroDadosDTO{ get; set; } 



        public ResumoFinanceiroManager(VendasRepository repository){
            this._repo = repository;
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
                List<ResumoFinanceiroDTO> resumoFinanceiroDTOs = ResumoFinanceiroDTO.MontarAnymarketDTO(skuMarketplaces, listaVenda);
                this.FinanceiroDadosDTO = new ResumoFinanceiroDadosDTO(resumoFinanceiroDTOs);
                this._dadosCarregados = true;
            }
        }

        public static List<ResumoFinanceiroDTO> SubtrairPorId(
        List<SkuMarketplaceDTO> origemSkuMarketplaces,
        List<ResumoFinanceiroDTO> destinoResumo,
        Func<SkuMarketplaceDTO, string> seletorIdOrigem,
        Func<ResumoFinanceiroDTO, string> seletorIdDestino)
        {
            var idsOrigem = origemSkuMarketplaces
                .Select(seletorIdOrigem)
                .ToHashSet();

            return destinoResumo
                .Where(item => idsOrigem.Contains(seletorIdDestino(item)))
                .ToList();
        }

    }
}