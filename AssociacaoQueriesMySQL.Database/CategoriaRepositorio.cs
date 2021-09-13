using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Entities;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class CategoriaRepositorio : Repositorio
    {
        public List<Categoria> Listar(string nomeFiltro)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM Categoria");

            if (!string.IsNullOrEmpty(nomeFiltro))
                sql.AppendLine("WHERE Nome LIKE @Nome");

            sql.AppendLine("ORDER BY Id ASC");

            var cmdText = sql.ToString();

            MySqlCommand comando = new(cmdText, _conexao);

            if (!string.IsNullOrEmpty(nomeFiltro))
                comando.Parameters.AddWithValue("@Nome", $"%{nomeFiltro}%");

            MySqlDataReader reader = comando.ExecuteReader();

            var categorias = new List<Categoria>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    categorias.Add(new Categoria
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome")
                    });
                }
            }

            comando.Dispose();
            reader.Dispose();

            return categorias;
        }

        public int Inserir(int id, string nome)
        {
            var cmdText = "INSERT INTO Categoria (Id, Nome) VALUES (@Id, @Nome)";

            MySqlCommand comando = new(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));
            comando.Parameters.Add(new MySqlParameter("Nome", nome));

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
                else if (e.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                {
                    if (e.Message.Contains("PRIMARY")) throw new DbException("Id já cadastrado");
                    else if (e.Message.Contains("Nome")) throw new DbException("Nome já cadastrado");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }

        public int Remover(int id)
        {
            var cmdText = "DELETE FROM Categoria WHERE Id = @Id";

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
                    throw new DbException("Não é possível excluir essa categoria pois existem produtos associados a ela");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }
    }
}
