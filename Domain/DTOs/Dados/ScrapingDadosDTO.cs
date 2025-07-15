using DashboardTrilhaEsporte.Domain.Entities;

namespace DashboardTrilhaEsporte.Domain.DTOs
{
    public class ScrapingDadosDTO
    {
        public List<Scraping> scrapings { get; set; }

        public int quantidadeLinksAtivos { get; set; }

        public int quantidadeLinksDesativados { get; set; }

        public int quantidadeProdutosSemEstoque { get; set; }


        public ScrapingDadosDTO(List<Scraping> scrapings)
        {
            this.scrapings = scrapings;
            quantidadeLinksDesativados = scrapings.Count(s => !s.linkAtivo);
            quantidadeProdutosSemEstoque = scrapings.Count(s => s.tagSemEstoque);
            quantidadeLinksAtivos = scrapings.Count- quantidadeLinksDesativados;
        }
        
    }
}