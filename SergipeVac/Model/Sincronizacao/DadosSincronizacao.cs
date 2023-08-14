namespace SergipeVac.Model.Sincronizacao
{
    public class DadosSincronizacao
    {
        public int Id { get; set; }
        public int QuantidadeRegistrosAdicionados { get; set; }
        public bool BemSucedida { get; set; }
        public DateTime UltimaSincronizacao { get; set; }
    }
}
