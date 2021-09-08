﻿using MySql.Data.MySqlClient;
using System;
using System.Text;

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

        public void Inserir(
            string nome,
            string cpf,
            string email,
            string senha,
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

            sql.AppendLine("INSERT INTO Usuario (");
            sql.AppendLine("Nome, Cpf, Email, Senha, DataNascimento, Endereco, Cidade,");
            sql.AppendLine("Estado, Pais, Cep, Fone, Imagem)");
            sql.AppendLine(" VALUES (@Nome, @Cpf, @Email, MD5(@Senha), @DataNascimento,");
            sql.AppendLine(" @Endereco, @Cidade, @Estado, @Pais, @Cep, @Fone, @Imagem)");

            // Alterar MD5 para HASH256

            var cmdText = sql.ToString();

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Nome", nome));
            comando.Parameters.Add(new MySqlParameter("Cpf", cpf));
            comando.Parameters.Add(new MySqlParameter("Email", email));
            comando.Parameters.Add(new MySqlParameter("Senha", senha));
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
                    Console.WriteLine("Usuário inserido com sucesso");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) Console.WriteLine("O Nome deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("CPF")) Console.WriteLine("O CPF deve possuir no máximo 11 caracteres");
                    else if (e.Message.Contains("Email")) Console.WriteLine("O Email deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Senha")) Console.WriteLine("A Senha deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Endereco")) Console.WriteLine("O Endereço deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Cidade")) Console.WriteLine("A Cidade deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Estado")) Console.WriteLine("O Estado deve possuir no máximo 2 caracteres");
                    else if (e.Message.Contains("Pais")) Console.WriteLine("O País deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("CEP")) Console.WriteLine("O CEP deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Fone")) Console.WriteLine("O Fone deve possuir no máximo 20 caracteres");
                    else if (e.Message.Contains("Imagem")) Console.WriteLine("A imagem deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.TruncatedWrongValue)
                {
                    if (e.Message.Contains("DataNascimento")) Console.WriteLine("Data Nascimento em formato inválido");
                }
            }

            comando.Dispose();
        }

        public void Remover(int id)
        {
            var cmdText = "DELETE FROM Usuario WHERE Id = @Id";

            MySqlCommand comando = new MySqlCommand(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            var linhasAfetadas = comando.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                Console.WriteLine("Usuário excluído com sucesso");
            }
            else
            {
                Console.WriteLine("Usuário não existe");
            }

            comando.Dispose();
        }

        public void Dispose()
        {
            _conexao?.Dispose();
        }
    }
}
