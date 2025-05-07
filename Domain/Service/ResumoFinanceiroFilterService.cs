
using DashboardTrilhasEsporte.Enums;
using DashboardTrilhasEsporte.Domain.DTOs;

namespace DashboardTrilhasEsporte.Domain.Service
{
    public class ResumoFinanceiroFilterService
    {
        StatusPagamento statusPagamento;

        public ResumoFinanceiroFilterService(StatusPagamento statusPagamento)
        {
            this.statusPagamento = statusPagamento;

        }



        public static List<ResumoFinanceiroDTO> AplicarFiltros(List<ResumoFinanceiroDTO> lista, StatusPagamento statusPagamento)
        {
            var query = lista.AsQueryable();

            query = query.Where(x => x.situacaoFinal == statusPagamento);

            return query.ToList();
        }

    }
}
