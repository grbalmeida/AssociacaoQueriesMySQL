using AssociacaoQueriesMySQL.Database;
using AssociacaoQueriesMySQL.Extensions;
using AssociacaoQueriesMySQL.Menus;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace AssociacaoQueriesMySQL
{
    class Program
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        static void Main(string[] args)
        {
            CriarTabelas();

            ExibirMenuInicial();

            Console.ReadKey();
        }

        static void ExibirMenuInicial()
        {
            Console.Clear();
            Console.WriteLine("1 - Usuários");
            Console.WriteLine("2 - Clientes");
            Console.WriteLine("3 - Categorias");
            Console.WriteLine();

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", () => UsuariosMenu.Iniciar(ExibirMenuInicial, _connectionString) },
                { "2", () => ClientesMenu.Iniciar(ExibirMenuInicial, _connectionString) },
                { "3", CategoriasMenu }
            };

            opcoes.ExecutarOpcao(opcao, ExibirMenuInicial);
        }

        static void CategoriasMenu()
        {
            Console.Clear();
            Console.WriteLine("1 - Listar Categorias");
            Console.WriteLine("2 - Inserir Categoria");
            Console.WriteLine("3 - Remover Categoria");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", ListarCategorias },
                { "2", InserirCategoria },
                { "3", RemoverCategoria }
            };

            opcoes.ExecutarOpcao(opcao, ExibirMenuInicial);
        }

        static void ListarCategorias()
        {
            Console.Clear();

            using var db = new CategoriaRepositorio(_connectionString);
            db.Listar();

            Console.ReadKey();
            CategoriasMenu();
        }

        static void InserirCategoria()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe o Nome: ");
            var nome = Console.ReadLine();

            using var db = new CategoriaRepositorio(_connectionString);
            db.Inserir(id, nome);

            Console.ReadKey();
            CategoriasMenu();
        }

        static void RemoverCategoria()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new CategoriaRepositorio(_connectionString);
            db.Remover(id);

            Console.ReadKey();
            CategoriasMenu();
        }

        static void CriarTabelas()
        {
            using var db = new InicializarTabelas(_connectionString);

            Console.Clear();
        }
    }
}
