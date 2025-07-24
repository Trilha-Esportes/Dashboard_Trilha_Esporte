using DashboardTrilhaEsporte.Data.Entities;
using Npgsql;
using System.Data.Common;

namespace DashboardTrilhaEsporte.Data.Repository
{
    public class ScrapingRepository
    {
        private readonly DBContext _dbContext;

        public ScrapingRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Scraping>> ObterTodosAsync()
        {
            var lista = new List<Scraping>();

            using var connection = _dbContext.CreateConnection();
            await ((DbConnection)connection).OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                WITH UltimoScraping AS (
                    SELECT 
                        s.*,
                        ROW_NUMBER() OVER (PARTITION BY s.id_produto ORDER BY s.data_criacao DESC) AS rn
                    FROM scraping s
                )
                SELECT 
                    s.id,
                    s.nome_produto AS nome_de_venda,
                    p.nome AS nome_produto,
                    m.nome AS nome_marketplace,
                    s.link_ativo,
                    s.tag_sem_estoque,
                    s.preco_produto, 
                    s.descricao_erro,
                    p.sku_marketplace,
                    s.id_scraping_historico,
                    s.id_produto,
                    s.data_criacao                 
                FROM UltimoScraping s
                LEFT JOIN produtos p ON p.id = s.id_produto
                LEFT JOIN marketplaces m ON p.marketplace_id = m.id
                WHERE s.rn = 1;
            ";

            await using var reader = await ((DbCommand)command).ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var scraping = Scraping.MapearRegistro((NpgsqlDataReader)reader);
                lista.Add(scraping);
            }

            return lista;
        }

        public async Task<List<Scraping>> BuscarPorIdScrapingHistoricoAsync(int idHistorico)
        {
            var lista = new List<Scraping>();

            using var connection = _dbContext.CreateConnection();
            await ((DbConnection)connection).OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM scraping WHERE id_scraping_historico = @id";

            var parametro = command.CreateParameter();
            parametro.ParameterName = "@id";
            parametro.Value = idHistorico;
            command.Parameters.Add(parametro);

            await using var reader = await ((DbCommand)command).ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var scraping = Scraping.MapearRegistro((NpgsqlDataReader)reader);
                lista.Add(scraping);
            }

            return lista;
        }
    }
}
