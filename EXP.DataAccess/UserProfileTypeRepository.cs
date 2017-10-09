using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Core;
using EXP.Core.Interface;
using EXP.Core.BaseClasses;
using EXP.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using AutoMapper;
using EXP.Core.Util;
using EXP.Core.Exceptions;

namespace EXP.DataAccess
{
    public class UserProfileTypeRepository : BaseRepository, IUserProfileTypeRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;            
        /// <summary>
        /// get list of profile type
        /// </summary>
        /// <returns></returns>
        public List<UserProfileType> ListUserProfileType()
        {
            List<UserProfileType> list = new List<UserProfileType>();                              
            try
            {
                Logger.DebugFormat("ListUserProfileType.");  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListUserProfileType]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mapper.CreateMap<IDataReader, UserProfileType>()
                                .ForMember(m => m.UserProfileType1, opt => opt.MapFrom(r => r["UserProfileType"]));
                            UserProfileType type = Mapper.DynamicMap<IDataReader, UserProfileType>(reader);
                            list.Add(type);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListUserProfileType failed.", exc);
            }
            return list;
        }
    }
}
