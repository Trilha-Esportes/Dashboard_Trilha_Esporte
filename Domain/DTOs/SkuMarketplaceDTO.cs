
using DashboardTrilhasEsporte.Enums;
using DashboardTrilhasEsporte.Domain.Service;
using DashboardTrilhasEsporte.Domain.Entities;

namespace DashboardTrilhasEsporte.Domain.DTOs
{
    public class SkuMarketplaceDTO
    {
        public SkuMarketplace skuMarketplace { get; set; }

        public List<Erros> listaErros { get; set; } = new List<Erros>();




        public SkuMarketplaceDTO(SkuMarketplace marketplace)
        {
            this.skuMarketplace = marketplace;
            listaErros.AddRange(SkuMarketplaceValidator.BuscaErros(marketplace));

        }


        public static List<SkuMarketplaceDTO> MapearDTOs(List<SkuMarketplace> skuMarketplaces)
        {
            List<SkuMarketplace> skuMarketplacesSemduplicacao = skuMarketplaces.Distinct().ToList();

            List<SkuMarketplaceDTO> SkuMarketplaceDTOs = new List<SkuMarketplaceDTO>();

            SkuMarketplaceValidator.ChecarDescontarHove(skuMarketplacesSemduplicacao);

            foreach (var marketplace in skuMarketplacesSemduplicacao)
            {
                SkuMarketplaceDTO dto = new SkuMarketplaceDTO(marketplace);

                if (marketplace.erroDevolucao)
                {
                    dto.listaErros.Add(Erros.ErroDevolucao);
                }
                SkuMarketplaceDTOs.Add(dto);
            }

            return SkuMarketplaceDTOs;
        }


    }
}