using Npgsql;

namespace DashboardTrilhasEsporte.Domain
{
    public class Vendas
    {
        public int vendaId { get; set; }
        public int skuMarketplaceId { get; set; }
        public decimal valorVenda { get; set; }

        public static Vendas MapearRegistro(NpgsqlDataReader reader)
        {
            Vendas vendas = new Vendas
            {
                skuMarketplaceId = reader["sku_marketplace_id"] is DBNull ? 0 : Convert.ToInt32(reader["sku_marketplace_id"]),
                vendaId = reader["venda_id"] is DBNull ? 0 : Convert.ToInt32(reader["venda_id"]),
                valorVenda = reader["valor_vendas"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_vendas"]),

            };
            return vendas;
        }
    }
}