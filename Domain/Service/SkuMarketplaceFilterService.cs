
using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.DTOs;

// class Responsável por constrolar o filtro avançado de SkuMarketplaceDTO

namespace DashboardTrilhaEsporte.Domain.Service
{
    public class SkuMarketplaceFilterService
    {
        public DateTime? dataEventoInicio { get; set; }
        public DateTime? dataEventoFinal { get; set; }
        public DateTime? dataCicloSelecionada { get; set; }


        public String? numeroPedido { get; set; }

        public List<Erros> listaErros { get; set; } = new List<Erros>();

        public List<Eventos> TipoEventos { get; set; }= new List<Eventos>();


        // Construtor vazio para inicializar a lista de skuMarketplaceDTOs
        public SkuMarketplaceFilterService (){

        }
        public SkuMarketplaceFilterService(
            DateTime? dataEventoInicio = null,
            DateTime? dataEventoFinal = null,
            DateTime? dataCicloSelecionada = null,
            string? numeroPedido = null,
            List<Erros>? listaErros = null,
            List<Eventos>? tipoEventos = null)
        {
            this.dataEventoInicio = dataEventoInicio;
            this.dataEventoFinal = dataEventoFinal;
            this.dataCicloSelecionada = dataCicloSelecionada;
            this.numeroPedido = numeroPedido;
            this.listaErros = listaErros ?? new List<Erros>();
            this.TipoEventos = tipoEventos ?? new List<Eventos>();
        }


        // Método responsável por aplicar um filtro sequencial na lista de SkuMarketplaceDTO
        // O filtro é aplicado de acordo com os parâmetros do SkuMarketplaceFilterService
        public static List<SkuMarketplaceDTO> AplicarFiltros(List<SkuMarketplaceDTO> lista, SkuMarketplaceFilterService filtros)
        {
            var query = lista.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtros.numeroPedido))
                query = query.Where(x => x.skuMarketplace.numeroPedido == filtros.numeroPedido);

            if (filtros.dataEventoInicio.HasValue)
                query = query.Where(x => x.skuMarketplace.dataEvento.HasValue &&
                                         x.skuMarketplace.dataEvento.Value >= filtros.dataEventoInicio.Value);

            if (filtros.dataEventoFinal.HasValue)
                query = query.Where(x => x.skuMarketplace.dataEvento.HasValue &&
                                         x.skuMarketplace.dataEvento.Value <= filtros.dataEventoFinal.Value);

           if (filtros.dataCicloSelecionada.HasValue)
            {
                var dataSelecionada = filtros.dataCicloSelecionada.Value.Date;

                query = query.Where(x =>
                    x.skuMarketplace != null &&
                    x.skuMarketplace.dataCiclo.HasValue &&
                    x.skuMarketplace.dataCiclo.Value.Date == dataSelecionada);
            }

            if (filtros.listaErros != null && filtros.listaErros.Any())
                query = query.Where(x => x.listaErros != null &&
                                         x.listaErros.Any(e => filtros.listaErros.Contains(e)));

            if (filtros.TipoEventos != null && filtros.TipoEventos.Any())
                query = query.Where(x => filtros.TipoEventos.Contains(x.skuMarketplace.tipoEventoNormalizado));

            

            return query.ToList();
        }

    }
}
