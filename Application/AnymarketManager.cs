using DashboardTrilhaEsporte.Data;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Entities;

namespace DashboardTrilhaEsporte.Application
{
    public class AnymarketManager
    {
        private readonly VendasRepository _repo;

        private Boolean _dadosCarregados = false;

        public AnymarketDadosDTO anymarketDadosDTO { get; private set; } = new AnymarketDadosDTO();

        public AnymarketManager(VendasRepository repository)
        {
            _repo = repository;

        }

        public  async Task CarregarDadosAsync(List<SkuMarketplaceDTO> skuMarketplaces)
        {
            if (this._dadosCarregados)
            {
                return;
            }
            else
            {
                DateTime inicio = DateTime.Now;


                List<Vendas> listaVenda = await _repo.ObterListaVendasAsync();
                List<AnymarketDTO> anymarketDTOs = AnymarketDTO.MontarAnymarketDTO(skuMarketplaces, listaVenda);
                this.anymarketDadosDTO = new AnymarketDadosDTO(anymarketDTOs);
                this._dadosCarregados = true;
                DateTime fim = DateTime.Now;

                TimeSpan duracao = fim - inicio;

                Console.WriteLine($"Duração montagem anymarket: {duracao.TotalMilliseconds} ms");
         }
        }

      public async Task<List<AnymarketDTO>> AtualizarListaAsync(List<SkuMarketplaceDTO> skuMarketplaces)
      {
            List<Vendas> lista = await _repo.ObterListaVendasAsync();
            return AnymarketDTO.MontarAnymarketDTO(skuMarketplaces, lista);
      }



    }
}