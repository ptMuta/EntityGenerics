using EntityGenerics.Examples.Data;
using EntityGenerics.Examples.Data.Repositories;

namespace EntityGenerics.UnitTests.Shared
{
    public class TaskRepositoryFixture
    {
        public TaskRepository Repository { get; set; }
        public EntityFactory EntityFactory { get; set; }

        public TaskRepositoryFixture()
        {
            Repository = ContextHelper.CreateRepository();
            EntityFactory = new EntityFactory((ExampleDbContext)Repository.Context);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}