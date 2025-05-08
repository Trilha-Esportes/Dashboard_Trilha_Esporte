using Npgsql;

namespace DashboardTrilhasEsporte.Domain.Entities{
    public class Vendas
    {
        public int vendaId { get; set; }
        public string skuMarketplaceId { get; set; }
        public decimal valorVenda { get; set; }

        public static Vendas MapearRegistro(NpgsqlDataReader reader)
        {
            Vendas vendas = new Vendas
            {
                skuMarketplaceId = reader["sku_marketplace_id"] is DBNull ? String.Empty : Convert.ToString(reader["sku_marketplace_id"]),
                vendaId = reader["venda_id"] is DBNull ? 0 : Convert.ToInt32(reader["venda_id"]),
                valorVenda = reader["valor_vendas"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_vendas"]),

            };
            return vendas;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Vendas other)
                return false;

            return vendaId == other.vendaId &&
                skuMarketplaceId == other.skuMarketplaceId &&
                valorVenda == other.valorVenda;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(vendaId, skuMarketplaceId, valorVenda);
        }
    }
}