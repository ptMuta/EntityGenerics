# EntityGenerics
Generic data entity repository abstractions for Entity Generics and ASP.NET 5

# How to use

The `IEntity<TKey>`
------

To ease abstraction and generic usability your entities have to implement this basic interface.

```c#
public interface IEntity<TKey>
{
        TKey Id { get; set; }
}
```

`IRepository` and `IAsyncRepository` interface methods
------

The IRepository and IAsyncRepository defined the interfaces for building repositories.
The following methods are exposed publicly. For `IAsyncRepository` just add Async-suffix to method names.
(eg. AddAsync)

```c#
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
```

`RepositoryBase` Generic repository base class
------

The RepositoryBase-class implements `` and `` interfaces via generics. However in order for it to function fully you need to implement
either one or both of the following methods.

```c#
public virtual IQueryable<TEntity> ExecuteQuery(TQuery query)
{
    throw new NotImplementedException();
}

public virtual Task<IQueryable<TEntity>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken = default(CancellationToken))
{
    throw new NotImplementedException();
}

public virtual TViewModel ConvertToViewModel(TEntity entity)
{
    throw new NotImplementedException();
}

public virtual Task<TViewModel> ConvertToViewModelAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
{
    throw new NotImplementedException();
}

public virtual void SyncEntity(TEntity entity, TViewModel viewModel)
{
    throw new NotImplementedException();
}

public virtual Task SyncEntityAsync(TEntity entity, TViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
{
    throw new NotImplementedException();
}
```

Example
------

The following example shows the minimum work needed to create a repository of entities.

###### Task.cs
```c#
public class Task : IEntity<int>
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool Done { get; set; }
    public DateTime? LastChangeAt { get; set; }
}
```

###### TaskViewModel.cs
```c#
public class TaskViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool Done { get; set; }
    public DateTime? LastChangeAt { get; set; }
}
```

###### TaskRepository.cs
```c#
public class TaskRepository : RepositoryBase<int, Task, TaskViewModel, ExampleRepositoryQuery>
{
    public TaskRepository(IDbContext context) : base(context)
    {
    }

    public TaskRepository(IDbContext context, bool ownsContext) : base(context, ownsContext)
    {
    }

    public override IQueryable<Task> ExecuteQuery(ExampleRepositoryQuery query)
    {
        var result = Entities.Where(t => t.Description.ToLowerInvariant().Contains(query.Query.ToLowerInvariant()))
            .Skip(query.Page * query.Limit)
            .Take(query.Limit);

        if (query.HideCompleted)
            result = result.Where(t => !t.Done);

        return result;
    }

    public override void SyncEntity(Task entity, TaskViewModel viewModel)
    {
        entity.Description = viewModel.Description;
        entity.Done = viewModel.Done;
        entity.LastChangeAt = DateTime.UtcNow;
    }

    public override TaskViewModel ConvertToViewModel(Task entity)
    {
        return new TaskViewModel
        {
            Id = entity.Id,
            Description = entity.Description,
            Done = entity.Done,
            LastChangeAt = entity.LastChangeAt
        };
    }
}
```

# License

The MIT License (MIT)

Copyright (c) 2015 Joona Romppanen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

