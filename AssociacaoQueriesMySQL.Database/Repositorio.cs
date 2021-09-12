using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace AssociacaoQueriesMySQL.Database
{
    public abstract class Repositorio : IDisposable
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        protected readonly MySqlConnection _conexao;

        public Repositorio()
        {
            _conexao = new MySqlConnection(_connectionString);
            _conexao.Open();
        }

        public void Dispose()
        {
            _conexao?.Dispose();
        }
    }
}
