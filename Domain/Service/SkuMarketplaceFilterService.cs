
using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.DTOs;


namespace DashboardTrilhaEsporte.Domain.Service
{
    public class SkuMarketplaceFilterService
    {
        public DateTime? dataComissaoInicio { get; set; }
        public DateTime? dataComissaoFinal { get; set; }
        public DateTime? dataCicloSelecionada { get; set; }

        public String? numeroPedido { get; set; }

        public List<Erros> listaErros { get; set; } = new List<Erros>();

        public List<Eventos> TipoEventos { get; set; }= new List<Eventos>();


        public SkuMarketplaceFilterService (){

        }
        public SkuMarketplaceFilterService(
            DateTime? dataComissaoInicio = null,
            DateTime? dataComissaoFinal = null,
            DateTime? dataCicloSelecionada = null,
            string? numeroPedido = null,
            List<Erros>? listaErros = null,
            List<Eventos>? tipoEventos = null)
        {
            this.dataComissaoInicio = dataComissaoInicio;
            this.dataComissaoFinal = dataComissaoFinal;
            this.dataCicloSelecionada = dataCicloSelecionada;
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
