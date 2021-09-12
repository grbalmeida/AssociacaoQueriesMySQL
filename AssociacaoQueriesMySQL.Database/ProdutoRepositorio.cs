using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

        public void Listar(ProdutoFiltro filtro)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT p.Id, p.Nome, p.Descricao, p.Ativo, p.Valor,");
            sql.AppendLine("p.QuantidadeEstoque, p.Altura, p.Largura, p.Profundidade, c.Nome as NomeCategoria");
            sql.AppendLine("FROM Produto p ");
            sql.AppendLine("INNER JOIN Categoria c ON p.CategoriaId = c.Id");

            sql.AppendLine("WHERE 1 = 1");

            var parametros = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                sql.AppendLine("AND p.Nome LIKE @Nome");
                parametros.Add(new MySqlParameter("Nome", $"%{filtro.Nome}%"));
            }

            if (!string.IsNullOrEmpty(filtro.Descricao))
            {
                sql.AppendLine("AND p.Descricao LIKE @Descricao");
                parametros.Add(new MySqlParameter("Descricao", $"%{filtro.Descricao}%"));
            }

            if (filtro.Ativo.HasValue)
            {
                sql.AppendLine("AND p.Ativo = @Ativo");
                parametros.Add(new MySqlParameter("Ativo", filtro.Ativo));
            }

            if (filtro.ValorMinimo.HasValue)
            {
                sql.AppendLine("AND p.Valor >= @ValorMinimo");
                parametros.Add(new MySqlParameter("ValorMinimo", filtro.ValorMinimo));
            }

            if (filtro.ValorMaximo.HasValue)
            {
                sql.AppendLine("AND p.Valor <= @ValorMaximo");
                parametros.Add(new MySqlParameter("ValorMaximo", filtro.ValorMaximo));
            }

            if (filtro.CategoriaId.HasValue)
            {
                sql.AppendLine("AND p.CategoriaId = @CategoriaId");
                parametros.Add(new MySqlParameter("CategoriaId", filtro.CategoriaId));
            }

            if (filtro.QuantidadeEstoqueMinima.HasValue)
            {
                sql.AppendLine("AND p.QuantidadeEstoque >= @QuantidadeEstoqueMinima");
                parametros.Add(new MySqlParameter("QuantidadeEstoqueMinima", filtro.QuantidadeEstoqueMinima));
            }

            if (filtro.QuantidadeEstoqueMaxima.HasValue)
            {
                sql.AppendLine("AND p.QuantidadeEstoque <= @QuantidadeEstoqueMaxima");
                parametros.Add(new MySqlParameter("QuantidadeEstoqueMaxima", filtro.QuantidadeEstoqueMaxima));
            }

            if (filtro.AlturaMinima.HasValue)
            {
                sql.AppendLine("AND p.Altura >= @AlturaMinima");
                parametros.Add(new MySqlParameter("AlturaMinima", filtro.AlturaMinima));
            }

            if (filtro.AlturaMaxima.HasValue)
            {
                sql.AppendLine("AND p.Altura <= @AlturaMaxima");
                parametros.Add(new MySqlParameter("AlturaMaxima", filtro.AlturaMaxima));
            }

            if (filtro.LarguraMinima.HasValue)
            {
                sql.AppendLine("AND p.Largura >= @LarguraMinima");
                parametros.Add(new MySqlParameter("LarguraMinima", filtro.LarguraMinima));
            }

            if (filtro.LarguraMaxima.HasValue)
            {
                sql.AppendLine("AND p.Largura <= @LarguraMaxima");
                parametros.Add(new MySqlParameter("LarguraMaxima", filtro.LarguraMaxima));
            }

            if (filtro.ProfundidadeMinima.HasValue)
            {
                sql.AppendLine("AND p.Profundidade >= @ProfundidadeMinima");
                parametros.Add(new MySqlParameter("ProfundidadeMinima", filtro.ProfundidadeMinima));
            }

            if (filtro.ProfundidadeMaxima.HasValue)
            {
                sql.AppendLine("AND p.Profundidade <= @ProfundidadeMaxima");
                parametros.Add(new MySqlParameter("ProfundidadeMaxima", filtro.ProfundidadeMaxima));
            }
            
            sql.AppendLine("ORDER BY p.Id ASC");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            comando.Parameters.AddRange(parametros.ToArray());

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

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Produto excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Produto não existe");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.RowIsReferenced2)
                {
                    if (e.Message.Contains("FK_Produto")) ConsoleExtensions.Error("Não é possível excluir esse produto pois existem empréstimos associados a ele");
                }
            }

            comando.Dispose();
        }

        public void Dispose()
        {
            _conexao?.Dispose();
        }
    }
}
