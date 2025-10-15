using System;
using System.Collections.Generic;
using solelel.TaskPlanner.Domain.Models;    

namespace solelel.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] items)
        {
            var itemsAsList = items.ToList();
            itemsAsList.Sort(CompareWorkItems);
            return itemsAsList.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            // Спочатку порівнюємо Priority за спаданням (вищий пріоритет - раніше)
            int priorityComparison = secondItem.Priority.CompareTo(firstItem.Priority);
            if (priorityComparison != 0)
                return priorityComparison;

            // Якщо пріоритети рівні, порівнюємо DueDate за зростанням (раніше - раніше)
            int dueDateComparison = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dueDateComparison != 0)
                return dueDateComparison;

            // Якщо дати рівні, порівнюємо Title в алфавітному порядку (зростання)
            return firstItem.Title.CompareTo(secondItem.Title);
        }
    }
}