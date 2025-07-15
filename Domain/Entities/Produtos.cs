/*
class Produto(Base):
    __tablename__ = "produtos"

    id = Column(Integer, primary_key=True, index=True)
    nome = Column(String(255))
    sku_anymarket = Column(String(50))
    sku_marketplace = Column(String(50))
    marketplace_id = Column(Integer)
    
    # Relacionamento com scraping
    scrapings = relationship("Scraping", back_populates="produto")
  
*/

using Npgsql;

namespace DashboardTrilhaEsporte.Domain.Entities
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