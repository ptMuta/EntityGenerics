using System;
using System.Linq;

namespace EntityGenerics.Core.Abstractions
{
    public interface IRepository<in TKey, TEntity, TViewModel> : IDisposable
    {
        IQueryable<TEntity> ToList();
        TEntity Add(TViewModel viewModel);
        TEntity Add(TEntity entity);
        IQueryable<TEntity> FindAll(IRepositoryQuery query = null);
        TEntity Find(TKey id);
        TEntity Find(Func<TEntity, bool> predicate);
        bool Update(TKey id, TViewModel viewModel);
        TEntity Remove(TKey id);
        bool Exists(TKey id);
        bool SaveChanges();
        TViewModel ConvertToViewModel(TEntity entity);
    }
}