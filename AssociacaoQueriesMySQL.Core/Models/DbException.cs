using System;

namespace AssociacaoQueriesMySQL.Core.Models
{
    public class DbException : Exception
    {
        public DbException(string message) : base(message) { }
    }
}
