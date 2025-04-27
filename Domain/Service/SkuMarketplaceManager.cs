using DashboardTrilhasEsporte.Data;
using DashboardTrilhasEsporte.Enums;

namespace DashboardTrilhasEsporte.Domain
{
    class SkuMarketplaceManager
    {
        private readonly SkuMarketplaceRepository _repo;


        public SkuMarketplaceListResultDTO resultDTO { get; private set; }
        private bool _dadosCarregados = false;



        public SkuMarketplaceManager(SkuMarketplaceRepository repo)
        {
            this._repo = repo;
        }

        public async Task CarregarDadosAsync()
        {
            if (this._dadosCarregados)
            {
                    return; 
            } else
            {
                var listaSku = await _repo.ObterlistaMarketplace();
                this.resultDTO = new SkuMarketplaceListResultDTO(listaSku);
            }
        }


        public SkuMarketplaceListResultDTO ObterListaFiltrada(
            DateTime? dataComissaoInicio = null,
            DateTime? dataComissaoFinal = null,
            DateTime? dataCicloInicio = null,
            DateTime? dataClicloFinal = null,
            string? numeroPedido = null,
            List<Erros>? listaErros = null,
            List<Eventos>? tipoEventos = null)
        {
            // Criando o filtro com os parâmetros recebidos
            var filtro = new SkuMarketplaceFilterService
            {
                dataComissaoInicio = dataComissaoInicio,
                dataComissaoFinal = dataComissaoFinal,
                dataCicloInicio = dataCicloInicio,
                dataClicloFinal = dataClicloFinal,
                numeroPedido = numeroPedido,
                listaErros = listaErros ?? new List<Erros>(),
                TipoEventos = tipoEventos ?? new List<Eventos>()
            };

            // Aplica os filtros na lista completa

            List<SkuMarketplaceDTO> listaFiltrada = SkuMarketplaceFilterService.
                                            AplicarFiltros(this.resultDTO.skuMarketplaceDTOs, filtro);

            // Cria o DTO de resultado
            return new SkuMarketplaceListResultDTO(listaFiltrada);
        }
    }
}