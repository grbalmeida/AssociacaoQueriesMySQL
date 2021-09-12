using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Database;
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
            Console.WriteLine("4 - Produtos");
            Console.WriteLine("5 - Empréstimos");
            Console.WriteLine();

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", () => UsuariosMenu.Iniciar(ExibirMenuInicial, _connectionString) },
                { "2", () => ClientesMenu.Iniciar(ExibirMenuInicial, _connectionString) },
                { "3", () => CategoriasMenu.Iniciar(ExibirMenuInicial, _connectionString) },
                { "4", () => ProdutosMenu.Iniciar(ExibirMenuInicial, _connectionString) },
                { "5", () => EmprestimosMenu.Iniciar(ExibirMenuInicial, _connectionString) }
            };

            opcoes.ExecutarOpcao(opcao, ExibirMenuInicial);
        }

        static void CriarTabelas()
        {
            using var db = new InicializarTabelas(_connectionString);

            Console.Clear();
        }
    }
}
