﻿using AssociacaoQueriesMySQL.Database;
using AssociacaoQueriesMySQL.Extensions;
using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Menus
{
    public static class ClientesMenu
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
            Console.WriteLine("1 - Listar Clientes");
            Console.WriteLine("2 - Inserir Cliente");
            Console.WriteLine("3 - Remover Cliente");

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

            using var db = new ClienteRepositorio(_connectionString);
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
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }

        private static void Remover()
        {
            Console.Clear();
            Console.Write("Informe o Id: ");
            var id = Convert.ToInt32(Console.ReadLine());

            using var db = new ClienteRepositorio(_connectionString);
            db.Remover(id);
            db.Dispose();

            Console.ReadKey();
            Iniciar();
        }
    }
}