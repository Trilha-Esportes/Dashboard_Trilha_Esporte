using Npgsql;

using DashboardTrilhaEsporte.Domain.Entities;
namespace DashboardTrilhaEsporte.Data
{
    public class ProdutoRepository
    {
        private readonly DBContext _dbContext;

        private Task<List<Produto>>? _listaProdutos;

        public ProdutoRepository(DBContext dBContext)
        {
            this._dbContext = dBContext;
        }

        public List<Produto> ObterListaProdutos()
        {
            if (_listaProdutos != null)
                return _listaProdutos.Result;

            var listaProdutos = new List<Produto>();

            using (var connection = _dbContext.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM produtos";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var produto = Produto.MapearRegistro((NpgsqlDataReader)reader);
                        listaProdutos.Add(produto);
                    }
                }
            }

            _listaProdutos = Task.FromResult(listaProdutos);
            return listaProdutos;
        }
    }
}