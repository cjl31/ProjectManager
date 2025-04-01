using System;
using System.Collections.Generic;
using System.IO;


namespace ProjectManager
{
    internal class Program
    {
        private static IUserInterface _userInterface;
        private static IUserDataService _userService;
        private static ITaskDataService _taskService;
        private static ILogger _logger;

        static void Main()
        {
            _logger = new TxtFileLogger();
            _userService = new UserDataService();
            _taskService = new TaskDataService(_logger);
            _userInterface = new ConsoleUserInterface(_userService, _taskService);

            _userInterface.ShowMainMenu();
        }
    }
}