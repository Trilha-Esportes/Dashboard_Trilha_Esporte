using DashboardTrilhasEsporte.Enums;
using DashboardTrilhasEsporte.Domain;

namespace DashboardTrilhasEsporte.Service
{
    public class SkuMarketplaceValidator
    {
        public static List<Erros> BuscaErros(SkuMarketplace marketplace)
        {
            List<Erros> listaErros = new List<Erros>();

            if (marketplace.tipoEventoNormalizado == Eventos.RepasseNormal)
            {
                Erros? erroComissao = SkuMarketplaceValidator.checarErroComissao(marketplace);

                if (marketplace.valorFinal < 0)
                {
                    listaErros.Add(Erros.ValorFinalNegativo);
                }
                if (marketplace.porcentagem == 0)
                {
                    listaErros.Add(Erros.FaltaDeComisao);
                }
                if (marketplace.dataComissao == null)
                {
                    listaErros.Add(Erros.FaltaDataComissao);
                }

                if (erroComissao.HasValue)
                {
                    listaErros.Add(Erros.ErroComissao);
                }

            }


            return listaErros;
        }
        private static Erros? checarErroComissao(SkuMarketplace marketplace)
        {
            if ((marketplace.porcentagem != 0))
            {

                Decimal valorCalculado = (marketplace.valorLiquido - (marketplace.valorLiquido * marketplace.porcentagem));

                if (Math.Abs(valorCalculado - marketplace.valorFinal) > 0.05m)
                {
                    return Erros.ErroComissao;
                }
            }
            return null;
        }

        public static void ChecarDescontarHove(List<SkuMarketplace> skuMarketplaces)
        {
            var grupos = skuMarketplaces
                .GroupBy(v => v.numeroPedido);

            foreach (var grupo in grupos)
            {
                var repasseNormal = grupo.FirstOrDefault(v => v.tipoEventoNormalizado == Eventos.RepasseNormal);
                var descontarHove = grupo.FirstOrDefault(v => v.tipoEventoNormalizado == Eventos.DescontarHoveHouve);

                if (repasseNormal != null && descontarHove != null)
                {
                    Decimal valor1 = Math.Abs(repasseNormal.valorLiquido);
                    Decimal valor2 = Math.Abs(descontarHove.valorFinal);

                    if (valor1 != valor2)
                    {
                        // Marca o erro apenas no evento Descontar
                        descontarHove.erroDevolucao = true;
                    }
                }
            }
    }

    }
}