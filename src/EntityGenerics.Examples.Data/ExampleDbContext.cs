using EntityGenerics.Core.Abstractions;
using EntityGenerics.Examples.Data.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace EntityGenerics.Examples.Data
{
    public class ExampleDbContext : DbContext, IDbContext
    {
        public DbSet<Task> Tasks { get; set; }

        protected ExampleDbContext()
        {
        }

        public ExampleDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}