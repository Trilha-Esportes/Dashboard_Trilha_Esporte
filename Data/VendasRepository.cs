using Npgsql;
using DashboardTrilhaEsporte.Domain.Entities;

// Class responsável por carregar os dados das tabelas Vendas

namespace DashboardTrilhaEsporte.Data
{
    public class VendasRepository
    {
        private readonly DBContext _dbContext;

        // Armazena a lista de vendas em cache
        private Task<List<Vendas>>? _listaVendasTask;

        public VendasRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Busca assíncrona da lista de vendas
        public async Task<List<Vendas>> ObterListaVendasAsync()
        {
            DateTime inicio = DateTime.Now;

            // Se já foi carregado antes, retorna diretamente
            if (_listaVendasTask != null)
                return await _listaVendasTask;

            _listaVendasTask = CarregarVendasDoBancoAsync();

            DateTime fim = DateTime.Now;
            TimeSpan duracao = fim - inicio;

            Console.WriteLine($"Duração vendas: {duracao.TotalMilliseconds} ms");

            return await _listaVendasTask;
        }

        private async Task<List<Vendas>> CarregarVendasDoBancoAsync()
        {
            var listaVendas = new List<Vendas>();

            using var connection = _dbContext.CreateConnection(); // usando normal
            await ((System.Data.Common.DbConnection)connection).OpenAsync();

            using var command = connection.CreateCommand(); // usando normal
            command.CommandText = @"
            
            SELECT
                v.id AS venda_id,
                v.sku_marketplace_id,
                v.valor_liquido AS valor_vendas,
                cp.porcentagem,
                cp.porcentagem_especiais              
                FROM vendas v
                LEFT JOIN  comissoes_periodo cp  ON v.data  BETWEEN cp.data_inicio AND cp.data_fim;
            ";

            await using var reader = await ((System.Data.Common.DbCommand)command).ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var registro = Vendas.MapearRegistro((NpgsqlDataReader)reader);
                listaVendas.Add(registro);
            }

            return listaVendas;
        }


    }
}