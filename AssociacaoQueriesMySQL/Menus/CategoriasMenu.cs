using AssociacaoQueriesMySQL.Core.Extensions;
using AssociacaoQueriesMySQL.Database;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public class CategoriasMenu : IDisposable
    {
        private readonly Action _menuInicial;
        private readonly CategoriaRepositorio _repo;

        public CategoriasMenu(Action menuInicial)
        {
            _menuInicial = menuInicial;
            _repo = new CategoriaRepositorio();
        }

        public void Iniciar()
        {
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
            
            _repo.Listar(nomeFiltro);
            Dispose();

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

            _repo.Inserir(id, nome);
            Dispose();

            Console.ReadKey();
            Iniciar();
        }

        private void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            _repo.Remover(id);
            Dispose();

            Console.ReadKey();
            Iniciar();
        }

        public void Dispose()
        {
            _repo?.Dispose();
        }
    }
}
