using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class EmprestimoRepositorio : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public EmprestimoRepositorio(string connectionString)
        {
            _conexao = new MySqlConnection(connectionString);
            _conexao.Open();
        }

        public void Listar()
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT e.Id, e.DataEmprestimo, e.DataLimiteDevolucao, e.DataDevolucao,");
            sql.AppendLine("e.EmprestimoAnteriorId, p.Nome as NomeProduto,");
            sql.AppendLine("c.Nome as NomeCliente, u.Nome as NomeUsuario");
            sql.AppendLine("FROM Emprestimo e");
            sql.AppendLine("INNER JOIN Produto p ON p.Id = e.ProdutoId");
            sql.AppendLine("INNER JOIN Cliente c ON c.Id = e.ClienteId");
            sql.AppendLine("INNER JOIN Usuario u ON u.Id = e.UsuarioId");
            sql.AppendLine("ORDER BY e.Id ASC");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("Id");
                    var dataEmprestimo = reader.GetDateTime("DataEmprestimo");
                    DateTime? dataLimiteDevolucao = reader["DataLimiteDevolucao"].ToString() != "" ? reader.GetDateTime("DataLimiteDevolucao") : null;
                    DateTime? dataDevolucao = reader["DataDevolucao"].ToString() != "" ? reader.GetDateTime("DataDevolucao") : null;
                    var emprestimoAnteriorId = reader["EmprestimoAnteriorId"];
                    var nomeProduto = reader.GetString("NomeProduto");
                    var nomeCliente = reader.GetString("NomeCliente");
                    var nomeUsuario = reader.GetString("NomeUsuario");

                    Console.WriteLine($"Id: {id}");
                    Console.WriteLine($"Data Empréstimo: {dataEmprestimo.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"Data Limite Devolução: {dataLimiteDevolucao?.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"Data Devolução: {dataDevolucao?.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"Id Empréstimo Anterior: {emprestimoAnteriorId}");
                    Console.WriteLine($"Nome Produto: {nomeProduto}");
                    Console.WriteLine($"Nome Cliente: {nomeCliente}");
                    Console.WriteLine($"Nome Usuário: {nomeUsuario}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhum empréstimo cadastrado");
                Console.WriteLine();
            }

            comando.Dispose();
            reader.Dispose();
        }

        public void Dispose()
        {
            _conexao?.Dispose();
        }
    }
}
