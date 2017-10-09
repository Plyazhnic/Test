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
    public class BuildingRepository : BaseRepository, IBuildingRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
               
        /// <summary>
        /// Create Building
        /// </summary>
        /// <param name="building"></param>
        public int CreateBuilding(Building building)
        {
            try
            {
                Logger.DebugFormat("CreateBuilding. building: {0}", building.ToString());
                            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateBuilding]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;                          

                    DatabaseUtils.AddInputParameter(cmd, "BuildingName", SqlDbType.VarChar, 256, building.BuildingName);
                    DatabaseUtils.AddInputParameter(cmd, "OwnerID", SqlDbType.Int, building.OwnerID);
                    DatabaseUtils.AddInputParameter(cmd, "ManagerID", SqlDbType.Int, building.ManagerID);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, building.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "MailingAddressID", SqlDbType.Int, building.MailingAddressID);
                    DatabaseUtils.AddInputParameter(cmd, "PrimaryPhoneNumber", SqlDbType.VarChar, 20, building.PrimaryPhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PrimaryFaxNumber", SqlDbType.VarChar, 20, building.PrimaryFaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingPhoneNumber", SqlDbType.VarChar, 20, building.MailingPhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingFaxNumber", SqlDbType.VarChar, 20, building.MailingFaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "BankName", SqlDbType.NVarChar, 50, building.BankName);
                    DatabaseUtils.AddInputParameter(cmd, "AccountName", SqlDbType.NVarChar, 20, building.AccountName);
                    DatabaseUtils.AddInputParameter(cmd, "AccountNumber", SqlDbType.NVarChar, 20, building.AccountNumber);
                    DatabaseUtils.AddInputParameter(cmd, "RoutingNumber", SqlDbType.NVarChar, 20, building.RoutingNumber);
                    DatabaseUtils.AddInputParameter(cmd, "Notes", SqlDbType.NVarChar, 256, building.Notes);
                    DatabaseUtils.AddInputParameter(cmd, "TaxRate", SqlDbType.Decimal, building.TaxRate);
                    DatabaseUtils.AddInputParameter(cmd, "DueDateReminder", SqlDbType.Int, building.DueDateReminder);
                    DatabaseUtils.AddInputParameter(cmd, "ReminderFrequency", SqlDbType.Int, building.ReminderFrequency);
                    DatabaseUtils.AddInputParameter(cmd, "ReminderCutoff", SqlDbType.Int, building.ReminderCutoff);
                    DatabaseUtils.AddInputParameter(cmd, "InvoiceCutoff", SqlDbType.Int, building.InvoiceCutoff);
                    DatabaseUtils.AddInputParameter(cmd, "ACHDiscountRate", SqlDbType.Decimal, building.ACHDiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "CCDiscountRate", SqlDbType.Decimal, building.CCDiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "MontlyFee", SqlDbType.Decimal, building.MontlyFee);
                    DatabaseUtils.AddInputParameter(cmd, "MFBillingDay", SqlDbType.Int, building.MFBillingDay);
                    DatabaseUtils.AddInputParameter(cmd, "VBNotification", SqlDbType.NVarChar, 256, building.VBNotification);
                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, building.IsActive);
                    
                    conn.Open();
                    object o = cmd.ExecuteScalar();      

                    return (int)o;
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateBuilding failed.", exc);
            }
        }

        //TODO: DELETE
        /// <summary>
        /// Get Building With Address
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public Building GetBuildingWithAddress(int? buildingId)
        {
            try
            {
                Building building = new Building();
                Logger.DebugFormat("GetBuilding. buildingId: {0}", buildingId);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingWithAddress]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, buildingId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            building = Mapper.DynamicMap<IDataReader, Building>(reader);

                            Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
                            building.BuildingAddress = address;
                        }
                    }
                    return building;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuilding failed.", exc);
            }
        }
              
        /// <summary>
        /// Get building by id
        /// </summary>
        /// <param name="buildingId"></param>
        public Building GetBuilding(int buildingId)
        {
            try
            {
                Building building = new Building();
                Logger.DebugFormat("GetBuilding. buildingId: {0}", buildingId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuilding]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, buildingId);
                    
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            building = Mapper.DynamicMap<IDataReader, Building>(reader);
                        }
                    }
                    return building;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuilding failed.", exc);
            }
        }
        
        /// <summary>
        /// Get buildings by lot id
        /// </summary>
        /// <param name="buildingId"></param>
        public List<Building> GetBuildingsForLot(int lotId)
        {    
            try
            {
                List<Building> buildings = new List<Building>();
                Building building = new Building();
                Logger.DebugFormat("GetBuildingsForLot. lotId: {0}", lotId);
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingForLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, lotId);
                    
                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            building = Mapper.DynamicMap<IDataReader, Building>(reader);
                            buildings.Add(building);
                        }
                    }
                    return buildings;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuildingsForLot failed.", exc);
            }
        }

        /// <summary>
        /// get building for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Building GetBuildingForCompany(int companyId)
        {
            try
            {
                Building building = new Building();
                Logger.DebugFormat("GetBuildingForCompany. companyId: {0}", companyId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingForCompany]", conn);

                    cmd.CommandType = CommandType.StoredProcedure;               
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, companyId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            building = Mapper.DynamicMap<IDataReader, Building>(reader);
                        }
                    }
                    return building;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuildingForCompany failed.", exc);
            }
        }

        /// <summary>
        /// Get list of buildings for page
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Building> ListBuildingsForPage(TableList listParam)
        {
            List<Building> list = new List<Building>();               
            try
            {
                Logger.DebugFormat("ListBuildingsForPage. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListBuildingsForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Building building = Building(reader);
                        list.Add(building);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListBuildingsForPage failed.", exc);
            }
            return list;
        }

        public List<BuildingWide> SearchBuildings(SearchBuilding searchParam)
        {
            List<BuildingWide> list = new List<BuildingWide>();
            try
            {
                Logger.DebugFormat("SearchBuildings. searchParam: {0}", searchParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_SearchBuildings]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "@BuildingName", SqlDbType.NVarChar, searchParam.BuildingName);
                    DatabaseUtils.AddInputParameter(cmd, "@OwnersFirstName", SqlDbType.NVarChar, searchParam.OwnersFirstName);
                    DatabaseUtils.AddInputParameter(cmd, "@OwnersLastName", SqlDbType.NVarChar, searchParam.OwnersLastName);
                    DatabaseUtils.AddInputParameter(cmd, "@ManagerFirstName", SqlDbType.NVarChar, searchParam.ManagerFirstName);
                    DatabaseUtils.AddInputParameter(cmd, "@ManagerLastName ", SqlDbType.NVarChar, searchParam.ManagerLastName);
                    DatabaseUtils.AddInputParameter(cmd, "@City", SqlDbType.NVarChar, searchParam.City);
                    DatabaseUtils.AddInputParameter(cmd, "@StateID", SqlDbType.Int, searchParam.StateID);
                    DatabaseUtils.AddInputParameter(cmd, "@ZipCode", SqlDbType.NVarChar, searchParam.ZipCode);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        BuildingWide building = Mapper.DynamicMap<IDataReader, BuildingWide>(reader);
                        list.Add(building);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("SearchBuildings failed.", exc);
            }
            return list;
        }       
        /// <summary>
        /// Get all owner's buildings
        /// </summary>
        /// <returns></returns>
        public List<Building> ListBuildings(int ownerId)
        {
            List<Building> list = new List<Building>();
            try
            {
                Logger.DebugFormat("ListBuildings. ownerId: {0}", ownerId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListBuildings]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "OwnerID", SqlDbType.Int, ownerId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Building building = Mapper.DynamicMap<IDataReader, Building>(reader);
                        list.Add(building);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListBuildings failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// Get list all buildings
        /// </summary>
        /// <returns></returns>
        public List<Building> ListBuildings()
        {
            List<Building> list = new List<Building>();
            try
            {
                Logger.DebugFormat("ListBuildings");
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListBuildings]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "OwnerID", SqlDbType.Int, DBNull.Value);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Building building = Mapper.DynamicMap<IDataReader, Building>(reader);
                        list.Add(building);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListBuildings failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// List Buildings For Dashboard
        /// </summary>
        /// <returns></returns>
        public List<Building> ListBuildingsForDashboard(int UserProfileID)
        {
            List<Building> list = new List<Building>();
            try
            {
                Logger.DebugFormat("ListBuildingsForDashboard. UserProfileID:", UserProfileID);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListBuildingsForDashboard]", conn);
                    DatabaseUtils.AddInputParameter(cmd, "OwnerID", SqlDbType.Int, UserProfileID);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Building building = Mapper.DynamicMap<IDataReader, Building>(reader);

                        Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
                        zState zstate = Mapper.DynamicMap<IDataReader, zState>(reader);

                        address.State = zstate;
                        building.BuildingAddress = address;

                        list.Add(building);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListBuildingsForDashboard failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// get count of buildings in DB
        /// </summary>
        /// <returns></returns>
        public int GetBuildingsCount(string search)
        {
            object value;
            try
            {
                Logger.DebugFormat("GetBuildingsCount. search:", search);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingsCount]",conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, search);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuildingsCount failed.", exc);
            }
            return (int)value;
        }

        //TODO: DELETE
        //public int GetBuildingLotsCount(string search, int buildingId)
        //{
        //    object value;
            
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingLotsCount]", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;           

        //        DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, search);
        //        DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, buildingId);
                
        //        conn.Open();
        //        value = cmd.ExecuteScalar();
        //    }
        //    return (int)value;
        //}

        /// <summary>
        /// Delete Building
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns>returns true or false in according of deleting building by id</returns>
        public bool DeleteBuilding(int buildingId)
        {
            try
            {
                Logger.DebugFormat("DeleteBuilding. buildingId: {0}", buildingId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteBuilding]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, buildingId);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0? true : false;
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteBuilding failed.", exc);
            }
        }

        /// <summary>
        /// Update building data
        /// </summary>
        /// <param name="building"></param>
        public void UpdateBuilding(Building building)
        {
            try
            {
                Logger.DebugFormat("UpdateBuilding. building: {0}", building.ToString());                          
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateBuilding]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;                     

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, building.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingName", SqlDbType.VarChar, 256, building.BuildingName);
                    DatabaseUtils.AddInputParameter(cmd, "OwnerID", SqlDbType.Int, building.OwnerID);
                    DatabaseUtils.AddInputParameter(cmd, "ManagerID", SqlDbType.Int, building.ManagerID);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, building.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "MailingAddressID", SqlDbType.Int, building.MailingAddressID);
                    DatabaseUtils.AddInputParameter(cmd, "PrimaryPhoneNumber", SqlDbType.VarChar, 20, building.PrimaryPhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "PrimaryFaxNumber", SqlDbType.VarChar, 20, building.PrimaryFaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingPhoneNumber", SqlDbType.VarChar, 20, building.MailingPhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingFaxNumber", SqlDbType.VarChar, 20, building.MailingFaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "BankName", SqlDbType.NVarChar,50, building.BankName);
                    DatabaseUtils.AddInputParameter(cmd, "AccountName", SqlDbType.NVarChar,20, building.AccountName);
                    DatabaseUtils.AddInputParameter(cmd, "AccountNumber", SqlDbType.NVarChar,20, building.AccountNumber);
                    DatabaseUtils.AddInputParameter(cmd, "RoutingNumber", SqlDbType.NVarChar,20, building.RoutingNumber);
                    DatabaseUtils.AddInputParameter(cmd, "Notes", SqlDbType.NVarChar,256, building.Notes);
                    DatabaseUtils.AddInputParameter(cmd, "TaxRate", SqlDbType.Decimal, building.TaxRate);
                    DatabaseUtils.AddInputParameter(cmd, "DueDateReminder", SqlDbType.Int, building.DueDateReminder);
                    DatabaseUtils.AddInputParameter(cmd, "ReminderFrequency", SqlDbType.Int, building.ReminderFrequency);
                    DatabaseUtils.AddInputParameter(cmd, "ReminderCutoff", SqlDbType.Int, building.ReminderCutoff);
                    DatabaseUtils.AddInputParameter(cmd, "InvoiceCutoff", SqlDbType.Int, building.InvoiceCutoff);
                    DatabaseUtils.AddInputParameter(cmd, "ACHDiscountRate", SqlDbType.Decimal, building.ACHDiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "CCDiscountRate", SqlDbType.Decimal, building.CCDiscountRate);
                    DatabaseUtils.AddInputParameter(cmd, "MontlyFee", SqlDbType.Decimal, building.MontlyFee);
                    DatabaseUtils.AddInputParameter(cmd, "MFBillingDay", SqlDbType.Int, building.MFBillingDay);
                    DatabaseUtils.AddInputParameter(cmd, "VBNotification", SqlDbType.NVarChar,256, building.VBNotification);
                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, building.IsActive);
	
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateBuilding failed.", exc);
            }
        }

        //TODO: Rework!!! (DETETE)
        /// <summary>
        /// Create Building. Argument data contains info only about building addres and name
        /// </summary>
        /// <returns> Returns id of created building</returns>
        public int CreateBuildingWithNewAddress(Building building)
        {
            building.BuildingAddress.Address2 = null; // There is no Address2 as input parametr

          //  building.AddressID = Unit.AddressRepository.CreateAddress(building.BuildingAddress);
            building.IsActive = true;

            try
            {
                Logger.DebugFormat("CreateBuilding. building: {0}", building.ToString());
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateBuildingForAddress]", conn);

                    cmd.CommandType = CommandType.StoredProcedure;               
                    DatabaseUtils.AddInputParameter(cmd, "BuildingName", SqlDbType.VarChar, 256, building.BuildingName);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, building.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, building.IsActive);
                    DatabaseUtils.AddInputParameter(cmd, "OwnerID", SqlDbType.Int, building.OwnerID);
                    
                    conn.Open();
                    object o = cmd.ExecuteScalar();

                    return (int)o;
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateBuilding failed.", exc);
            }       
        }

        //TODO: Rework!!! (DELETE)
        /// <summary>
        /// Update Building
        /// </summary>
        /// <param name="BuildingID"></param>
        /// <returns>return weather operation returns succes</returns>
        public bool UpdateBuildingWithNewAddress(Building building)
        {       
            try
            {
                Logger.DebugFormat("UpdateBuilding. building: {0}", building);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateBuildingForAddress]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, building.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingName", SqlDbType.VarChar, 256, building.BuildingName);
                    DatabaseUtils.AddInputParameter(cmd, "Address1", SqlDbType.VarChar, 256, building.BuildingAddress.Address1);                    
                    DatabaseUtils.AddInputParameter(cmd, "City", SqlDbType.VarChar, 50, building.BuildingAddress.City);
                    DatabaseUtils.AddInputParameter(cmd, "StateID", SqlDbType.Int, building.BuildingAddress.StateID);
                    DatabaseUtils.AddInputParameter(cmd, "ZipCode", SqlDbType.VarChar, 10, building.BuildingAddress.ZipCode);

                    conn.Open();
                    return  cmd.ExecuteNonQuery() > 0? true : false;
                }
            }
            catch (Exception exc)
            {
                Logger.Error("UpdateBuilding failed.", exc);
                throw new ExpDatabaseException("UpdateBuilding failed.", exc);
            }     
        }

        /// <summary>
        /// Create Building To Company Relationship
        /// </summary>
        /// <param name="bRelationship"></param>
        public void AddBuildingToCompany(BuildingToCompany bRelationship)
        {
            try
            {
                Logger.DebugFormat("AddBuildingToCompany. bRelationship: {0}", bRelationship.ToString());

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_AddBuildingToCompany]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.VarChar, 256, bRelationship.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "CompanyID", SqlDbType.Int, bRelationship.CompanyID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("AddBuildingToCompany failed.", exc);
            }
        }

        private Building Building(IDataReader reader)
        {
            Building building = Mapper.DynamicMap<IDataReader, Building>(reader);

            Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
            building.BuildingAddress = address;

            UserProfile owner = Mapper.DynamicMap<IDataReader, UserProfile>(reader);
            building.Owner = owner;
            return building;
        }

        
    }
}
