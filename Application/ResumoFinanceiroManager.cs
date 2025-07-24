using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Data.Entities;

using DashboardTrilhaEsporte.Data.Repository;

namespace DashboardTrilhaEsporte.Application
{
    partial class ResumoFinanceiroManager
    {

        private readonly VendasRepository _repo;

        private Boolean _dadosCarregados = false;

        public ResumoFinanceiroDadosDTO? FinanceiroDadosDTO { get; private set; } = new ResumoFinanceiroDadosDTO();



        public ResumoFinanceiroManager(VendasRepository repository)
        {
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

                List<Vendas> listaVenda = await _repo.ObterListaVendasAsync();
                List<ResumoFinanceiroDTO> resumoFinanceiroDTOs = ResumoFinanceiroDTO.MontarResumoFinanceiro(skuMarketplaces, listaVenda);
                this.FinanceiroDadosDTO = new ResumoFinanceiroDadosDTO(resumoFinanceiroDTOs);
                this._dadosCarregados = true;


            }
        }

        public async Task<List<ResumoFinanceiroDTO>> AtualizarListaAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            List<Vendas> lista = await _repo.ObterListaVendasAsync();
            return ResumoFinanceiroDTO.MontarResumoFinanceiro(skuMarketplaces, lista);
        }

    }
}