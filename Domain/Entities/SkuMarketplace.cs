using DashboardTrilhaEsporte.Enums;
using Npgsql;
using System;

namespace DashboardTrilhaEsporte.Domain.Entities{

// Essa classe representa a entidade SkuMarketplace
// Ela contém todos os parametros que vem da consulta SQL
// Somados de parametro que representa erro de devolução

public class SkuMarketplace : IEquatable<SkuMarketplace>
{
    public String? marketplace { get; set; } 
    public string? skuMarketplaceId { get; set; }
    public String? numeroPedido { get; set; }
    public Decimal valorLiquido { get; set; }
    public DateTime? dataComissao { get; set; }
    public Decimal porcentagem { get; set; }
    public Decimal comissao { get; set; }
    public Eventos tipoEventoNormalizado { get; set; }
    public Decimal valorFinal { get; set; }
    public DateTime? dataEvento { get; set; }
    public DateTime? dataCiclo { get; set; }

    public Boolean erroDevolucao { get; set; }

    // Método responsável por normalizar o tipo de evento
    // Ele recebe uma string e retorna um valor do enum Eventos
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

    // Método responsável por mapear o registro retornado do banco de dados 
    // para um objeto SkuMarketplace
    public static SkuMarketplace MapearRegistro(NpgsqlDataReader reader)
    {
        SkuMarketplace marketplace = new SkuMarketplace
        {
            marketplace = reader["marketplace"]?.ToString() ?? string.Empty,
            skuMarketplaceId = reader["sku_marketplace_id"] is DBNull ? string.Empty : Convert.ToString(reader["sku_marketplace_id"]),
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




}


}
