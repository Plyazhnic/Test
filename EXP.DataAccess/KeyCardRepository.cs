using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Interface;
using EXP.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using EXP.Core.Util;
using System;
using EXP.Core.Exceptions;
using System.Configuration;

namespace EXP.DataAccess
{
    public class KeyCardRepository: BaseRepository, IKeyCardRepository 
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        ///  Get List Key Cards For Page
        /// </summary>
        /// <returns></returns>
        public List<KeyCard> ListKeyCardsForPage(TableList listParam)
        {
            List<KeyCard> list = new List<KeyCard>();
            try
            {
                Logger.DebugFormat("ListKeyCardsForPage. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListKeyCards]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "LotId", SqlDbType.Int, listParam.LotID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {


                        KeyCard book = Mapper.DynamicMap<IDataReader, KeyCard>(reader);

                        Lot Lot = Mapper.DynamicMap<IDataReader, Lot>(reader);
                        book.Lot = Lot;

                        Building Building = Mapper.DynamicMap<IDataReader, Building>(reader);
                        book.Building = Building;

                        Address Address = Mapper.DynamicMap<IDataReader, Address>(reader);
                        book.Address = Address;

                        zState zState = Mapper.DynamicMap<IDataReader, zState>(reader);
                        book.zSate = zState;


                        list.Add(book);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ListKeyCardsForPage failed.", exc);
            }
            return list;
        }
        /// <summary>
        /// Get Key Cards Count
        /// </summary>
        /// <param name="LotId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public int GetKeyCardsCount(TableList listParam)
        {
            object value;
            try
            {
                Logger.DebugFormat("GetKeyCardsCount. listParam: {0}", listParam.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetKeyCardsCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "@LotId", SqlDbType.Int, listParam.LotID);
                
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
                return (int)value;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetKeyCardsCount failed.", exc);
            }
        }

        /// <summary>
        /// Get Key Card By Id
        /// </summary>
        /// <param name="Cardid"></param>
        /// <returns></returns>
        public KeyCard GetKeyCardById(int Cardid)
        {
            KeyCard KeyCard = null;
            try
            {
                Logger.DebugFormat("GetKeyCardById. Cardid: {0}", Cardid);
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetKeyCardById]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Cardid", SqlDbType.Int, Cardid);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //Mapper.CreateMap<IDataReader, ValidationBook>()
                        //    .ForMember(b => b.BookName, opt => opt.MapFrom(r => r["ValidationBook"]));
                        KeyCard = Mapper.DynamicMap<IDataReader, KeyCard>(reader);
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetKeyCardById failed.", exc);
            }
            return KeyCard;          
        }

        /// <summary>
        /// Create Key Card
        /// </summary>
        /// <param name="KeyCard"></param>
        /// <returns></returns>
        public bool CreateKeyCard(KeyCard KeyCard)
        {
            object value;
            try
            {
                Logger.DebugFormat("CreateKeyCard. KeyCard: {0}", KeyCard.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateKeyCard]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, KeyCard.IsActive );
                    DatabaseUtils.AddInputParameter(cmd, "KeyCardName", SqlDbType.VarChar, 50, KeyCard.KeyCardName);
                    DatabaseUtils.AddInputParameter(cmd, "Rate", SqlDbType.Decimal, KeyCard.Rate);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, KeyCard.LotID);

                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateKeyCard failed.", exc);
            }
            return (bool)value;
        }

        /// <summary>
        /// Update Key Card
        /// </summary>
        /// <param name="KeyCard"></param>
        /// <returns></returns>
        public bool UpdateKeyCard(KeyCard KeyCard)
        {
            object value;
            try
            {
                Logger.DebugFormat("UpdateKeyCard. KeyCard: {0}", KeyCard.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateKeyCard]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, KeyCard.IsActive);
                    DatabaseUtils.AddInputParameter(cmd, "KeyCardName", SqlDbType.VarChar, 50, KeyCard.KeyCardName);
                    DatabaseUtils.AddInputParameter(cmd, "Rate", SqlDbType.Decimal, KeyCard.Rate);
                    DatabaseUtils.AddInputParameter(cmd, "KeyCardID", SqlDbType.Int, KeyCard.KeyCardID);

                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateKeyCard failed.", exc);
            }
            return (bool)value;    
        }

        public void DeleteKeyCard(int KeyCardId)
        {
            try
            {
                Logger.DebugFormat("DeleteKeyCard. KeyCardID: {0}", KeyCardId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteKeyCard]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CardId", SqlDbType.Int, KeyCardId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteKeyCard failed.", exc);
            }
        }
     }
}
