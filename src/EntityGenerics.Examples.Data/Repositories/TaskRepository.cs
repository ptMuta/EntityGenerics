using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityGenerics.Core;
using EntityGenerics.Core.Abstractions;
using EntityGenerics.Examples.Data.ViewModels;
using Task = EntityGenerics.Examples.Data.Entities.Task;

namespace EntityGenerics.Examples.Data.Repositories
{
    public class TaskRepository : RepositoryBase<int, Task, TaskViewModel, ExampleRepositoryQuery>
    {
        public TaskRepository(IDbContext context) : base(context)
        {
        }

        public TaskRepository(IDbContext context, bool ownsContext) : base(context, ownsContext)
        {
        }

        public override Task<IQueryable<Task>> ExecuteQueryAsync(ExampleRepositoryQuery query, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = Entities.Where(t => t.Description.ToLowerInvariant().Contains(query.Query.ToLowerInvariant()))
                .Skip(query.Page * query.Limit)
                .Take(query.Limit);

            if (query.HideCompleted)
                result = result.Where(t => !t.Done);

            return System.Threading.Tasks.Task.FromResult(result);
        }

        public override IQueryable<Task> ExecuteQuery(ExampleRepositoryQuery query)
        {
            return ExecuteQueryAsync(query).Result;
        }

        public override System.Threading.Tasks.Task SyncEntityAsync(Task entity, TaskViewModel viewModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            entity.Description = viewModel.Description;
            entity.Done = viewModel.Done;
            entity.LastChangeAt = DateTime.UtcNow;
            return System.Threading.Tasks.Task.FromResult(0);
        }

        public override void SyncEntity(Task entity, TaskViewModel viewModel)
        {
            SyncEntityAsync(entity, viewModel).RunSynchronously();
        }

        public override Task<TaskViewModel> ConvertToViewModelAsync(Task entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return System.Threading.Tasks.Task.FromResult(new TaskViewModel
            {
                Id = entity.Id,
                Description = entity.Description,
                Done = entity.Done,
                LastChangeAt = entity.LastChangeAt
            });
        }

        public override TaskViewModel ConvertToViewModel(Task entity)
        {
            return ConvertToViewModelAsync(entity).Result;
        }
    }
}