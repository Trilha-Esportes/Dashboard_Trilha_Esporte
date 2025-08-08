using Npgsql;

using DashboardTrilhaEsporte.Enums;

namespace DashboardTrilhaEsporte.Data.Entities
{
   public class ProdutoEstornado
    {
        public long id { get; private set; }
        public long marketplaceId { get; private set; }

        public DateTime dataCriacao { get; private set; }
        public String numeroPedido { get; private set; } = String.Empty;
        public String skuNotaFiscal { get; private set; } = String.Empty;
        public DateTime dataChegada { get; private set; }
        public Boolean conferencia { get; private set; }
        public Boolean enviadoCorretamente { get; private set; }
        public Boolean entreque { get; private set; }
        public MotivoDevolucao motivo { get; private set; }
        public Operacao operacao { get; private set; }

        public static MotivoDevolucao MapearMotivoDevolucao(string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return MotivoDevolucao.Desconhecido;

            motivo = motivo.Trim().ToLowerInvariant();

            var mapping = new Dictionary<string, MotivoDevolucao>
            {
                { "produto errado", MotivoDevolucao.ProdutoErrado },
                { "arrependimento", MotivoDevolucao.Arrependimento },
                { "desistência", MotivoDevolucao.Desistencia },
                { "recusa", MotivoDevolucao.Recusa },
                { "cancelamento", MotivoDevolucao.Cancelamento },
                { "defeito", MotivoDevolucao.Defeito },
                { "fraude", MotivoDevolucao.Fraude },
                { "observação", MotivoDevolucao.Observacao },
                { "ao remetente", MotivoDevolucao.AoRemetente },
                { "não informado", MotivoDevolucao.NaoInformado },
                { "não apto (ml)", MotivoDevolucao.NaoAptoML },
                { "recusada a devolução", MotivoDevolucao.RecusadaADevolucao },
                { "não serviu", MotivoDevolucao.NaoServiu },
            };

            return mapping.TryGetValue(motivo, out var valor) ? valor : MotivoDevolucao.Desconhecido;
        }

        public static Operacao MapearOperacao(string operacao)
        {
            if (string.IsNullOrWhiteSpace(operacao))
                return Operacao.Desconhecido;

            operacao = operacao.Trim().ToLowerInvariant();

            var mapping = new Dictionary<string, Operacao>
            {
                { "indenização", Operacao.Indenizacao },
                { "devolução parcial", Operacao.DevolucaoParcial },
                { "recusa", Operacao.Recusa },
                { "devolução", Operacao.Devolucao },
                { "cancelamento", Operacao.Cancelamento },
                { "vale compras", Operacao.ValeCompras },
                { "reintegração", Operacao.Reintegracao },
                { "defeito", Operacao.Defeito },
                { "congelado", Operacao.Congelado },
                { "devolução/troca", Operacao.DevolucaoTroca },
                { "troca", Operacao.Troca },
            };

            return mapping.TryGetValue(operacao, out var valor) ? valor : Operacao.Desconhecido;
        }


        public static ProdutoEstornado MapearRegistro(NpgsqlDataReader reader)
    {
        return new ProdutoEstornado
        {
            id = reader["id"] is DBNull ? 0 : Convert.ToInt64(reader["id"]),
            marketplaceId = reader["marketplace_id"] is DBNull ? 0 : Convert.ToInt64(reader["marketplace_id"]),
            dataCriacao = reader["data_criacao"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["data_criacao"]),
            numeroPedido = reader["numero_pedido"]?.ToString() ?? string.Empty,
            skuNotaFiscal = reader["sku_na_nota_fiscal"]?.ToString() ?? string.Empty,
            dataChegada = reader["data_chegada_devolucao"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["data_chegada_devolucao"]),
            conferencia = reader["conferencia"] is DBNull ? false : Convert.ToBoolean(reader["conferencia"]),
            enviadoCorretamente = reader["enviado_corretamente"] is DBNull ? false : Convert.ToBoolean(reader["enviado_corretamente"]),
            entreque = reader["entregue"] is DBNull ? false : Convert.ToBoolean(reader["entregue"]),
            motivo = reader["motivo"] is string motivoDevolucao
                            ? ProdutoEstornado.MapearMotivoDevolucao(motivoDevolucao)
                            : MotivoDevolucao.Desconhecido,
            operacao = reader["operacao"] is string operacaoStr
                ? ProdutoEstornado.MapearOperacao(operacaoStr)
                : Operacao.Desconhecido
            };
    }


    }
}