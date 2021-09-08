using AssociacaoQueriesMySQL.Database;
using AssociacaoQueriesMySQL.Extensions;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class ProdutosMenu
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
            Console.WriteLine("1 - Listar Produtos");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", Listar },
            };

            opcoes.ExecutarOpcao(opcao, _menuInicial);
        }

        private static void Listar()
        {
            Console.Clear();

            using var db = new ProdutoRepositorio(_connectionString);
            db.Listar();
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }
    }
}
