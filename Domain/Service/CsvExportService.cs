using DashboardTrilhaEsporte.Domain.DTOs;
using DashboardTrilhaEsporte.Domain.Entities;
using System.Text;
using System.Globalization;
using ClosedXML.Excel;
using DashboardTrilhaEsporte.Enums;



namespace DashboardTrilhaEsporte.Domain.Service{


    public class CsvExportService
    {
        // Método responsável por exportar os dados para um arquivo CSV
        public static string GerarCsvSkuMarketplaceDTO(List<SkuMarketplaceDTO> lista){
            var sb = new StringBuilder();
            // Adiciona o cabeçalho do CSV
            sb.AppendLine("Id;Marketplace;Numero Pedido; Tipo do Evento ; Valor Final ;Valor Pedido;Porcentagem; Valor Comissão; data da Comissão;Data do Evento ; Data do Ciclo; Erro de Comissão; Erro de Valor final Negativo ; Erro de Falta de Comissao ; Falta de Data de Comissão; Erro de Devolução"); // exemplo de erros
           
            foreach (var item in lista)
            {
                    sb.AppendLine(item.ToString());
            }

           return sb.ToString();
        }

        public static string GerarCsvAnymarketDTO(List<AnymarketDTO> lista)
        {
            var sb = new StringBuilder();
            // Adiciona o cabeçalho do CSV
            sb.AppendLine("skuMarktplace ID;Numero Pedido; Valor da venda(TD_SkuMarketplace) ; Valor da Venda(TD_Vendas); Tipo do Evento ; Tipo de Erro ;"); // exemplo de erros
           
            foreach (var item in lista)
            {
                    sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }

        public static string GeraCsvResumoFinaceiroDTO(List<ResumoFinanceiroDTO> lista)
        {
            var sb = new StringBuilder();
            // Adiciona o cabeçalho do CSV
            sb.AppendLine("skuMarktplace ID;Marketplace;Numero Pedido;Data do Pedido; Valor total do Produto; Comissão Esperada; Valor Recebido; Valor a Receber; Valor Descontado; Desconto Frete; Situação do Pagamento;");

           
            foreach (var item in lista)
            {
                    sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }

        public static string GerarCsvSkuMarketplaceDescontarHouverDTO(List<SkuMarketplaceDTO> lista){

            var sb = new StringBuilder();
            // Adiciona o cabeçalho do CSV
            sb.AppendLine("skuMarktplace ID;Numero Pedido;Valor Pedido; Valor Final; Tipo de Evento ; Diferença entre os Valores;Data do Evento ; Data do Ciclo;"); // exemplo de erros
           
            foreach (var item in lista)
            {
                    sb.AppendLine(DescontarToString(item.skuMarketplace));
            }

           return sb.ToString();
        }

        public static string DescontarToString(SkuMarketplace item){
         var culture = new CultureInfo("pt-BR");

        decimal deferenca = item.valorFinal + item.valorLiquido;

        return $"{item.skuMarketplaceId};" +
           $"{item.numeroPedido};" +
           $"{item.valorLiquido.ToString("N2", culture)};" +
           $"{item.valorFinal.ToString("N2", culture)};" +
           $"{item.tipoEventoNormalizado.GetDescription()};" +
           $"{deferenca.ToString("N2", culture)};" +
           $"{(item.dataEvento.HasValue ? item.dataEvento.Value.ToString("dd/MM/yyyy") : "-")};" +
           $"{(item.dataCiclo.HasValue ? item.dataCiclo.Value.ToString("dd/MM/yyyy") : "-")}";
        }


         public static string GerarCsvScraping(List<Scraping> lista)
        {
                var sb = new StringBuilder();

                sb.AppendLine("Id;Marketplace;SKU;Nome de Venda;Nome Oficial;Link Ativo;Sem Estoque;Preço;Descrição de Erro;Data de Criação");

                foreach (var item in lista)
                {
                    sb.AppendLine(item.ToString());
                }

                return sb.ToString();
        }

        // Método responsável por converter o CSV para XLSX
        public static byte[] CsvToXlsx(string csvContent, char separator = ';')
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Planilha");

            using var reader = new StringReader(csvContent);
            string? line;
            int row = 1;

            while ((line = reader.ReadLine()) != null)
            {
                var columns = line.Split(separator);
                for (int col = 0; col < columns.Length; col++)
                {
                    worksheet.Cell(row, col + 1).Value = columns[col];
                }
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        


    }    


}