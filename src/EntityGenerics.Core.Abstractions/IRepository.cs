using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;

namespace EntityGenerics.Core.Abstractions
{
    public interface IRepository<in TKey, TEntity, in TViewModel, in TQuery> : IDisposable 
        where TEntity : class, IEntity<TKey>, new()
    {
        DbSet<TEntity> Entities { get; }
        List<TEntity> ToList();
        EntityEntry<TEntity> Add(TViewModel viewModel);
        EntityEntry<TEntity> Add(TEntity entity);
        IQueryable<TEntity> FindAll(TQuery query = default(TQuery));
        TEntity Find(TKey id);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        void Update(TKey id, TViewModel viewModel);
        EntityEntry<TEntity> Remove(TKey id);
        bool Exists(TKey id);
        int SaveChanges();
    }
}