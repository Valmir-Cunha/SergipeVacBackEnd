namespace SergipeVac.Model
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public int Idade { get; set; }
        public DateTime DataNascimento { get; set; }
        public int EnderecoId { get; set; }
        public int SexoBiologicoId { get; set; }
        public int RacaId { get; set; }
        public int NacionalidadeId { get; set; }

        public SexoBiologico SexoBiologico { get; set; }
        public Raca Raca { get; set; }
        public Endereco Endereco { get; set; }
        public Nacionalidade Nacionalidade{ get; set; }
    }
}
