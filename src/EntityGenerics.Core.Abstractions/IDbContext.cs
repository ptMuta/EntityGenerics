using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using EntityGenerics.Annotations;

namespace EntityGenerics.Core.Abstractions
{
    public interface IDbContext
    {
        /// <summary>
        /// Provides access to database related information and operations for this context.
        /// 
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// Provides access to information and operations for entity instances this context is tracking.
        /// 
        /// </summary>
        ChangeTracker ChangeTracker { get; }

        /// <summary>
        /// The metadata about the shape of entities, the relationships between them, and how they map to the database.
        /// 
        /// </summary>
        IModel Model { get; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// This method will automatically call <see cref="M:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.DetectChanges"/> to discover any
        ///                 changes to entity instances before saving to the underlying database. This can be disabled via
        ///                 <see cref="P:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled"/>.
        /// 
        /// </remarks>
        /// 
        /// <returns>
        /// The number of state entries written to the database.
        /// 
        /// </returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// 
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.AcceptAllChanges"/> is called after the changes have
        ///                 been sent successfully to the database.
        ///             </param>
        /// <remarks>
        /// This method will automatically call <see cref="M:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.DetectChanges"/> to discover any
        ///                 changes to entity instances before saving to the underlying database. This can be disabled via
        ///                 <see cref="P:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled"/>.
        /// 
        /// </remarks>
        /// 
        /// <returns>
        /// The number of state entries written to the database.
        /// 
        /// </returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.DetectChanges"/> to discover any
        ///                     changes to entity instances before saving to the underlying database. This can be disabled via
        ///                     <see cref="P:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled"/>.
        /// 
        /// </para>
        /// 
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///                     that any asynchronous operations have completed before calling another method on this context.
        /// 
        /// </para>
        /// 
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        ///                 number of state entries written to the database.
        /// 
        /// </returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// 
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.AcceptAllChanges"/> is called after the changes have
        ///                 been sent successfully to the database.
        ///             </param>
        /// <remarks>
        /// 
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.DetectChanges"/> to discover any
        ///                     changes to entity instances before saving to the underlying database. This can be disabled via
        ///                     <see cref="P:Microsoft.Data.Entity.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled"/>.
        /// 
        /// </para>
        /// 
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        ///                     that any asynchronous operations have completed before calling another method on this context.
        /// 
        /// </para>
        /// 
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        ///                 number of state entries written to the database.
        /// 
        /// </returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Releases the allocated resources for this context.
        /// 
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets an <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry`1"/> for the given entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity. </typeparam><param name="entity">The entity to get the entry for. </param>
        /// <returns>
        /// The entry for the given entity.
        /// </returns>
        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// 
        /// <para>
        /// Gets an <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> for the given entity. The entry provides
        ///                     access to change tracking information and operations for the entity.
        /// 
        /// </para>
        /// 
        /// <para>
        /// This method may be called on an entity that is not tracked. You can then
        ///                     set the <see cref="P:Microsoft.Data.Entity.ChangeTracking.EntityEntry.State"/> property on the returned entry
        ///                     to have the context begin tracking the entity in the specified state.
        /// 
        /// </para>
        /// 
        /// </summary>
        /// <param name="entity">The entity to get the entry for. </param>
        /// <returns>
        /// The entry for the given entity.
        /// </returns>
        EntityEntry Entry([NotNull] object entity);

        /// <summary>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state such that it will
        ///                 be inserted into the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity. </typeparam><param name="entity">The entity to add. </param><param name="behavior">Determines whether the context will bring in only the given entity or also other related entities.
        ///             </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry`1"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry<TEntity> Add<TEntity>([NotNull] TEntity entity, GraphBehavior behavior = GraphBehavior.IncludeDependents) where TEntity : class;

        /// <summary>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/> state such that no
        ///                 operation will be performed when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity. </typeparam><param name="entity">The entity to attach. </param><param name="behavior">Determines whether the context will bring in only the given entity or also other related entities.
        ///             </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry`1"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry<TEntity> Attach<TEntity>([NotNull] TEntity entity, GraphBehavior behavior = GraphBehavior.IncludeDependents) where TEntity : class;

        /// <summary>
        /// 
        /// <para>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Modified"/> state such that it will
        ///                     be updated in the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </para>
        /// 
        /// <para>
        /// All properties of the entity will be marked as modified. To mark only some properties as modified, use
        ///                     <see cref="M:Microsoft.Data.Entity.DbContext.Attach``1(``0,Microsoft.Data.Entity.GraphBehavior)"/> to begin tracking the entity in the
        ///                     <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/> state and then use the returned <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry`1"/>
        ///                     to mark the desired properties as modified.
        /// 
        /// </para>
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity. </typeparam><param name="entity">The entity to update. </param><param name="behavior">Determines whether the context will bring in only the given entity or also other related entities.
        ///             </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry`1"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity, GraphBehavior behavior = GraphBehavior.IncludeDependents) where TEntity : class;

        /// <summary>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/> state such that it will
        ///                 be removed from the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// If the entity is already tracked in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state then the context will
        ///                 stop tracking the entity (rather than marking it as <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/>) since the
        ///                 entity was previously added to the context and does not exist in the database.
        /// 
        /// </remarks>
        /// <typeparam name="TEntity">The type of the entity. </typeparam><param name="entity">The entity to remove. </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry`1"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry<TEntity> Remove<TEntity>([NotNull] TEntity entity) where TEntity : class;

        /// <summary>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state such that it will
        ///                 be inserted into the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <param name="entity">The entity to add. </param><param name="behavior">Determines whether the context will bring in only the given entity or also other related entities.
        ///             </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry Add([NotNull] object entity, GraphBehavior behavior = GraphBehavior.IncludeDependents);

        /// <summary>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/> state such that no
        ///                 operation will be performed when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <param name="entity">The entity to attach. </param><param name="behavior">Determines whether the context will bring in only the given entity or also other related entities.
        ///             </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry Attach([NotNull] object entity, GraphBehavior behavior = GraphBehavior.IncludeDependents);

        /// <summary>
        /// 
        /// <para>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Modified"/> state such that it will
        ///                     be updated in the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </para>
        /// 
        /// <para>
        /// All properties of the entity will be marked as modified. To mark only some properties as modified, use
        ///                     <see cref="M:Microsoft.Data.Entity.DbContext.Attach(System.Object,Microsoft.Data.Entity.GraphBehavior)"/> to begin tracking the entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/>
        ///                     state and then use the returned <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> to mark the desired properties as modified.
        /// 
        /// </para>
        /// 
        /// </summary>
        /// <param name="entity">The entity to update. </param><param name="behavior">Determines whether the context will bring in only the given entity or also other related entities.
        ///             </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry Update([NotNull] object entity, GraphBehavior behavior = GraphBehavior.IncludeDependents);

        /// <summary>
        /// Begins tracking the given entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/> state such that it will
        ///                 be removed from the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// If the entity is already tracked in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state then the context will
        ///                 stop tracking the entity (rather than marking it as <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/>) since the
        ///                 entity was previously added to the context and does not exist in the database.
        /// 
        /// </remarks>
        /// <param name="entity">The entity to remove. </param>
        /// <returns>
        /// The <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> for the entity. The entry provides
        ///                 access to change tracking information and operations for the entity.
        /// 
        /// </returns>
        EntityEntry Remove([NotNull] object entity);

        /// <summary>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state such that they will
        ///                 be inserted into the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <param name="entities">The entities to add. </param>
        void AddRange([NotNull] params object[] entities);

        /// <summary>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/> state such that no
        ///                 operation will be performed when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <param name="entities">The entities to attach. </param>
        void AttachRange([NotNull] params object[] entities);

        /// <summary>
        /// 
        /// <para>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Modified"/> state such that they will
        ///                     be updated in the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </para>
        /// 
        /// <para>
        /// All properties of the entities will be marked as modified. To mark only some properties as modified, use
        ///                     <see cref="M:Microsoft.Data.Entity.DbContext.Attach(System.Object,Microsoft.Data.Entity.GraphBehavior)"/> to begin tracking each entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/>
        ///                     state and then use the returned <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> to mark the desired properties as modified.
        /// 
        /// </para>
        /// 
        /// </summary>
        /// <param name="entities">The entities to update. </param>
        void UpdateRange([NotNull] params object[] entities);

        /// <summary>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/> state such that they will
        ///                 be removed from the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// If any of the entities are already tracked in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state then the context will
        ///                 stop tracking those entities (rather than marking them as <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/>) since those
        ///                 entities were previously added to the context and do not exist in the database.
        /// 
        /// </remarks>
        /// <param name="entities">The entities to remove. </param>
        void RemoveRange([NotNull] params object[] entities);

        /// <summary>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state such that they will
        ///                 be inserted into the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <param name="entities">The entities to add. </param><param name="behavior">Determines whether the context will bring in only the given entities or also other related entities.
        ///             </param>
        void AddRange([NotNull] IEnumerable<object> entities, GraphBehavior behavior = GraphBehavior.IncludeDependents);

        /// <summary>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/> state such that no
        ///                 operation will be performed when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// <param name="entities">The entities to attach. </param><param name="behavior">Determines whether the context will bring in only the given entities or also other related entities.
        ///             </param>
        void AttachRange([NotNull] IEnumerable<object> entities, GraphBehavior behavior = GraphBehavior.IncludeDependents);

        /// <summary>
        /// 
        /// <para>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Modified"/> state such that they will
        ///                     be updated in the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </para>
        /// 
        /// <para>
        /// All properties of the entities will be marked as modified. To mark only some properties as modified, use
        ///                     <see cref="M:Microsoft.Data.Entity.DbContext.Attach(System.Object,Microsoft.Data.Entity.GraphBehavior)"/> to begin tracking each entity in the <see cref="F:Microsoft.Data.Entity.EntityState.Unchanged"/>
        ///                     state and then use the returned <see cref="T:Microsoft.Data.Entity.ChangeTracking.EntityEntry"/> to mark the desired properties as modified.
        /// 
        /// </para>
        /// 
        /// </summary>
        /// <param name="entities">The entities to update. </param><param name="behavior">Determines whether the context will bring in only the given entities or also other related entities.
        ///             </param>
        void UpdateRange([NotNull] IEnumerable<object> entities, GraphBehavior behavior = GraphBehavior.IncludeDependents);

        /// <summary>
        /// Begins tracking the given entities in the <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/> state such that they will
        ///                 be removed from the database when <see cref="M:Microsoft.Data.Entity.DbContext.SaveChanges"/> is called.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// If any of the entities are already tracked in the <see cref="F:Microsoft.Data.Entity.EntityState.Added"/> state then the context will
        ///                 stop tracking those entities (rather than marking them as <see cref="F:Microsoft.Data.Entity.EntityState.Deleted"/>) since those
        ///                 entities were previously added to the context and do not exist in the database.
        /// 
        /// </remarks>
        /// <param name="entities">The entities to remove. </param>
        void RemoveRange([NotNull] IEnumerable<object> entities);

        /// <summary>
        /// Creates a <see cref="T:Microsoft.Data.Entity.DbSet`1"/> that can be used to query and save instances of <typeparamref name="TEntity"/>.
        /// 
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned. </typeparam>
        /// <returns>
        /// A set for the given entity type.
        /// </returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}