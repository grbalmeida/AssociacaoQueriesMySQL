using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Database;
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
            Console.WriteLine("3 - Remover Produto");

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

            Console.WriteLine("Filtros");
            Console.Write("Informe o Nome: ");
            var nomeFiltro = Console.ReadLine();
            Console.Write("Informe a Descrição: ");
            var descricaoFiltro = Console.ReadLine();
            Console.Write("Produto Ativo (S/N): ");
            var ativoFiltro = Console.ReadLine();
            Console.Write("Valor Mínimo: ");
            var valorMinimoFiltro = Console.ReadLine();
            Console.Write("Valor Máximo: ");
            var valorMaximoFiltro = Console.ReadLine();
            Console.Write("Categoria: ");
            var categoriaFiltro = Console.ReadLine();
            Console.Write("Quantidade Estoque Mínima: ");
            var quantidadeEstoqueMinimaFiltro = Console.ReadLine();
            Console.Write("Quantidade Estoque Máxima: ");
            var quantidadeEstoqueMaximaFiltro = Console.ReadLine();
            Console.Write("Altura Mínima: ");
            var alturaMinimaFiltro = Console.ReadLine();
            Console.Write("Altura Máxima: ");
            var alturaMaximaFiltro = Console.ReadLine();
            Console.Write("Largura Mínima: ");
            var larguraMinimaFiltro = Console.ReadLine();
            Console.Write("Largura Máxima: ");
            var larguraMaximaFiltro = Console.ReadLine();
            Console.Write("Profundidade Mínima: ");
            var profundidadeMinimaFiltro = Console.ReadLine();
            Console.Write("Profundidade Máxima: ");
            var profundidadeMaximaFiltro = Console.ReadLine();

            var produtoFiltro = new ProdutoFiltro
            {
                Nome = nomeFiltro,
                Descricao = descricaoFiltro,
                Ativo = (ativoFiltro == "S" || ativoFiltro == "N")
                    ? ativoFiltro.Equals("S") : null,
                AlturaMinima = !string.IsNullOrEmpty(alturaMinimaFiltro)
                    ? Convert.ToDecimal(alturaMinimaFiltro) : null,
                AlturaMaxima = !string.IsNullOrEmpty(alturaMaximaFiltro)
                    ? Convert.ToDecimal(alturaMaximaFiltro) : null,
                CategoriaId = !string.IsNullOrEmpty(categoriaFiltro)
                    ? Convert.ToInt32(categoriaFiltro) : null,
                LarguraMinima = !string.IsNullOrEmpty(larguraMinimaFiltro)
                    ? Convert.ToDecimal(larguraMinimaFiltro) : null,
                LarguraMaxima = !string.IsNullOrEmpty(larguraMaximaFiltro)
                    ? Convert.ToDecimal(larguraMaximaFiltro) : null,
                QuantidadeEstoqueMinima = !string.IsNullOrEmpty(quantidadeEstoqueMinimaFiltro)
                    ? Convert.ToInt32(quantidadeEstoqueMinimaFiltro) : null,
                QuantidadeEstoqueMaxima = !string.IsNullOrEmpty(quantidadeEstoqueMaximaFiltro)
                    ? Convert.ToInt32(quantidadeEstoqueMaximaFiltro) : null,
                ProfundidadeMinima = !string.IsNullOrEmpty(profundidadeMinimaFiltro)
                    ? Convert.ToDecimal(profundidadeMinimaFiltro) : null,
                ProfundidadeMaxima = !string.IsNullOrEmpty(profundidadeMaximaFiltro)
                    ? Convert.ToDecimal(profundidadeMaximaFiltro) : null,
                ValorMinimo = !string.IsNullOrEmpty(valorMinimoFiltro)
                    ? Convert.ToDecimal(valorMinimoFiltro) : null,
                ValorMaximo = !string.IsNullOrEmpty(valorMaximoFiltro)
                    ? Convert.ToDecimal(valorMaximoFiltro) : null
            };

            Console.Clear();

            using var db = new ProdutoRepositorio(_connectionString);
            db.Listar(produtoFiltro);
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

        private static void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new ProdutoRepositorio(_connectionString);
            db.Remover(id);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }
    }
}
