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
}