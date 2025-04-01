using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ProjectManager
{

    internal class TaskDataService : ITaskDataService
    {
        private readonly string _filePath = "tasks.json";
        private List<WorkTask> _tasks;
        private readonly ILogger _logger;

        public TaskDataService(ILogger logger)
        {
            _logger = logger;
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _tasks = JsonConvert.DeserializeObject<List<WorkTask>>(json) ?? new List<WorkTask>();
            }
            else
            {
                _tasks = new List<WorkTask>();
            }
        }

        public void CreateTask(WorkTask task)
        {
            _tasks.Add(task);
            SaveTasks();
        }
        public List<WorkTask> GetAllTasks()
        {
            return _tasks;
        }
        public List<WorkTask> GetTasksForUser(string username)
        {
            return _tasks.FindAll(t => t.AssignedTo == username);
        }

        public void UpdateTaskStatus(int taskId, string status, string username)
        {
            var task = _tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.Status = status;
                _logger.Log($"{DateTime.Now} - {username} изменил статус задачи {task.Title} на {status}");
                SaveTasks();
            }
        }

        private void SaveTasks()
        {
            var json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
        public bool CheckAssignedToUser(int taskId, string username)
        {
            if(_tasks.Any(task => task.AssignedTo == username && task.Id == taskId))
                return true;
            return false;
        }
        public bool HaveDuplicateProjectId(string Id)
        {
            if (_tasks.Any(task => task.ProjectId == Id))
                return true;
            else return false;
        }

    }
}
