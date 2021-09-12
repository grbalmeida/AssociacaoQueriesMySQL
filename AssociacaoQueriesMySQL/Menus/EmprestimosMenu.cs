using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class EmprestimosMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private readonly EmprestimoRepositorio _repo;

        public EmprestimosMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
            _repo = new EmprestimoRepositorio();
        }

        public void Iniciar()
        {
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

            _repo.Listar();
            Dispose();

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

            _repo.Inserir(produtoId, clienteId, usuarioId, dataEmprestimo, dataLimiteDevolucao);
            Dispose();

            Console.ReadKey();
            Iniciar();
        }

        private void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            _repo.Remover(id);
            Dispose();

            Console.ReadKey();
            Iniciar();
        }

        public void Dispose()
        {
            _repo?.Dispose();
        }
    }
}
