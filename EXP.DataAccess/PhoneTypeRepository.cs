using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Interface;
using EXP.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AutoMapper;
using EXP.Core.Exceptions;
using EXP.Core.Util;

namespace EXP.DataAccess
{
    public class PhoneTypeRepository : BaseRepository, IPhoneTypeRepository 
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        /// get list types of phone
        /// </summary>
        /// <returns></returns>
        public List<PhoneType> ListPhoneTypes()
        {
            List<PhoneType> list = new List<PhoneType>();                       
            try
            {
                Logger.DebugFormat("ListPhoneTypes.");   
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListPhoneTypes]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mapper.CreateMap<IDataReader, PhoneType>()
                                .ForMember(m => m.PhoneType1, opt => opt.MapFrom(r => r["PhoneType"]));
                            PhoneType phoneType = Mapper.DynamicMap<IDataReader, PhoneType>(reader);
                            list.Add(phoneType);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListPhoneTypes failed.", exc);
            }
            return list;
        }
    }
}
