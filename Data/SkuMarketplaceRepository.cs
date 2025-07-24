using Npgsql;
using DashboardTrilhaEsporte.Domain.Entities;
using System.Data.Common;

namespace DashboardTrilhaEsporte.Data
{
    public class SkuMarketplaceRepository
    {
        private readonly DBContext _dbContext;

        // Armazena os dados carregados em memória
        private Task<List<SkuMarketplace>>? _listaSkuMarketplace;

        public SkuMarketplaceRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SkuMarketplace>> ObterListaMarketplaceAsync()
        {
            DateTime inicio = DateTime.Now;

            if (_listaSkuMarketplace != null)
                return await _listaSkuMarketplace;

            _listaSkuMarketplace = CarregarMarketplaceDoBancoAsync();
            DateTime fim = DateTime.Now;
            TimeSpan duracao = fim - inicio;

            Console.WriteLine($"Duração skumarketplace data: {duracao.TotalMilliseconds} ms");

            return await _listaSkuMarketplace;
        }

        private async Task<List<SkuMarketplace>> CarregarMarketplaceDoBancoAsync()
        {
            var listaMarketplace = new List<SkuMarketplace>();

            using var connection = _dbContext.CreateConnection();
            await ((DbConnection)connection).OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"
               SELECT
                    mk.nome AS marketplace,
                    sm.id AS sku_marketplace_id,
                    sm.numero_pedido,

                    -- Valor do pedido:
                    COALESCE(v.valor_liquido, 0) AS valor_liquido,

                    -- Data e porcentagem da comissão (agora da comissoes_periodo):
                
                    cp.porcentagem,
                    cp.porcentagem_especiais,

                    -- Data e porcentagem da comissão:
                    cp2.data AS data_comissao,

                    -- Cálculo da comissão:
                    (cp.porcentagem * COALESCE(v.valor_liquido, 0)) AS comissao_calc,

                    -- Informações de evento (centauro):
                    ec.tipo_evento,
                    COALESCE(ec.repasse_liquido_evento, 0) AS valor_final,
                    v.data AS data_evento,
                    ec.data_repasse AS data_ciclo

                FROM sku_marketplace sm
                LEFT JOIN marketplaces mk
                    ON sm.marketplace_id = mk.id
                LEFT JOIN vendas v
                    ON sm.id = v.sku_marketplace_id
                LEFT JOIN comissoes_periodo cp
                    ON v.data BETWEEN cp.data_inicio AND cp.data_fim
                LEFT JOIN evento_centauro ec
                    ON ec.numero_pedido = sm.numero_pedido
                LEFT JOIN comissoes_pedido cp2
                    ON sm.id = cp2.sku_marketplace_id;

            ";

            await using var reader = await ((DbCommand)command).ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var registro = SkuMarketplace.MapearRegistro((NpgsqlDataReader)reader);
                listaMarketplace.Add(registro);
            }

            return listaMarketplace;
        }
    }
}
