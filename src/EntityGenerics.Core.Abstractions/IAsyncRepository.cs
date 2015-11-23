using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;

namespace EntityGenerics.Core.Abstractions
{
    public interface IAsyncRepository<in TKey, TEntity, in TViewModel, in TQuery> : IDisposable
        where TEntity : class, IEntity<TKey>, new()
    {
        DbSet<TEntity> Entities { get; }
        Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<EntityEntry<TEntity>> AddAsync(TViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken));
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<IQueryable<TEntity>> FindAllAsync(TQuery query = default(TQuery), CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateAsync(TKey id, TViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken));
        Task<EntityEntry<TEntity>> RemoveAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}