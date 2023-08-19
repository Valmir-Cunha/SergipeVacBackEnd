using SergipeVac.Model.Interface;
using SergipeVac.Model.ModeloDados;

namespace SergipeVac.Infra.Repositorios
{
    public class RepositorioPaciente : Repositorio<Paciente> , IRepositorioPaciente
    {
        private readonly Contexto _contexto;

        public RepositorioPaciente(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public void AdicionarConjunto(List<Paciente> entidades)
        {
            var idProperty = typeof(Paciente).GetProperty("Id");
            var existingIds = _contexto.Set<Paciente>().Select(e => idProperty.GetValue(e)).ToList();

            var entidadesParaAdicionar = new List<Paciente>();

            foreach (var entidade in entidades)
            {
                var entityId = idProperty.GetValue(entidade);

                if (!existingIds.Contains(entityId))
                {
                    entidadesParaAdicionar.Add(entidade);
                    existingIds.Add(entityId);
                }
            }

            _contexto.AddRange(entidadesParaAdicionar);
            _contexto.SaveChanges();
        }
    }
}
