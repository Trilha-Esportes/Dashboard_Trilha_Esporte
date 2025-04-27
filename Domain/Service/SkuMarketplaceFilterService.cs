
using DashboardTrilhasEsporte.Enums;

namespace DashboardTrilhasEsporte.Domain
{
    public class SkuMarketplaceFilterService
    {
        public DateTime? dataComissaoInicio { get; set; }
        public DateTime? dataComissaoFinal { get; set; }
        public DateTime? dataCicloInicio { get; set; }
        public DateTime? dataClicloFinal { get; set; }

        public String? numeroPedido { get; set; }

        public List<Erros> listaErros { get; set; }

        public List<Eventos> TipoEventos { get; set; }


        public SkuMarketplaceFilterService(
            DateTime? dataComissaoInicio = null,
            DateTime? dataComissaoFinal = null,
            DateTime? dataCicloInicio = null,
            DateTime? dataClicloFinal = null,
            string? numeroPedido = null,
            List<Erros>? listaErros = null,
            List<Eventos>? tipoEventos = null)
        {
            this.dataComissaoInicio = dataComissaoInicio;
            this.dataComissaoFinal = dataComissaoFinal;
            this.dataCicloInicio = dataCicloInicio;
            this.dataClicloFinal = dataClicloFinal;
            this.numeroPedido = numeroPedido;
            this.listaErros = listaErros ?? new List<Erros>();
            this.TipoEventos = tipoEventos ?? new List<Eventos>();
        }



        public static List<SkuMarketplaceDTO> AplicarFiltros(List<SkuMarketplaceDTO> lista, SkuMarketplaceFilterService filtros)
        {
            var query = lista.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtros.numeroPedido))
                query = query.Where(x => x.skuMarketplace.numeroPedido == filtros.numeroPedido);

            if (filtros.dataComissaoInicio.HasValue)
                query = query.Where(x => x.skuMarketplace.dataComissao.HasValue &&
                                         x.skuMarketplace.dataComissao.Value >= filtros.dataComissaoInicio.Value);

            if (filtros.dataComissaoFinal.HasValue)
                query = query.Where(x => x.skuMarketplace.dataComissao.HasValue &&
                                         x.skuMarketplace.dataComissao.Value <= filtros.dataComissaoFinal.Value);

            if (filtros.dataCicloInicio.HasValue)
                query = query.Where(x => x.skuMarketplace.dataCiclo.HasValue &&
                                         x.skuMarketplace.dataCiclo.Value >= filtros.dataCicloInicio.Value);

            if (filtros.dataClicloFinal.HasValue)
                query = query.Where(x => x.skuMarketplace.dataCiclo.HasValue &&
                                         x.skuMarketplace.dataCiclo.Value <= filtros.dataClicloFinal.Value);

            if (filtros.listaErros != null && filtros.listaErros.Any())
                query = query.Where(x => x.listaErros != null &&
                                         x.listaErros.Any(e => filtros.listaErros.Contains(e)));

            if (filtros.TipoEventos != null && filtros.TipoEventos.Any())
                query = query.Where(x => filtros.TipoEventos.Contains(x.skuMarketplace.tipoEventoNormalizado));

            return query.ToList();
        }

    }
}
