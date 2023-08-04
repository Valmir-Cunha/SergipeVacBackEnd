using SergipeVac.Model.Autenticacao;

namespace SergipeVac.Model.Interface
{
    public interface IServicoToken
    {
        string GerarToken(Usuario usuario);
    }
}
