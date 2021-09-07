using AssociacaoQueriesMySQL.Database;
using AssociacaoQueriesMySQL.Extensions;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public static class CategoriasMenu
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
            Console.WriteLine("1 - Listar Categorias");
            Console.WriteLine("2 - Inserir Categoria");
            Console.WriteLine("3 - Remover Categoria");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", Listar },
                { "2", Inserir },
                { "3", Remover }
            };

            opcoes.ExecutarOpcao(opcao, _menuInicial);
        }

        static void Listar()
        {
            Console.Clear();

            using var db = new CategoriaRepositorio(_connectionString);
            db.Listar();
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }

        private static void Inserir()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe o Nome: ");
            var nome = Console.ReadLine();

            using var db = new CategoriaRepositorio(_connectionString);
            db.Inserir(id, nome);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }

        static void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new CategoriaRepositorio(_connectionString);
            db.Remover(id);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }
    }
}
