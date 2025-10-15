using solelel.TaskPlanner.Domain.Logic;
using solelel.TaskPlanner.Domain.Models;
using solelel.TaskPlanner.Domain.Models.Enums;

namespace solelel.TaskPlanner
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var workItems = new List<WorkItem>();

            Console.WriteLine("Input data");

            while (true)
            {
                var item = new WorkItem
                {
                    CreationDate = DateTime.Now,  
                    IsCompleted = false  
                };

                Console.Write("Title: ");
                item.Title = Console.ReadLine();

                Console.Write("Description: ");
                item.Description = Console.ReadLine();

                Console.Write("DueDate (формат: dd.MM.yyyy): ");
                item.DueDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Priority (None, Low, Medium, High, Urgent): ");
                item.Priority = Enum.Parse<Priority>(Console.ReadLine(), true);  

                Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
                item.Complexity = Enum.Parse<Complexity>(Console.ReadLine(), true);

                workItems.Add(item);

                Console.Write("Add task? (y/n): ");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }

            
            var planner = new SimpleTaskPlanner();
            var sortedItems = planner.CreatePlan(workItems.ToArray());

           
            Console.WriteLine("\nSorted plan:");
            foreach (var item in sortedItems)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}