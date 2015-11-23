using EntityGenerics.Core.Abstractions;
using EntityGenerics.Examples.Data;
using EntityGenerics.Examples.Data.Repositories;
using Microsoft.Data.Entity;

namespace EntityGenerics.UnitTests.Shared
{
    public static class ContextHelper
    {
        public static ExampleDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExampleDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            return new ExampleDbContext(optionsBuilder.Options);
        }

        public static TaskRepository CreateRepository()
        {
            var context = CreateDbContext();
            return new TaskRepository(context, true);
        }
    }
}