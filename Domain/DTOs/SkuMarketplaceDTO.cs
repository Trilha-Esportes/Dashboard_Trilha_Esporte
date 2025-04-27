
using DashboardTrilhasEsporte.Enums;
using DashboardTrilhasEsporte.Service;

namespace DashboardTrilhasEsporte.Domain
{
    public class SkuMarketplaceDTO
    {
        public SkuMarketplace skuMarketplace { get; set; }

        public List<Erros> listaErros { get; set; }




        public SkuMarketplaceDTO(SkuMarketplace marketplace)
        {
            this.skuMarketplace = marketplace;
            this.listaErros = new List<Erros>();
            listaErros.AddRange(SkuMarketplaceValidator.BuscaErros(marketplace));

        }


        public static List<SkuMarketplaceDTO> MapearDTOs(List<SkuMarketplace> skuMarketplaces)
        {
            List<SkuMarketplaceDTO> SkuMarketplaceDTOs = new List<SkuMarketplaceDTO>();

            SkuMarketplace.ChecarDescontarHove(skuMarketplaces);

            foreach (var marketplace in skuMarketplaces)
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