using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Core;
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Core.BaseClasses;
using EXP.Core.Util;
using EXP.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using AutoMapper;
using EXP.Entity.Enumerations;

namespace EXP.DataAccess
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        #region Users

        /// <summary>
        /// Activate user by email and password salt
        /// </summary>
        /// <param name="email"></param>
        /// <param name="salt"></param>
        /// <returns>Success or not</returns>
        public bool ActivateUser(string email, string salt)
        {
            try
            {
                int rowsAffected;
                Logger.DebugFormat("ActivateUser. email: {0}, salt: {1}", email, salt);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ActivateUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Email", SqlDbType.VarChar, 255, email);
                    DatabaseUtils.AddInputParameter(cmd, "Salt", SqlDbType.VarChar, 1024, salt);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();    
                }
                return rowsAffected == 1;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ActivateUser failed.", exc);
            }
        }

        /// <summary>
        /// Set Profile type
        /// </summary>
        /// <param name="userName">Login name</param>
        /// <param name="profileTypeId">Type ID</param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>Success or not</returns>
        public bool UpdateProfile(UserProfile profile)
        {
            try
            {
                int rowsAffected = 0;
                Logger.DebugFormat("UpdateProfile. Profile: {0}", profile.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdateProfile]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserName", SqlDbType.NChar, 10, profile.LoginName);
                    DatabaseUtils.AddInputParameter(cmd, "ProfileTypeId", SqlDbType.Int, profile.UserProfileTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "Email", SqlDbType.VarChar, 256, profile.EmailAddress);
                    DatabaseUtils.AddInputParameter(cmd, "FirstName", SqlDbType.VarChar, 256, profile.FirstName);
                    DatabaseUtils.AddInputParameter(cmd, "LastName", SqlDbType.VarChar, 256, profile.LastName);
                    DatabaseUtils.AddInputParameter(cmd, "StatusID", SqlDbType.Int, profile.UserProfileStatusID);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, profile.AddressID);   
                    DatabaseUtils.AddInputParameter(cmd, "Comments", SqlDbType.VarChar, 1024, profile.Comments);
                    DatabaseUtils.AddInputParameter(cmd, "tPrefix", SqlDbType.VarChar, 4, profile.TitlePreffix);
                    DatabaseUtils.AddInputParameter(cmd, "SessionID", SqlDbType.VarChar, 32, profile.SessionID);
                    DatabaseUtils.AddInputParameter(cmd, "TenantID", SqlDbType.Int, profile.TenantID);
                    DatabaseUtils.AddInputParameter(cmd, "isSupervisor", SqlDbType.Bit, profile.isSupervisor);
                    DatabaseUtils.AddInputParameter(cmd, "HireDate", SqlDbType.Date, profile.HireDate);
                    DatabaseUtils.AddInputParameter(cmd, "OperatorID", SqlDbType.Int, profile.OperatorID);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();   
                }
                return rowsAffected > 0;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateProfile failed.", exc);
            }
        }

        /// <summary>
        /// Change password for user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <param name="newSalt"></param>
        /// <returns>Success or not</returns>
        public bool ChangePassword(string userName, string newPassword, string newSalt)
        {
            try
            {
                Logger.DebugFormat("ChangePassword. userName: {0}, newPassword: {1}, newSalt: {2}",
                    userName, newPassword, newSalt);                              
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdatePassword]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserName", SqlDbType.NChar, 10, userName);
                    DatabaseUtils.AddInputParameter(cmd, "Password", SqlDbType.VarChar, 1024, newPassword);
                    DatabaseUtils.AddInputParameter(cmd, "Salt", SqlDbType.VarChar, 1024, newSalt);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected == 1;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ChangePassword failed.", exc);
            }
        }

        /// <summary>
        /// Returns user by name
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetUserNameByEmail(string email)
        {
            string userName;                
            try
            {
                Logger.DebugFormat("GetUserNameByEmail. email: {0}", email);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetUserByEmail]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Email", SqlDbType.VarChar, 256, email);
                    SqlParameter userNameParam = DatabaseUtils.AddOutputParameter(cmd, "UserName", SqlDbType.VarChar, 256);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    userName = userNameParam.Value != null ? userNameParam.Value.ToString() : String.Empty;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetUserNameByEmail failed.", exc);
            }
            return userName;
        }
        /// <summary>
        /// Returns user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public UserProfile GetUser(string userName, int status)
        {           
            UserProfile user = null;
            try
            {
                Logger.DebugFormat("GetUser. userName: {0}, status: {1}", userName, status);                          
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand com = new SqlCommand("[UserProfile].[usp_GetUser]", conn);
                    com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.Add(new SqlParameter("@UserName", userName));
                    com.Parameters.Add(new SqlParameter("@Status", status));                    
                
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    SqlDataReader reader = com.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
                    }
                    /*
                    SqlCommand com = new SqlCommand("SELECT userName, ProfilePassword FROM UserProfile WHERE userName = '" + userName +"'", conn);
                    
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(com);
                    SqlDataReader reader = com.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
                    }    */                
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetUser failed.", exc);
            }
            return user;
        }

        public UserProfile GetUserById(int userId)
        {
            UserProfile user = null;
            try
            {
                Logger.DebugFormat("GetUserById. userId: {0}", userId);  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetUserById]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserId", SqlDbType.Int, userId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
                    }                   
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetUserById failed.", exc);
            }
            return user;
        }

        public int GetUsersCount(string filter)
        {
            object value;
            try
            {
                Logger.DebugFormat("GetUsersCount. filter: {0}", filter);  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetUsersCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if ((filter ?? "") == "")
                    {
                        DatabaseUtils.AddInputParameter(cmd, "Filter", SqlDbType.NVarChar, DBNull.Value);
                    }
                    else
                    {
                        DatabaseUtils.AddInputParameter(cmd, "Filter", SqlDbType.NVarChar, filter);
                    }
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetUsersCount failed.", exc);
            }
            return (int)value;
        }

        public List<UserProfile> ListUsers(TableList listParam)
        {
            List<UserProfile> list = new List<UserProfile>();
            try
            {
                Logger.DebugFormat("ListUsers. listParam: {0}", listParam.ToString());  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListUsers]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    if ((listParam.SearchString ?? "") == "")
                    {
                        DatabaseUtils.AddInputParameter(cmd, "Filter", SqlDbType.NVarChar, DBNull.Value);
                    }
                    else
                    {
                        DatabaseUtils.AddInputParameter(cmd, "Filter", SqlDbType.NVarChar, listParam.SearchString);
                    }
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserProfile user = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
                        list.Add(user);
                    }      
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListUsers failed.", exc);
            }

            return list;
        }

        /// <summary>
        /// Get user by UserProfileType
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<UserProfile> GetUsersByType(int typeId)
        {
            List<UserProfile> list = new List<UserProfile>();
            try
            {
                Logger.DebugFormat("GetUsersByType. typeId: {0}", typeId);  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetUsersByType]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileTypeID", SqlDbType.Int, typeId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserProfile user = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
                        list.Add(user);
                    }                  
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetUsersByType failed.", exc);
            }
            return list;
        }
        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public bool CreateUser(string userName, string hashedPassword, string passwordSalt, string email, bool isApproved)
        {
            try
            {
                Logger.DebugFormat("CreateUser. userName : {0}, hashedPassword: {1}, passwordSalt: {2}, email: {3}, isApproved: {4}",
                    userName, hashedPassword, passwordSalt, email, isApproved);
                int result;
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreateUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileTypeID", SqlDbType.Int, 2/*TODO: Fix value*/);
                    int status = (int)(isApproved ? UserProfileStatusEnum.Active : UserProfileStatusEnum.Pending);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileStatusID", SqlDbType.TinyInt, status);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileEncryptionTypeID", SqlDbType.Int, 2/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "LoginName", SqlDbType.Char, 10, userName);
                    DatabaseUtils.AddInputParameter(cmd, "ProfilePassword", SqlDbType.VarChar, 255, hashedPassword);
                    DatabaseUtils.AddInputParameter(cmd, "ProfilePasswordSalt", SqlDbType.VarChar, 255, passwordSalt);
                    DatabaseUtils.AddInputParameter(cmd, "EmailAddress", SqlDbType.VarChar, 255, email);
                    DatabaseUtils.AddInputParameter(cmd, "TitlePreffix", SqlDbType.Char, string.Empty/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "TitleSuffix", SqlDbType.Char, string.Empty/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "FirstName", SqlDbType.VarChar, 255, string.Empty/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "LastName", SqlDbType.VarChar, 255, string.Empty/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "Comments", SqlDbType.VarChar, 255, string.Empty/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "AgreementVersion", SqlDbType.VarChar, 255, string.Empty/*TODO: Fix value*/);
                    DatabaseUtils.AddInputParameter(cmd, "AgreementDate", SqlDbType.DateTime, DateTime.Now/*TODO: Fix value*/);
                    
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                }
                return (result >= 0);

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateUser failed.", exc);
            }
        }

        /// <summary>
        /// update address for user profile
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="addressId"></param>
        public void UpdatePersonalAddress(int userID, int addressId)
        {
            try
            {
                Logger.DebugFormat("UpdatePersonalAddress. userID: {0}, addressId: {1}", userID, addressId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdatePersonalAddress]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userID);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, addressId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdatePersonalAddress failed.", exc);
            }
        }

        /// <summary>
        /// Set status for user
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="statusId"></param>
        public void SetStatus(int userProfileId, int statusId)
        {
            try
            {
                Logger.DebugFormat("SetStatus. userProfileId: {0}, statusId: {1}",userProfileId, statusId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_SetStatus]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileId", SqlDbType.Int, 10, userProfileId);
                    DatabaseUtils.AddInputParameter(cmd, "StatusID", SqlDbType.Int, statusId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("SetStatus failed.", exc);
            }
        }

        #endregion

        #region Roles
        /// <summary>
        /// Adds users to specific roles
        /// </summary>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        public void AddUsersToRoles(string[] users, string[] roles)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Creates role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets role by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public SecurityRole GetRole(string roleName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets users by roleID
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public string[] GetUsersByRoleName(string roleName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets roles by userID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string[] GetRolesByUserName(string userName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Checks if the role exists
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool RoleExists(string role)
        {
            throw new NotImplementedException();
        }
        #endregion             

        /// <summary>
        /// Search SessioID for user
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool SearchSessionId(string sessionId)
        {
            try
            {
                Logger.DebugFormat("SearchSessionId. sessionId: {0}", sessionId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_SearchSessionId]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "SessionID", SqlDbType.VarChar, 32, sessionId);
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("SearchSessionId failed.", exc);
            }
            return false;
        }
    }
}
