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
    public class ParkingInventoryRepository : BaseRepository, IParkingInventoryRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        /// <summary>
        /// Create record parking inventory for company(reserved and unreserved)
        /// </summary>
        /// <param name="editParkingInventory"></param>
        /// <param name="countStall"></param>
        public void CreateParkingInventoryForCompany(ParkingInventory newParkingInventory)
        {
            try
            {
                Logger.DebugFormat("CreateParkingInventoryForCompany. newParkingInventory: {0}", newParkingInventory.ToString());
                                              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateParkingInventoryForCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, newParkingInventory.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, newParkingInventory.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, newParkingInventory.CompanyID);
                    DatabaseUtils.AddInputParameter(cmd, "DiscountRate", SqlDbType.Decimal, newParkingInventory.DiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "MarketRate", SqlDbType.Bit, newParkingInventory.MarketRate);
                    DatabaseUtils.AddInputParameter(cmd, "ReservedSpace", SqlDbType.Int, newParkingInventory.ReservedSpaces);
                    DatabaseUtils.AddInputParameter(cmd, "UnreservedSpace", SqlDbType.Int, newParkingInventory.UnReservedSpaces);
                    DatabaseUtils.AddInputParameter(cmd, "StartDate", SqlDbType.DateTime, newParkingInventory.EffectiveFrom);
                    DatabaseUtils.AddInputParameter(cmd, "EndDate", SqlDbType.DateTime, newParkingInventory.EffectiveTo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateParkingInventoryForCompany failed.", exc);
            }
        }

        /// <summary>
        /// Create record parking inventory 
        /// </summary>
        /// <param name="editParkingInventory"></param>
        /// <param name="countStall"></param>
        public void CreateParkingInventory(ParkingInventory newParkingInventory)
        {
            try
            {
                Logger.DebugFormat("CreateParkingInventory. newParkingInventory: {0}", newParkingInventory.ToString());
                                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateParkingInventory]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, newParkingInventory.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, newParkingInventory.ParkingStallID);
                    DatabaseUtils.AddInputParameter(cmd, "NumberOfStalls", SqlDbType.Int, newParkingInventory.NumberOfStalls);
                    DatabaseUtils.AddInputParameter(cmd, "MontlyRate", SqlDbType.Decimal, newParkingInventory.MontlyRate);
                    DatabaseUtils.AddInputParameter(cmd, "MarketRate", SqlDbType.Bit, newParkingInventory.MarketRate);
                    DatabaseUtils.AddInputParameter(cmd, "DiscountRate", SqlDbType.Decimal, newParkingInventory.DiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "OverSell", SqlDbType.Decimal, newParkingInventory.OverSell);
                    DatabaseUtils.AddInputParameter(cmd, "UserVehicleID", SqlDbType.Int, newParkingInventory.UserVehicleID);
                    DatabaseUtils.AddInputParameter(cmd, "PermitNumber", SqlDbType.VarChar, 32, newParkingInventory.PermitNumber);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, newParkingInventory.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, newParkingInventory.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, newParkingInventory.CompanyID);
                    DatabaseUtils.AddInputParameter(cmd, "isActive", SqlDbType.Bit, newParkingInventory.isActive);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveFrom", SqlDbType.DateTime, newParkingInventory.EffectiveFrom);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveTo", SqlDbType.DateTime, newParkingInventory.EffectiveTo); 

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateParkingInventory failed.", exc);
            }
        }
        
        /// <summary> 
        /// Get ParkingInventory by parkingIventoryID 
        /// </summary> 
        /// <param name="inventoryId"></param> 
        /// <returns></returns> 
        public ParkingInventory GetParkingInventoryByID(int inventoryId) 
        { 
            ParkingInventory inventory = new ParkingInventory(); 
            try 
            { 
                Logger.DebugFormat("GetParkingInventoryByID. ParkingInventoryID: {0}", inventoryId);                 
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                { 
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetParkingInventoryByID]", conn); 
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingInventoryID", SqlDbType.Int, inventoryId); 
                    
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader()) 
                    { 
                        if(reader.Read()) 
                        { 
                            inventory = ParkingInventory(reader);                             
                        } 
                    } 
                } 
                return inventory; 
            } 
            catch (Exception exc) 
            { 
                throw new ExpDatabaseException("GetParkingInventoryByID failed.", exc); 
             } 
          }

        /// <summary>
        /// List allocated parking stalls for page
        /// </summary>
        /// <param name="newParkingInventory"></param>
        /// <param name="countStall"></param>
        public List<ParkingInventory> ListAllocatedStallsForPage(TableList listParam)
        {
            try
            {
                Logger.DebugFormat("ListAllocatedStallForPage. ListAllocatedStallForPage: {0}", listParam.ToString());
                List<ParkingInventory> list = new List<ParkingInventory>();
                ParkingInventory parkingInv = new ParkingInventory();                                               
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListAllocatedParkingStallForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, listParam.StallID);
                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        Company company;
                        while (reader.Read())
                        {
                            parkingInv = Mapper.DynamicMap<IDataReader, ParkingInventory>(reader);
                            company = Mapper.DynamicMap<IDataReader, Company>(reader);
                            parkingInv.Company = company;
                            list.Add(parkingInv);
                        }
                    }
                }
                return list;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListAllocatedStallForPage failed.", exc);
            }
        }

        /// <summary>
        /// Get allocated stall for stall 
        /// </summary>
        /// <param name="newParkingInventory"></param>
        /// <param name="countStall"></param>
        public ParkingInventory GetAllocatedStall(int stallId)
        {
            try
            {
                Logger.DebugFormat("GetAllocatedStall. stallId: {0}", stallId);
                ParkingInventory inventory = new ParkingInventory();                               
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetAllocatedStall]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, stallId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inventory = ParkingInventory(reader);
                        }
                    }
                }
                return inventory;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetAllocatedStall failed.", exc);
            }
        }

        /// <summary>
        /// get count of allocated stalls in DB
        /// </summary>
        /// <returns></returns>
        public int GetAllocatedStallsCount(TableList listParam)
        {
            object value;                                   
            try
            {
                Logger.DebugFormat("GetAllocatedStallsCount. listParam: {0}", listParam.ToString());   
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetAllocatedParkingStallsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, listParam.StallID);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetAllocatedStallsCount failed.", exc);
            }
            return (int)value;
        }      

        /// <summary>
        /// Get list of inventories for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<ParkingInventory> GetParkingInventoryForCompany(int companyId)
        {
            List<ParkingInventory> list = new List<ParkingInventory>();
            ParkingInventory inventory = new ParkingInventory(); 

            try
            {
                Logger.DebugFormat("GetParkingInventoryForCompany. CompanyID: {0}", companyId);                               
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetParkingInventoryForCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inventory = ParkingInventory(reader);
                            list.Add(inventory);
                        }
                    }
                }
                return list;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetParkingInventoryForCompany failed.", exc);
            }
        }

        /// <summary>
        /// Update parking inventory
        /// </summary>
        /// <param name="editParkingInventory"></param>
        public void UpdateParkingInventory(ParkingInventory editParkingInventory)
        {
            try
            {
                Logger.DebugFormat("UpdateParkingInventory. editParkingInventory: {0}", editParkingInventory.ToString());                                                            
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateParkingInventory]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingInventoryID", SqlDbType.Int, editParkingInventory.ParkingInventoryID);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, editParkingInventory.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingStallID", SqlDbType.Int, editParkingInventory.ParkingStallID);
                    DatabaseUtils.AddInputParameter(cmd, "NumberOfStalls", SqlDbType.Int, editParkingInventory.NumberOfStalls);
                    DatabaseUtils.AddInputParameter(cmd, "MontlyRate", SqlDbType.Decimal, editParkingInventory.MontlyRate);
                    DatabaseUtils.AddInputParameter(cmd, "MarketRate", SqlDbType.Bit, editParkingInventory.MarketRate);
                    DatabaseUtils.AddInputParameter(cmd, "DiscountRate", SqlDbType.Decimal, editParkingInventory.DiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "OverSell", SqlDbType.Decimal, editParkingInventory.OverSell);
                    DatabaseUtils.AddInputParameter(cmd, "UserVehicleID", SqlDbType.Int, editParkingInventory.UserVehicleID);
                    DatabaseUtils.AddInputParameter(cmd, "PermitNumber", SqlDbType.VarChar,32, editParkingInventory.PermitNumber);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, editParkingInventory.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, editParkingInventory.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, editParkingInventory.CompanyID);
                    DatabaseUtils.AddInputParameter(cmd, "isActive", SqlDbType.Bit, editParkingInventory.isActive);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveFrom", SqlDbType.DateTime, editParkingInventory.EffectiveFrom);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveTo", SqlDbType.DateTime, editParkingInventory.EffectiveTo); 
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();   
                }   
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateParkingInventory failed.", exc);
            }
        }

        /// <summary>
        /// Delete parking inventory by id
        /// </summary>
        /// <param name="inventoryId"></param>
        public void DeleteParkingInventory (int inventoryId)
        {
            try
            {
                Logger.DebugFormat("DeleteParkingInventory. inventoryId: {0}", inventoryId);                                                
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteParkingInventory]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "ParkingInventoryID", SqlDbType.Int, inventoryId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteParkingInventory failed.", exc);
            }
        }

        private ParkingInventory ParkingInventory(IDataReader reader)
        {
            ParkingInventory parkingInv = Mapper.DynamicMap<IDataReader, ParkingInventory>(reader);
            ParkingStall parkingStall = Mapper.DynamicMap<IDataReader, ParkingStall>(reader);
            Mapper.CreateMap<IDataReader, ParkingStallType>()
                .ForMember(m => m.ParkingStallType1, opt => opt.MapFrom(r => r["ParkingStallType"]));
            ParkingStallType parkingStallType = Mapper.DynamicMap<IDataReader, ParkingStallType>(reader);
            parkingStall.ParkingStallType = parkingStallType;
            Lot lot = Mapper.DynamicMap<IDataReader, Lot>(reader);
            Building building = Mapper.DynamicMap<IDataReader, Building>(reader);
            parkingInv.Building = building;
            parkingInv.Lot = lot;
            parkingInv.ParkingStall = parkingStall;

            return parkingInv;
        }
    }
}
