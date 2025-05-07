using Npgsql;
using DashboardTrilhasEsporte.Domain.Entities;

namespace DashboardTrilhasEsporte.Data
{
    public class VendasRepository
    {
        private readonly DBContext _dbContext;

        public Task<List<Vendas>> _listaVendas;

        public VendasRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Vendas>> ObterlistaVendas()
        {

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