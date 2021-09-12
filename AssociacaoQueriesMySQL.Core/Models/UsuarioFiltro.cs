namespace AssociacaoQueriesMySQL.Core.Models
{
    public class UsuarioFiltro
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string DataNascimentoInicial { get; set; }
        public string DataNascimentoFinal { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string CEP { get; set; }
        public string Fone { get; set; }
    }
}
