using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Filtros;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class UsuariosMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private UsuarioRepositorio _repo;

        public UsuariosMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
        }

        public void Iniciar()
        {
            _repo = new UsuarioRepositorio();

            Console.Clear();
            Console.WriteLine("1 - Listar Usuários");
            Console.WriteLine("2 - Inserir Usuário");
            Console.WriteLine("3 - Remover Usuário");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", Listar },
                { "2", Inserir },
                { "3", Remover }
            };

            opcoes.ExecutarOpcao(opcao, _menuInicial);
        }

        private void Listar()
        {
            Console.Clear();

            Console.WriteLine("Filtros");
            Console.Write("Informe o Nome: ");
            var nomeFiltro = Console.ReadLine();
            Console.Write("Data Nascimento Inicial (yyyy-mm-dd): ");
            var dataNascimentoInicialFiltro = Console.ReadLine();
            Console.Write("Data Nascimento Final (yyyy-mm-dd): ");
            var dataNascimentoFinalFiltro = Console.ReadLine();
            Console.Write("Informe o CPF: ");
            var cpfFiltro = Console.ReadLine();
            Console.Write("Informe o Email: ");
            var emailFiltro = Console.ReadLine();
            Console.Write("Informe o Fone: ");
            var foneFiltro = Console.ReadLine();
            Console.Write("Informe o Endereço: ");
            var enderecoFiltro = Console.ReadLine();
            Console.Write("Informe a Cidade: ");
            var cidadeFiltro = Console.ReadLine();
            Console.Write("Informe o Estado: ");
            var estadoFiltro = Console.ReadLine();
            Console.Write("Informe o País: ");
            var paisFiltro = Console.ReadLine();
            Console.Write("Informe o CEP: ");
            var cepFiltro = Console.ReadLine();

            var usuarioFiltro = new UsuarioFiltro
            {
                Nome = nomeFiltro,
                DataNascimentoInicial = dataNascimentoInicialFiltro,
                DataNascimentoFinal = dataNascimentoFinalFiltro,
                CPF = cpfFiltro,
                Email = emailFiltro,
                Fone = foneFiltro,
                Endereco = enderecoFiltro,
                Cidade = cidadeFiltro,
                Estado = estadoFiltro,
                Pais = paisFiltro,
                CEP = cepFiltro,
            };

            Console.Clear();

            var usuarios = _repo.Listar(usuarioFiltro);
            Dispose();

            if (usuarios.Count > 0)
            {
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine($"Id: {usuario.Id}");
                    Console.WriteLine($"Nome: {usuario.Nome}");
                    Console.WriteLine($"CPF: {usuario.CPF}");
                    Console.WriteLine($"Email: {usuario.Email}");
                    Console.WriteLine($"Data Nascimento: {usuario.DataNascimento:dd/MM/yyyy}");
                    Console.WriteLine($"Endereço: {usuario.Endereco}");
                    Console.WriteLine($"Estado: {usuario.Estado}");
                    Console.WriteLine($"País: {usuario.Pais}");
                    Console.WriteLine($"CEP: {usuario.CEP}");
                    Console.WriteLine($"Fone: {usuario.Fone}");
                    Console.WriteLine($"Imagem: {usuario.Imagem}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhum usuário cadastrado");
                Console.WriteLine();
            }

            Console.ReadKey();
            Iniciar();
        }

        private void Inserir()
        {
            Console.Clear();
            Console.Write("Informe o Nome: ");
            var nome = Console.ReadLine();
            Console.Write("Informe o CPF: ");
            var cpf = Console.ReadLine();
            Console.Write("Informe o Email: ");
            var email = Console.ReadLine();
            Console.Write("Informe a Data Nascimento - (yyyy-mm-dd): ");
            var dataNascimento = Console.ReadLine();
            Console.Write("Informe a Senha: ");
            var senha = PasswordExtensions.ReadPassword();
            Console.WriteLine();
            Console.Write("Informe o Endereço: ");
            var endereco = Console.ReadLine();
            Console.Write("Informe a Cidade: ");
            var cidade = Console.ReadLine();
            Console.Write("Informe o Estado: ");
            var estado = Console.ReadLine();
            Console.Write("Informe o País: ");
            var pais = Console.ReadLine();
            Console.Write("Informe o CEP: ");
            var cep = Console.ReadLine();
            Console.Write("Informe o Fone: ");
            var fone = Console.ReadLine();
            Console.Write("Informe a Imagem: ");
            var imagem = Console.ReadLine();

            try
            {
                var linhasAfetadas = _repo.Inserir(nome, cpf, email, senha, dataNascimento, endereco, cidade, estado, pais, cep, fone, imagem);

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Usuário inserido com sucesso");
                }
            }
            catch (DbException e)
            {
                ConsoleExtensions.Error(e.Message);
            }
            finally
            {
                Dispose();
            }

            Console.ReadKey();
            Iniciar();
        }

        private void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            try
            {
                var linhasAfetadas = _repo.Remover(id);

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Usuário excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Usuário não existe");
                }
            }
            catch (DbException e)
            {
                ConsoleExtensions.Error(e.Message);
            }
            finally
            {
                Dispose();
            }

            Console.ReadKey();
            Iniciar();
        }

        public void Dispose()
        {
            _repo?.Dispose();
        }
    }
}
