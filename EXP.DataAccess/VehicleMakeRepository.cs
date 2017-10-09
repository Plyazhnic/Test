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
    public class VehicleMakeRepository : BaseRepository, IVehicleMakeRepository 
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;            
        /// <summary>
        /// List of vehicle models for page
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<VehicleModel> ListVehicleModelsForPage(TableList listParam)
        {
            List<VehicleModel> list = new List<VehicleModel>();                   
            try
            {
                Logger.DebugFormat("ListVehicleModelsForPage. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListVehicleModelsForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "MakeId", SqlDbType.Int, listParam.MakeID);
                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
             
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Mapper.CreateMap<IDataReader, VehicleModel>()
                            .ForMember(m => m.VehicleModel1, opt => opt.MapFrom(r => r["VehicleModel"]));
                        VehicleModel make = Mapper.DynamicMap<IDataReader, VehicleModel>(reader);
                        list.Add(make);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListVehicleModelsForPage failed.", exc);
            }
            return list;
        }
         
        /// <summary>
        /// List of vehicle makes for page
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<VehicleMake> ListVehicleMakesForPage(TableList listParam)
        {
            List<VehicleMake> list = new List<VehicleMake>();                                          
            try
            {
                Logger.DebugFormat("ListVehicleMakesForPage. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListVehicleMakesForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Mapper.CreateMap<IDataReader, VehicleMake>()
                            .ForMember(m => m.VehicleMake1, opt => opt.MapFrom(r => r["VehicleMake"]));
                        VehicleMake make = Mapper.DynamicMap<IDataReader, VehicleMake>(reader);
                        list.Add(make);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListVehicleMakesForPage failed.", exc);
            }
            return list;
        }
        /// <summary>
        /// Add Vehicle Model
        /// </summary>
        /// <returns></returns>
        public bool AddVehicleModel(VehicleModel model)
        {
            object value;
            try
            {
                Logger.DebugFormat("AddVehicleModel. model: {0}", model.ToString());            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreateVehicleModel]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, model.isActive);
                    DatabaseUtils.AddInputParameter(cmd, "Name", SqlDbType.VarChar, 256, model.VehicleModel1);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.VarChar, 1024, model.VehicleModelDescription);
                    DatabaseUtils.AddInputParameter(cmd, "Year", SqlDbType.Int, model.Year1);
                    DatabaseUtils.AddInputParameter(cmd, "MakeId", SqlDbType.Int, model.VehicleMakeID);
                
                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("AddVehicleModel failed.", exc);
            }
            return (bool)value;
        }

        /// <summary>
        /// Change Vehicle Make
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsActive"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        public bool UpdateVehicleMake(VehicleMake make)
        {
            object value;                
            try
            {
                Logger.DebugFormat("UpdateVehicleMake. make: {0}", make.ToString());             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ChangeVehicleMake]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, make.isActive);
                    DatabaseUtils.AddInputParameter(cmd, "Name", SqlDbType.VarChar, 50, make.VehicleMake1);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.VarChar, make.VehicleMakeDescription);
                    DatabaseUtils.AddInputParameter(cmd, "Id", SqlDbType.Int, make.VehicleMakeID);
                
                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateVehicleMake failed.", exc);
            }
            return (bool)value;
        }

        /// <summary>
        /// Get VehicleModel by Id
        /// </summary>
        /// <param name="ModelId"></param>
        /// <returns></returns>
        public VehicleModel GetVehicleModelById(int ModelId)
        {
            VehicleModel V = null;                                  
            try
            {
                Logger.DebugFormat("GetVehicleModelById. ModelId: {0}", ModelId);  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetVehicleModelById]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ModelId", SqlDbType.Int, ModelId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Mapper.CreateMap<IDataReader, VehicleModel>()
                            .ForMember(m => m.VehicleModel1, opt => opt.MapFrom(r => r["VehicleModel"]));
                        V = Mapper.DynamicMap<IDataReader, VehicleModel>(reader);
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVehicleModelById failed.", exc);
            }
            return V;
        }

        /// <summary>
        /// Change Vehicle Model
        /// </summary>
        /// <returns></returns>
        public bool ChangeVehicleModel(VehicleModel model)
        {
            object value;                       
            try
            {
                Logger.DebugFormat("ChangeVehicleModel. model: {0}", model);              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ChangeVehicleModel]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, model.isActive);
                    DatabaseUtils.AddInputParameter(cmd, "Name", SqlDbType.VarChar, 50, model.VehicleModel1);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.VarChar, model.VehicleModelDescription);
                    DatabaseUtils.AddInputParameter(cmd, "Id", SqlDbType.Int, model.VehicleModelID);
                    DatabaseUtils.AddInputParameter(cmd, "Year", SqlDbType.Int, model.Year1);

                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ChangeVehicleModel failed.", exc);
            }
            return (bool)value;
        }
               
        /// <summary>
        /// Get Vehicle MakeBy Id
        /// </summary>
        /// <param name="MakeId"></param>
        /// <returns></returns>
        public VehicleMake GetVehicleMakeById(int MakeId)
        {
            VehicleMake V = null;                      
            try
            {
                Logger.DebugFormat("GetVehicleMakeById. MakeId: {0}", MakeId);                 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetVehicleMakeById]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "MakeId", SqlDbType.Int, MakeId);
           
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Mapper.CreateMap<IDataReader, VehicleMake>()
                            .ForMember(m => m.VehicleMake1, opt => opt.MapFrom(r => r["VehicleMake"]));
                        V = Mapper.DynamicMap<IDataReader, VehicleMake>(reader);
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVehicleMakeById failed.", exc);
            }
            return V;     
        }
       
        /// <summary>
        /// get count of vehicle makes in DB
        /// </summary>
        /// <returns></returns>
        public int GetVehicleMakesCount(string search)
        {
            object value;                                   
            try
            {
                Logger.DebugFormat("GetVehicleMakesCount. search: {0}", search);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetVehicleMakesCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, search);
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVehicleMakesCount failed.", exc);
            }
            return (int)value;
        }

        /// <summary>
        /// get count of vehicle models in DB
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int GetVehicleModelsCount(TableList listParam)
        {
            object value;                     
            try
            {
                Logger.DebugFormat("GetVehicleModelsCount. listParam: {0}", listParam.ToString());            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetVehicleModelsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "MakeId", SqlDbType.Int, listParam.MakeID);  
                    conn.Open();

                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVehicleModelsCount failed.", exc);
            }
            return (int)value;
        }

        /// <summary>
        /// delete vehicle make by id
        /// </summary>
        /// <param name="vehicleMakeId"></param>
        public bool AddVehicleMake(VehicleMake make)
        {
            object value;                       
            try
            {
                Logger.DebugFormat("AddVehicleMake. make: {0}", make.ToString()); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreateVehicleMake]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;   

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, make.isActive);
                    DatabaseUtils.AddInputParameter(cmd, "Name", SqlDbType.VarChar,50, make.VehicleMake1);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.VarChar, make.VehicleMakeDescription);
                    
                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("AddVehicleMake failed.", exc);
            }
            return (bool)value;
        }
       
        public void DeleteVehicleMake(int vehicleMakeId)
        {
            try
            {
                Logger.DebugFormat("DeleteVehicleMake. vehicleMakeId: {0}", vehicleMakeId);                               
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_DeleteVehicleMake]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "VehicleMakeID", SqlDbType.Int, vehicleMakeId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteVehicleMake failed.", exc);
            }
        }

        public List<VehicleMake> ListMakes()
        {
            List<VehicleMake> list = new List<VehicleMake>();                  
            try
            {
                Logger.DebugFormat("ListMakes.");             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListMakes]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VehicleMake make = VehicleMake(reader);
                            list.Add(make);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListMakes failed.", exc);
            }
            return list;
        }

        public VehicleMake GetMake(string makeName)
        {
            VehicleMake makeEntity = null;                     
            try
            {
                Logger.DebugFormat("GetMake. makeName: {0}", makeName);               
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetMake]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Make", SqlDbType.VarChar, 256, makeName);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            makeEntity = VehicleMake(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetMake failed.", exc);
            }
            return makeEntity;
        }

        private static VehicleMake VehicleMake(IDataReader reader)
        {
            Mapper.CreateMap<IDataReader, VehicleMake>()
                .ForMember(m => m.VehicleMake1, opt => opt.MapFrom(r => r["VehicleMake"]));
            VehicleMake make = Mapper.DynamicMap<IDataReader, VehicleMake>(reader);
            return make;
        }
    }
}
