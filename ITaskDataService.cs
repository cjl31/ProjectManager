using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    internal interface ITaskDataService
    {
        void CreateTask(WorkTask task);
        List<WorkTask> GetAllTasks();
        List<WorkTask> GetTasksForUser(string username);
        void UpdateTaskStatus(int taskId, string status, string username);
        bool CheckAssignedToUser(int taskId, string username);
        bool HaveDuplicateProjectId(string Id);

    }
}
