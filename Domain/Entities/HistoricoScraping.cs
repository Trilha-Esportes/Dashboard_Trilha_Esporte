using Npgsql;

namespace DashboardTrilhaEsporte.Domain.Entities
{
    public class ScrapingHistorico
    {
        public long id { get; set; }
        public DateTime dataScraping { get; set; }
        public DateTime inicioExecucao { get; set; }
        public DateTime fimExecucao { get; set; }
        public int numeroDeLinks { get; set; }
        public int numeroErros { get; set; }
        public string status { get; set; } = "em_andamento";

        public static ScrapingHistorico MapearRegistro(NpgsqlDataReader reader)
        {
            return new ScrapingHistorico
            {
                id = reader["id"] is DBNull ? 0 : Convert.ToInt64(reader["id"]),
                dataScraping = reader["data_scraping"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["data_scraping"]),
                inicioExecucao = reader["inicio_execucao"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["inicio_execucao"]),
                fimExecucao = reader["fim_execucao"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["fim_execucao"]),
                numeroDeLinks = reader["numero_de_links"] is DBNull ? 0 : Convert.ToInt32(reader["numero_de_links"]),
                numeroErros = reader["numero_erros"] is DBNull ? 0 : Convert.ToInt32(reader["numero_erros"]),
                status = reader["status"]?.ToString() ?? "em_andamento"
            };
        }
    }
}
