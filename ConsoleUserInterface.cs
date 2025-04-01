using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    internal class ConsoleUserInterface : IUserInterface
    {
        private readonly IUserDataService _userService;
        private readonly ITaskDataService _taskService;

        public ConsoleUserInterface(IUserDataService userService, ITaskDataService taskService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Система управления проектом.");
                Console.WriteLine("1. Войти");
                Console.WriteLine("2. Выход");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AuthenticateUser();
                        break;
                    case "2":
                        return;
                    default:
                        break;
                }
            }
        }

        public void AuthenticateUser()
        {
            Console.Clear();
            Console.WriteLine("Введите имя пользователя: ");
            var username = Console.ReadLine();
            Console.WriteLine("Введите пароль: ");
            var password = Console.ReadLine();

            var user = _userService.Authenticate(username, password);
            if (user != null)
            {
                Console.Clear();
                if (user.Role == "Manager")
                {
                    ManagerMenu(user.Username);
                }
                else
                {
                    EmployeeMenu(user.Username);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Неверное имя пользователя или пароль. Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
            }
        }

        public void RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("Введите имя пользователя: ");
            var username = Console.ReadLine(); 
            Console.WriteLine("Введите пароль: ");
            var password = Console.ReadLine();
            Console.WriteLine("Введите роль (Manager/Employee): ");
            var role = Console.ReadLine();

            if ((role == "Manager" || role == "Employee") && (!_userService.HaveDuplicateUsername(username)))
            {
                var newUser = new User { Username = username, Password = password, Role = role };
                _userService.RegisterUser(newUser);
                Console.WriteLine("Пользователь успешно зарегистрирован. Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Не удалось зарегистрировать пользователя. Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
            }
        }

        public void CreateTask()
        {
            Console.WriteLine("Введите ID проекта: ");
            var projectId = Console.ReadLine();
            Console.WriteLine("Введите название задачи: ");
            var title = Console.ReadLine();
            Console.WriteLine("Введите описание задачи: ");
            var description = Console.ReadLine();
            Console.WriteLine("Введите имя пользователя, которому назначить задачу: ");
            var assignedTo = Console.ReadLine();

            var task = new WorkTask
            {
                Id = _taskService.GetAllTasks().Count,
                ProjectId = projectId,
                Title = title,
                Description = description,
                AssignedTo = assignedTo,
                Status = "To do"
            };
            if (_userService.GetRoleByUsername(assignedTo) == "Employee" && ( ! _taskService.HaveDuplicateProjectId(task.ProjectId)) )
            {
                _taskService.CreateTask(task);
                Console.WriteLine("Задача успешно создана. Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Не удалось назначить задачу.\n" +
                    "Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
            }
        }

        public void ManagerMenu(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добро пожаловать, {username}!");
                Console.WriteLine("Меню управляющего:");
                Console.WriteLine("1. Создать задачу");
                Console.WriteLine("2. Посмотреть все задачи");
                Console.WriteLine("3. Зарегистрировать пользователя");
                Console.WriteLine("4. Выход");
                Console.WriteLine("Выберите действие: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateTask();
                        break;
                    case "2":
                        ViewAllTasks();
                        break;
                    case "3":
                        RegisterUser();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите любую клавишу чтобы вернуться");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void EmployeeMenu(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добро пожаловать, {username}!");
                Console.WriteLine("Меню сотрудника:");
                Console.WriteLine("1. Просмотреть задачи");
                Console.WriteLine("2. Выход");
                Console.WriteLine("Выберите действие: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewTasks(username);
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова. Нажмите любую клавишу чтобы вернуться");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void ViewAllTasks()
        {
            Console.Clear();
            var tasks = _taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Нет назначенных задач. Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
                return;
            }

            foreach (var task in tasks)
            {
                Console.WriteLine($"ID: {task.Id}, \nID проекта: {task.ProjectId}, \nНазвание: {task.Title}, \nНазначена для: {task.AssignedTo}, \nОписание: {task.Description}, \nСтатус: {task.Status}\n");
            }
            Console.WriteLine("Нажмите любую клавишу чтобы вернуться");
            Console.ReadKey();
        }

        public void ViewTasks(string username)
        {
            var tasks = _taskService.GetTasksForUser(username);
            if (tasks.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("У вас нет назначенных задач. Нажмите любую клавишу чтобы вернуться");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                foreach (var task in tasks)
                {
                    Console.WriteLine($"ID: {task.Id}, Название: {task.Title}, Описание: {task.Description}, Статус: {task.Status}");
                }
                Console.WriteLine("1. Изменить статус задачи");
                Console.WriteLine("2. Выход");
                Console.WriteLine("Выберите действие: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        UpdateTaskStatus(username);
                        break;
                    case "2":
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                        break;
                }
            }
        }

        public void UpdateTaskStatus(string username)
        {
            Console.WriteLine("Введите ID задачи, чтобы изменить статус: ");
            var taskIdInput = Console.ReadLine();
            if (int.TryParse(taskIdInput, out int taskId))
            {
                if (_taskService.CheckAssignedToUser(taskId, username))
                {
                    Console.WriteLine("Введите новый статус (To do, In Progress, Done): ");
                    var newStatus = Console.ReadLine();

                    if (newStatus == "To do" || newStatus == "In Progress" || newStatus == "Done")
                    {
                        _taskService.UpdateTaskStatus(taskId, newStatus, username);
                        Console.WriteLine("Статус задачи успешно обновлен. Нажмите любую клавишу чтобы вернуться");
                    }
                    else
                    {
                        Console.WriteLine("Неверный статус. Пожалуйста, введите один из следующих: To do, In Progress, Done.");
                    }
                }
                else
                {
                    Console.WriteLine("У вас нет доступа к этой задаче. Нажмите любую клавишу чтобы вернуться");
                }

                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Неверный ID задачи.");
            }
        }
    }
}

