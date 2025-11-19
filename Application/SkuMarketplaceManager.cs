using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Service;

using DashboardTrilhaEsporte.Data.Repository;

namespace DashboardTrilhaEsporte.Application
{
    class SkuMarketplaceManager
    {
        private readonly SkuMarketplaceRepository _repo;


        public SkuMarketplaceDadosDTO resultDTO { get; private set; } = new SkuMarketplaceDadosDTO();
        private bool _dadosCarregados = false;



        public SkuMarketplaceManager(SkuMarketplaceRepository repository)
        {
            this._repo = repository;
        }

        public async Task CarregarDadosAsync()
        {
            if (this._dadosCarregados)
            {
                return;
            }
            else
            {
                var listaSku = await _repo.ObterListaMarketplaceAsync();
                this.resultDTO = new SkuMarketplaceDadosDTO(listaSku);
                this._dadosCarregados = true;
            }
        }


        public SkuMarketplaceDadosDTO? ObterListaFiltrada(
            DateTime? dataEventoInicio = null,
            DateTime? dataEventoFinal = null,
            DateTime? dataCicloSelecionada = null,
            string? numeroPedido = null,
            List<Erros>? listaErros = null,
            List<Eventos>? tipoEventos = null)
        {
            // Criando o filtro com os parâmetros recebidos
            var filtro = new SkuMarketplaceFilterService
            {
                dataEventoInicio = dataEventoInicio,
                dataEventoFinal = dataEventoFinal,
                dataCicloSelecionada = dataCicloSelecionada,
                numeroPedido = numeroPedido,
                listaErros = listaErros ?? new List<Erros>(),
                TipoEventos = tipoEventos ?? new List<Eventos>()
            };

            if (this.resultDTO != null)
            {
                // Aplica os filtros na lista completa
                List<SkuMarketplaceDTO> listaFiltrada = SkuMarketplaceFilterService.
                                                AplicarFiltros(this.resultDTO.skuMarketplaceDTOs, filtro);

                // Cria o DTO de resultado
                return new SkuMarketplaceDadosDTO(listaFiltrada);
            }
            return null;
        }
    }
}