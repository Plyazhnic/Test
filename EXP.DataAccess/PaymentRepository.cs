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
    public class PaymentRepository : BaseRepository, IPaymentRepository 
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
        /// <summary>
        /// Create payment credit card for user
        /// </summary>
        /// <param name="newPayment"></param>
        public void CreatePaymentCreditCard(Payment newPayment)
        {
            try
            {
                Logger.DebugFormat("CreatePaymentCreditCard. newPayment: {0}, creaditCard: {1}", newPayment.ToString(), newPayment.CreditCard.ToString());                               
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreatePaymentCreditCard]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CHFirstName", SqlDbType.VarChar, 50, newPayment.CreditCard.CHFirstName);
                    DatabaseUtils.AddInputParameter(cmd, "CHLastName", SqlDbType.VarChar, 50, newPayment.CreditCard.CHLastName);
                    DatabaseUtils.AddInputParameter(cmd, "CardNumber", SqlDbType.VarChar, 20, newPayment.CreditCard.CardNumber);
                    DatabaseUtils.AddInputParameter(cmd, "ExpDateMount", SqlDbType.VarChar, 2, newPayment.CreditCard.ExpDateMount);
                    DatabaseUtils.AddInputParameter(cmd, "ExpDateYear", SqlDbType.VarChar, 4, newPayment.CreditCard.ExpDateYear);
                    DatabaseUtils.AddInputParameter(cmd, "CVV", SqlDbType.VarChar, 4, newPayment.CreditCard.CVV);
                    DatabaseUtils.AddInputParameter(cmd, "AutoPay", SqlDbType.Bit, newPayment.CreditCard.AutoPay);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, newPayment.CreditCard.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "isHome", SqlDbType.Bit, newPayment.CreditCard.isHome);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, newPayment.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "isCreditCard", SqlDbType.Bit, newPayment.isCreditCard);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreatePaymentCreditCard failed.", exc);
            }
        }

        /// <summary>
        /// Create payment online check for user
        /// </summary>
        /// <param name="newPayment"></param>
        public void CreatePaymentOnlineCheck(Payment newPayment)
        {
            try
            {
                Logger.DebugFormat("CreatePaymentOnlineCheck. newPayment: {0}, onlineCheck: {1}", newPayment.ToString(), newPayment.OnlineCheck.ToString());                              
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_CreatePaymentOnlineCheck]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "NameOnAccount", SqlDbType.VarChar, 256, newPayment.OnlineCheck.NameOnAccount);
                    DatabaseUtils.AddInputParameter(cmd, "OnlineCheckingTypeID", SqlDbType.Int, newPayment.OnlineCheck.OnlineCheckingTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "BankName", SqlDbType.VarChar, 128, newPayment.OnlineCheck.BankName);
                    DatabaseUtils.AddInputParameter(cmd, "RoutingNumber", SqlDbType.VarChar, 20, newPayment.OnlineCheck.RoutingNumber);
                    DatabaseUtils.AddInputParameter(cmd, "CheckingAccountNumber", SqlDbType.VarChar, 50, newPayment.OnlineCheck.CheckingAccountNumber);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, newPayment.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "isCreditCard", SqlDbType.Bit, newPayment.isCreditCard);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreatePaymentOnlineCheck failed.", exc);
            }
        }

        /// <summary>
        /// Get payment for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Payment GetPaymentForUser(int userId)
        {
            Payment payment = null;                      
            try
            {
                Logger.DebugFormat("GetPaymentForUser. userId: {0}", userId);              
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_GetPaymentForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            payment = Payment(reader);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetPaymentForUser failed.", exc);
            }
            return payment;
        }

        /// <summary>
        /// Update credit card data 
        /// </summary>
        /// <param name="creditCard"></param>
        public void UpdateCreditCard(CreditCard creditCard)
        {
            try
            {
                Logger.DebugFormat("UpdateCreditCard. creditCard: {0}", creditCard.ToString());                                      
                                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdateCreditCard]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "CreditCardID", SqlDbType.Int, creditCard.CreditCardID);
                    DatabaseUtils.AddInputParameter(cmd, "CHFirstName", SqlDbType.VarChar, 50, creditCard.CHFirstName);
                    DatabaseUtils.AddInputParameter(cmd, "CHLastName", SqlDbType.VarChar, 50, creditCard.CHLastName);
                    DatabaseUtils.AddInputParameter(cmd, "CardNumber", SqlDbType.VarChar, 20, creditCard.CardNumber);
                    DatabaseUtils.AddInputParameter(cmd, "ExpDateMount", SqlDbType.VarChar, 2, creditCard.ExpDateMount);
                    DatabaseUtils.AddInputParameter(cmd, "ExpDateYear", SqlDbType.VarChar, 4, creditCard.ExpDateYear);
                    DatabaseUtils.AddInputParameter(cmd, "CVV", SqlDbType.VarChar, 4, creditCard.CVV);
                    DatabaseUtils.AddInputParameter(cmd, "AutoPay", SqlDbType.Bit, creditCard.AutoPay);
                    DatabaseUtils.AddInputParameter(cmd, "AddressID", SqlDbType.Int, creditCard.AddressID);
                    DatabaseUtils.AddInputParameter(cmd, "isHome", SqlDbType.Int, creditCard.isHome);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateCreditCard failed.", exc);
            }
        }

        /// <summary>
        /// Update online check data 
        /// </summary>
        /// <param name="onlineCheck"></param>
        public void UpdateOnlineCheck(OnlineCheck onlineCheck)
        {
            try
            {
                Logger.DebugFormat("UpdateOnlineCheck. onlineCheck: {0}", onlineCheck.ToString());                                               

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[UserProfile].[usp_UpdateOnlineCheck]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DatabaseUtils.AddInputParameter(cmd, "OnlineCheckID", SqlDbType.Int, onlineCheck.OnlineCheckID);
                    DatabaseUtils.AddInputParameter(cmd, "NameOnAccount", SqlDbType.VarChar, 256, onlineCheck.NameOnAccount);
                    DatabaseUtils.AddInputParameter(cmd, "OnlineCheckingTypeID", SqlDbType.Int, onlineCheck.OnlineCheckingTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "BankName", SqlDbType.VarChar, 128, onlineCheck.BankName);
                    DatabaseUtils.AddInputParameter(cmd, "RoutingNumber", SqlDbType.VarChar, 20, onlineCheck.RoutingNumber);
                    DatabaseUtils.AddInputParameter(cmd, "CheckingAccountNumber", SqlDbType.VarChar, 50, onlineCheck.CheckingAccountNumber);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("UpdateOnlineCheck failed.", exc);
            }
        }

        private Payment Payment(IDataReader reader)
        {
            Payment payment = Mapper.DynamicMap<IDataReader, Payment>(reader);

            CreditCard creditCard = Mapper.DynamicMap<IDataReader, CreditCard>(reader);
            try
            {
                Address address = Mapper.DynamicMap<IDataReader, Address>(reader);
                creditCard.Address = address;
            }
            catch (Exception)
            {
            }
            
            OnlineCheck onlineCheck = Mapper.DynamicMap<IDataReader, OnlineCheck>(reader);

            if (creditCard != null && creditCard.CreditCardID != 0)
            {
                payment.CreditCard = creditCard;
            }
            if (onlineCheck != null && onlineCheck.OnlineCheckID != 0)
            {
                payment.OnlineCheck = onlineCheck;
            }

            return payment;
        }
    }
}
