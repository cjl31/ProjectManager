using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager
{
    internal interface IUserDataService
    {
        User Authenticate(string username, string password);
        void RegisterUser(User user);
        string GetRoleByUsername(string username);
        bool HaveDuplicateUsername(string username);
    }
}
