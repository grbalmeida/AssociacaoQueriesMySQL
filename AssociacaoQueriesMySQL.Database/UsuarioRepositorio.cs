using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Entities;
using AssociacaoQueriesMySQL.Core.Models.Filtros;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace AssociacaoQueriesMySQL.Database
{
    public class UsuarioRepositorio : Repositorio
    {
        public List<Usuario> Listar(UsuarioFiltro filtro)
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

            var usuarios = new List<Usuario>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = reader.GetInt32("Id"),
                        Nome = reader.GetString("Nome"),
                        CPF = reader.GetString("CPF"),
                        Email = reader.GetString("Email"),
                        DataNascimento = reader.GetDateTime("DataNascimento"),
                        Endereco = reader.GetString("Endereco"),
                        Estado = reader.GetString("Estado"),
                        Pais = reader.GetString("Pais"),
                        CEP = reader.GetString("CEP"),
                        Fone = reader.GetString("Fone"),
                        Imagem = reader.GetString("Imagem")
                    });
                }
            }

            comando.Dispose();
            reader.Dispose();

            return usuarios;
        }

        public int Inserir(
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
                    else if (e.Message.Contains("CPF")) throw new DbException("O CPF deve possuir no máximo 11 caracteres");
                    else if (e.Message.Contains("Email")) throw new DbException("O Email deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Senha")) throw new DbException("A Senha deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Endereco")) throw new DbException("O Endereço deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Cidade")) throw new DbException("A Cidade deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Estado")) throw new DbException("O Estado deve possuir no máximo 2 caracteres");
                    else if (e.Message.Contains("Pais")) throw new DbException("O País deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("CEP")) throw new DbException("O CEP deve possuir no máximo 100 caracteres");
                    else if (e.Message.Contains("Fone")) throw new DbException("O Fone deve possuir no máximo 20 caracteres");
                    else if (e.Message.Contains("Imagem")) throw new DbException("A imagem deve possuir no máximo 100 caracteres");
                }
                else if (e.Number == (int)MySqlErrorCode.TruncatedWrongValue)
                {
                    if (e.Message.Contains("DataNascimento")) throw new DbException("Data Nascimento em formato inválido");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }

        public int Remover(int id)
        {
            var cmdText = "DELETE FROM Usuario WHERE Id = @Id";

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
                    if (e.Message.Contains("FK_Usuario")) throw new DbException("Não é possível excluir esse usuário pois existem empréstimos associados a ele");
                }
            }

            comando.Dispose();

            return linhasAfetadas;
        }
    }
}
