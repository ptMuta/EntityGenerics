using System;
using System.Collections.Generic;
using System.Linq;
using EntityGenerics.Core.Abstractions;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;

namespace EntityGenerics.Core
{
    public abstract class RepositoryBase<TKey, TEntity, TViewModel> : IRepository<TKey, TEntity, TViewModel> 
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly bool _ownsContext;
        protected IDbContext Context { get; set; }
        public DbSet<TEntity> Entities => Context.Set<TEntity>();

        protected RepositoryBase(IDbContext context)
        {
            Context = context;
        }

        protected RepositoryBase(IDbContext context, bool ownsContext)
        {
            _ownsContext = ownsContext;
        } 

        public List<TEntity> ToList()
        {
            return Entities.ToList();
        }

        public EntityEntry<TEntity> Add(TViewModel viewModel)
        {
            var newEntity = new TEntity();
            SyncEntity(newEntity, viewModel);
            return Add(newEntity);
        }

        public EntityEntry<TEntity> Add(TEntity entity)
        {
            return Entities.Add(entity);
        }

        public IQueryable<TEntity> FindAll(IRepositoryQuery query = null)
        {
            return ExecuteQuery(query);
        }

        public TEntity Find(TKey id)
        {
            return Entities.FirstOrDefault(t => t.Id.Equals(id));
        }

        public TEntity Find(Func<TEntity, bool> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        private TEntity GetOrThrow(TKey id)
        {
            var entity = Find(id);
            if (entity == null) throw new KeyNotFoundException(nameof(id));
            return entity;
        }

        public void Update(TKey id, TViewModel viewModel)
        {
            var entity = GetOrThrow(id);
            SyncEntity(entity, viewModel);
        }

        public EntityEntry<TEntity> Remove(TKey id)
        {
            var entity = GetOrThrow(id);
            return Entities.Remove(entity);
        }

        public bool Exists(TKey id)
        {
            return Entities.Any(t => t.Id.Equals(id));
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public void Dispose()
        {
            if (_ownsContext) Context.Dispose();
        }

        public abstract IQueryable<TEntity> ExecuteQuery(IRepositoryQuery query); 

        public abstract TViewModel ConvertToViewModel(TEntity entity);

        public abstract void SyncEntity(TEntity entity, TViewModel viewModel);
    }
}