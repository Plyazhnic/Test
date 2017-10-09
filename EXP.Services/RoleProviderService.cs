using System;
using System.Web.Security;
using System.Collections.Specialized;
using EXP.Core;
using EXP.DataAccess;
using EXP.Entity;
using EXP.Entity.Enumerations;

namespace EXP.Service
{
    public class RoleProviderService : RoleProvider
    {
        private string _ApplicationName;
        

        #region Methods
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Role Provider");
            }

            base.Initialize(name, config);
            _ApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }

        public override string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            new UserProfileRepository().AddUsersToRoles(usernames, roleNames);
        }

        public override void CreateRole(string roleName)
        {
            if (!RoleExists(roleName))
                new UserProfileRepository().CreateRole(roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            UserProfile profile = new UserProfileRepository().GetUser(username, 0);
            if (profile.UserProfileTypeID == (int)UserProfileTypeEnum.Administrator)
            {
                return new string[] { UserProfileTypeEnum.Administrator.ToString() };
            }
            else
            {
                return new string[0];
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return new UserProfileRepository().GetUsersByRoleName(roleName); 
        }

        public override bool RoleExists(string roleName)
        {
            return new UserProfileRepository().GetRole(roleName) != null;
        }
        #endregion

        #region Not implemented
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}