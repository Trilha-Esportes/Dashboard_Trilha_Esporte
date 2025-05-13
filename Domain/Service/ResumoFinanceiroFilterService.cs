
using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Enums;

 // Essa classe é responsável por aplicar os filtros na lista de ResumoFinanceiroDTO
namespace DashboardTrilhaEsporte.Domain.Service
{
    public class ResumoFinanceiroFilterService
    {
        StatusPagamento statusPagamento;

        public ResumoFinanceiroFilterService(StatusPagamento statusPagamento)
        {
            this.statusPagamento = statusPagamento;

        }


        // Método responsável por aplicar  o filtro de acordo com o status de pagamento
        public static List<ResumoFinanceiroDTO> AplicarFiltros(List<ResumoFinanceiroDTO> lista, StatusPagamento statusPagamento)
        {
            var query = lista.AsQueryable();

            query = query.Where(x => x.situacaoFinal == statusPagamento);

            return query.ToList();
        }

    }
}
