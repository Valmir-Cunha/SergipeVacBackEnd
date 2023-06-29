namespace SergipeVac.Model
{
    public class Endereco
    {
        public int Id{ get; set; }
        public string CEP { get; set; }
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; }
    }
}
