using Microsoft.EntityFrameworkCore;
using SergipeVac.Model.Interface;
using System.Linq.Expressions;

namespace SergipeVac.Infra.Repositorios
{
    public class Repositorio<T> : IDisposable, IRepositorio<T> where T : class
    {
        private readonly Contexto _contexto;
        public Repositorio(Contexto contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(T entidade)
        {
            _contexto.Add(entidade);

            _contexto.SaveChanges();
        }

        public void AdicionarConjunto(List<T> entidades)
        {
            _contexto.AddRange(entidades);
            _contexto.SaveChanges();
        }

        public void Atualizar(T entidade)
        {
            _contexto.Update(entidade);
            _contexto.SaveChanges();
        }

        public void Excluir(T entidade)
        {
            _contexto.Remove(entidade);
            _contexto.SaveChanges();
        }

        public IQueryable<T> ObterTodos()
        {
            return _contexto.Set<T>();
        }

        public IQueryable<T> Obter(Expression<Func<T, bool>> expressao)
        {
            return _contexto.Set<T>().Where(expressao).AsQueryable<T>();
        }

        public int QuantidadeDe(Expression<Func<T, bool>> expressao)
        {
            return _contexto.Set<T>().Where(expressao).Count();
        }

        public int QuantidadeTotal()
        {
            return _contexto.Set<T>().Count();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }
    }
}
