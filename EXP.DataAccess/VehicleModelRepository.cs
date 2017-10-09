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
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Core.Util;
using EXP.Entity;

namespace EXP.DataAccess
{
    public class VehicleModelRepository : BaseRepository, IVehicleModelRepository 
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        /// <summary>
        /// List Models
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<VehicleModel> ListModels(int makeId)
        {
            List<VehicleModel> list = new List<VehicleModel>();                       
            try
            {
                Logger.DebugFormat("ListModels. makeId: {0}", makeId);   
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListModels]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "makeID", SqlDbType.Int, makeId);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VehicleModel model = VehicleModel(reader);
                            list.Add(model);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListModels failed.", exc);
            }
            return list;
        }

        public VehicleModel GetModel(string modelName)
        {
            VehicleModel model = null;                     
            try
            {
                Logger.DebugFormat("GetModel. modelName: {0}", modelName);              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetModel]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Model", SqlDbType.VarChar, 256, modelName);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model = VehicleModel(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetModel failed.", exc);
            }
            return model;
        }

        private static VehicleModel VehicleModel(IDataReader reader)
        {
            Mapper.CreateMap<IDataReader, VehicleModel>()
                .ForMember(m => m.VehicleModel1, opt => opt.MapFrom(r => r["VehicleModel"]));
            VehicleModel model = Mapper.DynamicMap<IDataReader, VehicleModel>(reader);
            return model;
        }

    }
}
