using System;
using System.Collections.Generic;

namespace AssociacaoQueriesMySQL.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static void ExecutarOpcao(this Dictionary<string, Action> dicionario, string opcao, Action acaoPadrao)
        {
            if (dicionario.ContainsKey(opcao))
            {
                dicionario[opcao].Invoke();
            }
            else
            {
                acaoPadrao.Invoke();
            }
        }
    }
}
