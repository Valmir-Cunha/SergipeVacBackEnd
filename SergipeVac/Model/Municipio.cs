namespace SergipeVac.Model
{
    public class Municipio
    {
        public int Id { get; set; }
        public int CodigoIBGE { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }
        public int PaisId { get; set; }

        public Pais Pais { get; set; }
    }
}
