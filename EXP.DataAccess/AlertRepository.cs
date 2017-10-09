using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using AutoMapper;
using EXP.Core;
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Core.Util;
using EXP.Entity;
using EXP.Core.BaseClasses;
using System.Configuration;
using EXP.Entity.Enumerations;

namespace EXP.DataAccess
{
    public class AlertRepository : BaseRepository, IAlertRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["exp.connection"].ConnectionString;
               
        /// <summary>
        /// Create alert for user 
        /// </summary>
        /// <param name="alert"></param>
        public void CreateAlert(Alert alert)
        {
            try
            {
                Logger.DebugFormat("CreateAlert. alert: {0}", alert.ToString());
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_CreateAlert]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "AlertTypeID", SqlDbType.Int, alert.AlertTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, alert.UserProfileID);
                    DatabaseUtils.AddInputParameter(cmd, "EntityTypeID", SqlDbType.Int, alert.EntityTypeID);
                    DatabaseUtils.AddInputParameter(cmd, "EntityID", SqlDbType.Int, alert.EntityID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("CreateAlert failed.", exc);
            }
        }

        /// <summary>
        /// get active alerts for current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Alert> GetActiveAlertsForUser(int userId)
        {
            List<Alert> list = new List<Alert>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetActiveAlertForUser]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;               

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alert alert = Alert(reader);
                            list.Add(alert);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("GetActiveAlertsForUser failed.", exc);
            }
            return list;
        }

        /// <summary>
        /// get alert for user by entity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public List<Alert> GetAlertsForUserByEntity(int userId, AlertEntityTypeEnum entityType, int entityId)
        {
            List<Alert> list = new List<Alert>();
            try
            {
                Logger.DebugFormat("GetAlertsForUserByEntity. userId: {0}, entityType: {1}, entityId: {2}", userId, entityType.ToString(), entityId);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_GetAlertForUserByEntity]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "UserProfileID", SqlDbType.Int, userId);
                    DatabaseUtils.AddInputParameter(cmd, "EntityTypeID", SqlDbType.Int, (int)entityType);
                    DatabaseUtils.AddInputParameter(cmd, "EntityID", SqlDbType.Int, entityId);

                    conn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Alert alert = Alert(reader);
                            list.Add(alert);
                        }
                    }
                }
            }
            catch (Exception exc)
            {   //TODO: Rework
              //  throw new ExpDatabaseException("GetAlertsForUserByEntity failed.", exc);
            }
            return list;
        }

        private Alert Alert(IDataReader reader)
        {
            Alert alert = new Alert();
            try
            {
                alert = Mapper.DynamicMap<IDataReader, Alert>(reader);

                alert.AlertType = AlertType(reader);

                alert.AlertText = GetAlertText(alert);
            }
            catch (Exception)
            {
            }
            

            return alert;
        }

        private static AlertType AlertType(IDataReader reader)
        {
            Mapper.CreateMap<IDataReader, AlertType>()
                .ForMember(m => m.AlertType1, opt => opt.MapFrom(r => r["AlertType"]));
            AlertType alertType = Mapper.DynamicMap<IDataReader, AlertType>(reader);

            return alertType;
        }

        /// <summary>
        /// Activate alert
        /// </summary>
        /// <param name="alertId"></param>
        public void ActivateAlert(int alertId)
        {
            try
            {
                Logger.DebugFormat("ActivateAlert. alertId: {0}", alertId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_ActivateAlert]", conn);

                    cmd.CommandType = CommandType.StoredProcedure;           
                    DatabaseUtils.AddInputParameter(cmd, "AlertID", SqlDbType.Int, alertId);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("ActivateAlert failed.", exc);
            }
        }

        /// <summary>
        /// Deactivate alert
        /// </summary>
        /// <param name="alertId"></param>
        public void DeactivateAlert(int alertId)
        {
            try
            {
                Logger.DebugFormat("DeactivateAlert. alertId: {0}", alertId);
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[usp_DeactivateAlert]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;           

                    DatabaseUtils.AddInputParameter(cmd, "AlertID", SqlDbType.Int, alertId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                throw new ExpDatabaseException("DeactivateAlert failed.", exc);
            }
        }

        private string GetAlertText(Alert alert)
        {
            const string PersonalAddressAlertMessage = "You are not fully specify personal information";
            const string VehicleAlertMessage = "Please fill in all fields";
            const string EmptyVehicleAlertMessage = "Please add vehicle to profile";
            const string ParkingInformationAlertMessage = "You are not fully specify parking information";
            const string EmptyPaymentAlertMessage = "No payment details specified";
            const string PaymentAlertMessage = "Payment filled incompletely";
            const string CreditCardAlertMessage = "Сredit card number is incorrect";

            switch (alert.EntityTypeID)
            {
                case (int)AlertEntityTypeEnum.PersonInfo:
                    return PersonalAddressAlertMessage;
                case (int)AlertEntityTypeEnum.EmptyVehicle:
                    return EmptyVehicleAlertMessage;
                case (int)AlertEntityTypeEnum.Vehicle:
                    VehicleRepository VehicleRepository = new VehicleRepository(); 
                    Vehicle vehicle = VehicleRepository.GetVehicle(alert.EntityID);
                    if (vehicle != null)
                    {
                        if (!vehicle.VehicleMakeID.HasValue && !vehicle.VehicleModelID.HasValue)
                        {
                            return string.Format("#{0} Vehicle-{1}", vehicle.VehicleID, VehicleAlertMessage);
                        }
                        return string.Format("#{0} {1} {2}-{3}",
                            vehicle.VehicleID,
                            vehicle.VehicleMakeID.HasValue ? vehicle.VehicleMake.VehicleMake1 : string.Empty,
                            vehicle.VehicleModelID.HasValue ? vehicle.VehicleModel.VehicleModel1 : string.Empty,
                            VehicleAlertMessage);
                    }
                    else
                    {
                        throw new ArgumentException("Cann't find vehicle for alert.");
                    }
                case (int)AlertEntityTypeEnum.ParkingInformation:
                    return ParkingInformationAlertMessage;
                case (int)AlertEntityTypeEnum.EmptyPayment:
                    return EmptyPaymentAlertMessage;
                case (int)AlertEntityTypeEnum.Payment:
                    return PaymentAlertMessage;
                case (int)AlertEntityTypeEnum.CreditCard:
                    return CreditCardAlertMessage;
                default:
                    throw new ArgumentException("Wrong alert type.");
            }
        }
    }
}
