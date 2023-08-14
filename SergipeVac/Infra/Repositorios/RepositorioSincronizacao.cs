using SergipeVac.Model.Sincronizacao;

namespace SergipeVac.Infra.Repositorios
{
    public class RepositorioSincronizacao : Repositorio<DadosSincronizacao>
    {
        private readonly Contexto _contexto;

        public RepositorioSincronizacao(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public DateTime ObterUltimaSincronizacaoBemSucedida()
        {
            return _contexto.Set<DadosSincronizacao>().Where(p => p.BemSucedida).Max(p => p.UltimaSincronizacao).Date;
        }
            
    }
}
