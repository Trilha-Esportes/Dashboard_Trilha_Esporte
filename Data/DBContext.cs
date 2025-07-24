using Npgsql;
using System.Data;

// Class responsável por gerenciar a conexão com o banco de dados.
// Ele utiliza a biblioteca Npgsql para se conectar a um banco de dados PostgreSQL.

namespace DashboardTrilhaEsporte.Data
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
               return new NpgsqlConnection(_connectionString); 
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