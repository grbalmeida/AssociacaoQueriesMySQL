using MySql.Data.MySqlClient;
using System;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class ClienteRepositorio : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public ClienteRepositorio(string connectionString)
        {
            _conexao = new MySqlConnection(connectionString);
            _conexao.Open();
        }

        public void Listar()
        {
            var cmdText = "SELECT * FROM Cliente ORDER BY Id ASC";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);

            MySqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32("Id");
                    var nome = reader.GetString("Nome");
                    var documento = reader.GetString("Documento");
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
                    Console.WriteLine($"Documento: {documento}");
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
                ConsoleExtensions.Warning("Nenhum cliente cadastrado");
                Console.WriteLine();
            }

            comando.Dispose();
            reader.Dispose();
        }

        public void Inserir(
            string nome,
            string documento,
            string email,
            string dataNascimento,
            string endereco,
            string cidade,
            string estado,
            string pais,
            string cep,
            string fone,
            string imagem
        )
        {
            var sql = new StringBuilder();

            sql.AppendLine("INSERT INTO Cliente (");
            sql.AppendLine("Nome, Documento, Email, DataNascimento, Endereco, Cidade,");
            sql.AppendLine("Estado, Pais, Cep, Fone, Imagem)");
            sql.AppendLine(" VALUES (@Nome, @Documento, @Email, @DataNascimento,");
            sql.AppendLine(" @Endereco, @Cidade, @Estado, @Pais, @Cep, @Fone, @Imagem)");

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Nome", nome));
            comando.Parameters.Add(new MySqlParameter("Documento", documento));
            comando.Parameters.Add(new MySqlParameter("Email", email));
            comando.Parameters.Add(new MySqlParameter("DataNascimento", dataNascimento));
            comando.Parameters.Add(new MySqlParameter("Endereco", endereco));
            comando.Parameters.Add(new MySqlParameter("Cidade", cidade));
            comando.Parameters.Add(new MySqlParameter("Estado", estado));
            comando.Parameters.Add(new MySqlParameter("Pais", pais));
            comando.Parameters.Add(new MySqlParameter("Cep", cep));
            comando.Parameters.Add(new MySqlParameter("Fone", fone));
            comando.Parameters.Add(new MySqlParameter("Imagem", imagem));

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Cliente inserido com sucesso");
                }
            }
            catch(MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) ConsoleExtensions.Error("O Nome deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Documento")) ConsoleExtensions.Error("O Documento deve possuir no máximo 20 caracteres");
                    else if (e.Message.Contains("Email")) ConsoleExtensions.Error("O Email deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Endereco")) ConsoleExtensions.Error("O Endereço deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Cidade")) ConsoleExtensions.Error("A Cidade deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Estado")) ConsoleExtensions.Error("O Estado deve possuir no máximo 2 caracteres");
                    else if (e.Message.Contains("Pais")) ConsoleExtensions.Error("O País deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("CEP")) ConsoleExtensions.Error("O CEP deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Fone")) ConsoleExtensions.Error("O Fone deve possuir no máximo 20 caracteres");
                    else if (e.Message.Contains("Imagem")) ConsoleExtensions.Error("A imagem deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.TruncatedWrongValue)
                {
                    if (e.Message.Contains("DataNascimento")) ConsoleExtensions.Error("Data Nascimento em formato inválido");
                }
            }

            comando.Dispose();
        }

        public void Remover(int id)
        {
            var cmdText = "DELETE FROM Cliente WHERE Id = @Id";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Cliente excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Cliente não existe");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.RowIsReferenced2)
                {
                    if (e.Message.Contains("FK_Cliente")) ConsoleExtensions.Error("Não é possível excluir esse cliente pois existem empréstimos associados a ele");
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
