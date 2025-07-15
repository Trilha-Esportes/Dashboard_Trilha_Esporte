using DashboardTrilhaEsporte.Data;
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Entities;

namespace DashboardTrilhaEsporte.Application
{

    public class ScrapingManager
    {
        private ScrapingRepository _repo;

        public Boolean _dadosCarregados = false;

        public ScrapingDadosDTO? scrapingDadosDTO;

        public ScrapingManager(ScrapingRepository repository)
        {
            this._repo = repository;

        }
         public void  CarregarDadosAsync()
        {
            if (this._dadosCarregados)
            {
                    return; 
            } else
            {
                List<Scraping> listaOriginal = _repo.ObterTodos();
                this.scrapingDadosDTO = new ScrapingDadosDTO(listaOriginal);
                this._dadosCarregados=true;
            }
        }



    }
}