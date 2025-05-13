using Npgsql;
using DashboardTrilhaEsporte.Domain.Entities;

// Class responsável por carregar os dados das tabelas Vendas

namespace DashboardTrilhaEsporte.Data
{
    public class VendasRepository
    {
        private readonly DBContext _dbContext;

        // Variável privada para armazenar a lista de Vendas (Mantendo em memória)
        public Task<List<Vendas>>? _listaVendas;

        public VendasRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Usa a conexão do DBContext para criar um comando SQL e executar a consulta.
        // O resultado é mapeado para uma lista de objetos Vendas (Busca Asincrona).
        public Task<List<Vendas>> ObterlistaVendas()
        {
            // Verifica se a lista já foi carregada. Se não, carrega os dados do banco.
            if (_listaVendas == null)
            {

                List<Vendas> listaVendas = new List<Vendas>();

                using (var connection = _dbContext.CreateConnection())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                        id AS venda_id,
                        sku_marketplace_id,
                        valor_liquido AS valor_vendas
                        FROM vendas
                    ";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Mapeia cada linha do resultado para um objeto Vendas
                            var registro = Vendas.MapearRegistro((NpgsqlDataReader)reader);
                            listaVendas.Add(registro);
                        }
                    }
                }
                this._listaVendas = Task.FromResult(listaVendas);

            }

            return this._listaVendas;

        }

    }
}