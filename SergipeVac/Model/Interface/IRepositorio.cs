using System.Linq.Expressions;

namespace SergipeVac.Model.Interface
{
    public interface IRepositorio<T> where T : class
    {
        IQueryable<T> ObterTodos();
        void Adicionar(T entidade);
        void AdicionarConjunto(List<T> entidades);
        IQueryable<T> Obter(Expression<Func<T, bool>> expressao);
    }
}
