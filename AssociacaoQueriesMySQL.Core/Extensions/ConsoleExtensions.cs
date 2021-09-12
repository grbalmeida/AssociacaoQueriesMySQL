using System;

namespace AssociacaoQueriesMySQL.Core.Extensions
{
    public static class ConsoleExtensions
    {
        public static void Success(object value)
        {
            var defaultForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultForegroundColor;
        }

        public static void Error(object value)
        {
            var defaultForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultForegroundColor;
        }

        public static void Warning(object value)
        {
            var defaultForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(value);
            Console.ForegroundColor = defaultForegroundColor;
        }
    }
}
