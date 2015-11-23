using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntityGenerics.Core.Abstractions
{
    public interface IAsyncRepository<in TKey, TEntity, TViewModel> : IDisposable
    {
        Task<IQueryable<TEntity>> ToListAsync();
        Task<TEntity> AddAsync(TViewModel viewModel);
        Task<TEntity> AddAsync(TEntity entity);
        Task<IQueryable<TEntity>> FindAllAsync(IRepositoryQuery query = null);
        Task<TEntity> FindAsync(TKey id);
        Task<TEntity> FindAsync(Func<TEntity, bool> predicate);
        Task<bool> UpdateAsync(TKey id, TViewModel viewModel);
        Task<TEntity> RemoveAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
        Task<bool> SaveChangesAsync();
        Task<TViewModel> ConvertToViewModelAsync(TEntity entity);
    }
}