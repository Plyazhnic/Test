using System;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using EXP.Core;
using EXP.Core.BaseClasses;
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Core.Util;
using EXP.Entity;
using System.Configuration;

namespace EXP.DataAccess
{
    public class EmailRepository : BaseRepository, IEmailRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;

        public void CreateEmail(Email email)
        {
            try
            {
                Logger.DebugFormat("CreateEmail. email: {0}", email.ToString());                                               
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateEmail]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "Subject", SqlDbType.NVarChar, email.Subject);
                    DatabaseUtils.AddInputParameter(cmd, "Body", SqlDbType.NVarChar, email.Body);
                    DatabaseUtils.AddInputParameter(cmd, "Destination", SqlDbType.NVarChar, email.Destination);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateEmail failed.", exc);
            }
        }

        public Email GetNextEmail()
        {
            Email email = null;
            try
            {
                Logger.DebugFormat("GetNextEmail.");                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetNextEmail]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            email = Mapper.DynamicMap<IDataReader, Email>(reader);
                        }
                    }
                }
                return email;
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetNextEmail failed.", exc);
            }
        }

        public void SetEmailSent(Email email)
        {
            try
            {
                Logger.DebugFormat("SetEmailSent. email: {0}", email.ToString());
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_SetEmailSent]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "EmailID", SqlDbType.Int, email.EmailID);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("SetEmailSent failed.", exc);
            }
        }
    }
}