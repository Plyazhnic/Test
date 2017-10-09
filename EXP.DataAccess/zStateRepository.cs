using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AutoMapper;
using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Interface;
using EXP.Entity;
using EXP.Core.Util;
using EXP.Core.Exceptions;

namespace EXP.DataAccess
{
    public class zStateRepository : BaseRepository, IzStateRepository  
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
                
        /// <summary>
        /// get list state
        /// </summary>
        /// <returns></returns>
        public List<zState> ListStates()
        {
            List<zState> list = new List<zState>();                                 
            try
            {
                Logger.DebugFormat("ListStates.");            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListStates]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zState state = Mapper.DynamicMap<IDataReader, zState>(reader);
                            list.Add(state);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListStates failed.", exc);
            }
            return list;
        }
        
        public string StateName(int StateId)
        {
            zState state = null;                   
            try
            {
                Logger.DebugFormat("StateName. StateId: {0}", StateId);              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_StateNameById]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "StateID", SqlDbType.Int, StateId);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             state = Mapper.DynamicMap<IDataReader, zState>(reader);     
                        }
                    }
                    return state.StateName;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("StateName failed.", exc);
            }
        }
    }
}
