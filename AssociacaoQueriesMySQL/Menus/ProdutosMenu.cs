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
            Console.WriteLine("2 - Inserir Produto");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", Listar },
                { "2", Inserir }
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

        private static void Inserir()
        {
            Console.Clear();
            Console.Write("Informe o Nome: ");
            var nome = Console.ReadLine();
            Console.Write("Informe a Descrição: ");
            var descricao = Console.ReadLine();
            Console.Write("Produto Ativo (S/N): ");
            var ativo = Console.ReadLine();
            Console.Write("Informe o Valor: ");
            var valor = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Informe o id da Categoria: ");
            var categoriaId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe a Quantidade em Estoque: ");
            var quantidadeEstoque = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe a Altura: ");
            var altura = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Informe a Largura: ");
            var largura = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Informe a Profundidade: ");
            var profundidade = Convert.ToDecimal(Console.ReadLine());
            

            using var db = new ProdutoRepositorio(_connectionString);
            db.Inserir(nome, descricao, ativo.Equals("S"), valor, categoriaId, quantidadeEstoque, altura, largura, profundidade);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }
    }
}
