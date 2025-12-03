using Newtonsoft.Json;
using solelel.TaskPlanner.Domain.Models;
using solelel.TaskPlanner.DataAccess.Abstractions;

namespace solelel.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FileName = "work-items.json";
        private readonly Dictionary<Guid, WorkItem> _workItems;

        public FileWorkItemsRepository()
        {
            _workItems = new Dictionary<Guid, WorkItem>();

            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                if (!string.IsNullOrEmpty(json))
                {
                    var items = JsonConvert.DeserializeObject<WorkItem[]>(json);
                    foreach (var item in items)
                    {
                        _workItems[item.Id] = item;
                    }
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            var copy = workItem.Clone();
            copy.Id = Guid.NewGuid();
            _workItems.Add(copy.Id, copy);
            return copy.Id;
        }

        public WorkItem Get(Guid id)
        {
            _workItems.TryGetValue(id, out var item);
            return item;
        }

        public WorkItem[] GetAll()
        {
            return _workItems.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (_workItems.ContainsKey(workItem.Id))
            {
                _workItems[workItem.Id] = workItem.Clone();
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            return _workItems.Remove(id);
        }

        public void SaveChanges()
        {
            var items = _workItems.Values.ToArray();
            var json = JsonConvert.SerializeObject(items);
            File.WriteAllText(FileName, json);
        }
    }
}