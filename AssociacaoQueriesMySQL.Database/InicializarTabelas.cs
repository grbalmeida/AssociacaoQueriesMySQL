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
            InicializarTabelaProduto();
            InicializarTabelaEmprestimo();
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

        private void InicializarTabelaProduto()
        {
            var sql = new StringBuilder();

            sql.AppendLine("CREATE TABLE IF NOT EXISTS Produto (");
            sql.AppendLine("  Id INT NOT NULL AUTO_INCREMENT,");
            sql.AppendLine("  Nome VARCHAR(100) NOT NULL,");
            sql.AppendLine("  Descricao TEXT,");
            sql.AppendLine("  Ativo BIT NOT NULL,");
            sql.AppendLine("  Valor DECIMAL(10, 2) NOT NULL,");
            sql.AppendLine("  CategoriaId INT NOT NULL,");
            sql.AppendLine("  QuantidadeEstoque INT NOT NULL,");
            sql.AppendLine("  Altura DECIMAL(4, 2) NOT NULL,");
            sql.AppendLine("  Largura DECIMAL(4, 2) NOT NULL,");
            sql.AppendLine("  Profundidade DECIMAL(4, 2) NOT NULL,");
            sql.AppendLine("  PRIMARY KEY(Id),");
            sql.AppendLine("  CONSTRAINT FK_Categoria FOREIGN KEY(CategoriaId) REFERENCES Categoria(Id)");
            sql.AppendLine(");");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            comando.ExecuteNonQuery();
            comando.Dispose();
        }

        private void InicializarTabelaEmprestimo()
        {
            var sql = new StringBuilder();

            sql.AppendLine("CREATE TABLE IF NOT EXISTS Emprestimo (");
            sql.AppendLine("  Id INT NOT NULL AUTO_INCREMENT,");
            sql.AppendLine("  ProdutoId INT NOT NULL,");
            sql.AppendLine("  ClienteId INT NOT NULL,");
            sql.AppendLine("  UsuarioId INT NOT NULL,");
            sql.AppendLine("  EmprestimoAnteriorId INT,");
            sql.AppendLine("  DataEmprestimo DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,");
            sql.AppendLine("  DataLimiteDevolucao DATETIME,");
            sql.AppendLine("  DataDevolucao DATETIME,");
            sql.AppendLine("  PRIMARY KEY(Id),");
            sql.AppendLine("  CONSTRAINT CH_DataLimiteDevolucao CHECK (DataLimiteDevolucao > DataEmprestimo),");
            sql.AppendLine("  CONSTRAINT CH_DataDevolucao CHECK (DataDevolucao > DataEmprestimo),");
            sql.AppendLine("  CONSTRAINT FK_Produto FOREIGN KEY(ProdutoId) REFERENCES Produto(Id),");
            sql.AppendLine("  CONSTRAINT FK_Cliente FOREIGN KEY(ClienteId) REFERENCES Cliente(Id),");
            sql.AppendLine("  CONSTRAINT FK_Usuario FOREIGN KEY(UsuarioId) REFERENCES Usuario(Id),");
            sql.AppendLine("  CONSTRAINT FK_EmprestimoAnterior FOREIGN KEY(EmprestimoAnteriorId) REFERENCES Emprestimo(Id)");
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
