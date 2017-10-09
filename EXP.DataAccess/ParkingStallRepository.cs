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
using EXP.Core.Util;
using EXP.Entity;
using EXP.Core.Exceptions;

namespace EXP.DataAccess
{
    public class ParkingStallRepository : BaseRepository, IParkingStallRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        /// Create Parking Stall
        /// </summary>
        /// <param name="parkingStall"></param>
        public int CreateParkingStall(ParkingStall parkingStall)
        {
            int parkingStallId = 0;
            try
            {
                Logger.DebugFormat("CreateParkingStall. parkingStall: {0}", parkingStall.ToString());                                              
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateParkingStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, parkingStall.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "StallLocation", SqlDbType.VarChar, 256, parkingStall.StallLocation);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallTypeID", SqlDbType.Int, parkingStall.ParkingStallTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "StallNumber", SqlDbType.Int, parkingStall.StallNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MontlyRate", SqlDbType.Decimal, parkingStall.MontlyRate);
                    DatabaseUtils.AddInputParameter(cmd, "OverSell", SqlDbType.Decimal, parkingStall.OverSell);
                    DatabaseUtils.AddInputParameter(cmd, "Rate", SqlDbType.Decimal, parkingStall.Rate);
                    DatabaseUtils.AddInputParameter(cmd, "MaxRate", SqlDbType.Decimal, parkingStall.MaxRate);
                    DatabaseUtils.AddInputParameter(cmd, "FlatRate", SqlDbType.Decimal, parkingStall.FlatRate);
                    DatabaseUtils.AddInputParameter(cmd, "TimeIncrement", SqlDbType.Int, parkingStall.TimeIncrement);
                    DatabaseUtils.AddInputParameter(cmd, "GracePeriod", SqlDbType.Int, parkingStall.GracePeriod);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.VarChar, 256, parkingStall.StallDescription);
                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, parkingStall.isActive);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewStallID", SqlDbType.Int, 0);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    parkingStallId = (int)param.Value;
                }
                return parkingStallId;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateParkingStall failed.", exc);
            }
        }

        /// <summary>
        /// Get Parking Stall by id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        public ParkingStall GetParkingStall(int parkingStallID)
        {
            ParkingStall stall = new ParkingStall();
            try
            {
                Logger.DebugFormat("GetParkingStall. parkingStallID: {0}", parkingStallID);              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetParkingStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, parkingStallID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetParkingStall failed.", exc);
            }
            return stall;
        }

        /// <summary>
        /// Update Parking Stall
        /// </summary>
        /// <param name="parkingStall"></param>
        public void UpdateParkingStall(ParkingStall parkingStall)
        {
            try
            {
                Logger.DebugFormat("UpdateParkingStall. parkingStall: {0}", parkingStall.ToString());            
                                             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateParkingStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, parkingStall.ParkingStallID);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, parkingStall.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "StallLocation", SqlDbType.VarChar, 256, parkingStall.StallLocation);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallTypeID", SqlDbType.Int, parkingStall.ParkingStallTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "StallNumber", SqlDbType.Int, parkingStall.StallNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MontlyRate", SqlDbType.Decimal, parkingStall.MontlyRate);
                    DatabaseUtils.AddInputParameter(cmd, "OverSell", SqlDbType.Decimal, parkingStall.OverSell);
                    DatabaseUtils.AddInputParameter(cmd, "Rate", SqlDbType.Decimal, parkingStall.Rate);
                    DatabaseUtils.AddInputParameter(cmd, "MaxRate", SqlDbType.Decimal, parkingStall.MaxRate);
                    DatabaseUtils.AddInputParameter(cmd, "FlatRate", SqlDbType.Decimal, parkingStall.FlatRate);
                    DatabaseUtils.AddInputParameter(cmd, "TimeIncrement", SqlDbType.Int, parkingStall.TimeIncrement);
                    DatabaseUtils.AddInputParameter(cmd, "GracePeriod", SqlDbType.Int, parkingStall.GracePeriod);
                    DatabaseUtils.AddInputParameter(cmd, "Description", SqlDbType.VarChar, 256, parkingStall.StallDescription);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateParkingStall failed.", exc);
            }
        }

        /// <summary>
        /// Get parking information for lot
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        public ParkingStall GetParkingInfoForLot(int lotID)
        {
            ParkingStall stall = new ParkingStall();                        
            try
            {
                Logger.DebugFormat("GetParkingInfoForLot. lotID: {0}", lotID);      
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetParkingInfoForLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, lotID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetParkingInfoForLot failed.", exc);
            }
            return stall;
        }

        /// <summary>
        /// List parking stalls 
        /// </summary>
        /// <returns></returns>
        public List<ParkingStall> ListParkingStall()
        {
            List<ParkingStall> stalls = new List<ParkingStall>();
            ParkingStall stall = new ParkingStall();                       
            try
            {
                Logger.DebugFormat("ListParkingStall."); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListParkingStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                            stalls.Add(stall);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListParkingStall failed.", exc);
            }
            return stalls;
        }

        /// <summary>
        /// List parking stalls by stall type
        /// </summary>
        /// <param name="stallType"></param>
        /// <returns></returns>
        public List<ParkingStall> ListParkingStallByType(int lotId, int stallType)
        {
            List<ParkingStall> stalls = new List<ParkingStall>();
            ParkingStall stall = new ParkingStall();                       
            try
            {
                Logger.DebugFormat("ListParkingStallByType. lotID: {0}, stallType: {1}", lotId, stallType); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListParkingStallByType]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallTypeID", SqlDbType.Int, stallType);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, lotId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            stall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                            Mapper.CreateMap<IDataReader, ParkingStallType>()
                                .ForMember(m => m.ParkingStallType1, opt => opt.MapFrom(r => r["ParkingStallType"]));
                            ParkingStallType type = Mapper.DynamicMap<IDataReader, ParkingStallType>(reader);
                            stall.ParkingStallType = type;
                            stalls.Add(stall);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListParkingStallByType failed.", exc);
            }
            return stalls;
        }
        
        /// <summary>
        /// List montly parking stalls for page
        /// </summary>
        /// <param name="stallType"></param>
        /// <returns></returns>
        public List<ParkingStall> ListMontlyParkingStallsForPage(TableList listParam)
        {
            List<ParkingStall> stalls = new List<ParkingStall>();
            ParkingStall stall = new ParkingStall();                                 
            try
            {
                Logger.DebugFormat("ListMontlyParkingStallsForPage. listParam: {0}", listParam.ToString());   

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListMontlyParkingStallForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, listParam.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            stall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                            Mapper.CreateMap<IDataReader, ParkingStallType>()
                                .ForMember(m => m.ParkingStallType1, opt => opt.MapFrom(r => r["ParkingStallType"]));
                            ParkingStallType type = Mapper.DynamicMap<IDataReader, ParkingStallType>(reader);
                            stall.ParkingStallType = type;
                            stalls.Add(stall);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListMontlyParkingStallsForPage failed.", exc);
            }
            return stalls;
        }

        /// <summary>
        /// get count of montly stalls in DB
        /// </summary>
        /// <returns></returns>
        public int GetMontlyStallsCount(TableList listParam)
        {
            object value;                               
            try
            {
                Logger.DebugFormat("GetMontlyStallsCount. listParam: {0}", listParam.ToString()); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetMontlyParkingStallsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, listParam.LotID);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetMontlyStallsCount failed.", exc);
            }
            return (int)value;
        }

        /// <summary>
        /// List visitor parking stalls for page
        /// </summary>
        /// <param name="stallType"></param>
        /// <returns></returns>
        public List<ParkingStall> ListVisitorParkingStallsForPage(TableList listParam)
        {
            List<ParkingStall> stalls = new List<ParkingStall>();
            ParkingStall stall = new ParkingStall();        
            try
            {
                Logger.DebugFormat("ListVisitorParkingStallsForPage. listParam: {0}", listParam.ToString());                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListVisitorParkingStallForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, listParam.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);

                    conn.Open();                    
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
                            Mapper.CreateMap<IDataReader, ParkingStallType>()
                                .ForMember(m => m.ParkingStallType1, opt => opt.MapFrom(r => r["ParkingStallType"]));
                            ParkingStallType type = Mapper.DynamicMap<IDataReader, ParkingStallType>(reader);
                            stall.ParkingStallType = type;
                            stalls.Add(stall);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListVisitorParkingStallsForPage failed.", exc);
            }
            return stalls;
        }

        /// <summary>
        /// get count of visitor stalls in DB
        /// </summary>
        /// <returns></returns>
        public int GetVisitorStallsCount(TableList listParam)
        {
            object value;           
            try
            {
                Logger.DebugFormat("GetVisitorStallsCount. listParam: {0}", listParam.ToString());                         
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetVisitorParkingStallsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, listParam.LotID);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetVisitorStallsCount failed.", exc);
            }
            return (int)value;
        }

        /// <summary>
        /// delete parking stall by id
        /// </summary>
        /// <param name="lotId"></param>
        public void DeleteParkingStall(int stallId)
        {
            try
            {
                Logger.DebugFormat("DeleteParkingStall. stallId: {0}", stallId);            
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteParkingStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "stallID", SqlDbType.Int, stallId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteParkingStall failed.", exc);
            }
        }
    }
}
