

using DashboardTrilhaEsporte.Enums;
using DashboardTrilhaEsporte.Domain.Entities;
using Microsoft.VisualBasic;



// Essa classe amazena as informaçoes estatisticas de lista de ResumoFinanceiroDTO
// Essa informação é usada para construir o dashboard

namespace DashboardTrilhaEsporte.Domain.DTOs
{
        public class SkuMarketplaceDadosDTO
        {
                public List<SkuMarketplaceDTO> skuMarketplaceDTOs { get; set; }

                public Dictionary<Eventos, double>? quantidadeTotalRegistrosPorEvento { get; set; }
                public Dictionary<Erros, double>? quantidadeErrosPorTipo { get; set; }

                public int quantidadeTotalRegistro { get; set; }

                public int quantidadeTotalErros { get; set; }

                public decimal somatorioValorFinal { get; set; }

                public DateTime? dataComissaoInicial { get; set; }

                public DateTime? dataComissaoFinal { get; set; }

                public List<DateTime> dateTimesCiclos { get; set; }


        
                public SkuMarketplaceDadosDTO(List<SkuMarketplace> skuMarketplaces)
                {
                        this.skuMarketplaceDTOs = SkuMarketplaceDTO.MapearDTOs(skuMarketplaces);

                        this.quantidadeTotalRegistro = this.skuMarketplaceDTOs.Count;

                        this.SomarValorFinal();
                        this.ContarErros();
                        this.ObterIntervalosDatas();
                        this.ContarTipoEvento();
                }
                

                // Construtor vazio para inicializar a lista de skuMarketplaceDTOs
                public SkuMarketplaceDadosDTO()
                {
                        this.skuMarketplaceDTOs = new List<SkuMarketplaceDTO>();
                        this.quantidadeTotalRegistro = 0;
                        this.somatorioValorFinal = 0;
                        this.quantidadeTotalErros = 0;
                        this.dataComissaoInicial = null;
                        this.dataComissaoFinal = null;
                        this.dateTimesCiclos = new List<DateTime>();
                }

                // Construtor que recebe uma lista de SkuMarketplaceDTO
                // Extrair as informações estatísticas 
                public SkuMarketplaceDadosDTO(List<SkuMarketplaceDTO> skuMarketplacesDto)
                {
                        this.skuMarketplaceDTOs = skuMarketplacesDto;
                        this.quantidadeTotalRegistro = this.skuMarketplaceDTOs.Count;
                        this.SomarValorFinal();
                        this.ContarErros();
                        this.ContarTipoEvento();
                        this.ObterIntervalosDatas();
                }

                // Metodo responsável por calcular o somatório dos valor final
                private void SomarValorFinal()
                {
                        this.somatorioValorFinal = this.skuMarketplaceDTOs
                            .Sum(v => v.skuMarketplace.valorFinal);
                }

                // Método responsável por contar os erros
                private void ContarErros()
                {
                        if (skuMarketplaceDTOs == null)
                                return;

                        if (quantidadeErrosPorTipo == null)
                                quantidadeErrosPorTipo = new Dictionary<Erros, double>();

                        Erros[] errosParaContar = new[]
                        {
                        Erros.ErroComissao,
                        Erros.ValorFinalNegativo,
                        Erros.FaltaDeComisao,
                        Erros.FaltaDataComissao,
                        Erros.ErroDevolucao
                        };

                        foreach (var erro in errosParaContar)
                        {
                                int quantidade = skuMarketplaceDTOs.Count(v =>
                                v?.listaErros != null && v.listaErros.Contains(erro));

                                quantidadeErrosPorTipo[erro] = quantidade;
                        }

                        quantidadeTotalErros = (int)quantidadeErrosPorTipo.Values.Sum();
                }

                // Metodo responsável por obter os intervalos de datas
                public void ObterIntervalosDatas()
                {
                        var comissoes = this.skuMarketplaceDTOs
                            .Where(x => x.skuMarketplace.dataComissao.HasValue)
                            .Select(x => x.skuMarketplace.dataComissao.Value)
                            .ToList();

                        this.dateTimesCiclos = this.skuMarketplaceDTOs
                        .Where(x => x.skuMarketplace.dataCiclo.HasValue)
                        .Select(x => x.skuMarketplace.dataCiclo.Value.Date) 
                        .Distinct()
                        .OrderByDescending(d => d)
                        .ToList();


                        this.dataComissaoInicial = comissoes.Any() ? comissoes.Min() : null;
                        this.dataComissaoFinal = comissoes.Any() ? comissoes.Max() : null;
                       
                }

                // Método responsável por contar o tipo de evento
                public void ContarTipoEvento()
                {
                        this.quantidadeTotalRegistrosPorEvento = new Dictionary<Eventos, double>();

                        foreach (var item in this.skuMarketplaceDTOs)
                        {
                                Eventos tipo = item.skuMarketplace.tipoEventoNormalizado;

                                if (!quantidadeTotalRegistrosPorEvento.ContainsKey(tipo))
                                        quantidadeTotalRegistrosPorEvento[tipo] = 0;

                                this.quantidadeTotalRegistrosPorEvento[tipo]++;

                        }
                }

                // Método responsável por obter a distribuição de Erros 
                // Essa informação é para cosntrir graficos
                public Dictionary<string, double> ObterErros()
                {
                        Erros[] erros = new[]
                        {
                                Erros.ErroComissao,
                                Erros.ValorFinalNegativo,
                                Erros.FaltaDeComisao,
                                Erros.FaltaDataComissao,
                                Erros.ErroDevolucao
                        };


                        var resultado = new Dictionary<string, double>();

                        foreach (var erro in erros)
                        {
                                if (quantidadeErrosPorTipo != null && quantidadeErrosPorTipo.TryGetValue(erro, out var count))
                                {
                                        resultado.Add(erro.GetDescription(), count);
                                }
                                else
                                {
                                        resultado.Add(erro.GetDescription(), 0);
                                }
                        }

                        return resultado;
                }


                // Método responsável por obter a distribuição de eventos 
                // Essa informação é para cosntrir graficos
                public Dictionary<string, double> ObterDistribuicaoEventos()
                {
                        var resultado = new Dictionary<string, double>();

                        if (quantidadeTotalRegistrosPorEvento == null || !quantidadeTotalRegistrosPorEvento.Any()){
                                Console.WriteLine();
                                return resultado;
                        }
                               

                        double total = this.quantidadeTotalRegistro;
                        if (total == 0)
                                return resultado;

                        foreach (var sku in quantidadeTotalRegistrosPorEvento)
                        {
                                string chave = sku.Key.GetDescription();
                                double porcentagem = (sku.Value);
                                resultado[chave] = porcentagem;
                        }

                        return resultado;
                }





        }
}
