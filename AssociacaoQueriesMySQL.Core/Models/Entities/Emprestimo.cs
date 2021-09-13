using System;

namespace AssociacaoQueriesMySQL.Core.Models.Entities
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataLimiteDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public int? EmprestimoAnteriorId { get; set; }
        public Produto Produto { get; set; }
        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }
    }
}
