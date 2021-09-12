using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Database;
using AssociacaoQueriesMySQL.Menus;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL
{
    class Program
    {
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

            using var categoriaMenu = new CategoriasMenu(ExibirMenuInicial);
            using var clienteMenu = new ClientesMenu(ExibirMenuInicial);
            using var usuarioMenu = new UsuariosMenu(ExibirMenuInicial);
            using var produtoMenu = new ProdutosMenu(ExibirMenuInicial);
            using var emprestimoMenu = new EmprestimosMenu(ExibirMenuInicial);

            var opcoes = new Dictionary<string, Action>
            {
                { "1", () => usuarioMenu.Iniciar() },
                { "2", () => clienteMenu.Iniciar() },
                { "3", () => categoriaMenu.Iniciar() },
                { "4", () => produtoMenu.Iniciar() },
                { "5", () => emprestimoMenu.Iniciar() }
            };

            opcoes.ExecutarOpcao(opcao, ExibirMenuInicial);
        }

        static void CriarTabelas()
        {
            using var db = new InicializarTabelas();

            Console.Clear();
        }
    }
}
