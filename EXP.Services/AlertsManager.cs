using System;
using System.Collections.Generic;
using System.Linq;
using EXP.Entity;
using EXP.Entity.Enumerations;
using EXP.Core;
using EXP.Core.Interface;
using EXP.DataAccess;

namespace EXP.Services
{
    public class AlertsManager
    {
        private readonly IAlertRepository _alertRepo;

        public AlertsManager()
        {
            _alertRepo = new AlertRepository();
        }
        
        public void CreateParkingInformationAlert(int userProfileID)
        {
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.ParkingInformation, 0);
            if (alerts.Count == 0)
            {
                Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.ParkingInformation, EntityID = 0 };
                _alertRepo.CreateAlert(newAlert);
            }
        }

        public int DeleteParkingInformationAlert(int userProfileID)
        {
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.ParkingInformation, 0);
            if (alerts.Count != 0)
            {
                _alertRepo.DeactivateAlert(alerts[0].AlertID);
                return alerts[0].AlertID;
            }
            return 0;
        }

        public void CreateEmptyPaymentAlert(int userProfileID)
        {
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.EmptyPayment, 0);
            if (alerts.Count == 0)
            {
                Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2,
                    EntityTypeID = (int)AlertEntityTypeEnum.EmptyPayment, EntityID = 0 };
                _alertRepo.CreateAlert(newAlert);
            }
        }

        public Alert CreateOrDeleteAlertForPayment(Payment payment, int userProfileID)
        {
            Alert alert = null;
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.Payment, payment.PaymentID);
            if (alerts != null)
            {
                alert = alerts.LastOrDefault();
            }

            //If there are empty fields
            if (!payment.isCreditCard && 
                (string.IsNullOrEmpty(payment.OnlineCheck.NameOnAccount) || string.IsNullOrEmpty(payment.OnlineCheck.BankName) ||
                string.IsNullOrEmpty(payment.OnlineCheck.RoutingNumber) || string.IsNullOrEmpty(payment.OnlineCheck.CheckingAccountNumber)))
            {
                if (alert == null || !alert.isActive)
                {
                    Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.Payment, EntityID = payment.PaymentID};
                    _alertRepo.CreateAlert(newAlert);
                }
                alert = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.Payment, payment.PaymentID).LastOrDefault();//get created entry
            }
            else if (payment.isCreditCard &&
                (string.IsNullOrEmpty(payment.CreditCard.CHFirstName) || string.IsNullOrEmpty(payment.CreditCard.CHLastName) ||
                string.IsNullOrEmpty(payment.CreditCard.CardNumber) || string.IsNullOrEmpty(payment.CreditCard.CVV) ||
                !payment.CreditCard.AddressID.HasValue))
            {
                if (alert == null || !alert.isActive)
                {
                    Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.Payment, EntityID = payment.PaymentID };
                    _alertRepo.CreateAlert(newAlert);
                }
                alert = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.Payment, payment.PaymentID).LastOrDefault();//get created entry
            }
            else
            {
                if (alert != null && alert.isActive)
                {
                    _alertRepo.DeactivateAlert(alert.AlertID);
                    alert.isActive = false;
                }
            }
            return alert;
        }

        public void CreateEmptyVehicleAlert(int userProfileID)
        {
            List<Alert> alertsForEmptyVehicle = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.EmptyVehicle, 0);
            if (alertsForEmptyVehicle.Count == 0)
            {
                Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2,
                    EntityTypeID = (int)AlertEntityTypeEnum.EmptyVehicle, EntityID = 0 };
                _alertRepo.CreateAlert(newAlert);
            }
        }

        public int DeleteEmptyVehicleAlert(int userProfileID)
        {
            List<Alert> alertsForEmptyVehicle = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.EmptyVehicle, 0);
            if (alertsForEmptyVehicle.Count != 0)
            {
                _alertRepo.DeactivateAlert(alertsForEmptyVehicle[0].AlertID);
                return alertsForEmptyVehicle[0].AlertID;
            }
            return 0;
        }

        public Alert CreateOrDeleteAlertForVehicle(Vehicle vehicle, int userProfileID)
        {
            Alert alert = null;
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.Vehicle, vehicle.VehicleID);
            if (alerts.Count != 0)
            {
                alert = alerts.LastOrDefault();
            }

            //If there are empty fields
            if (vehicle.VehicleMakeID == null || vehicle.VehicleModelID == null || vehicle.Year == null ||
                String.IsNullOrEmpty(vehicle.Color) || String.IsNullOrEmpty(vehicle.LicensePlateNumber) ||
                String.IsNullOrEmpty(vehicle.PermitNumber))
            {
                if (alert == null || !alert.isActive)
                {
                    Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.Vehicle, EntityID = vehicle.VehicleID };
                    _alertRepo.CreateAlert(newAlert);
                }
                alert = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.Vehicle, vehicle.VehicleID).LastOrDefault();//get created entry
            }
            else
            {
                if (alert != null && alert.isActive)
                {
                    _alertRepo.DeactivateAlert(alert.AlertID);
                    alert.isActive = false;
                }
            }
            return alert;
        }

        public Alert CreateOrDeleteAlertForPersonInfo(string city, string address1, int? stateId, string zip, int userProfileID)
        {
            Alert alert = null;
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.PersonInfo, userProfileID);
            if (alerts != null)
            {
                alert = alerts.LastOrDefault();
            }
            //If there are empty fields
            if (String.IsNullOrEmpty(city) || String.IsNullOrEmpty(address1) || stateId == null || String.IsNullOrEmpty(zip))
            {
                if (alert == null || !alert.isActive)
                {
                    Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.PersonInfo, EntityID = userProfileID };
                    _alertRepo.CreateAlert(newAlert);
                }
                alert = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.PersonInfo, userProfileID).LastOrDefault();//get created entry
            }
            else
            {
                if (alert != null && alert.isActive)
                {
                    _alertRepo.DeactivateAlert(alert.AlertID);
                    alert.isActive = false;
                }
            }
            return alert;
        }

        public Alert CreateCreditCardAlert(int userProfileID)
        {
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.CreditCard, 0);
            if (alerts.Count == 0)
            {
                Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.CreditCard, EntityID = 0 };
                _alertRepo.CreateAlert(newAlert);
                newAlert = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.CreditCard, 0).LastOrDefault();
                return newAlert;
            }
            else
            {
                if (!alerts[0].isActive)
                {
                    _alertRepo.ActivateAlert(alerts[0].AlertID);
                    alerts[0].isActive = true;
                }
                return alerts[0];
            }
        }

        public Alert DeleteCreditCardAlert(int userProfileID)
        {
            List<Alert> alerts = _alertRepo.GetAlertsForUserByEntity(userProfileID, AlertEntityTypeEnum.CreditCard, 0);
            if (alerts.Count != 0 && alerts[0].isActive)
            {
                Alert newAlert = new Alert { UserProfileID = userProfileID, AlertTypeID = 2, EntityTypeID = (int)AlertEntityTypeEnum.CreditCard, EntityID = 0 };
                _alertRepo.DeactivateAlert(alerts[0].AlertID);
                alerts[0].isActive = false;
            }
            return alerts[0];
        }
    }
}