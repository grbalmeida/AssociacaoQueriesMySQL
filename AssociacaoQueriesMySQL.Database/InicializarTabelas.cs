using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class InicializarTabelas : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public InicializarTabelas(string connectionString)
        {
            _conexao = new MySqlConnection(connectionString);
            _conexao.Open();

            InicializarTabelaUsuario();
            InicializarTabelaCliente();
            InicializarTabelaCategoria();
        }

        private void InicializarTabelaUsuario()
        {
            var sql = new StringBuilder();

            sql.AppendLine("CREATE TABLE IF NOT EXISTS Usuario (");
            sql.AppendLine("  Id INT NOT NULL AUTO_INCREMENT,");
            sql.AppendLine("  Nome VARCHAR(100) NOT NULL,");
            sql.AppendLine("  CPF VARCHAR(11) NOT NULL,");
            sql.AppendLine("  Email VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Senha VARCHAR(100) NOT NULL,");
            sql.AppendLine("  DataNascimento DATE NOT NULL,");
            sql.AppendLine("  Endereco VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Cidade VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Estado CHAR(2) NOT NULL,");
            sql.AppendLine("  Pais VARCHAR(100) NOT NULL,");
            sql.AppendLine("  CEP CHAR(8) NOT NULL,");
            sql.AppendLine("  Fone VARCHAR(20) NOT NULL,");
            sql.AppendLine("  Imagem VARCHAR(100),");
            sql.AppendLine("  PRIMARY KEY(Id)");
            sql.AppendLine(");");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            comando.ExecuteNonQuery();
            comando.Dispose();
        }

        private void InicializarTabelaCliente()
        {
            var sql = new StringBuilder();

            sql.AppendLine("CREATE TABLE IF NOT EXISTS Cliente (");
            sql.AppendLine("  Id INT NOT NULL AUTO_INCREMENT,");
            sql.AppendLine("  Nome VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Documento VARCHAR(20) NOT NULL,");
            sql.AppendLine("  Email VARCHAR(100) NOT NULL,");
            sql.AppendLine("  DataNascimento DATE NOT NULL,");
            sql.AppendLine("  Endereco VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Cidade VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Estado CHAR(2) NOT NULL,");
            sql.AppendLine("  Pais VARCHAR(100) NOT NULL,");
            sql.AppendLine("  CEP CHAR(8) NOT NULL,");
            sql.AppendLine("  Fone VARCHAR(20) NOT NULL,");
            sql.AppendLine("  Imagem VARCHAR(100),");
            sql.AppendLine("  PRIMARY KEY(Id)");
            sql.AppendLine(");");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            
            comando.ExecuteNonQuery();
            comando.Dispose();
        }

        private void InicializarTabelaCategoria()
        {
            var sql = new StringBuilder();

            sql.AppendLine("CREATE TABLE IF NOT EXISTS Categoria (");
            sql.AppendLine("  Id INT NOT NULL UNIQUE,");
            sql.AppendLine("  Nome VARCHAR(100) UNIQUE,");
            sql.AppendLine("  PRIMARY KEY(Id)");
            sql.AppendLine(");");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            comando.ExecuteNonQuery();
            comando.Dispose();
        }

        public void Dispose()
        {
            _conexao?.Close();
        }
    }
}
