using AssociacaoQueriesMySQL.Database;
using AssociacaoQueriesMySQL.Extensions;
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
                { "1", UsuariosMenu },
                { "2", ClientesMenu },
                { "3", CategoriasMenu }
            };

            opcoes.ExecutarOpcao(opcao, ExibirMenuInicial);
        }

        static void UsuariosMenu()
        {
            Console.Clear();
            Console.WriteLine("1 - Listar Usuários");
            Console.WriteLine("3 - Remover Usuário");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", ListarUsuarios },
                { "3", RemoverUsuario }
            };

            opcoes.ExecutarOpcao(opcao, ExibirMenuInicial);
        }

        static void ClientesMenu()
        {
            Console.Clear();
            Console.WriteLine("1 - Listar Clientes");
            Console.WriteLine("2 - Inserir Cliente");
            Console.WriteLine("3 - Remover Cliente");

            var opcao = Console.ReadLine();

            var opcoes = new Dictionary<string, Action>
            {
                { "1", ListarClientes },
                { "2", InserirCliente },
                { "3", RemoverCliente }
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

        static void ListarClientes()
        {
            Console.Clear();

            using var db = new ClienteRepositorio(_connectionString);
            db.Listar();

            Console.ReadKey();
            ClientesMenu();
        }

        static void ListarUsuarios()
        {
            Console.Clear();

            using var db = new UsuarioRepositorio(_connectionString);
            db.Listar();

            Console.ReadKey();
            UsuariosMenu();
        }

        static void ListarCategorias()
        {
            Console.Clear();

            using var db = new CategoriaRepositorio(_connectionString);
            db.Listar();

            Console.ReadKey();
            CategoriasMenu();
        }

        static void InserirCliente()
        {
            Console.Clear();
            Console.Write("Informe o Nome: ");
            var nome = Console.ReadLine();
            Console.Write("Informe o Documento: ");
            var documento = Console.ReadLine();
            Console.Write("Informe o Email: ");
            var email = Console.ReadLine();
            Console.Write("Informe a Data Nascimento - (yyyy-mm-dd): ");
            var dataNascimento = Console.ReadLine();
            Console.Write("Informe o Endereço: ");
            var endereco = Console.ReadLine();
            Console.Write("Informe a Cidade: ");
            var cidade = Console.ReadLine();
            Console.Write("Informe o Estado: ");
            var estado = Console.ReadLine();
            Console.Write("Informe o País: ");
            var pais = Console.ReadLine();
            Console.Write("Informe o CEP: ");
            var cep = Console.ReadLine();
            Console.Write("Informe o Fone: ");
            var fone = Console.ReadLine();
            Console.Write("Informe a Imagem: ");
            var imagem = Console.ReadLine();

            using var db = new ClienteRepositorio(_connectionString);
            db.Inserir(nome, documento, email, dataNascimento, endereco, cidade, estado, pais, cep, fone, imagem);

            Console.ReadKey();
            ClientesMenu();
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

        static void RemoverCliente()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new ClienteRepositorio(_connectionString);
            db.Remover(id);

            Console.ReadKey();
            ClientesMenu();
        }

        static void RemoverUsuario()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new UsuarioRepositorio(_connectionString);
            db.Remover(id);

            Console.ReadKey();
            UsuariosMenu();
        }

        static void CriarTabelas()
        {
            
            using var db = new InicializarTabelas(_connectionString);

            Console.Clear();
        }
    }
}
