﻿using System.Linq.Expressions;

namespace SergipeVac.Model.Interface
{
    public interface IRepositorio<T> where T : class
    {
        IQueryable<T> ObterTodos();
        void Adicionar(T entidade);
        void AdicionarConjunto(List<T> entidades);
        void Atualizar(T entidade);
        void Excluir(T entidade);
        IQueryable<T> Obter(Expression<Func<T, bool>> expressao);
        int QuantidadeDe(Expression<Func<T, bool>> expressao);
        int QuantidadeTotal();
        int ObterProximoId(string nomeCampoId);
    }
}
