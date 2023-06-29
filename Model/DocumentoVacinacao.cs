namespace SergipeVac.Model
{
    public class DocumentoVacinacao
    {
        public int Id{ get; set; }
        public string Guid{ get; set; }
        public int PacienteId { get; set; }
        public int EstabelecimentoId { get; set; }
        public int SistemaOrigemId { get; set; }
        public int GrupoAtendimentoId { get; set; }
        public int CategoriaId { get; set; }
        public DateTime DataAplicacao { get; set; }
        public int FabricanteId { get; set; }
        public int VacinaCodigo { get; set; }
        public string Lote { get; set; }
        public string DescricaoDose { get; set; }
        public string Nome { get; set; }


        public Fabricante Fabricante { get; set; }


        public Paciente Paciente { get; set; }
        public Estabelecimento Estabelecimento { get; set; }
        public Sistema SistemaOrigem { get; set; }
        public GrupoAtendimento GrupoAtendimento { get; set; }
        public Categoria Categoria { get; set; }

    }
}
