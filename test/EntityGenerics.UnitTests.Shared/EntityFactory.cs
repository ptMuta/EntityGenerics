using System.Linq;
using EntityGenerics.Examples.Data;
using EntityGenerics.Examples.Data.Entities;

namespace EntityGenerics.UnitTests.Shared
{
    public class EntityFactory
    {
        public ExampleDbContext Context { get; set; }

        public EntityFactory(ExampleDbContext context)
        {
            Context = context;
        }

        public Task CreateTask()
        {
            var nextId = 1;
            if (Context.Tasks.Any()) nextId = Context.Tasks.Last().Id + 1;

            return new Task
            {
                Id = nextId,
                Description = $"Task {nextId}",
                Done = false,
                LastChangeAt = null
            };
        }
    }
}