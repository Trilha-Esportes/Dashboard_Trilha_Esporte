using Npgsql;

namespace DashboardTrilhaEsporte.Data.Entities
{
    public class ScrapingHistorico
    {
        public long id { get; private set; }
        public DateTime dataScraping { get; private set; }
        public DateTime inicioExecucao { get; private set; }
        public DateTime fimExecucao { get; private set; }
        public int numeroDeLinks { get; private set; }
        public int numeroErros { get; private set; }
        public string status { get; private set; } = "em_andamento";

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
