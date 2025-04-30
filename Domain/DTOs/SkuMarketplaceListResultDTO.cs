

using DashboardTrilhasEsporte.Enums;
using Microsoft.VisualBasic;


namespace DashboardTrilhasEsporte.Domain
{
        public class SkuMarketplaceListResultDTO
        {
                public List<SkuMarketplaceDTO> skuMarketplaceDTOs { get; set; }
                public decimal valorFinalVendas { get; set; }
                public int quantidadeVendasErroComissao { get; set; }
                public int quantidadeVendasErroValorFinalNegativo { get; set; }
                public int quantidadeVendasErroFaltaDeComisao { get; set; }
                public int quantidadeVendasErroFaltaDataComissao { get; set; }
                public int quantidadeVendasErroDevolucao { get; set; }
                public int quantidadeVendasErroValoresDivergentes { get; set; }
                public int quantidadeTotalRegistro { get; set; }

                public int quantidadeTotalErros { get; set; }

                public decimal somatorioValorFinal { get; set; }

                DateTime? dataComissaoInicial { get; set; }

                DateTime? dataComissaoFinal { get; set; }

                DateTime? dataCicloInicial { get; set; }

                DateTime? dataCicloFinal { get; set; }

                public Dictionary<string, double> ObterErros()
                {
                        string[] labels = {
                                "Erro no valor da Comissão",
                                "Valor Final Negativo",
                                "Erro na Data da Comissão",
                                "Falta de valor na Comisao",
                                "Erro na Devolucao",
                                "Valores Divergentes"
                        };

                        double[] erros = {
                                this.quantidadeVendasErroComissao,
                                this.quantidadeVendasErroValorFinalNegativo,
                                this.quantidadeVendasErroFaltaDataComissao,
                                this.quantidadeVendasErroFaltaDeComisao,
                                this.quantidadeVendasErroDevolucao,
                                this.quantidadeVendasErroValoresDivergentes
                        };

                        var resultado = new Dictionary<string, double>();
                        for (int i = 0; i < labels.Length; i++)
                        {
                                resultado[labels[i]] = erros[i];
                        }

                        return resultado;
                }


                public SkuMarketplaceListResultDTO(List<SkuMarketplace> skuMarketplaces)
                {
                        this.skuMarketplaceDTOs = SkuMarketplaceDTO.MapearDTOs(skuMarketplaces);

                        this.quantidadeTotalRegistro = this.skuMarketplaceDTOs.Count;

                        this.SomarValorFinal();
                        this.ContarErros();
                        this.ObterIntervalosDatas();
                }


                public SkuMarketplaceListResultDTO(List<SkuMarketplaceDTO> skuMarketplacesDto)
                {
                        this.skuMarketplaceDTOs = skuMarketplacesDto;
                        this.quantidadeTotalRegistro = this.skuMarketplaceDTOs.Count;
                        this.SomarValorFinal();
                        this.ContarErros();
                        this.ObterIntervalosDatas();
                }

                private void SomarValorFinal()
                {
                        this.somatorioValorFinal = this.skuMarketplaceDTOs
                            .Sum(v => v.skuMarketplace.valorFinal);
                }

                private void ContarErros()
                {
                        quantidadeVendasErroComissao = skuMarketplaceDTOs.Count(v => v.listaErros.Contains(Erros.ErroComissao));
                        quantidadeVendasErroValorFinalNegativo = skuMarketplaceDTOs.Count(v => v.listaErros.Contains(Erros.ValorFinalNegativo));
                        quantidadeVendasErroFaltaDeComisao = skuMarketplaceDTOs.Count(v => v.listaErros.Contains(Erros.FaltaDeComisao));
                        quantidadeVendasErroFaltaDataComissao = skuMarketplaceDTOs.Count(v => v.listaErros.Contains(Erros.FaltaDataComissao));
                        quantidadeVendasErroDevolucao = skuMarketplaceDTOs.Count(v => v.listaErros.Contains(Erros.ErroDevolucao));
                        quantidadeVendasErroValoresDivergentes = skuMarketplaceDTOs.Count(v => v.listaErros.Contains(Erros.ValoresDivergentes));

                        quantidadeTotalErros = quantidadeVendasErroComissao + quantidadeVendasErroValorFinalNegativo + quantidadeVendasErroFaltaDeComisao
                                                + quantidadeVendasErroFaltaDataComissao + quantidadeVendasErroDevolucao + quantidadeVendasErroValoresDivergentes;
                }

                public void ObterIntervalosDatas()
                {
                        var comissoes = this.skuMarketplaceDTOs
                            .Where(x => x.skuMarketplace.dataComissao.HasValue)
                            .Select(x => x.skuMarketplace.dataComissao.Value)
                            .ToList();

                        var ciclos = this.skuMarketplaceDTOs
                            .Where(x => x.skuMarketplace.dataCiclo.HasValue)
                            .Select(x => x.skuMarketplace.dataCiclo.Value)
                            .ToList();

                        this.dataComissaoInicial = comissoes.Any() ? comissoes.Min() : null;
                        this.dataComissaoFinal = comissoes.Any() ? comissoes.Max() : null;
                        this.dataCicloInicial = ciclos.Any() ? ciclos.Min() : null;
                        this.dataCicloFinal = ciclos.Any() ? ciclos.Max() : null;
                }

        }
}
