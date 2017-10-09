using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using EXP.Core.Util;
using AutoMapper;

namespace EXP.DataAccess
{
    public class PhoneRepository : BaseRepository, IPhoneRepository  
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        /// Create phone
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="userProfileId"></param>
        public int CreatePhone(Phone phone)
        {
            try
            {
                Logger.DebugFormat("CreatePhone. phone: {0}", phone.ToString());
                int result;
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreatePhone]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "PhoneTypeID", SqlDbType.TinyInt, phone.PhoneTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "AreaCode", SqlDbType.VarChar, 8, phone.AreaCode);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneNumber", SqlDbType.VarChar, 15, phone.PhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneDescription", SqlDbType.VarChar, 128, phone.PhoneDescription);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewPhoneID", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    result = (int)param.Value;
                }
                return result;

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreatePhone failed.", exc);
            }
        }

        /// <summary>
        /// Create phone for user 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="userProfileId"></param>
        public int CreatePhoneForUser(Phone phone,int userProfileId)
        {
            try
            {
                Logger.DebugFormat("CreatePhoneForUser. phone: {0}, user: {1}", phone.ToString(), userProfileId);
                int result;                              

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreatePhoneForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userProfileId);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneTypeID", SqlDbType.TinyInt, phone.PhoneTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "AreaCode", SqlDbType.VarChar, 8, phone.AreaCode);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneNumber", SqlDbType.VarChar, 15, phone.PhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneDescription", SqlDbType.VarChar, 128, phone.PhoneDescription);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewPhoneID", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    result = (int)param.Value;
                }
                return result;

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreatePhoneForUser failed.", exc);
            }
        }

        /// <summary>
        /// Update phone 
        /// </summary>
        /// <param name="phone"></param>
        public void UpdatePhone(Phone phone)
        {
            try
            {
                Logger.DebugFormat("UpdatePhone. phone: {0}", phone.ToString());              
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdatePhone]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "PhoneID", SqlDbType.Int, phone.PhoneID);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneTypeID", SqlDbType.TinyInt, phone.PhoneTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "AreaCode", SqlDbType.VarChar, 8, phone.AreaCode);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneNumber", SqlDbType.VarChar, 15, phone.PhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneDescription", SqlDbType.VarChar, 128, phone.PhoneDescription);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdatePhone failed.", exc);
            }
        }

        /// <summary>
        /// Remove user phone
        /// </summary>
        /// <param name="phoneID"></param>
        /// <param name="profileID"></param>
        public void RemovePhoneForUser(int phoneID, int profileID)
        {
            try
            {
                Logger.DebugFormat("RemovePhone. phoneID: {0}, profileID: {1}", phoneID, profileID);               
                                                 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_RemovePhoneForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "PhoneID", SqlDbType.Int, phoneID);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, profileID);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("RemovePhone failed.", exc);
            }
        }
        /// <summary>
        /// get phone data by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        public Phone GetPhone(int phoneId)
        {
            Phone phone = new Phone();                                  
            try
            {
                Logger.DebugFormat("GetPhone. phoneId: {0}", phoneId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetPhone]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "PhoneID", SqlDbType.Int, phoneId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            phone = Phone(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetPhone failed.", exc);
            }
            return phone;
        }

        /// <summary>
        /// get phones for current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Phone> GetPhonesForUser(int userId)
        {
            List<Phone> list = new List<Phone>();                       
            try
            {
                Logger.DebugFormat("GetPhonesForUser. userId: {0}", userId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetPhonesForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phone phone = Phone(reader);
                            list.Add(phone);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetPhonesForUser failed.", exc);
            }
            return list;
        }
        /// <summary>
        /// Get all phones for current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Phone> GetAllPhonesForUser(int userId)
        {
            List<Phone> list = new List<Phone>();                                              
            try
            {
                Logger.DebugFormat("GetAllPhonesForUser. userId: {0}", userId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetAllPhonesForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phone phone = Phone(reader);
                            list.Add(phone);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetAllPhonesForUser failed.", exc);
            }
            return list;
        }
        //TODO: Delete
        /// <summary>
        /// get phones for current user by type
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Phone GetPhoneForUserByType(int userId, int phoneTypeId)
        {
            Phone phone = new Phone();
            try
            {
                Logger.DebugFormat("GetPhoneForUserByType. userId: {0}, phoneTypeId{1}", userId, phoneTypeId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetPhoneForUserByType]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneTypeID", SqlDbType.Int, phoneTypeId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            phone = Phone(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetPhoneForUserByType failed.", exc);
            }
            return phone;
        }

        /// <summary>
        /// list phones for current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Phone> ListPhonesForUser(int userId)
        {
            List<Phone> list = new List<Phone>();
            try
            {
                Logger.DebugFormat("ListPhonesForUser. userId: {0}", userId);            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListPhonesForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phone phone = Phone(reader);
                            list.Add(phone);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListPhonesForUser failed.", exc);
            }
            return list;
        }

        private Phone Phone(IDataReader reader)
        {
            Phone phone = new Phone();
            try
            {
                phone = Mapper.DynamicMap<IDataReader, Phone>(reader);
            
                Mapper.CreateMap<IDataReader, PhoneType>()
                    .ForMember(m => m.PhoneType1, opt => opt.MapFrom(r => r["PhoneType"]));
                PhoneType phoneType = Mapper.DynamicMap<IDataReader, PhoneType>(reader);
            
                phone.PhoneType = phoneType;
            }
            catch (Exception exc)
            {
            }

            return phone;
        }
    }
}
