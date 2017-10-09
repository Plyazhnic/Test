using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Core.Interface
{
    public interface IRoleService
    {
        bool AdminExists();
        void AddUsersToRoles(string[] usernames, string[] rolenames);
        void RemoveUsersFromRoles(string[] usernames, string[] rolenames);
        void CreateRole(string roleName);
    }
}
