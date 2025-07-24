using Npgsql;

// Class responsável por representar a entidade Vendas
// Ela contém todos os parametros que vem da consulta SQL
namespace DashboardTrilhaEsporte.Domain.Entities{
    public class Vendas
    {
        public int vendaId { get; set; }
        public String skuMarketplaceId { get; set; } = String.Empty;
        public Decimal valorVenda { get; set; }

        public Decimal procentagemPeriodo { set; get; }

        public Vendas()
        {
            
        }


        // Método responsável por mapear o registro do banco de dados para um objeto Vendas
        public static Vendas MapearRegistro(NpgsqlDataReader reader)
        {
            Vendas vendas = new Vendas
            {
                skuMarketplaceId = reader["sku_marketplace_id"] is DBNull ? string.Empty : Convert.ToString(reader["sku_marketplace_id"]) ?? string.Empty,
                vendaId = reader["venda_id"] is DBNull ? 0 : Convert.ToInt32(reader["venda_id"]),
                valorVenda = reader["valor_vendas"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_vendas"]),
                procentagemPeriodo = reader["porcentagem"] is DBNull ? 0 : Convert.ToDecimal(reader["porcentagem"]),
            };
            return vendas;
        }


        //Todos esse métosdos abaixo são usados pelo distinct para comparar os objetos

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