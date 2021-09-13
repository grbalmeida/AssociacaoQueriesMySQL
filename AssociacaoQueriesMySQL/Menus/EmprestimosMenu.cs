using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class EmprestimosMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private EmprestimoRepositorio _repo;

        public EmprestimosMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
        }

        public void Iniciar()
        {
            _repo = new EmprestimoRepositorio();

            Console.Clear();
            Console.WriteLine("1 - Listar Empréstimos");
            Console.WriteLine("2 - Inserir Empréstimo");
            Console.WriteLine("3 - Remover Empréstimo");

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

            var emprestimos = _repo.Listar();
            Dispose();

            if (emprestimos.Count > 0)
            {
                foreach (var emprestimo in emprestimos)
                {
                    Console.WriteLine($"Id: {emprestimo.Id}");
                    Console.WriteLine($"Data Empréstimo: {emprestimo.DataEmprestimo:dd/MM/yyyy HH:mm:ss}");
                    Console.WriteLine($"Data Limite Devolução: {emprestimo.DataLimiteDevolucao?.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"Data Devolução: {emprestimo.DataDevolucao?.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"Id Empréstimo Anterior: {emprestimo.EmprestimoAnteriorId}");
                    Console.WriteLine($"Nome Produto: {emprestimo.Produto.Nome}");
                    Console.WriteLine($"Nome Cliente: {emprestimo.Cliente.Nome}");
                    Console.WriteLine($"Nome Usuário: {emprestimo.Usuario.Nome}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhum empréstimo cadastrado");
                Console.WriteLine();
            }

            Console.ReadKey();
            Iniciar();
        }

        private void Inserir()
        {
            Console.Clear();

            Console.Write("Informe o id do Produto: ");
            var produtoId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe o id do Cliente: ");
            var clienteId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe o id do Usuário: ");
            var usuarioId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe a Data do Empréstimo (yyyy-MM-dd HH:mm:ss): ");
            var dataEmprestimo = Console.ReadLine();
            Console.Write("Informe a Data Limite de Devolução do Produto (yyyy-MM-dd HH:mm:ss): ");
            var dataLimiteDevolucao = Console.ReadLine();

            try
            {
                var linhasAfetadas = _repo.Inserir(produtoId, clienteId, usuarioId, dataEmprestimo, dataLimiteDevolucao);

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Empréstimo inserido com sucesso");
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
                    ConsoleExtensions.Success("Empréstimo excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Empréstimo não existe");
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
