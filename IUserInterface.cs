using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    internal interface IUserInterface
    {
        void ShowMainMenu();
        void AuthenticateUser();
        void RegisterUser();
        void CreateTask();
        void ViewAllTasks();
        void ManagerMenu(string username);
        void EmployeeMenu(string username);
        void UpdateTaskStatus(string username);
    }
}
