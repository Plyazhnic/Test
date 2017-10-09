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
    public class AddressRepository : BaseRepository, IAddressRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;               
        /// <summary>
        /// Create address
        /// </summary>
        /// <param name="newAddress"></param>
        /// <returns></returns>
        public int CreateAddress(Address newAddress)
        {
            try
            {
                Logger.DebugFormat("CreateAddress. newAddress: {0}", newAddress.ToString());
                int addressId = 0;
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreateAddress]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;   

                    DatabaseUtils.AddInputParameter(cmd, "City", SqlDbType.VarChar, 50, newAddress.City);
                    DatabaseUtils.AddInputParameter(cmd, "Address1", SqlDbType.VarChar, 256, newAddress.Address1);
                    DatabaseUtils.AddInputParameter(cmd, "Address2", SqlDbType.VarChar, 256, newAddress.Address2);
                    DatabaseUtils.AddInputParameter(cmd, "StateID", SqlDbType.Int, newAddress.StateID);
                    DatabaseUtils.AddInputParameter(cmd, "ZipCode", SqlDbType.VarChar, 10, newAddress.ZipCode);
                    SqlParameter param = DatabaseUtils.AddOutputParameter(cmd, "NewAddressID", SqlDbType.Int, 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    addressId = (int)param.Value;
                }
                return addressId;

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateAddress failed.", exc);
            }
        }

        /// <summary>
        /// Update address
        /// </summary>
        /// <param name="address"></param>
        public void UpdateAddress(Address address)
        {
            try
            {
                Logger.DebugFormat("UpdateAddress. address: {0}", address.ToString());                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdateAddress]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;   

                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, address.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "City", SqlDbType.VarChar, 50, address.City);
                    DatabaseUtils.AddInputParameter(cmd, "Address1", SqlDbType.VarChar, 256, address.Address1);
                    DatabaseUtils.AddInputParameter(cmd, "Address2", SqlDbType.VarChar, 256, address.Address2);
                    DatabaseUtils.AddInputParameter(cmd, "StateID", SqlDbType.Int, address.StateID);
                    DatabaseUtils.AddInputParameter(cmd, "ZipCode", SqlDbType.VarChar, 10, address.ZipCode);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateAddress failed.", exc);
            }
        }

        /// <summary>
        /// Get address by id
        /// </summary>
        /// <param name="addressID"></param>
        /// <returns></returns>
        public Address GetAddress(int addressID)
        {
            Address address = new Address();
            try
            {
                Logger.DebugFormat("GetAddress. addressId: {0}", addressID);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetAddress]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;   

                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, addressID);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            address =  Address(reader);
                        }
                    }
                }
                return address;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetAddress failed.", exc);
            }
        }

        /// <summary>
        /// Delete address by id
        /// </summary>
        /// <param name="addressId"></param>
        public void DeleteAddress(int addressId)
        {
            try
            {
                Logger.DebugFormat("DeleteAddress. addressId: {0}", addressId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_DeleteAddress]",conn);
                    cmd.CommandType = CommandType.StoredProcedure;   

                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, addressId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteAddress failed.", exc);
            }
        }

        /// <summary>
        /// Get building address by lot id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        public Address GetBuildingAddressByLot(int buildingToLotId)
        {
            Address address = new Address();
            try
            {
                Logger.DebugFormat("GetBuildingAddressByLot. buildingToLotId: {0}", buildingToLotId);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetBuildingAddressByLot]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;   

                    DatabaseUtils.AddInputParameter(cmd, "BuildingToLotID", SqlDbType.Int, buildingToLotId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            address = Mapper.DynamicMap<IDataReader, Address>(reader);
                        }
                    }
                }

                return address;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetBuildingAddressByLot failed.", exc);
            }
        }

        private Address Address(IDataReader reader)
        {
            Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
            zState state = Mapper.DynamicMap<IDataReader, zState>(reader);
            address.State = state;
            return address;
        }
    }
}
