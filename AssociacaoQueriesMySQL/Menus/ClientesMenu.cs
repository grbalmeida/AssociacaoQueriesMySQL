using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Filtros;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class ClientesMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private ClienteRepositorio _repo;

        public ClientesMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
        }

        public void Iniciar()
        {
            _repo = new ClienteRepositorio();

            Console.Clear();
            Console.WriteLine("1 - Listar Clientes");
            Console.WriteLine("2 - Inserir Cliente");
            Console.WriteLine("3 - Remover Cliente");

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
            Console.Write("Informe o Documento: ");
            var documentoFiltro = Console.ReadLine();
            Console.Write("Informe o Email: ");
            var emailFiltro = Console.ReadLine();
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
            Console.Write("Informe o Fone: ");
            var foneFiltro = Console.ReadLine();

            var clienteFiltro = new ClienteFiltro
            {
                Nome = nomeFiltro,
                Documento = documentoFiltro,
                Email = emailFiltro,
                Endereco = enderecoFiltro,
                Cidade = cidadeFiltro,
                Estado = estadoFiltro,
                Pais = paisFiltro,
                CEP = cepFiltro,
                Fone = foneFiltro
            };

            Console.Clear();

            var clientes = _repo.Listar(clienteFiltro);
            Dispose();

            if (clientes.Count > 0)
            {
                foreach (var cliente in clientes)
                {
                    Console.WriteLine($"Id: {cliente.Id}");
                    Console.WriteLine($"Nome: {cliente.Nome}");
                    Console.WriteLine($"Documento: {cliente.Documento}");
                    Console.WriteLine($"Email: {cliente.Email}");
                    Console.WriteLine($"Data Nascimento: {cliente.DataNascimento:dd/MM/yyyy}");
                    Console.WriteLine($"Endereço: {cliente.Endereco}");
                    Console.WriteLine($"Estado: {cliente.Estado}");
                    Console.WriteLine($"País: {cliente.Pais}");
                    Console.WriteLine($"CEP: {cliente.CEP}");
                    Console.WriteLine($"Fone: {cliente.Fone}");
                    Console.WriteLine($"Imagem: {cliente.Imagem}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhum cliente cadastrado");
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
            Console.Write("Informe o Documento: ");
            var documento = Console.ReadLine();
            Console.Write("Informe o Email: ");
            var email = Console.ReadLine();
            Console.Write("Informe a Data Nascimento - (yyyy-mm-dd): ");
            var dataNascimento = Console.ReadLine();
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
                var linhasAfetadas = _repo.Inserir(nome, documento, email, dataNascimento, endereco, cidade, estado, pais, cep, fone, imagem);

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Cliente inserido com sucesso");
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
                    ConsoleExtensions.Success("Cliente excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Cliente não existe");
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
