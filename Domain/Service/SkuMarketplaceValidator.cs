using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.Entities;

// Essa classe é responsável por validar os dados do SkuMarketplace
// Ela verifica se os dados estão corretos e se não há erros

namespace DashboardTrilhaEsporte.Domain.Service
{
    public class SkuMarketplaceValidator
    {
        // Esse médoto é responsável por verificar se há erros nos dados do SkuMarketplace
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

        // Método responsável por verificar  a consistencia do valor de comissão
     private static Erros? checarErroComissao(SkuMarketplace marketplace)
        {
            if (marketplace.porcentagem != 0)
            {
                decimal valorCalculado = marketplace.valorLiquido - (marketplace.valorLiquido * marketplace.porcentagem);

                if (Math.Abs(valorCalculado - marketplace.valorFinal) > 0.05m)
                {
                    // Tenta validar com as porcentagens especiais
                    if (marketplace.porcentagemPeriodoEspecial != null && marketplace.porcentagemPeriodoEspecial.Count > 0)
                    {
                        foreach (var p in marketplace.porcentagemPeriodoEspecial)
                        {
                            decimal valorAlternativo = marketplace.valorLiquido - (marketplace.valorLiquido * (p/100));

                            if (Math.Abs(valorAlternativo - marketplace.valorFinal) <= 0.05m)
                            {
                                // Encontrou uma porcentagem especial compatível, atualiza e retorna sucesso
                                marketplace.porcentagem = p/100;
                                return null;
                            }
                        }
                    }

                    // Nenhuma porcentagem especial bateu com o valor final
                    return Erros.ErroComissao;
                }
            }

            return null;
        }





        // Método responsável por verificar se o valor do repasse normal e o valor do descontar hove são iguais
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