using System;
using solelel.TaskPlanner.Domain.Models;
using solelel.TaskPlanner.Domain.Models.Enums;
using solelel.TaskPlanner.Domain.Logic;
using solelel.TaskPlanner.DataAccess;
using solelel.TaskPlanner.DataAccess.Abstractions;

namespace solelel.TaskPlanner
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            IWorkItemsRepository repo = new FileWorkItemsRepository();
            var planner = new SimpleTaskPlanner(repo);  // Для завдання 4

            while (true)
            {
                Console.WriteLine("\n[A]dd work item; [B]uild a plan; [M]ark as completed; [R]emove; [Q]uit");
                var choice = Console.ReadLine()?.ToUpper();

                if (choice == "Q") break;

                if (choice == "A")
                {
                    var item = new WorkItem { CreationDate = DateTime.Now, IsCompleted = false };
                    Console.Write("Title: "); item.Title = Console.ReadLine();
                    Console.Write("Description: "); item.Description = Console.ReadLine();
                    Console.Write("DueDate (dd.MM.yyyy): "); item.DueDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Priority: "); item.Priority = Enum.Parse<Priority>(Console.ReadLine(), true);
                    Console.Write("Complexity: "); item.Complexity = Enum.Parse<Complexity>(Console.ReadLine(), true);
                    repo.Add(item);
                    repo.SaveChanges();
                    Console.WriteLine("Додано!");
                }
                else if (choice == "B")
                {
                    var plan = planner.CreatePlan();
                    Console.WriteLine("\nПлан:");
                    foreach (var item in plan) Console.WriteLine(item.ToString());
                }
                else if (choice == "M")
                {
                    Console.Write("ID задачі: "); Guid id = Guid.Parse(Console.ReadLine());
                    var item = repo.Get(id);
                    if (item != null)
                    {
                        item.IsCompleted = true;
                        repo.Update(item);
                        repo.SaveChanges();
                        Console.WriteLine("Позначено як завершену!");
                    }
                }
                else if (choice == "R")
                {
                    Console.Write("ID задачі: "); Guid id = Guid.Parse(Console.ReadLine());
                    if (repo.Remove(id))
                    {
                        repo.SaveChanges();
                        Console.WriteLine("Видалено!");
                    }
                }
            }
        }
    }
}