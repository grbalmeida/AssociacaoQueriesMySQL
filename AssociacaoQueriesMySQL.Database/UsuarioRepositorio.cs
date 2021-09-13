﻿using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models.Filtros;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class UsuarioRepositorio : Repositorio
    {
        public void Listar(UsuarioFiltro filtro)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM Usuario");

            sql.AppendLine("WHERE 1 = 1");

            var parametros = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                sql.AppendLine("AND Nome LIKE @Nome");
                parametros.Add(new MySqlParameter("Nome", $"%{filtro.Nome}%"));
            }

            if (!string.IsNullOrEmpty(filtro.CPF))
            {
                sql.AppendLine("AND CPF LIKE @CPF");
                parametros.Add(new MySqlParameter("CPF", $"%{filtro.CPF}%"));
            }

            if (!string.IsNullOrEmpty(filtro.Email))
            {
                sql.AppendLine("AND Email LIKE @Email");
                parametros.Add(new MySqlParameter("Email", $"%{filtro.Email}%"));
            }

            if (!string.IsNullOrEmpty(filtro.DataNascimentoInicial))
            {
                sql.AppendLine("AND DataNascimento >= @DataNascimentoInicial");
                parametros.Add(new MySqlParameter("DataNascimentoInicial", $"{filtro.DataNascimentoInicial} 00:00:00"));
            }

            if (!string.IsNullOrEmpty(filtro.DataNascimentoFinal))
            {
                sql.AppendLine("AND DataNascimento <= @DataNascimentoFinal");
                parametros.Add(new MySqlParameter("DataNascimentoFinal", $"{filtro.DataNascimentoFinal} 23:59:59"));
            }

            if (!string.IsNullOrEmpty(filtro.Endereco))
            {
                sql.AppendLine("AND Endereco LIKE @Endereco");
                parametros.Add(new MySqlParameter("Endereco", $"%{filtro.Endereco}%"));
            }

            if (!string.IsNullOrEmpty(filtro.Cidade))
            {
                sql.AppendLine("AND Cidade LIKE @Cidade");
                parametros.Add(new MySqlParameter("Cidade", $"%{filtro.Cidade}%"));
            }

            if (!string.IsNullOrEmpty(filtro.Estado))
            {
                sql.AppendLine("AND Estado LIKE @Estado");
                parametros.Add(new MySqlParameter("Estado", $"%{filtro.Estado}%"));
            }

            if (!string.IsNullOrEmpty(filtro.Pais))
            {
                sql.AppendLine("AND Pais LIKE @Pais");
                parametros.Add(new MySqlParameter("Pais", $"%{filtro.Pais}%"));
            }

            if (!string.IsNullOrEmpty(filtro.CEP))
            {
                sql.AppendLine("AND CEP LIKE @CEP");
                parametros.Add(new MySqlParameter("CEP", $"%{filtro.CEP}%"));
            }

            if (!string.IsNullOrEmpty(filtro.Fone))
            {
                sql.AppendLine("AND Fone LIKE @Fone");
                parametros.Add(new MySqlParameter("Fone", $"%{filtro.Fone}%"));
            }

            sql.AppendLine("ORDER BY Id ASC");

            var cmdText = sql.ToString();

            MySqlCommand comando = new(cmdText, _conexao);

            comando.Parameters.AddRange(parametros.ToArray());

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
                    Console.WriteLine($"Data Nascimento: {dataNascimento:dd/MM/yyyy}");
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
                ConsoleExtensions.Warning("Nenhum usuário cadastrado");
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

            MySqlCommand comando = new(cmdText, _conexao);
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
                    ConsoleExtensions.Success("Usuário inserido com sucesso");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DataTooLong)
                {
                    if (e.Message.Contains("Nome")) ConsoleExtensions.Error("O Nome deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("CPF")) ConsoleExtensions.Error("O CPF deve possuir no máximo 11 caracteres");
                    else if (e.Message.Contains("Email")) ConsoleExtensions.Error("O Email deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Senha")) ConsoleExtensions.Error("A Senha deve possuir no máximo 100 caracteres");
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
            var cmdText = "DELETE FROM Usuario WHERE Id = @Id";

            MySqlCommand comando = new(cmdText, _conexao);
            comando.Parameters.Add(new MySqlParameter("Id", id));

            try
            {
                var linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Usuário excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Usuário não existe");
                }
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.RowIsReferenced2)
                {
                    if (e.Message.Contains("FK_Usuario")) ConsoleExtensions.Error("Não é possível excluir esse usuário pois existem empréstimos associados a ele");
                }
            }

            comando.Dispose();
        }
    }
}
