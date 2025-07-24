using DashboardTrilhaEsporte.Data.Entities;
using Npgsql;

namespace DashboardTrilhaEsporte.Data.Repository
{
    public class ScrapingHistoricoRepository
    {
        private readonly DBContext _dbContext;

        public ScrapingHistoricoRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ScrapingHistorico> ObterTodos()
        {
            var lista = new List<ScrapingHistorico>();

            using (var connection = _dbContext.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM scraping_historico";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var historico = ScrapingHistorico.MapearRegistro((NpgsqlDataReader)reader);
                        lista.Add(historico);
                    }
                }
            }

            return lista;
        }
    }
}
