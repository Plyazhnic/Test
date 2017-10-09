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
    public class ValidationBookRepository : BaseRepository, IValidationBookRepository 
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;            
        /// <summary>
        /// Get list validation books for page
        /// </summary>
        /// <returns></returns>
        public List<ValidationBook> ListValidationBooksForPage(TableList listParam)
        {
            List<ValidationBook> list = new List<ValidationBook>();           
            try
            {
                Logger.DebugFormat("ListValidationBooksForPage. listParam: {0}", listParam.ToString());             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ListValidationBooks]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Start", SqlDbType.Int, listParam.Start);
                    DatabaseUtils.AddInputParameter(cmd, "End", SqlDbType.Int, listParam.End);
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "LotId", SqlDbType.Int, listParam.LotID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {                   
                        ValidationBook book = Mapper.DynamicMap<IDataReader, ValidationBook>(reader);

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
                throw new ExpDatabaseException("ListValidationBooksForPage failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// Get validation books count
        /// </summary>
        /// <returns></returns>
        public int GetValidationBooksCount(TableList listParam)
        {
            object value;
            try
            {
                Logger.DebugFormat("GetValidationBooksCount. listParam: {0}", listParam.ToString());             
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetValidationBooksCount]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DatabaseUtils.AddInputParameter(cmd, "Search", SqlDbType.VarChar, 50, listParam.SearchString);
                    DatabaseUtils.AddInputParameter(cmd, "@LotId", SqlDbType.Int, listParam.LotID);
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetValidationBooksCount failed.", exc);
            }
            return (int)value;
        }
        /// <summary>
        /// Get Validation Book By Id
        /// </summary>
        /// <param name="LotID"></param>
        /// <returns></returns>
        public ValidationBook GetValidationBookById(int BookID)
        {
            ValidationBook ValidationBook = null;                            
            try
            {
                Logger.DebugFormat("GetValidationBookById. BookID: {0}", BookID);   
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetValidationBookById]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BookID", SqlDbType.Int, BookID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //Mapper.CreateMap<IDataReader, ValidationBook>()
                        //    .ForMember(b => b.BookName, opt => opt.MapFrom(r => r["ValidationBook"]));
                        ValidationBook = Mapper.DynamicMap<IDataReader, ValidationBook>(reader);
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetValidationBookById failed.", exc);
            }
            return ValidationBook;      
        }

        public bool CreateValidationBook(ValidationBook ValidationBook)
        {
            object value;                      
            try
            {
                Logger.DebugFormat("CreateValidationBook. ValidationBook: {0}", ValidationBook.ToString()); 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateValidationBook]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, ValidationBook.IsActive);
                    DatabaseUtils.AddInputParameter(cmd, "BookName", SqlDbType.VarChar, 50, ValidationBook.BookName);
                    DatabaseUtils.AddInputParameter(cmd, "TicketCount", SqlDbType.Int, ValidationBook.TicketCount);
                    DatabaseUtils.AddInputParameter(cmd, "Rate", SqlDbType.Decimal, ValidationBook.Rate);
                    DatabaseUtils.AddInputParameter(cmd, "LotID", SqlDbType.Int, ValidationBook.LotID );
                
                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateValidationBook failed.", exc);
            }
            return (bool)value;
        }

        public bool UpdateValidationBook(ValidationBook ValidationBook)
        {
            object value;                      
            try
            {
                Logger.DebugFormat("UpdateValidationBook. ValidationBook: {0}", ValidationBook.ToString());
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_UpdateValidationBook]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "IsActive", SqlDbType.Bit, ValidationBook.IsActive);
                    DatabaseUtils.AddInputParameter(cmd, "BookName", SqlDbType.VarChar, 50, ValidationBook.BookName);
                    DatabaseUtils.AddInputParameter(cmd, "TicketCount", SqlDbType.Int, ValidationBook.TicketCount);
                    DatabaseUtils.AddInputParameter(cmd, "Rate", SqlDbType.Decimal, ValidationBook.Rate);
                    DatabaseUtils.AddInputParameter(cmd, "BookID", SqlDbType.Int, ValidationBook.BookID);

                    conn.Open();
                    value = (bool)(cmd.ExecuteNonQuery() > 0 ? true : false);
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateValidationBook failed.", exc);
            }
            return (bool)value;
        }

        public void DeleteValidationBook(int ValidationBookID)
        {
            try
            {
                Logger.DebugFormat("DeleteValidationBook. ValidationBookID: {0}", ValidationBookID);
                 
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeleteValidationBook]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "BookID", SqlDbType.Int, ValidationBookID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeleteValidationBook failed.", exc);
            }
        }
    }
}
