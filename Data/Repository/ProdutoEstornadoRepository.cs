using Npgsql;
using DashboardTrilhaEsporte.Data.Entities;
using System.Data.Common;

namespace DashboardTrilhaEsporte.Data.Repository
{
    public class ProdutoEstornadoRepository
    {
        private readonly DBContext _dbContext;

        // Armazena os dados carregados em memória
        private Task<List<ProdutoEstornado>>? _listaDevolucoes;

        public ProdutoEstornadoRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProdutoEstornado>> ObterListaDevolucoesAsync()
        {

            if (_listaDevolucoes != null)
                return await _listaDevolucoes;

            _listaDevolucoes = CarregarDevolucoesDoBancoAsync();
          
            return await _listaDevolucoes;
        }

        private async Task<List<ProdutoEstornado>> CarregarDevolucoesDoBancoAsync()
        {
            var listaDevolucoes = new List<ProdutoEstornado>();

            using var connection = _dbContext.CreateConnection();
            await ((DbConnection)connection).OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
             select pe.id , pe.marketplace_id , pe.data_criacao ,pe.numero_pedido , 
            pe.sku_na_nota_fiscal ,pe.data_chegada_devolucao , 
            pe.conferencia,pe.enviado_corretamente ,pe.entregue,
            pe.motivo ,pe.operacao  from produto_estornado pe 
            ";

            await using var reader = await ((DbCommand)command).ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var registro = ProdutoEstornado.MapearRegistro((NpgsqlDataReader)reader);
                listaDevolucoes.Add(registro);
            }

            return listaDevolucoes;
        }
    }
}
