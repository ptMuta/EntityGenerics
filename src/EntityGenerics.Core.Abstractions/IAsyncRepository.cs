using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;

namespace EntityGenerics.Core.Abstractions
{
    public interface IAsyncRepository<in TKey, TEntity, TViewModel> : IDisposable
        where TEntity : class, IEntity<TKey>, new()
    {
        DbSet<TEntity> Entities { get; }
        Task<List<TEntity>> ToList();
        Task<EntityEntry<TEntity>> AddAsync(TViewModel viewModel);
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
        Task<IQueryable<TEntity>> FindAllAsync(IRepositoryQuery query = null);
        Task<TEntity> FindAsync(TKey id);
        Task<TEntity> FindAsync(Func<TEntity, bool> predicate);
        Task UpdateAsync(TKey id, TViewModel viewModel);
        Task<EntityEntry<TEntity>> RemoveAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
        Task<int> SaveChangesAsync();
        Task<TViewModel> ConvertToViewModelAsync(TEntity entity);
    }
}