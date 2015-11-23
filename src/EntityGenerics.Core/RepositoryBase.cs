using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityGenerics.Core.Abstractions;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;

namespace EntityGenerics.Core
{
    public abstract class RepositoryBase<TKey, TEntity, TViewModel> : IRepository<TKey, TEntity, TViewModel>, IAsyncRepository<TKey, TEntity, TViewModel>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly bool _ownsContext;
        protected IDbContext Context { get; set; }
        public DbSet<TEntity> Entities => Context.Set<TEntity>();

        protected RepositoryBase(IDbContext context)
        {
            Context = context;
        }

        protected RepositoryBase(IDbContext context, bool ownsContext) : this(context)
        {
            _ownsContext = ownsContext;
        } 

        public List<TEntity> ToList()
        {
            return Entities.ToList();
        }

        public Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entities.ToListAsync(cancellationToken);
        }

        public EntityEntry<TEntity> Add(TViewModel viewModel)
        {
            var entity = new TEntity();
            SyncEntity(entity, viewModel);
            return Add(entity);
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = new TEntity();
            await SyncEntityAsync(entity, viewModel, cancellationToken);
            return Add(entity);
        }

        public EntityEntry<TEntity> Add(TEntity entity)
        {
            return Entities.Add(entity);
        }

        public Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(Add(entity));
        }

        public IQueryable<TEntity> FindAll(IRepositoryQuery query = null)
        {
            return ExecuteQuery(query);
        }

        public Task<IQueryable<TEntity>> FindAllAsync(IRepositoryQuery query = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return ExecuteQueryAsync(query, cancellationToken);
        }

        public TEntity Find(TKey id)
        {
            return Entities.FirstOrDefault(t => t.Id.Equals(id));
        }

        public Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entities.FirstOrDefaultAsync(t => t.Id.Equals(id), cancellationToken);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entities.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        private TEntity GetOrThrow(TKey id)
        {
            var entity = Find(id);
            if (entity == null) throw new KeyNotFoundException(nameof(id));
            return entity;
        }

        private async Task<TEntity> GetOrThrowAsync(TKey id, CancellationToken cancellationToken)
        {
            var entity = await FindAsync(id, cancellationToken);
            if (entity == null) throw new KeyNotFoundException(nameof(id));
            return entity;
        }

        public void Update(TKey id, TViewModel viewModel)
        {
            var entity = GetOrThrow(id);
            SyncEntity(entity, viewModel);
        }

        public async Task UpdateAsync(TKey id, TViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await GetOrThrowAsync(id, cancellationToken);
            await SyncEntityAsync(entity, viewModel, cancellationToken);
        }

        public EntityEntry<TEntity> Remove(TKey id)
        {
            var entity = GetOrThrow(id);
            return Entities.Remove(entity);
        }

        public async Task<EntityEntry<TEntity>> RemoveAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await GetOrThrowAsync(id, cancellationToken);
            return Entities.Remove(entity);
        }

        public bool Exists(TKey id)
        {
            return Entities.Any(t => t.Id.Equals(id));
        }

        public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entities.AnyAsync(t => t.Id.Equals(id), cancellationToken);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            if (_ownsContext) Context.Dispose();
        }

        public virtual IQueryable<TEntity> ExecuteQuery(IRepositoryQuery query)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IQueryable<TEntity>> ExecuteQueryAsync(IRepositoryQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual TViewModel ConvertToViewModel(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TViewModel> ConvertToViewModelAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual void SyncEntity(TEntity entity, TViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public virtual Task SyncEntityAsync(TEntity entity, TViewModel viewModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}