using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntityGenerics.Examples.Data;
using EntityGenerics.Examples.Data.Repositories;
using Microsoft.Data.Entity;

namespace EntityGenerics.Examples
{
    public class Program
    {
        private static TaskRepository _repository;
        private static bool _running = true;

        public static void Main(string[] args)
        {
            _repository = new TaskRepository(CreateContext(), true);

            ClearAndPrint("Screens/Menu.txt");

            while (_running)
            {
                var input = Console.ReadKey();

                try
                {
                    if (char.IsDigit(input.KeyChar))
                    {
                        int taskId;
                        switch ((int)char.GetNumericValue(input.KeyChar))
                        {
                            case 1:
                                Clear();
                                Console.WriteLine("Current open tasks:");
                                if (!_repository.Entities.Any())
                                {
                                    Console.WriteLine("No open tasks...");
                                }

                                foreach (var task in _repository.ExecuteQuery(new ExampleRepositoryQuery { HideCompleted = true }))
                                {
                                    Console.WriteLine($"{task.Id} - {task.Description}");
                                }

                                Console.Write("\nPress any key to return...");
                                Console.ReadKey();
                                break;
                            case 2:
                                Clear();
                                var newTask = new Data.Entities.Task();
                                Console.WriteLine("Input task description:");
                                Console.Write("> ");
                                newTask.Description = Console.ReadLine();
                                _repository.Add(newTask);
                                _repository.SaveChanges();
                                break;
                            case 3:
                                Clear();
                                Console.Write("Task ID to mark as done > ");
                                taskId = (int)char.GetNumericValue(Console.ReadKey().KeyChar);
                                var taskToMark = _repository.Find(taskId);
                                taskToMark.Done = true;
                                taskToMark.LastChangeAt = DateTime.UtcNow;
                                _repository.SaveChanges();
                                break;
                            case 4:
                                Clear();
                                Console.Write("Task ID to delete > ");
                                taskId = (int)char.GetNumericValue(Console.ReadKey().KeyChar);
                                _repository.Remove(taskId);
                                _repository.SaveChanges();
                                break;
                            case 0:
                                _running = false;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(input.KeyChar));
                        }
                    }
                    else
                    {
                        throw new InvalidCastException(nameof(input.KeyChar));
                    }
                }
                catch (Exception exception)
                {
                    if (exception is ArgumentOutOfRangeException) Console.WriteLine("Invalid option selected.");
                    else if (exception is InvalidCastException) Console.WriteLine("Invalid character given.");
                    else if (exception is KeyNotFoundException) Console.WriteLine("No such task was found.");
                    else throw;
                }

                ClearAndPrint("Screens/Menu.txt");
            }
        }

        private static void Clear()
        {
#if DNX451
            Console.Clear();
            Console.SetCursorPosition(0, 0);
#else
            Console.Write("\n\n");
#endif
        }

        private static void ClearAndPrint(string screenFilePath, bool drawCursor = true)
        {
            Clear();
            using (var screenFile = new FileStream(screenFilePath, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(screenFile))
            {
                Console.Write(streamReader.ReadToEnd());
                if (drawCursor) Console.Write("\n> ");
            }
        }

        private static ExampleDbContext CreateContext()
        {
            var dbOptions = new DbContextOptionsBuilder();
            dbOptions.UseInMemoryDatabase();
            return new ExampleDbContext(dbOptions.Options);
        }
    }
}
