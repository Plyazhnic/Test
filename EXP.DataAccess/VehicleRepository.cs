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
    public class VehicleRepository : BaseRepository, IVehicleRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;      

        public List<Vehicle> ListVehicles(int userProfileID)
        {
            List<Vehicle> list = new List<Vehicle>();                      
            try
            {
                Logger.DebugFormat("ListVehicles. userProfileID: {0}", userProfileID);  
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_ListVehicles]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userProfileID);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vehicle vehicle = Vehicle(reader);

                            list.Add(vehicle);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListVehicles failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// Get two vehicles for user
        /// </summary>
        /// <param name="userProfileID"></param>
        /// <returns></returns>
        public List<Vehicle> GetVehicles(int userProfileID)
        {
            List<Vehicle> list = new List<Vehicle>();                                          
            try
            {
                Logger.DebugFormat("GetVehicles. userProfileID: {0}", userProfileID); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetVehicles]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userProfileID);
                
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vehicle vehicle = Vehicle(reader);

                            list.Add(vehicle);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVehicles failed.", exc);
            }
            return list;
        }

        public Vehicle GetVehicle(int vehicleId)
        {
            Vehicle vehicle = null;                       
            try
            {
                Logger.DebugFormat("GetVehicle. vehicleId: {0}", vehicleId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetVehicle]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "VehicleID", SqlDbType.Int, vehicleId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            vehicle = Vehicle(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVehicle failed.", exc);
            }
            return vehicle;
        }

        public int CreateVehicle(Vehicle vehicle)
        {
            try
            {
                Logger.DebugFormat("CreateVehicle. vehicle: {0}", vehicle.ToString());
                int vehicleId = 0;                              

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreateVehicle]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, vehicle.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "Year", SqlDbType.Int, vehicle.Year);
                    DatabaseUtils.AddInputParameter(cmd, "VehicleMakeID", SqlDbType.Int, vehicle.VehicleMakeID);
                    DatabaseUtils.AddInputParameter(cmd, "VehicleModelID", SqlDbType.Int, vehicle.VehicleModelID);
                    DatabaseUtils.AddInputParameter(cmd, "Color", SqlDbType.VarChar, 32, vehicle.Color);
                    DatabaseUtils.AddInputParameter(cmd, "LicensePlateNumber", SqlDbType.VarChar, 32, vehicle.LicensePlateNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PermitNumber", SqlDbType.VarChar, 32, vehicle.PermitNumber);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewVehicleId", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    vehicleId = (int)param.Value;
                }
                return vehicleId;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateVehicle failed.", exc);
            }
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                Logger.DebugFormat("UpdateVehicle. vehicle: {0}", vehicle.ToString());                                

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdateVehicle]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, vehicle.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "VehicleID", SqlDbType.Int, vehicle.VehicleID);
                    DatabaseUtils.AddInputParameter(cmd, "Year", SqlDbType.Int, vehicle.Year);
                    DatabaseUtils.AddInputParameter(cmd, "VehicleMakeID", SqlDbType.Int, vehicle.VehicleMakeID);
                    DatabaseUtils.AddInputParameter(cmd, "VehicleModelID", SqlDbType.Int, vehicle.VehicleModelID);
                    DatabaseUtils.AddInputParameter(cmd, "Color", SqlDbType.VarChar, 32, vehicle.Color);
                    DatabaseUtils.AddInputParameter(cmd, "LicensePlateNumber", SqlDbType.VarChar, 32, vehicle.LicensePlateNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PermitNumber", SqlDbType.VarChar, 32, vehicle.PermitNumber);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateVehicle failed.", exc);
            }
        }

        public void RemoveVehicle(int vehicleId)
        {
            try
            {
                Logger.DebugFormat("RemoveVehicle. vehicleId: {0}", vehicleId);                                        
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_RemoveVehicle]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "VehicleID", SqlDbType.Int, vehicleId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("RemoveVehicle failed.", exc);
            }
        }

        private static VehicleMake VehicleMake(IDataReader reader)
        {
            Mapper.CreateMap<IDataReader, VehicleMake>()
                .ForMember(m => m.VehicleMake1, opt => opt.MapFrom(r => r["VehicleMake"]));
            VehicleMake make = Mapper.DynamicMap<IDataReader, VehicleMake>(reader);
            return make;
        }

        private static VehicleModel VehicleModel(IDataReader reader)
        {
            Mapper.CreateMap<IDataReader, VehicleModel>()
                .ForMember(m => m.VehicleModel1, opt => opt.MapFrom(r => r["VehicleModel"]));
            VehicleModel model = Mapper.DynamicMap<IDataReader, VehicleModel>(reader);
            return model;
        }

        private static Vehicle Vehicle(IDataReader reader)
        {
            Vehicle vehicle = Mapper.DynamicMap<IDataReader, Vehicle>(reader);
            if (vehicle.VehicleMakeID.HasValue)
            {
                vehicle.VehicleMake = VehicleMake(reader);
            }

            if (vehicle.VehicleModelID.HasValue)
            {
                vehicle.VehicleModel = VehicleModel(reader);
            }

            return vehicle;
        }

    }
}
