

using Npgsql;

namespace DashboardTrilhaEsporte.Data.Entities
{
    public class Produto
    {
        public long id { get; set; }
        public String name { get; set; } = String.Empty;
        public String skuAnymarket { get; set; } = String.Empty;

        public String skuMarketplace { get; set; } = String.Empty;


        public static Produto MapearRegistro(NpgsqlDataReader reader)
        {
            Produto produto = new Produto
            {
                id = reader["id"] is DBNull ? 0 : Convert.ToInt64(reader["id"]),
                name = reader["nome"]?.ToString() ?? string.Empty,
                skuAnymarket = reader["sku_anymarket"]?.ToString() ?? string.Empty,
                skuMarketplace = reader["sku_marketplace"]?.ToString() ?? string.Empty,
            };

            return produto;
        }

    }
}