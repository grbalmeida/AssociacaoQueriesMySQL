using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Entities;
using AssociacaoQueriesMySQL.Core.Models.Filtros;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class ProdutoRepositorio : Repositorio
    {
        public List<Produto> Listar(ProdutoFiltro filtro)
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

            MySqlCommand comando = new(cmdText, _conexao);

            comando.Parameters.AddRange(parametros.ToArray());

            MySqlDataReader reader = comando.ExecuteReader();

            var produtos = new List<Produto>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    produtos.Add(new Produto
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        Descricao = reader["Descricao"] as string,
                        Ativo = reader.GetBoolean("Ativo"),
                        Valor = reader.GetDecimal("Valor"),
                        QuantidadeEstoque = reader.GetInt32("QuantidadeEstoque"),
                        Altura = reader.GetDecimal("Altura"),
                        Largura = reader.GetDecimal("Largura"),
                        Profundidade = reader.GetDecimal("Profundidade"),
                        Categoria = new Categoria
                        {
                            Nome = reader.GetString("NomeCategoria")
                        }
                    });
                }
            }

            comando.Dispose();
            reader.Dispose();

            return produtos;
        }

        public int Inserir(
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

            MySqlCommand comando = new(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Nome", nome));
            comando.Parameters.Add(new MySqlParameter("Descricao", descricao));
            comando.Parameters.Add(new MySqlParameter("Ativo", ativo));
            comando.Parameters.Add(new MySqlParameter("Valor", valor));
            comando.Parameters.Add(new MySqlParameter("CategoriaId", categoriaId));
            comando.Parameters.Add(new MySqlParameter("QuantidadeEstoque", quantidadeEstoque));
            comando.Parameters.Add(new MySqlParameter("Altura", altura));
            comando.Parameters.Add(new MySqlParameter("Largura", largura));
            comando.Parameters.Add(new MySqlParameter("Profundidade", profundidade));

            int linhasAfetadas = 0;

            try
            {
                linhasAfetadas = comando.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) throw new DbException("O Nome deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.NoReferencedRow2) {
                    if (e.Message.Contains("FK_Categoria")) throw new DbException("Categoria não existe");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }

        public int Remover(int id)
        {
            var cmdText = "DELETE FROM Produto WHERE Id = @Id";

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
                    if (e.Message.Contains("FK_Produto")) throw new DbException("Não é possível excluir esse produto pois existem empréstimos associados a ele");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }
    }
}
