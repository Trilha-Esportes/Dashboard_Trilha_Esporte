using DashboardTrilhasEsporte.Data;
using DashboardTrilhasEsporte.Enums;
using  Blazored.LocalStorage;

namespace DashboardTrilhasEsporte.Domain
{
    class SkuMarketplaceManager
    {
        private readonly SkuMarketplaceRepository _repo;

        private readonly ILocalStorageService _localStorage;


        public SkuMarketplaceListResultDTO resultDTO { get; private set; }
        private bool _dadosCarregados = false;



        public SkuMarketplaceManager(SkuMarketplaceRepository repo, ILocalStorageService localStorage)
        {
            this._repo = repo;
            this._localStorage = localStorage;
        }

        public async Task CarregarDadosAsync()
        {
            if ( this._dadosCarregados)
            {
                return; // Se os dados já foram carregados, não recarregue.
            }

            var dadosLocalStorage = await _localStorage.GetItemAsync<SkuMarketplaceListResultDTO>("dadosSkuMarketplace");
            
            if (dadosLocalStorage != null)
            {
                this.resultDTO = dadosLocalStorage; // Carregar os dados armazenados no LocalStorage
                this. _dadosCarregados = true;
                return;
            }

            var listaSku = await _repo.ObterlistaMarketplace();

            this.resultDTO = new SkuMarketplaceListResultDTO(listaSku);

            await _localStorage.SetItemAsync("dadosSkuMarketplace", this.resultDTO); // Armazenar no LocalStorage

            _dadosCarregados = true;
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