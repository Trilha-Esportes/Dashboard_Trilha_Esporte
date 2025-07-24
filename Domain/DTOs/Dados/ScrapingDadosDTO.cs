using DashboardTrilhaEsporte.Data.Entities;
using DashboardTrilhaEsporte.Enums;
namespace DashboardTrilhaEsporte.Domain.DTOs
{
    public class ScrapingDadosDTO
    {
        public List<Scraping> scrapings { get; set; }

        public int quantidadeLinksAtivos { get; set; }

        public int quantidadeLinksDesativados { get; set; }

        public int quantidadeProdutosSemEstoque { get; set; }

        public ScrapingDadosDTO()
        {
            scrapings = new List<Scraping>();
        }
        public ScrapingDadosDTO(List<Scraping> scrapings)
        {
            this.scrapings = scrapings;
            quantidadeLinksDesativados = scrapings.Count(s => s.linkAtivo == ScrapingStatus.DESATIVADO);
            quantidadeProdutosSemEstoque = scrapings.Count(s => s.tagSemEstoque == ScrapingStatus.SEMESTOQUE);
            quantidadeLinksAtivos = scrapings.Count- quantidadeLinksDesativados;
        }
        
    }
}