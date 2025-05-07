using Npgsql;

using DashboardTrilhasEsporte.Domain.Entities;
namespace DashboardTrilhasEsporte.Data
{
    public class SkuMarketplaceRepository
    {
        private readonly DBContext _dbContext;

        Task<List<SkuMarketplace>> _listaSkuMarketplace;

        public SkuMarketplaceRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<SkuMarketplace>> ObterlistaMarketplace()

        {

            if (_listaSkuMarketplace== null)
            {
                List<SkuMarketplace> listaMarketplace = new List<SkuMarketplace>();

                using (var connection = _dbContext.CreateConnection())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            mk.nome AS marketplace,
                            sm.id AS sku_marketplace_id,
                            sm.numero_pedido,

                            -- Valor do pedido (universal) buscado da tabela vendas:
                            COALESCE(v.valor_liquido, 0) AS valor_liquido,

                            -- Data e porcentagem da comissão:
                            cp.data AS data_comissao,
                            cp.porcentagem,

                            -- Cálculo da comissão baseado no valor de 'vendas':
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
                        LEFT JOIN comissoes_pedido cp
                            ON sm.id = cp.sku_marketplace_id
                        LEFT JOIN evento_centauro ec
                            ON ec.numero_pedido = sm.numero_pedido;

                    ";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var registro = SkuMarketplace.MapearRegistro((NpgsqlDataReader)reader);
                            listaMarketplace.Add(registro);
                        }
                    }
                }

                this._listaSkuMarketplace = Task.FromResult(listaMarketplace);

            }

            return this._listaSkuMarketplace;
        }

    }
}