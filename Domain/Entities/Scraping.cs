using Npgsql;
using System.Globalization;
using DashboardTrilhaEsporte.Enums;

namespace DashboardTrilhaEsporte.Domain.Entities

{
    public class Scraping
    {
        public int id { get; set; }
        public int idScrapingHistorico { get; set; }
        public int idProduto { get; set; }

        public ScrapingStatus linkAtivo { get; set; }
        public string nomeProduto { get; set; } = string.Empty;
        public decimal precoProduto { get; set; }
        public ScrapingStatus tagSemEstoque { get; set; }
        public string descricaoErro { get; set; } = string.Empty;
        public DateTime dataCriacao { get; set; }

        public string nomeProdutoOficial { get; set; } = string.Empty;
        public string nomeMarketplace { get; set; } = string.Empty;
        public string skuID { get; set; } = string.Empty;

        public static Scraping MapearRegistro(NpgsqlDataReader reader)
        {
            return new Scraping
            {
                id = reader["id"] is DBNull ? 0 : Convert.ToInt32(reader["id"]),
                idScrapingHistorico = reader["id_scraping_historico"] is DBNull ? 0 : Convert.ToInt32(reader["id_scraping_historico"]),
                idProduto = reader["id_produto"] is DBNull ? 0 : Convert.ToInt32(reader["id_produto"]),
                linkAtivo = reader["link_ativo"] is DBNull || !Convert.ToBoolean(reader["link_ativo"])
                    ? ScrapingStatus.DESATIVADO
                    : ScrapingStatus.Ativo,
               
                nomeProduto = reader["nome_de_venda"]?.ToString() ?? string.Empty,
                precoProduto = reader["preco_produto"] is DBNull ? 0 : Convert.ToDecimal(reader["preco_produto"]),
               
                tagSemEstoque = reader["tag_sem_estoque"] is DBNull || !Convert.ToBoolean(reader["tag_sem_estoque"])
                    ? ScrapingStatus.DISPONIVEL
                    : ScrapingStatus.SEMESTOQUE,

                descricaoErro = reader["descricao_erro"]?.ToString() ?? "Sem Erros",
                dataCriacao = reader["data_criacao"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["data_criacao"]),
                nomeProdutoOficial = reader["nome_produto"]?.ToString() ?? string.Empty,
                nomeMarketplace = reader["nome_marketplace"]?.ToString() ?? string.Empty,
                skuID = reader["sku_marketplace"]?.ToString() ?? string.Empty
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Scraping other)
                return false;

            return id == other.id &&
                   idScrapingHistorico == other.idScrapingHistorico &&
                   idProduto == other.idProduto &&
                   linkAtivo == other.linkAtivo &&
                   nomeProduto == other.nomeProduto &&
                   precoProduto == other.precoProduto &&
                   tagSemEstoque == other.tagSemEstoque &&
                   descricaoErro == other.descricaoErro &&
                   dataCriacao == other.dataCriacao &&
                   nomeProdutoOficial == other.nomeProdutoOficial &&
                   nomeMarketplace == other.nomeMarketplace &&
                   skuID == other.skuID;
        }
        public override int GetHashCode()
        {
            int hash1 = HashCode.Combine(id, idScrapingHistorico, idProduto, linkAtivo, nomeProduto, precoProduto, tagSemEstoque);
            int hash2 = HashCode.Combine(descricaoErro, dataCriacao, nomeProdutoOficial, nomeMarketplace, skuID);
            
            return HashCode.Combine(hash1, hash2);
        }


      public override string ToString()
    {
        var culture = new CultureInfo("pt-BR");

            return $"{id};" +
                $"{nomeMarketplace};" +
                $"{skuID};" +
                $"{nomeProduto};" +
                $"{nomeProdutoOficial};" +
                $"{(linkAtivo.GetDescription())};" +
                $"{(tagSemEstoque.GetDescription())};" +
                $"{precoProduto.ToString("N2", culture)};" +
                $"{(string.IsNullOrWhiteSpace(descricaoErro) ? "-" : descricaoErro)};" +
                $"{dataCriacao.ToString("dd/MM/yyyy")}";
        }

    }
}
