using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Core.Models.Filtros;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class ProdutosMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private ProdutoRepositorio _repo;

        public ProdutosMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
        }

        public void Iniciar()
        {
            _repo = new ProdutoRepositorio();

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

        private void Listar()
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

            var produtos = _repo.Listar(produtoFiltro);
            Dispose();

            if (produtos.Count > 0)
            {
                foreach (var produto in produtos)
                {
                    Console.WriteLine($"Id: {produto.Id}");
                    Console.WriteLine($"Nome: {produto.Nome}");
                    Console.WriteLine($"Descrição: {produto.Descricao}");
                    Console.WriteLine(string.Format("Ativo: {0}", produto.Ativo ? "S" : "N"));
                    Console.WriteLine($"Valor: {produto.Valor}");
                    Console.WriteLine($"Quantidade em Estoque: {produto.QuantidadeEstoque}");
                    Console.WriteLine($"Altura: {produto.Altura}");
                    Console.WriteLine(
                        $"Dimensões do produto: Largura: {produto.Largura} cm / Profundidade: {produto.Profundidade} cm / Altura: {produto.Altura} cm"
                    );
                    Console.WriteLine($"Categoria: {produto.Categoria.Nome}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhum produto cadastrado");
                Console.WriteLine();
            }

            Console.ReadKey();
            Iniciar();
        }

        private void Inserir()
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

            try
            {
                var linhasAfetadas = _repo.Inserir(nome, descricao, ativo.Equals("S"), valor, categoriaId, quantidadeEstoque, altura, largura, profundidade);

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Produto inserido com sucesso");
                }
            }
            catch(DbException e)
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
                    ConsoleExtensions.Success("Produto excluído com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Produto não existe");
                }
            }
            catch(DbException e)
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
