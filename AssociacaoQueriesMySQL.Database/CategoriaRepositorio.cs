using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class CategoriaRepositorio : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public CategoriaRepositorio(string connectionString)
        {
            _conexao = new MySqlConnection(connectionString);
            _conexao.Open();
        }

        public void Listar(string nomeFiltro)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM Categoria");

            if (!string.IsNullOrEmpty(nomeFiltro))
                sql.AppendLine("WHERE Nome LIKE @Nome");

            sql.AppendLine("ORDER BY Id ASC");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            if (!string.IsNullOrEmpty(nomeFiltro))
                comando.Parameters.AddWithValue("@Nome", $"%{nomeFiltro}%");

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("Id");
                    var nome = reader.GetString("Nome");

                    Console.WriteLine($"Id: {id}");
                    Console.WriteLine($"Nome: {nome}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhuma categoria cadastrada");
                Console.WriteLine();
            }

            comando.Dispose();
            reader.Dispose();
        }

        public void Inserir(int id, string nome)
        {
            var cmdText = "INSERT INTO Categoria (Id, Nome) VALUES (@Id, @Nome)";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));
            comando.Parameters.Add(new MySqlParameter("Nome", nome));

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Categoria inserida com sucesso");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) ConsoleExtensions.Error("O Nome deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                {
                    if (e.Message.Contains("PRIMARY")) ConsoleExtensions.Error("Id já cadastrado");
                    else if (e.Message.Contains("Nome")) ConsoleExtensions.Error("Nome já cadastrado");
                }
            }

            comando.Dispose();
        }

        public void Remover(int id)
        {
            var cmdText = "DELETE FROM Categoria WHERE Id = @Id";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Categoria excluída com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Categoria não existe");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.RowIsReferenced2)
                {
                    ConsoleExtensions.Error("Não é possível excluir essa categoria pois existem produtos associados a ela");
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
