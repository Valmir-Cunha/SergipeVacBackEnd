namespace SergipeVac.Model
{
    public class Estabelecimento
    {
        public int Id { get; set; }
        public int Valor { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; }
    }
}
