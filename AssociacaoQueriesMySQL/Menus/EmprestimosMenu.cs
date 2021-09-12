using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public static class EmprestimosMenu
    {
        private static Action _menuInicial;
        private static string _connectionString;

        private static void Iniciar()
        {
            Iniciar(_menuInicial, _connectionString);
        }

        public static void Iniciar(Action menuInicial, string connectionString)
        {
            _menuInicial = menuInicial;
            _connectionString = connectionString;

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

        private static void Listar()
        {
            Console.Clear();

            using var db = new EmprestimoRepositorio(_connectionString);
            db.Listar();
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }

        private static void Inserir()
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

            using var db = new EmprestimoRepositorio(_connectionString);
            db.Inserir(produtoId, clienteId, usuarioId, dataEmprestimo, dataLimiteDevolucao);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }

        private static void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new EmprestimoRepositorio(_connectionString);
            db.Remover(id);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }
    }
}
