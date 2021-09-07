using MySql.Data.MySqlClient;
using System;

namespace AssociacaoQueriesMySQL.Database
{
    public class UsuarioRepositorio : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public UsuarioRepositorio(string connectionString)
        {
            _conexao = new MySqlConnection(connectionString);
            _conexao.Open();
        }

        public void Listar()
        {
            var cmdText = "SELECT * FROM Usuario ORDER BY Id ASC";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("Id");
                    var nome = reader.GetString("Nome");
                    var cpf = reader.GetString("CPF");
                    var email = reader.GetString("Email");
                    var dataNascimento = reader.GetDateTime("DataNascimento");
                    var endereco = reader.GetString("Endereco");
                    var estado = reader.GetString("Estado");
                    var pais = reader.GetString("Pais");
                    var cep = reader.GetString("CEP");
                    var fone = reader.GetString("Fone");
                    var imagem = reader.GetString("Imagem");

                    Console.WriteLine($"Id: {id}");
                    Console.WriteLine($"Nome: {nome}");
                    Console.WriteLine($"CPF: {cpf}");
                    Console.WriteLine($"Email: {email}");
                    Console.WriteLine($"Data Nascimento: {dataNascimento.ToString("dd/MM/yyyy")}");
                    Console.WriteLine($"Endereço: {endereco}");
                    Console.WriteLine($"Estado: {estado}");
                    Console.WriteLine($"País: {pais}");
                    Console.WriteLine($"CEP: {cep}");
                    Console.WriteLine($"Fone: {fone}");
                    Console.WriteLine($"Imagem: {imagem}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Nenhum usuário cadastrado");
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
