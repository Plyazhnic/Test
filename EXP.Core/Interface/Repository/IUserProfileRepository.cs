using System;
using System.Collections.Generic;
using EXP.Entity;
namespace EXP.Core.Interface
{
    public interface IUserProfileRepository
    {
        #region Users
        /// <summary>
        /// Activate user by email and password salt
        /// </summary>
        /// <param name="email"></param>
        /// <param name="salt"></param>
        /// <returns>Success or not</returns>
        bool ActivateUser(string email, string salt);

        /// <summary>
        /// Set Profile type
        /// </summary>
        /// <param name="userName">Login name</param>
        /// <param name="profileTypeId">Type ID</param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>Success or not</returns>
        bool UpdateProfile(UserProfile profile);

        bool ChangePassword(string userName, string newPassword, string newSalt);

        /// <summary>
        /// Returns user by name
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string GetUserNameByEmail(string email);
        /// <summary>
        /// Returns user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        UserProfile  GetUser(string userName, int status);

        UserProfile GetUserById(int userId);

        int GetUsersCount(string filter);
        List<UserProfile> ListUsers(TableList listParam);

        /// <summary>
        /// Get user by UserProfileType
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<UserProfile> GetUsersByType(int typeId);

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        bool CreateUser(string userName, string hashedPassword, string passwordSalt, string email, bool isApproved);

        /// <summary>
        /// update address for user profile
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="addressId"></param>
        void UpdatePersonalAddress(int userID, int addressId);

        /// <summary>
        /// Set status for user
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="statusId"></param>
        void SetStatus(int userProfileId, int statusId);
        #endregion

        #region Roles
        /// <summary>
        /// Adds users to specific roles
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        void AddUsersToRoles(string[] users, string[] roles);
        /// <summary>
        /// Creates role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool CreateRole(string roleName);
        /// <summary>
        /// Gets role by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        SecurityRole  GetRole(string roleName);
        /// <summary>
        /// Gets users by roleID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        string[] GetUsersByRoleName(string roleID);
        /// <summary>
        /// Gets roles by userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        string[] GetRolesByUserName(string userID);
        /// <summary>
        /// Checks if the role exists
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool RoleExists(string role);

        #endregion

        //List<VehicleMake> ListMakes();
        //List<VehicleModel> ListModels(int makeId);
        //VehicleMake GetMake(string makeName);
        //VehicleModel GetModel(string modelName);
        //List<Vehicle> ListVehicles(int userProfileID);
        ///// <summary>
        ///// Get two vehicles for user
        ///// </summary>
        ///// <param name="userProfileID"></param>
        ///// <returns></returns>
        //List<Vehicle> GetVehicles(int userProfileID);
        //Vehicle GetVehicle(int userProfileID, int vehicleId);
        ///// <summary>
        ///// Search SessioID for user
        ///// </summary>
        ///// <param name="sessionId"></param>
        ///// <returns></returns>
        bool SearchSessionId(string sessionId);
        //int CreateVehicle(Vehicle vehicle);
        //void UpdateVehicle(Vehicle vehicle);
        //void RemoveVehicle(int vehicleId);
    }
}
