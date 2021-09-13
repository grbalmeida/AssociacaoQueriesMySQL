using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Core.Models;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class CategoriasMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private CategoriaRepositorio _repo;

        public CategoriasMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
        }

        public void Iniciar()
        {
            _repo = new CategoriaRepositorio();

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

        private void Listar()
        {
            Console.Clear();

            Console.WriteLine("Filtros");
            Console.Write("Informe o Nome: ");
            var nomeFiltro = Console.ReadLine();

            Console.Clear();
            
            var categorias = _repo.Listar(nomeFiltro);
            Dispose();

            if (categorias.Count > 0)
            {
                foreach (var categoria in categorias)
                {
                    Console.WriteLine($"Id: {categoria.Id}");
                    Console.WriteLine($"Nome: {categoria.Nome}");
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleExtensions.Warning("Nenhuma categoria cadastrada");
                Console.WriteLine();
            }

            Console.ReadKey();
            Iniciar();
        }

        private void Inserir()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Informe o Nome: ");
            var nome = Console.ReadLine();

            try
            {
                var linhasAfetadas = _repo.Inserir(id, nome);

                if (linhasAfetadas > 0)
                {
                    ConsoleExtensions.Success("Categoria inserida com sucesso");
                }
            }
            catch (DbException e)
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
                    ConsoleExtensions.Success("Categoria excluída com sucesso");
                }
                else
                {
                    ConsoleExtensions.Warning("Categoria não existe");
                }
            }
            catch (DbException e)
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
