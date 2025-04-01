using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    internal class UserDataService : IUserDataService
    {
        private readonly string _filePath = "users.json";
        private List<User> _users;

        public UserDataService()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (File.Exists(_filePath) && new FileInfo(_filePath).Length != 0)
            {
                var json = File.ReadAllText(_filePath);
                _users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            else
            {
                _users = new List<User>
                {
                    new User { Username = "admin", Password = "admin", Role = "Manager" },
                    new User { Username = "asd", Password = "asd", Role = "Employee" }
                };

            }
        }

        public User Authenticate(string username, string password)
        {
            return _users.Find(user => user.Username == username && user.Password == password);
        }

        public void RegisterUser(User user)
        {
            _users.Add(user);
            SaveUsers();
        }

        public string GetRoleByUsername(string username)
        {
            var _user = _users.Find(user => user.Username == username);
            if (_user != null)
            {
                return _user.Role;
            }
            else
            {
                return "";
            }
        }

        private void SaveUsers()
        {
            var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public bool HaveDuplicateUsername(string username)
        {
            if (_users.Any(user => user.Username == username))
            {
                return true;
            }
            return false;
        }
    }
}
