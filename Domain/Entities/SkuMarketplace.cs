using DashboardTrilhasEsporte.Enums;
namespace DashboardTrilhasEsporte.Domain;
using Npgsql;

public class SkuMarketplace
{
    public string? marketplace { get; set; }
    public int skuMarketplaceId { get; set; }
    public string? numeroPedido { get; set; }
    public Decimal valorLiquido { get; set; }
    public DateTime? dataComissao { get; set; }
    public Decimal? porcentagem { get; set; }
    public Decimal? comissao { get; set; }
    public Eventos tipoEventoNormalizado { get; set; }
    public Decimal valorFinal { get; set; }
    public DateTime? dataEvento { get; set; }
    public DateTime? dataCiclo { get; set; }

    public Boolean erroDevolucao { get; set; }

    public static Eventos normalizarTipoEvento(string evento)
    {
        if (string.IsNullOrWhiteSpace(evento))
            return Eventos.Desconhecido;

        evento = evento.Trim().ToLower();

        var mapping = new Dictionary<string, Eventos>
        {
            { "repasse normal", Eventos.RepasseNormal },
            { "repasse - normal", Eventos.RepasseNormal },
            { "repassse normal", Eventos.RepasseNormal },
            { "repassse - normal", Eventos.RepasseNormal },

            { "descontar hove", Eventos.DescontarHoveHouve },
            { "descontar houve", Eventos.DescontarHoveHouve },
            { "descontar - houve", Eventos.DescontarHoveHouve },
            { "descontar - hove", Eventos.DescontarHoveHouve },

            { "descontar reversa centauro envios", Eventos.DescontarReversaCentauroEnvios },
            { "descontar - reversa centauro envios", Eventos.DescontarReversaCentauroEnvios },

            { "ajuste de ciclo", Eventos.AjusteDeCiclo },

            { "descontar retroativo", Eventos.DescontarRetroativo },
            { "descontar - retroativo", Eventos.DescontarRetroativo },
            { "descontar retroativo sac", Eventos.DescontarRetroativo },
            { "descontar - retroativo sac", Eventos.DescontarRetroativo },
            { "descontar retroativos", Eventos.DescontarRetroativo },
            { "descontar - retroativos", Eventos.DescontarRetroativo },
            { "descontar retroativos sac", Eventos.DescontarRetroativo },
            { "descontar - retroativos sac", Eventos.DescontarRetroativo },
        };

        return mapping.TryGetValue(evento, out var valorNormalizado)
            ? valorNormalizado
            : Eventos.Outros;
    }

    public static SkuMarketplace MapearRegistro(NpgsqlDataReader reader)
    {
        SkuMarketplace marketplace = new SkuMarketplace
        {
            marketplace = reader["marketplace"]?.ToString() ?? string.Empty,
            skuMarketplaceId = reader["sku_marketplace_id"] is DBNull ? 0 : Convert.ToInt32(reader["sku_marketplace_id"]),
            numeroPedido = reader["numero_pedido"]?.ToString() ?? string.Empty,
            valorFinal = reader["valor_final"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_final"]),
            dataComissao = reader["data_comissao"] is DBNull ? null : Convert.ToDateTime(reader["data_comissao"]),
            porcentagem = reader["porcentagem"] is DBNull ? 0 : Convert.ToDecimal(reader["porcentagem"]),
            comissao = reader["comissao_calc"] is DBNull ? 0 : Convert.ToDecimal(reader["comissao_calc"]),
            dataEvento = reader["data_evento"] is DBNull ? null : Convert.ToDateTime(reader["data_evento"]),
            dataCiclo = reader["data_ciclo"] is DBNull ? null : Convert.ToDateTime(reader["data_ciclo"]),
            valorLiquido = reader["valor_liquido"] is DBNull ? 0 : Convert.ToDecimal(reader["valor_liquido"])

        };
        string tipoEvento = reader["tipo_evento"]?.ToString() ?? string.Empty;
        marketplace.tipoEventoNormalizado = SkuMarketplace.normalizarTipoEvento(tipoEvento);
        marketplace.erroDevolucao = false;
        return marketplace;
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
