using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Data.Entities;

using DashboardTrilhaEsporte.Data.Repository;

namespace DashboardTrilhaEsporte.Application
{

    public class ScrapingManager
    {
        private ScrapingRepository _repo;

        public Boolean _dadosCarregados { get; private set; } = false;

        public ScrapingDadosDTO scrapingDadosDTO { get; private set; } = new ScrapingDadosDTO();


        public ScrapingManager(ScrapingRepository repository)
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
                List<Scraping> listaOriginal = await _repo.ObterTodosAsync();
                this.scrapingDadosDTO = new ScrapingDadosDTO(listaOriginal);
                this._dadosCarregados = true;
            }
        }



    }
}