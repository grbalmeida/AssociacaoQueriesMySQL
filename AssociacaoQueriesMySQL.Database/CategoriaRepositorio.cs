using MySql.Data.MySqlClient;
using System;

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

        public void Listar()
        {
            var cmdText = "SELECT * FROM Categoria ORDER BY Id ASC";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

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
                Console.WriteLine("Nenhuma categoria cadastrada");
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
                    Console.WriteLine("Categoria inserida com sucesso");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) Console.WriteLine("O Nome deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                {
                    if (e.Message.Contains("PRIMARY")) Console.WriteLine("Id já cadastrado");
                    else if (e.Message.Contains("Nome")) Console.WriteLine("Nome já cadastrado");
                }
            }

            comando.Dispose();
        }

        public void Remover(int id)
        {
            var cmdText = "DELETE FROM Categoria WHERE Id = @Id";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            var linhasAfetadas = comando.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                Console.WriteLine("Categoria excluída com sucesso");
            }
            else
            {
                Console.WriteLine("Categoria não existe");
            }

            comando.Dispose();
        } 

        public void Dispose()
        {
            _conexao?.Dispose();
        }
    }
}
