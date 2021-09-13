namespace AssociacaoQueriesMySQL.Core.Models.Filtros
{
    public class ProdutoFiltro
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMaximo { get; set; }
        public int? CategoriaId { get; set; }
        public int? QuantidadeEstoqueMinima { get; set; }
        public int? QuantidadeEstoqueMaxima { get; set; }
        public decimal? AlturaMinima { get; set; }
        public decimal? AlturaMaxima { get; set; }
        public decimal? LarguraMinima { get; set; }
        public decimal? LarguraMaxima { get; set; }
        public decimal? ProfundidadeMinima { get; set; }
        public decimal? ProfundidadeMaxima { get; set; }
    }
}
