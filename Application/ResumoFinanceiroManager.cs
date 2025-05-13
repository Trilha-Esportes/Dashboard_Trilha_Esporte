using DashboardTrilhaEsporte.Data;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Entities;

namespace DashboardTrilhaEsporte.Application
{
    partial class ResumoFinanceiroManager
    {

        private readonly VendasRepository _repo;

        private Boolean _dadosCarregados = false;

        public ResumoFinanceiroDadosDTO? FinanceiroDadosDTO { get; set; }



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
                List<Vendas> listaVenda = await _repo.ObterlistaVendas();
                List<ResumoFinanceiroDTO> resumoFinanceiroDTOs = ResumoFinanceiroDTO.MontarResumoFinanceiro(skuMarketplaces, listaVenda);
                this.FinanceiroDadosDTO = new ResumoFinanceiroDadosDTO(resumoFinanceiroDTOs);
                this._dadosCarregados = true;
            }
        }

        public async Task<List<ResumoFinanceiroDTO>> AtualizarListaAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            List<Vendas> lista = await _repo._listaVendas!;
            return ResumoFinanceiroDTO.MontarResumoFinanceiro(skuMarketplaces, lista);
        }

    }
}