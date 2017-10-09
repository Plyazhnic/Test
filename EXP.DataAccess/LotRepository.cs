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
using AutoMapper;
using EXP.Core.Util;

namespace EXP.DataAccess
{
    public class LotRepository : BaseRepository, ILotRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        /// <summary>
        /// Create Lot
        /// </summary>
        /// <param name="lot"></param>
        public int CreateLot(Lot lot)
        {
            int lotId = 0;
            try
            {
                Logger.DebugFormat("CreateLot. lot: {0}", lot.ToString());
                                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotName", SqlDbType.VarChar, 10, lot.LotName);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingOperatorID", SqlDbType.Int, lot.ParkingOperatorID);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingManagerID", SqlDbType.Int, lot.ParkingManagerID);
                    DatabaseUtils.AddInputParameter(cmd, "LotNumber", SqlDbType.VarChar, 10, lot.LotNumber);
                    DatabaseUtils.AddInputParameter(cmd, "LotLevel", SqlDbType.VarChar, 10, lot.LotLevel);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, lot.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "IsBuilding", SqlDbType.Bit, lot.IsBuilding);
                    DatabaseUtils.AddInputParameter(cmd, "MailingAddressID", SqlDbType.Int, lot.MailingAddressID);
                    DatabaseUtils.AddInputParameter(cmd, "ReservedPrice", SqlDbType.Int, lot.ReservedPrice);
                    DatabaseUtils.AddInputParameter(cmd, "UnReservedPrice", SqlDbType.Int, lot.UnReservedPrice);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneNumber", SqlDbType.VarChar, 20, lot.PhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "FaxNumber", SqlDbType.VarChar, 20, lot.FaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingPhoneNumber", SqlDbType.VarChar, 20, lot.MailingPhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingFaxNumber", SqlDbType.VarChar, 20, lot.MailingFaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, lot.IsActive);
                    DatabaseUtils.AddInputParameter(cmd, "Comments", SqlDbType.VarChar, 500, lot.Comments);
                    DatabaseUtils.AddInputParameter(cmd, "hasMultipleBuildings", SqlDbType.VarChar, 10, lot.hasMultipleBuildings);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveFrom", SqlDbType.DateTime, lot.EffectiveFrom);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveTo", SqlDbType.DateTime, lot.EffectiveTo);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewLotID", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lotId = (int)param.Value;
                }
                return lotId;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateLot failed.", exc);
            }
        }

        /// <summary>
        /// Get lot by id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        public Lot GetLot(int? lotID)
        {
            Lot lot = new Lot();                       
            try
            {
                Logger.DebugFormat("GetLot. lotID: {0}", lotID);            
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, lotID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lot = Lot(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetLot failed.", exc);
            }
            return lot;
        }

        /// <summary>
        /// get list of lots
        /// </summary>
        /// <returns></returns>
        public List<Lot> ListLots()
        {
            List<Lot> list = new List<Lot>();                     
            try
            {
                Logger.DebugFormat("ListLots.");                        
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListLots]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lot lot = Mapper.DynamicMap<IDataReader, Lot>(reader);
                            list.Add(lot);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListLots failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// get list of lots for building
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public List<Lot> ListLotsForBuilding(int buildingId)
        {
            List<Lot> list = new List<Lot>();  
            try
            {
                Logger.DebugFormat("ListLotsForBuilding. buildingId: {0}", buildingId);                         
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListLotsForBuilding]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                
                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.Int, buildingId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lot lot = Mapper.DynamicMap<IDataReader, Lot>(reader);
                            list.Add(lot);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListLotsForBuilding failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// List of lots for page
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Lot> ListLotsForPage(TableList listParam)
        {
            List<Lot> list = new List<Lot>();                                   
            try
            {
                Logger.DebugFormat("ListLotsForPage. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListLotsForPage]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "BuildingId", SqlDbType.Int, listParam.BuildingID);
                
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lot lot = Lot(reader);
                        list.Add(lot);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListLotsForPage failed.", exc);
            }
            return list;
        }

        private Lot Lot(IDataReader reader)
        {
            Lot lot = new Lot();
            try
            {
                lot = Mapper.DynamicMap<IDataReader, Lot>(reader);
                Building building = Mapper.DynamicMap<IDataReader, Building>(reader);
                Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
                zState state = Mapper.DynamicMap<IDataReader, zState>(reader);
                address.State = state;

                lot.Address = address;
                lot.Building = building;
            }
            catch (Exception)
            {
            }
            return lot;
        }

        /// <summary>
        /// get count of lots in DB
        /// </summary>
        /// <returns></returns>
        public int GetLotsCount(TableList listParam)
        {
            int value;                                   
            try
            {
                Logger.DebugFormat("GetLotsCount. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetLotsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "@BuildingId", SqlDbType.Int, listParam.BuildingID);
                
                    conn.Open();
                    value =(int)cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetLotsCount failed.", exc);
            }
            return value;
        }

        /// <summary>
        /// delete lot by id
        /// </summary>
        /// <param name="lotId"></param>
        public void DeleteLot(int lotId)
        {
            try
            {
                Logger.DebugFormat("DeleteLot. lotId: {0}", lotId);                              
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, lotId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteLot failed.", exc);
            }
        }

        /// <summary>
        /// Update Lot
        /// </summary>
        /// <param name="lot"></param>
        public void UpdateLot(Lot lot)
        {
            try
            {
                Logger.DebugFormat("UpdateLot. lot: {0}", lot.ToString());                                             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, lot.LotID);
                    DatabaseUtils.AddInputParameter(cmd, "LotName", SqlDbType.VarChar, 10, lot.LotName);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingOperatorID", SqlDbType.Int, lot.ParkingOperatorID);
                    DatabaseUtils.AddInputParameter(cmd, "ParkingManagerID", SqlDbType.Int, lot.ParkingManagerID);
                    DatabaseUtils.AddInputParameter(cmd, "LotNumber", SqlDbType.VarChar, 10, lot.LotNumber);
                    DatabaseUtils.AddInputParameter(cmd, "LotLevel", SqlDbType.VarChar, 10, lot.LotLevel);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, lot.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "IsBuilding", SqlDbType.Bit, lot.IsBuilding);
                    DatabaseUtils.AddInputParameter(cmd, "MailingAddressID", SqlDbType.Int, lot.MailingAddressID);
                    DatabaseUtils.AddInputParameter(cmd, "ReservedPrice", SqlDbType.Int, lot.ReservedPrice);
                    DatabaseUtils.AddInputParameter(cmd, "UnReservedPrice", SqlDbType.Int, lot.UnReservedPrice);
                    DatabaseUtils.AddInputParameter(cmd, "PhoneNumber", SqlDbType.VarChar, 20, lot.PhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "FaxNumber", SqlDbType.VarChar, 20, lot.FaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingPhoneNumber", SqlDbType.VarChar, 20, lot.MailingPhoneNumber);
                    DatabaseUtils.AddInputParameter(cmd, "MailingFaxNumber", SqlDbType.VarChar, 20, lot.MailingFaxNumber);
                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, lot.IsActive);
                    DatabaseUtils.AddInputParameter(cmd, "Comments", SqlDbType.VarChar, 500, lot.Comments);
                    DatabaseUtils.AddInputParameter(cmd, "hasMultipleBuildings", SqlDbType.VarChar, 10, lot.hasMultipleBuildings);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveFrom", SqlDbType.DateTime, lot.EffectiveFrom);
                    DatabaseUtils.AddInputParameter(cmd, "EffectiveTo", SqlDbType.DateTime, lot.EffectiveTo);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateLot failed.", exc);
            }
        }

        //TODO: DELETE
        public Building GetBuildingAddress(int LotID)
        {
            Building building = null; 
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingAddressByLotID]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                DatabaseUtils.AddInputParameter(cmd, "LotId", SqlDbType.Int, LotID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    building = Mapper.DynamicMap<IDataReader, Building>(reader);

                    Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
                    zState zState = Mapper.DynamicMap<IDataReader, zState>(reader);
                    address.State = zState;

                    building.BuildingAddress = address;
                    break;
                }
            }
            return building;
        }

        /// <summary>
        /// Search Lot
        /// </summary>
        /// <param name="LotName"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="ZipCode"></param>
        /// <param name="stateID"></param>
        /// <returns></returns>
        public List<LotWide> SearchLot(SearchLot searchParam)
        {
            List<LotWide> list = new List<LotWide>();
            try
            {
                Logger.DebugFormat("SearchLot. searchParam: {0}", searchParam.ToString()); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_SearchLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "@LotName", SqlDbType.NVarChar, searchParam.LotName);
                    DatabaseUtils.AddInputParameter(cmd, "@City", SqlDbType.NVarChar, searchParam.City);
                    DatabaseUtils.AddInputParameter(cmd, "@ZipCode", SqlDbType.NVarChar, searchParam.ZipCode);
                    DatabaseUtils.AddInputParameter(cmd, "@StateID", SqlDbType.Int, searchParam.StateID);        

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        LotWide lot = Mapper.DynamicMap<IDataReader, LotWide>(reader);
                        list.Add(lot);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("SearchLot failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// Create Building To Lot Relationship
        /// </summary>
        /// <param name="bRelationship"></param>
        public void AddBuildingToLot(BuildingToLot bRelationship)
        {
            try
            {
                Logger.DebugFormat("AddBuildingToLot. bRelationship: {0}", bRelationship.ToString());

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_AddBuildingToLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.VarChar, 256, bRelationship.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, bRelationship.LotID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("AddBuildingToLot failed.", exc);
            }
        }

        /// <summary>
        /// get list of lots to building
        /// </summary>
        /// <returns></returns>
        public List<BuildingToLot> GetListLots()
        {
            List<BuildingToLot> list = new List<BuildingToLot>();
            try
            {
                Logger.DebugFormat("GetListLots.");
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListBuildingToLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BuildingToLot lot = BuildingToLot(reader);
                            list.Add(lot);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetListLots failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// delete  BuildingToLot Entry in db
        /// </summary>
        /// <param name="bRelationship"></param>
        public void DeleteBuildingToLotEntry(BuildingToLot bRelationship)
        {
            try
            {
                Logger.DebugFormat("DeleteBuildingToLotEntry. bRelationship: {0}", bRelationship.ToString());

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteBuildingToLotEntry]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BuildingID", SqlDbType.VarChar, 256, bRelationship.BuildingID);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, bRelationship.LotID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteBuildingToLotEntry failed.", exc);
            }
        }

        /// <summary>
        /// Get building to lot record by id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        public BuildingToLot GetBuildingToLot(int buildingToLotID)
        {
            BuildingToLot bToL = new BuildingToLot();
            try
            {
                Logger.DebugFormat("GetBuildingToLot. buildingToLotID: {0}", buildingToLotID);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetBuildingToLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BuildingToLotID", SqlDbType.Int, buildingToLotID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bToL = BuildingToLot(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuildingToLot failed.", exc);
            }
            return bToL;
        }

        private BuildingToLot BuildingToLot(IDataReader reader)
        {
            BuildingToLot buildingToLot = new BuildingToLot();

            buildingToLot = Mapper.DynamicMap<IDataReader, BuildingToLot>(reader);
            Lot lot = Mapper.DynamicMap<IDataReader, Lot>(reader);
            Building building = Mapper.DynamicMap<IDataReader, Building>(reader);
            buildingToLot.Lot = lot;
            buildingToLot.Building = building;
            return buildingToLot;
        }
    }
}


