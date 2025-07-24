using System.Threading.Tasks;
using DashboardTrilhaEsporte.Data;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Entities;

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

        public async Task  CarregarDadosAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            if (this._dadosCarregados)
            {
                return;
            }
            else
            {
                 DateTime inicio = DateTime.Now;

                List<Vendas> listaVenda = await _repo.ObterListaVendasAsync();
                List<ResumoFinanceiroDTO> resumoFinanceiroDTOs = ResumoFinanceiroDTO.MontarResumoFinanceiro(skuMarketplaces, listaVenda);
                this.FinanceiroDadosDTO = new ResumoFinanceiroDadosDTO(resumoFinanceiroDTOs);
                this._dadosCarregados = true;

                DateTime fim = DateTime.Now;

                TimeSpan duracao = fim - inicio;

                Console.WriteLine($"Duração montagem resumo: {duracao.TotalMilliseconds} ms");
            }
        }

        public async Task<List<ResumoFinanceiroDTO>> AtualizarListaAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            List<Vendas> lista = await _repo.ObterListaVendasAsync();
            return ResumoFinanceiroDTO.MontarResumoFinanceiro(skuMarketplaces, lista);
        }

    }
}