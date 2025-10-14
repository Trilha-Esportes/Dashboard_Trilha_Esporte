using DashboardTrilhaEsporte.Enums;
using Npgsql;
using System;
using System.Globalization;

namespace DashboardTrilhaEsporte.Data.Entities
{

    // Essa classe representa a entidade SkuMarketplace
    // Ela contém todos os parametros que vem da consulta SQL
    // Somados de parametro que representa erro de devolução

    public class SkuMarketplace : IEquatable<SkuMarketplace>
    {
        public String? marketplace { get; private set; }
        public String skuMarketplaceId { get; private set; } = String.Empty;
        public long marketplaceId { get; private set; }

        public String? numeroPedido { get; private set; }
        public Decimal valorLiquido { get; private set; }
        public DateTime? dataComissao { get; private set; }
        public Decimal porcentagem { get; private set; }
        public Decimal comissao { get; private set; }
        public Eventos tipoEventoNormalizado { get; private set; }
        public Decimal valorFinal { get; private set; }
        public DateTime? dataEvento { get; private set; }
        public DateTime? dataCiclo { get; private set; }

        public List<Decimal> porcentagemPeriodoEspecial { get; private set; } = new List<decimal>();


        public Boolean erroDevolucao { get; set; }

        private static string RemoverAcentos(string texto)
        {
            var normalized = texto.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }


        // Método responsável por normalizar o tipo de evento
        // Ele recebe uma string e retorna um valor do enum Eventos
        public static Eventos normalizarTipoEvento(string evento)
        {
            if (string.IsNullOrWhiteSpace(evento))
                return Eventos.Desconhecido;

            // Normalização básica
            evento = evento.Trim().ToLowerInvariant();

            // Remove acentos e espaços/hífens duplicados
            evento = RemoverAcentos(evento)
                .Replace("--", "-")
                .Replace("  ", " ")
                .Replace(" - ", "-")
                .Trim();

            // Dicionário de correspondência
            var mapping = new Dictionary<string, Eventos>(StringComparer.OrdinalIgnoreCase)
    {
        // ========================
        // REPASSE NORMAL
        // ========================
        { "repasse normal", Eventos.RepasseNormal },
        { "repasse-normal", Eventos.RepasseNormal },
        { "repassar normal", Eventos.RepasseNormal },
        { "repassar-normal", Eventos.RepasseNormal },
        { "repassse normal", Eventos.RepasseNormal },
        { "repassse-normal", Eventos.RepasseNormal },

        // ========================
        // NÃO REPASSAR
        // ========================
        { "nao repassar", Eventos.NaoRepassar },
        { "nao-repassar", Eventos.NaoRepassar },
        { "nao repasse", Eventos.NaoRepassar },
        { "nao-repasse", Eventos.NaoRepassar },

        // ========================
        // DESCONTAR HOUVE / HOVE
        // ========================
        { "descontar houve", Eventos.DescontarHoveHouve },
        { "descontar-houve", Eventos.DescontarHoveHouve },
        { "descontar hove", Eventos.DescontarHoveHouve },
        { "descontar-hove", Eventos.DescontarHoveHouve },

        // ========================
        // DESCONTAR REVERSA CENTAURO ENVIOS
        // ========================
        { "descontar reversa centauro envios", Eventos.DescontarReversaCentauroEnvios },
        { "descontar-reversa-centauro-envios", Eventos.DescontarReversaCentauroEnvios },
        { "descontar reverse centauro envios", Eventos.DescontarReversaCentauroEnvios }, // erro comum
        { "descontar-reverse-centauro-envios", Eventos.DescontarReversaCentauroEnvios },
        { "descontar reserva", Eventos.DescontarReversaCentauroEnvios }, // erro comum "reserva"

        // ========================
        // DESCONTAR RETROATIVOS
        // ========================
        { "descontar retroativo", Eventos.DescontarRetroativo },
        { "descontar-retroativo", Eventos.DescontarRetroativo },
        { "descontar retroativo sac", Eventos.DescontarRetroativo },
        { "descontar-retroativo-sac", Eventos.DescontarRetroativo },
        { "descontar retroativos", Eventos.DescontarRetroativo },
        { "descontar-retroativos", Eventos.DescontarRetroativo },
        { "descontar retroativos sac", Eventos.DescontarRetroativo },
        { "descontar-retroativos-sac", Eventos.DescontarRetroativo },

        // ========================
        // AJUSTE DE CICLO
        // ========================
        { "ajuste de ciclo", Eventos.AjusteDeCiclo },
        { "ajuste ciclo", Eventos.AjusteDeCiclo },
        { "ajuste-ciclo", Eventos.AjusteDeCiclo },
        { "ajuste deciclo", Eventos.AjusteDeCiclo }, // erro de espaço
        { "ajusteciclo", Eventos.AjusteDeCiclo },
    };

            // Normaliza para procurar sem erro de acento/hífen
            if (mapping.TryGetValue(evento, out var valorNormalizado))
                return valorNormalizado;

            // Procura fuzzy simples — caso contenha palavras-chave conhecidas
            if (evento.Contains("repasse") || evento.Contains("repassar"))
                return Eventos.RepasseNormal;
            if (evento.Contains("nao repassar") || evento.Contains("não repassar"))
                return Eventos.NaoRepassar;
            if (evento.Contains("descontar") && evento.Contains("hou"))
                return Eventos.DescontarHoveHouve;
            if (evento.Contains("reversa") || evento.Contains("reverse") || evento.Contains("reserva"))
                return Eventos.DescontarReversaCentauroEnvios;
            if (evento.Contains("retroativo"))
                return Eventos.DescontarRetroativo;
            if (evento.Contains("ajuste") && evento.Contains("ciclo"))
                return Eventos.AjusteDeCiclo;

            // Caso nada corresponda
            return Eventos.Outros;
        }



        // Método responsável por mapear o registro retornado do banco de dados 
        // para um objeto SkuMarketplace
        public static SkuMarketplace MapearRegistro(NpgsqlDataReader reader)
        {
            SkuMarketplace marketplace = new SkuMarketplace
            {
                marketplace = reader["marketplace"]?.ToString() ?? string.Empty,

                skuMarketplaceId = reader["sku_marketplace_id"] is DBNull
                    ? string.Empty
                    : Convert.ToString(reader["sku_marketplace_id"]) ?? string.Empty,
                marketplaceId = reader["marketplace_Id"] is DBNull ? 0 : Convert.ToInt64(reader["marketplace_Id"]),

                numeroPedido = reader["numero_pedido"]?.ToString() ?? string.Empty,
                valorFinal = reader["valor_final"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_final"]),
                dataComissao = reader["data_comissao"] is DBNull ? null : Convert.ToDateTime(reader["data_comissao"]),
                porcentagem = reader["porcentagem"] is DBNull ? 0 : Convert.ToDecimal(reader["porcentagem"]),
                comissao = reader["comissao_calc"] is DBNull ? 0 : Convert.ToDecimal(reader["comissao_calc"]),
                dataEvento = reader["data_evento"] is DBNull ? null : Convert.ToDateTime(reader["data_evento"]),
                dataCiclo = reader["data_ciclo"] is DBNull ? null : Convert.ToDateTime(reader["data_ciclo"]),
                valorLiquido = reader["valor_liquido"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_liquido"]),
                porcentagemPeriodoEspecial = reader["porcentagem_especiais"] is DBNull
                    ? new List<decimal>()
                    : ConverterParaPorcentagens(Convert.ToString(reader["porcentagem_especiais"]) ?? string.Empty)

            };
            string tipoEvento = reader["tipo_evento"]?.ToString() ?? string.Empty;
            marketplace.tipoEventoNormalizado = SkuMarketplace.normalizarTipoEvento(tipoEvento);
            marketplace.erroDevolucao = false;
            return marketplace;
        }



        public static List<decimal> ConverterParaPorcentagens(string entrada)
        {
            var resultado = new List<decimal>();

            if (string.IsNullOrWhiteSpace(entrada))
                return resultado;

            var partes = entrada.Split(',');

            foreach (var parte in partes)
            {
                if (decimal.TryParse(parte.Trim(), out decimal valor))
                {
                    resultado.Add(valor);
                }
                else
                {
                    resultado.Add(0m); // ou apenas ignore: continue;
                }
            }

            return resultado;
        }



        //Todos esse métosdos abaixo são usados pelo distinct para comparar os objetos

        public bool Equals(SkuMarketplace? other)
        {
            if (other is null) return false;

            return skuMarketplaceId == other.skuMarketplaceId
                && marketplace == other.marketplace
                && numeroPedido == other.numeroPedido
                && valorLiquido == other.valorLiquido
                && dataComissao == other.dataComissao
                && porcentagem == other.porcentagem
                && comissao == other.comissao
                && tipoEventoNormalizado == other.tipoEventoNormalizado
                && valorFinal == other.valorFinal
                && dataEvento == other.dataEvento
                && dataCiclo == other.dataCiclo;
        }
        public override bool Equals(object? obj) => Equals(obj as SkuMarketplace);

        // Método responsável por comparar dois objetos SkuMarketplace (coleções)
        public override int GetHashCode()
        {
            var hash1 = HashCode.Combine(skuMarketplaceId, marketplace, numeroPedido, valorLiquido);
            var hash2 = HashCode.Combine(dataComissao, porcentagem, comissao, tipoEventoNormalizado);
            var hash3 = HashCode.Combine(valorFinal, dataEvento, dataCiclo);

            return HashCode.Combine(hash1, hash2, hash3);
        }

        public override string ToString()
        {
            var culture = new CultureInfo("pt-BR");

            return $"{skuMarketplaceId};" +
                   $"{marketplace};" +
                   $"{numeroPedido};" +
                   $"{tipoEventoNormalizado.GetDescription()};" +
                   $"{valorFinal.ToString("N2", culture)};" +
                   $"{valorLiquido.ToString("N2", culture)};" +
                   $"{porcentagem.ToString("N2", culture)};" +
                   $"{comissao.ToString("N2", culture)};" +
                   $"{(dataComissao.HasValue ? dataComissao.Value.ToString("dd/MM/yyyy") : "-")};" +
                   $"{(dataEvento.HasValue ? dataEvento.Value.ToString("dd/MM/yyyy") : "-")};" +
                   $"{(dataCiclo.HasValue ? dataCiclo.Value.ToString("dd/MM/yyyy") : "-")}";
        }


        public void SetProcentagem(decimal procentagem)
        {
            if (porcentagem <= 1 && procentagem >= 0)
                this.porcentagem = porcentagem;
        }



    }


}
