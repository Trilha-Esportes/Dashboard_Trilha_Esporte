using Npgsql;
using System.Data;

namespace DashboardTrilhasEsporte.Data
{
    public class DBContext
    {
        private readonly string _connectionString;

        public DBContext(string connectionString)
        {
            _connectionString = connectionString;
        }


        //Cria e retorna uma conexão com banco 
        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Método simples para verificar a conexão
        public string TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    return "Conexão bem-sucedida!";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao conectar: {ex.Message}";
            }

        }


    }
}