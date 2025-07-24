
using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.Service;
using DashboardTrilhaEsporte.Data.Entities;
using System.Text;


// Class responsável por construir a estrutura representação (DTO) do SkuMarketplace
// O DTO é utilizado para transferir os dados do banco para interface depois de realizar o preprocessamento.
// Ele contém a lista de erros encontrados durante o processamento do SkuMarketplace
namespace DashboardTrilhaEsporte.Domain.DTOs
{
    public class SkuMarketplaceDTO
    {
        public SkuMarketplace skuMarketplace { get; set; }


        // Amazenando os erros encontrados durante o processamento do SkuMarketplace
        public List<Erros> listaErros { get; set; } = new List<Erros>();




        public SkuMarketplaceDTO(SkuMarketplace marketplace)
        {
            this.skuMarketplace = marketplace;
            listaErros.AddRange(SkuMarketplaceValidator.BuscaErros(marketplace));

        }



        // Método responsável por mapear a lista de SkuMarketplace para uma lista de SkuMarketplaceDTO
        // Existe duplicação de dados no banco, então é necessário remover os duplicados 
        // Checar se os erros em um  SkuMarketplace      
        public static List<SkuMarketplaceDTO> MapearDTOs(List<SkuMarketplace> skuMarketplaces)
        {
            // Remove duplicados da lista de SkuMarketplace
            List<SkuMarketplace> skuMarketplacesSemduplicacao = skuMarketplaces.Distinct().ToList();

            List<SkuMarketplaceDTO> SkuMarketplaceDTOs = new List<SkuMarketplaceDTO>();

            // Verifica se o SkuMarketplace tem erro de devolução
            SkuMarketplaceValidator.ChecarDescontarHove(skuMarketplacesSemduplicacao);

            foreach (var marketplace in skuMarketplacesSemduplicacao)
            {
                SkuMarketplaceDTO dto = new SkuMarketplaceDTO(marketplace);

                // Como é necessário verificar se o SkuMarketplace os dados completos 
                // para inferir os erros de devolução essa etapa é feita internament em SkuMarketplace 
                // e transferida para o DTO nesse trecho
                if (marketplace.erroDevolucao)
                {
                    dto.listaErros.Add(Erros.ErroDevolucao);
                }
                SkuMarketplaceDTOs.Add(dto);
            }

            return SkuMarketplaceDTOs;
        }


        public override string ToString()
        {
            var strErros = new StringBuilder();

            foreach (Erros erro in Enum.GetValues(typeof(Erros)))
            {
                strErros.Append(listaErros.Contains(erro) ? "Sim;" : "Não;");
            }

            return $"{skuMarketplace.ToString()};{strErros.ToString().TrimEnd(';')}";
        }

    }
}