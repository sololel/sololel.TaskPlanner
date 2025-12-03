using System;
using Xunit;
using Moq;
using solelel.TaskPlanner.Domain.Models;
using solelel.TaskPlanner.Domain.Models.Enums;
using solelel.TaskPlanner.DataAccess.Abstractions;
using solelel.TaskPlanner.Domain.Logic;

namespace solelel.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_SortsCorrectly()
        {
            var mockRepo = new Mock<IWorkItemsRepository>();
            var items = new[]
            {
                new WorkItem { Id = Guid.NewGuid(), Title = "B", DueDate = DateTime.Now.AddDays(2), Priority = Priority.Medium, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "A", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "C", DueDate = DateTime.Now.AddDays(1), Priority = Priority.High, IsCompleted = false }
            };
            mockRepo.Setup(r => r.GetAll()).Returns(items);

            var planner = new SimpleTaskPlanner(mockRepo.Object);
            var plan = planner.CreatePlan();

            Assert.Equal("A", plan[0].Title);  
            Assert.Equal("C", plan[1].Title);
            Assert.Equal("B", plan[2].Title);
        }

        [Fact]
        public void CreatePlan_IncludesOnlyUncompleted()
        {
            var mockRepo = new Mock<IWorkItemsRepository>();
            var items = new[]
            {
                new WorkItem { Id = Guid.NewGuid(), Title = "Uncompleted", IsCompleted = false },
                new WorkItem { Id = Guid.NewGuid(), Title = "Completed", IsCompleted = true }
            };
            mockRepo.Setup(r => r.GetAll()).Returns(items);

            var planner = new SimpleTaskPlanner(mockRepo.Object);
            var plan = planner.CreatePlan();

            Assert.Single(plan);
            Assert.Equal("Uncompleted", plan[0].Title);
        }
    }
}