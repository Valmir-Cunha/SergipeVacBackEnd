using SergipeVac.Model.ModeloDados;

namespace SergipeVac.Model.Interface
{
    public interface IRepositorioPaciente : IRepositorio<Paciente>
    {
        void AdicionarConjunto(List<Paciente> entidades);
    }
}
