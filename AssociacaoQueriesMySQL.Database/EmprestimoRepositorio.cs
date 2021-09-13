using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Entities;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class EmprestimoRepositorio : Repositorio
    {
        public List<Emprestimo> Listar()
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

            MySqlCommand comando = new(cmdText, _conexao);

            MySqlDataReader reader = comando.ExecuteReader();

            var emprestimos = new List<Emprestimo>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    emprestimos.Add(new Emprestimo
                    {
                        Id = reader.GetInt32("Id"),
                        DataEmprestimo = reader.GetDateTime("DataEmprestimo"),
                        DataLimiteDevolucao = reader["DataLimiteDevolucao"].ToString() != "" ? reader.GetDateTime("DataLimiteDevolucao") : null,
                        DataDevolucao = reader["DataDevolucao"].ToString() != "" ? reader.GetDateTime("DataDevolucao") : null,
                        EmprestimoAnteriorId = reader["EmprestimoAnteriorId"].ToString() != "" ? reader.GetInt32("EmprestimoAnteriorId") : null,
                        Produto = new Produto
                        {
                            Nome = reader.GetString("NomeProduto")
                        },
                        Cliente = new Cliente
                        {
                            Nome = reader.GetString("NomeCliente")
                        },
                        Usuario = new Usuario
                        {
                            Nome = reader.GetString("NomeUsuario")
                        }
                    });
                }
            }

            comando.Dispose();
            reader.Dispose();

            return emprestimos;
        }

        public int Inserir(
            int produtoId,
            int clienteId,
            int usuarioId,
            string dataEmprestimo,
            string dataLimiteDevolucao
        )
        {
            var sql = new StringBuilder();

            sql.AppendLine("INSERT INTO Emprestimo (ProdutoId, ClienteId, UsuarioId,");
            sql.AppendLine("DataEmprestimo, DataLimiteDevolucao) VALUES (@ProdutoId,");
            sql.AppendLine("@ClienteId, @UsuarioId, @DataEmprestimo, @DataLimiteDevolucao)");

            var cmdText = sql.ToString();

            MySqlCommand comando = new(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("ProdutoId", produtoId));
            comando.Parameters.Add(new MySqlParameter("ClienteId", clienteId));
            comando.Parameters.Add(new MySqlParameter("UsuarioId", usuarioId));
            comando.Parameters.Add(new MySqlParameter("DataEmprestimo", dataEmprestimo));
            comando.Parameters.Add(new MySqlParameter("DataLimiteDevolucao", dataLimiteDevolucao));

            int linhasAfetadas = 0;

            try
            {
                linhasAfetadas = comando.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.NoReferencedRow2)
                {
                    if (e.Message.Contains("FK_Produto")) throw new DbException("Produto não existe");
                    else if (e.Message.Contains("FK_Cliente")) throw new DbException("Cliente não existe");
                    else if (e.Message.Contains("FK_Usuario")) throw new DbException("Usuário não existe");
                }
                else if (e.Number == (int)MySqlErrorCode.TruncatedWrongValue)
                {
                    if (e.Message.Contains("DataEmprestimo")) throw new DbException("Data Empréstimo em formato inválido");
                    else if (e.Message.Contains("DataLimiteDevolucao")) throw new DbException("Data Limite Devolução em formato inválido");
                }
                else
                {
                    if (e.Message.Contains("CH_DataLimiteDevolucao")) throw new DbException("A Data Limite Devolução não pode ser menor que a Data Empréstimo");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }

        public int Remover(int id)
        {
            var cmdText = "DELETE FROM Emprestimo WHERE Id = @Id";

            MySqlCommand comando = new(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            int linhasAfetadas = 0;

            try
            {
                linhasAfetadas = comando.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.RowIsReferenced2)
                {
                    if (e.Message.Contains("FK_EmprestimoAnterior")) throw new DbException("Não é possível excluir esse empréstimo pois existem empréstimos associados a ele");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }
    }
}
