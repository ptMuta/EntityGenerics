using System.Linq;
using EntityGenerics.Examples.Data;
using EntityGenerics.Examples.Data.Entities;
using EntityGenerics.Examples.Data.Repositories;
using EntityGenerics.UnitTests.Shared;
using Microsoft.Data.Entity.ChangeTracking;
using Xunit;

namespace EntityGenerics.Core.UnitTests
{
    public class RepositoryBase : IClassFixture<TaskRepositoryFixture>
    {
        public TaskRepository Repository { get; set; }
        public EntityFactory EntityFactory { get; set; }

        public RepositoryBase(TaskRepositoryFixture repositoryFixture)
        {
            Repository = repositoryFixture.Repository;
            EntityFactory = repositoryFixture.EntityFactory;
        }

        [Fact]
        public void Repository_Should_Find_With_A_Query()
        {
            CreateTaskIfNotAny();

            var tasks = Repository.FindAll(new ExampleRepositoryQuery
            {
                Query = "Tas",
                Limit = 10,
                Page = 0
            });
            Assert.NotNull(tasks);
            Assert.NotEmpty(tasks);
        }

        [Fact]
        public void Repository_Should_Find_By_Id()
        {
            CreateTaskIfNotAny();

            var task = Repository.Find(1);
            Assert.NotNull(task);
        }

        [Fact]
        public void Repository_Should_Be_Able_To_Convert_To_View_Model()
        {
            var task = EntityFactory.CreateTask();
            var viewModel = Repository.ConvertToViewModel(task);
            Assert.Equal($"Task {task.Id}", viewModel.Description);
        }

        [Fact]
        public void Repository_Should_Add_To_Context()
        {
            CreateTaskIfNotAny();

            Assert.NotEmpty(Repository.Entities);

            var createdTask = Repository.Find(1);
            Assert.NotNull(createdTask);
        }

        [Fact]
        public void Repository_Should_Update_To_Context()
        {
            CreateTaskIfNotAny();

            var taskToUpdate = Repository.Find(1);
            Assert.NotNull(taskToUpdate);
            taskToUpdate.Description = "Updated Task 1";
            Repository.SaveChanges();

            var updatedTask = Repository.Find(1);
            Assert.NotNull(updatedTask);
            Assert.Equal("Updated Task 1", updatedTask.Description);
        }

        [Fact]
        public void Repository_Should_Delete_From_Context()
        {
            CreateTaskIfNotAny();

            var taskToDelete = Repository.Find(1);
            var removedTask = Repository.Remove(taskToDelete.Id);
            Repository.SaveChanges();

            Assert.False(Repository.Exists(1), "Repository.Exists(1) should return false after deletion of Task with Id 1");
            Assert.NotNull(removedTask);
        }

        private void CreateTaskIfNotAny()
        {
            if (Repository.Entities.Any()) return;
            var task = EntityFactory.CreateTask();
            Repository.Add(task);
            Repository.SaveChanges();
        }
    }
}