using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class ProdutoRepositorio : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public ProdutoRepositorio(string connectionString)
        {
            _conexao = new MySqlConnection(connectionString);
            _conexao.Open();
        }

        public void Listar()
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT p.Id, p.Nome, p.Descricao, p.Ativo, p.Valor,");
            sql.AppendLine("p.QuantidadeEstoque, p.Altura, p.Largura, p.Profundidade, c.Nome as NomeCategoria");
            sql.AppendLine("FROM Produto p ");
            sql.AppendLine("INNER JOIN Categoria c ON p.CategoriaId = c.Id");
            sql.AppendLine("ORDER BY p.Id ASC");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("Id");
                    var nome = reader.GetString("Nome");
                    var descricao = reader["Descricao"];
                    var ativo = reader.GetBoolean("Ativo") ? "S" : "N";
                    var valor = reader.GetDecimal("Valor");
                    var quantidadeEstoque = reader.GetInt32("QuantidadeEstoque");
                    var altura = reader.GetDecimal("Altura");
                    var largura = reader.GetDecimal("Largura");
                    var profundidade = reader.GetDecimal("Profundidade");
                    var categoria = reader.GetString("NomeCategoria");

                    Console.WriteLine($"Id: {id}");
                    Console.WriteLine($"Nome: {nome}");
                    Console.WriteLine($"Descrição: {descricao}");
                    Console.WriteLine($"Ativo: {ativo}");
                    Console.WriteLine($"Valor: {valor}");
                    Console.WriteLine($"Quantidade em Estoque: {quantidadeEstoque}");
                    Console.WriteLine($"Altura: {altura}");
                    Console.WriteLine($"Dimensões do produto: Largura: {largura} cm / Profundidade: {profundidade} cm / Altura: {altura} cm");
                    Console.WriteLine($"Categoria: {categoria}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhum produto cadastrado");
                Console.WriteLine();
            }

            comando.Dispose();
            reader.Dispose();
        }

        public void Inserir(
            string nome,
            string descricao,
            bool ativo,
            decimal valor,
            int categoriaId,
            int quantidadeEstoque,
            decimal altura,
            decimal largura,
            decimal profundidade
        )
        {
            var sql = new StringBuilder();
            sql.AppendLine("INSERT INTO Produto (Nome, Descricao, Ativo, Valor,");
            sql.AppendLine("CategoriaId, QuantidadeEstoque, Altura, Largura, Profundidade)");
            sql.AppendLine("VALUES (@Nome, @Descricao, @Ativo, @Valor, @CategoriaId,");
            sql.AppendLine("@QuantidadeEstoque, @Altura, @Largura, @Profundidade)");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Nome", nome));
            comando.Parameters.Add(new MySqlParameter("Descricao", descricao));
            comando.Parameters.Add(new MySqlParameter("Ativo", ativo));
            comando.Parameters.Add(new MySqlParameter("Valor", valor));
            comando.Parameters.Add(new MySqlParameter("CategoriaId", categoriaId));
            comando.Parameters.Add(new MySqlParameter("QuantidadeEstoque", quantidadeEstoque));
            comando.Parameters.Add(new MySqlParameter("Altura", altura));
            comando.Parameters.Add(new MySqlParameter("Largura", largura));
            comando.Parameters.Add(new MySqlParameter("Profundidade", profundidade));

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Produto inserido com sucesso");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) ConsoleExtensions.Error("O Nome deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.NoReferencedRow2) {
                    if (e.Message.Contains("FK_Categoria")) ConsoleExtensions.Error("Categoria não existe");
                }
            }

            comando.Dispose();
        }

        public void Remover(int id)
        {
            var cmdText = "DELETE FROM Produto WHERE Id = @Id";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            var linhasAfetadas = comando.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                ConsoleExtensions.Success("Produto excluído com sucesso");
            }
            else
            {
                ConsoleExtensions.Warning("Produto não existe");
            }

            comando.Dispose();
        }

        public void Dispose()
        {
            _conexao?.Dispose();
        }
    }
}
