using EXP.Core.Interface;
using EXP.Entity;
using EXP.Services.Models;
using AutoMapper;
using EXP.DataAccess;
using System.EnterpriseServices;
using System.Web.Security;
using System.Reflection;
using System.Transactions;
using EXP.Core;
using EXP.Entity.Enumerations;
using EXP.Services.Interface;
using System;
using System.Web;
using EXP.Core.Util;
using System.Collections.Generic;
using System.Transactions;

namespace EXP.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUserProfileRepository UserProfileRepository; 
        private readonly IVehicleRepository VehicleRepository;
        private readonly IAlertRepository AlertRepository; 
        private readonly IVehicleMakeRepository VehicleMakeRepository  ;
        private readonly IVehicleModelRepository VehicleModelRepository ;
        private readonly IAddressRepository AddressRepository;
        private readonly IPhoneRepository PhoneRepository;
        private readonly IPaymentRepository PaymentRepository;
        private readonly IBuildingRepository BuildingRepository;
        private readonly IPhoneTypeRepository PhoneTypeRepository;         
        private readonly IzStateRepository zStateRepository;
        private readonly ICompanyRepository CompanyRepository;               

        private readonly AlertsManager _alertsManager;

        public DashboardService()
        {
            UserProfileRepository = new UserProfileRepository();
            VehicleRepository = new VehicleRepository();
            AlertRepository = new AlertRepository();
            VehicleMakeRepository = new VehicleMakeRepository();
            AddressRepository = new AddressRepository();
            PhoneRepository = new PhoneRepository();
            PaymentRepository = new PaymentRepository();
            BuildingRepository = new BuildingRepository();
            PhoneTypeRepository = new PhoneTypeRepository();
            zStateRepository = new zStateRepository();
            CompanyRepository = new CompanyRepository();
            VehicleModelRepository = new VehicleModelRepository();

            _alertsManager = new AlertsManager();
        }
        public bool ChangePassword(NewPasswordModel NewPassword)
        {
            var salt = CryptoUtils.CreateSalt();
            var hashedPassword = CryptoUtils.CreatePasswordHash(NewPassword.NewPassword, salt);
            return UserProfileRepository.ChangePassword( NewPassword.UserName, hashedPassword, salt);      
        }
        public NewVehicleReturnedModel NewVehicle(NewVehicleModel vehicleModel, GetUserModel User)
        {
            try
            {
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, User.UserProfileStatusID);

                Vehicle vehicle = new Vehicle();                
                                
                vehicle = Mapper.DynamicMap<NewVehicleModel, Vehicle>(vehicleModel);
                vehicle.UserProfileID = profile.UserProfileID;             

                int deleteAlertId;
                Alert alert;
                //using (TransactionScope scope = new TransactionScope())
                //{
                    vehicle.VehicleID = VehicleRepository.CreateVehicle(vehicle);

                    deleteAlertId = _alertsManager.DeleteEmptyVehicleAlert(profile.UserProfileID);

                    alert = _alertsManager.CreateOrDeleteAlertForVehicle(vehicle, profile.UserProfileID);

                    //scope.Complete();
                //}
                {
                    return new NewVehicleReturnedModel() { success = true, Message = string.Empty, Alert = alert, deleteAlertId = deleteAlertId, VehicleID = vehicle.VehicleID };
                }
            }
            catch (Exception exc)
            {
                return new NewVehicleReturnedModel() { success = false, Message = exc.Message};
            }
        }

        public EditVehicleReturnedModel EditVehicle(EditVehicleModel vehicleModel, GetUserModel User)
        {
            try
            {
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);

                Vehicle vehicle = new Vehicle();
                vehicle = Mapper.DynamicMap<EditVehicleModel, Vehicle>(vehicleModel);
                vehicle.UserProfileID = profile.UserProfileID;                              

                Alert alert;
                //using (TransactionScope scope = new TransactionScope())
               // {
                    VehicleRepository.UpdateVehicle(vehicle);
                    alert = _alertsManager.CreateOrDeleteAlertForVehicle(vehicle, profile.UserProfileID);
                    
                 //   scope.Complete();
               // }
                {
                   return new EditVehicleReturnedModel() {  success = true, Message = string.Empty, Alert = alert, VehicleID  = vehicle.VehicleID };
                }
            }
            catch (Exception exc)
            {
                return new EditVehicleReturnedModel() { success = false, Message = exc.Message };                
            }                     
        }

        public RemoveVehicleReturnedModel RemoveVehicle(RemoveVehicleModel vehicleModel, GetUserModel User)
        {
            try
            {
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, (int)UserProfileStatusEnum.Active);
                List<Alert> alerts = AlertRepository.GetAlertsForUserByEntity(
                profile.UserProfileID, AlertEntityTypeEnum.Vehicle, vehicleModel.VehicleId);

                using (TransactionScope scope = new TransactionScope())
                {
                    VehicleRepository.RemoveVehicle(vehicleModel.VehicleId);
                    
                    foreach (var alert in alerts)
                    {
                        AlertRepository.DeactivateAlert(alert.AlertID);
                        alert.isActive = false;
                    }
                    scope.Complete();
                }

                List<Vehicle> vehicles = VehicleRepository.ListVehicles(profile.UserProfileID);

                return new RemoveVehicleReturnedModel() { success = true, Message = "", Alerts = alerts, Vehicles = vehicles };                
            }
            catch (Exception exc)
            {
                return new RemoveVehicleReturnedModel() { success = false, Message = exc.Message };                
            }        
        }

        public GetVehicleReturnedModel GetVehicle(GetVehicleModel GetVehicleModel)
        {
            try
            {
                UserProfile profile = UserProfileRepository.GetUser(GetVehicleModel.UserName, (int)UserProfileStatusEnum.Active);
                Vehicle vehicle = VehicleRepository.GetVehicle(GetVehicleModel.VehicleID);

                return new GetVehicleReturnedModel { success = true, Message = "", Vehicle = vehicle };                
            }
            catch (Exception exc)
            {
                return new GetVehicleReturnedModel { success = false, Message = exc.Message };                
            }       
        }

        public ListVehiclesModelsReturnedModel ListVehicles(GetUserModel User)
        {
            UserProfile profile = UserProfileRepository.GetUser(User.UserName, (int)UserProfileStatusEnum.Active);
            List<Vehicle> list = VehicleRepository.ListVehicles(profile.UserProfileID);

            return new ListVehiclesModelsReturnedModel() { ListVehicles = list };
        }
        
        public ListVehicleMakesModelsReturnedModel ListMakes()
        {
            List<VehicleMake> makes = VehicleMakeRepository.ListMakes();
            return new ListVehicleMakesModelsReturnedModel() { ListVehicleMakes = makes }; 
        }
        
        public ListVehicleModelsReturnedModel ListModels(int makeId)
        {
           List<VehicleModel> models = VehicleModelRepository.ListModels(makeId);
           return new ListVehicleModelsReturnedModel() { ListVehicleModels = models };
        }

        public PersonallyAddressModelReturnedModel GetPersonallyAddress(GetUserModel User)
        {
            UserProfile profile = UserProfileRepository.GetUser(User.UserName, User.UserProfileStatusID);
            Address address = new Address();

            if (profile.AddressID != null)
            {
                address = AddressRepository.GetAddress((int)profile.AddressID);
            }
            return new PersonallyAddressModelReturnedModel(){ success = true, Message = "", Address = address };            
        }

        public EditPersonalAddressModelReturnedModel EditPersonalAddress(PersonalAddressModel addressModel, GetUserModel User)
        {
            try
            {
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, User.UserProfileStatusID);

                Address address = new Address();
                address = Mapper.DynamicMap<PersonalAddressModel, Address>(addressModel);
                address.AddressID = profile.AddressID ?? 0;

                Alert alert;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (address.AddressID == 0)
                    {
                        int newAddressId = AddressRepository.CreateAddress(address);
                        UserProfileRepository.UpdatePersonalAddress(profile.UserProfileID, newAddressId);
                    }
                    else
                    {
                        AddressRepository.UpdateAddress(address);
                    }

                    alert = _alertsManager.CreateOrDeleteAlertForPersonInfo(addressModel.City, addressModel.Address1,
                                                                            addressModel.StateID, addressModel.Zipcode,
                                                                            profile.UserProfileID);

                    scope.Complete();
                }

               return new EditPersonalAddressModelReturnedModel() { success = true, Message = "", Alert = alert };                
            }
            catch (Exception exc)
            {
                return new EditPersonalAddressModelReturnedModel() { success = false, Message = exc.Message };                
            }                       
        }

        public DashboardReturnedModel Dashboard(GetUserModel User)
        {
            DashboardReturnedModel DashboardReturnedModel = new DashboardReturnedModel();  
            
            UserProfile userProfile = UserProfileRepository.GetUser(User.UserName, (int)UserProfileStatusEnum.Active);
            DashboardReturnedModel.UserProfile = userProfile;

            Address address = userProfile.AddressID != null && userProfile.AddressID > 0 ? AddressRepository.GetAddress((int)userProfile.AddressID) : new Address();
            DashboardReturnedModel.Address = address;

            List<Alert> alerts = AlertRepository.GetActiveAlertsForUser(userProfile.UserProfileID);
            DashboardReturnedModel.Alerts = alerts;

            List<Phone> phones = PhoneRepository.GetPhonesForUser(userProfile.UserProfileID);
            DashboardReturnedModel.Phones = phones;

            if (userProfile.UserProfileTypeID != (int)UserProfileTypeEnum.BuildingOwner)
            {
                List<Vehicle> vehicles = VehicleRepository.GetVehicles(userProfile.UserProfileID);
                DashboardReturnedModel.Vehicles = vehicles;

                Payment payment = PaymentRepository.GetPaymentForUser(userProfile.UserProfileID);
                string cardNumber = "";
                if (payment != null && payment.CreditCard != null)
                {
                    cardNumber = payment.CreditCard.CardNumber;
                    cardNumber = cardNumber.Substring(cardNumber.Length - 4, 4);
                }
                DashboardReturnedModel.CreditCard = cardNumber;
            }

            if (userProfile.UserProfileTypeID == (int)UserProfileTypeEnum.BuildingOwner)
            {
                List<Building> buildings = BuildingRepository.ListBuildingsForDashboard(userProfile.UserProfileID);

                DashboardReturnedModel.Buildings = buildings;
            }
            return DashboardReturnedModel;
        }

        public MessageReturnedModel DeleteAlert(int[] alertIds)
        {
            try
            {
                if (alertIds != null)
                {
                    foreach (int alertId in alertIds)
                    {
                        AlertRepository.DeactivateAlert(alertId);
                    }
                    return new MessageReturnedModel { success = true, Message = "" };
                    
                }
                else
                {
                    return new MessageReturnedModel { success = false, Message = "Unknown error" };
                    
                }
            }
            catch (Exception exc)
            {
                return new MessageReturnedModel { success = false, Message= exc.Message };                
            }                               
        }

        public ListPhoneTypesReturnedModel ListPhoneTypes()
        {
            List<PhoneType> phoneTypes = PhoneTypeRepository.ListPhoneTypes();
            return new ListPhoneTypesReturnedModel { PhoneTypes = phoneTypes };
        }

        public GetPhonesReturnedModel GetPhones(GetUserModel User)
        {
            try
            {
                UserProfile userProfile = UserProfileRepository.GetUser(User.UserName, (int)UserProfileStatusEnum.Active);

                List<Phone> Phones = PhoneRepository.GetAllPhonesForUser(userProfile.UserProfileID);

                string phones = string.Empty;

                if (Phones == null || Phones.Count == 0)
                {
                    phones = "[]";
                    return new GetPhonesReturnedModel { success = true, Message = "", PhoneObject = new { phones = phones } };
                }
                else
                {
                    int i = 0;
                    foreach (Phone phone in Phones)
                    {
                        if (i == 0)
                        {
                            phones += "[{" + string.Format("id: '{0}', number: '{1}', type1: '{2}' ", phone.PhoneID, phone.PhoneNumber, phone.PhoneType.PhoneType1) + "}";
                        }
                        else
                        {
                            phones += ",{" + string.Format("id: '{0}', number: '{1}', type1: '{2}' ", phone.PhoneID, phone.PhoneNumber, phone.PhoneType.PhoneType1) + "}";
                        }
                        if (i == Phones.Count - 1)
                        {
                            phones += "]";
                        }
                        i++;
                    }
                }
                return new GetPhonesReturnedModel { success = true, Message = "", PhoneObject = new { phones = phones } };

            }
            catch (Exception exc)
            {
                return new GetPhonesReturnedModel { success = false, Message = exc.Message };
            }
        }

        public NewPhoneReturnedModel NewPhone(NewPhoneModel newPhone, GetUserModel User)
        {
            try
            {
                Phone phone = new Phone();
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);
                phone = Mapper.DynamicMap<NewPhoneModel, Phone>(newPhone);
                                
                int newPhoneId = PhoneRepository.CreatePhoneForUser(phone, profile.UserProfileID);
                {
                    return new NewPhoneReturnedModel { success = true, Message = "", NewPhoneId = newPhoneId };                    
                }
            }
            catch (Exception exc)
            {
                return new NewPhoneReturnedModel { success = false, Message = exc.Message };                
            }          
        }

        public ListPhonesReturnedModel ListPhones(GetUserModel User)
        {
            UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);
            List<Phone> phones = PhoneRepository.ListPhonesForUser(profile.UserProfileID);
            return new ListPhonesReturnedModel { Phones = phones };
        }

        public EditPhoneReturnedModel EditPhone(EditPhoneModel editPhone, GetUserModel User)
        {
            try
            {
                Phone phone = new Phone();
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);

                phone = Mapper.DynamicMap<EditPhoneModel, Phone>(editPhone);
                                
                PhoneRepository.UpdatePhone(phone);  //profile.UserProfileID;
                {
                   return new  EditPhoneReturnedModel { success = true, Message = "", PhoneId  = phone.PhoneID };                    
                }
            }
            catch (Exception exc)
            {
                return new EditPhoneReturnedModel { success = false, Message = exc.Message };                
            }        
        }

        public RemovePhoneReturnedModel RemovePhone(RemovePhoneModel removePhone, GetUserModel User)
        {
            try
            {
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);
                PhoneRepository.RemovePhoneForUser(removePhone.PhoneID, profile.UserProfileID);
                List<Phone> phones = PhoneRepository.ListPhonesForUser(profile.UserProfileID);
                {
                    return new RemovePhoneReturnedModel{ success = true, Message = "", PhonesCount = phones.Count };                    
                }
            }
            catch (Exception exc)
            {
                return new RemovePhoneReturnedModel { success = false, Message = exc.Message };                
            }                  
        }
      
        public ListStatesReturnedModel ListStates()
        {
            List<zState> states = zStateRepository.ListStates();
            return new ListStatesReturnedModel { States = states };
        }
        
        public UserProfileModel GetUser(string userName)
        {
            UserProfile user = UserProfileRepository.GetUser(userName, (int)UserProfileStatusEnum.Active);
            UserProfileModel model = Mapper.DynamicMap<UserProfile,UserProfileModel>(user);
            return model;
        }

        public bool UserProfile(UserProfileModel editUser)
        {
            UserProfile user = Mapper.DynamicMap<UserProfileModel,UserProfile>(editUser);
            user.LoginName = editUser.UserName;
            user.AddressID = null;
            return UserProfileRepository.UpdateProfile(user);
        }

        public PhoneModel GetPhone(int phoneId)
        {
            Phone phone = PhoneRepository.GetPhone(phoneId);
            PhoneModel model = Mapper.DynamicMap<Phone, PhoneModel>(phone);
            model.PhoneType = Mapper.DynamicMap<PhoneType, PhoneTypeModel>(phone.PhoneType);
            
            return model;
        }

        //public void SaveEmployeePaymentMethod(PaymentModel payment)
        //{
        //    RegistrationData.Current.Employee.Payment = payment;
        //    SetCurrentStep(0);
        //}

        //public void SaveTenantPaymentMethod(PaymentModel payment)
        //{
        //    RegistrationData.Current.Tenant.Payment = payment;
        //    SetCurrentStep(0);
        //}

        private void SetCurrentStep(byte step)
        {
            RegistrationData.Current.Step = step;
            RegistrationData.Current.Save();
        }

        public List<Alert> NewPaymentCreditCard(CreditCardModel creditCardModel, GetUserModel User )
        {
            try
            {
                UserProfileModel profile = GetUser(User.UserName);
                Payment newPayment = new Payment();
                newPayment.UserProfileID = profile.UserProfileID;
                List<Alert> alerts = new List<Alert>();

                if (creditCardModel != null)
                {
                    Address newAddress = new Address();
                    if (creditCardModel.AddressID != null)
                    {
                        newAddress.AddressID = (int)creditCardModel.AddressID;
                    }

                    newAddress = Mapper.DynamicMap<CreditCardModel, Address>(creditCardModel);
                    newAddress.StateID = creditCardModel.StateID == 0 ? 1 : creditCardModel.StateID;
                    newPayment.isCreditCard = true;

                    newPayment.CreditCard = new CreditCard();
                    newPayment.CreditCard = Mapper.DynamicMap<CreditCardModel, CreditCard>(creditCardModel);
                    
                    Alert alert;
                    if (!String.IsNullOrEmpty(newPayment.CreditCard.CardNumber.Trim()))
                    {
                        newPayment.CreditCard.CardNumber = newPayment.CreditCard.CardNumber.Replace(" ","");
                        if (!newPayment.CreditCard.IsCardNumberValid())
                            alert = _alertsManager.CreateCreditCardAlert(profile.UserProfileID);
                        else
                        {
                            alert = _alertsManager.DeleteCreditCardAlert(profile.UserProfileID);
                        }
                        if (alert != null)
                        {
                            alerts.Add(alert);
                        }
                    }                                      

                    using (TransactionScope scope = new TransactionScope())
                    {
                        if (creditCardModel.isHome)
                        {
                            newAddress.AddressID = (int)creditCardModel.AddressID;
                            AddressRepository.UpdateAddress(newAddress);
                            newPayment.CreditCard.AddressID = creditCardModel.AddressID;
                        }
                        else
                        {
                            newPayment.CreditCard.AddressID = AddressRepository.CreateAddress(newAddress);
                        }

                        PaymentRepository.CreatePaymentCreditCard(newPayment);

                        //PaymentModel PaymentModel = Mapper.DynamicMap<Payment, PaymentModel>(newPayment);

                        //if (creditCardModel.isHome != null)
                        //{
                        //    newPayment.CreditCard.isHome = (bool)creditCardModel.isHome;

                        //    SaveEmployeePaymentMethod(PaymentModel);
                        //}
                        //else
                        //{
                        //    newPayment.CreditCard.isHome = (bool)creditCardModel.isOffice;

                        //    SaveTenantPaymentMethod(PaymentModel);
                        //}

                        scope.Complete();
                    }
                    Payment payment = PaymentRepository.GetPaymentForUser(profile.UserProfileID);
                    alert = _alertsManager.CreateOrDeleteAlertForPayment(payment, profile.UserProfileID);
                    if (alert != null)
                    {
                        alerts.Add(alert);
                    }
                }
                return alerts;
            }
            catch (Exception exc)
            {
                throw exc;
            }                        
        }

        public List<Alert> NewPaymentOnlineCheck(OnlineCheckModel onlineCheckModel, GetUserModel User)
        { 
            try
            {
                List<Alert> alerts = new List<Alert>();
                UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);
                Payment newPayment = new Payment();
                newPayment.UserProfileID = profile.UserProfileID;

                if (onlineCheckModel != null)
                {
                    newPayment.isCreditCard = false;
                    newPayment.OnlineCheck = new OnlineCheck();
                    newPayment.OnlineCheck = Mapper.DynamicMap<OnlineCheckModel, OnlineCheck>(onlineCheckModel); 
                    
                    PaymentRepository.CreatePaymentOnlineCheck(newPayment);

                    PaymentModel PaymentModel = Mapper.DynamicMap<Payment, PaymentModel>(newPayment);
     //               SaveEmployeePaymentMethod(PaymentModel);
                }
                Payment payment = PaymentRepository.GetPaymentForUser(profile.UserProfileID);
                Alert alert = _alertsManager.CreateOrDeleteAlertForPayment(payment, profile.UserProfileID);
                if (alert != null)
                {
                    alerts.Add(alert);
                }
                return alerts;                    
            }
            catch (Exception exc)
            {
                throw exc;
            }                                                
        }

        public PaymentModel GetPayment(GetUserModel User)
        {
            UserProfile profile = UserProfileRepository.GetUser(User.UserName, 0);
            Payment payment = PaymentRepository.GetPaymentForUser(profile.UserProfileID) ?? new Payment();
            PaymentModel PaymentModel = Mapper.DynamicMap<Payment, PaymentModel>(payment);
            if (PaymentModel.isCreditCard && payment.CreditCard != null && payment.CreditCard.Address != null)
	        {
                PaymentModel.CreditCard.AddressID = payment.CreditCard.AddressID;
                PaymentModel.CreditCard.Address1 = payment.CreditCard.Address.Address1;
                PaymentModel.CreditCard.Address2 = payment.CreditCard.Address.Address2;
                PaymentModel.CreditCard.City = payment.CreditCard.Address.City;
                PaymentModel.CreditCard.StateID = payment.CreditCard.Address.StateID;
                PaymentModel.CreditCard.ZipCode = payment.CreditCard.Address.ZipCode;
	        }
            return PaymentModel;
        }

        public List<Alert> EditCreditCard(CreditCardModel creditCardModel, string userName)
        {
            try
            {
                List<Alert> alerts = new List<Alert>();
                UserProfile profile = UserProfileRepository.GetUser(userName, 0);
                if (creditCardModel != null)
                {
                    Address editAddress = new Address();                  
                    editAddress = Mapper.DynamicMap<CreditCardModel, Address>(creditCardModel);

                    CreditCard editCreditCard = new CreditCard();
                    editCreditCard = Mapper.DynamicMap<CreditCardModel, CreditCard>(creditCardModel);

                    if (!String.IsNullOrEmpty(editCreditCard.CardNumber.Trim()))
                    {
                        
                        Alert alert;
                        editCreditCard.CardNumber = editCreditCard.CardNumber.Replace(" ", "");
                        if (!editCreditCard.IsCardNumberValid())
                            alert = _alertsManager.CreateCreditCardAlert(profile.UserProfileID);
                        else
                        {
                            alert = _alertsManager.DeleteCreditCardAlert(profile.UserProfileID);
                        }
                        if (alert != null)
                        {
                            alerts.Add(alert);
                        }
                    }

                    editCreditCard.isHome = (bool)creditCardModel.isHome;

                    using (TransactionScope scope = new TransactionScope())
                    {
                        if (creditCardModel.AddressID != null)
                        {
                            editAddress.AddressID = (int)creditCardModel.AddressID;
                            AddressRepository.UpdateAddress(editAddress);
                            editCreditCard.AddressID = editAddress.AddressID;
                        }
                        else
                        {
                            editCreditCard.AddressID = AddressRepository.CreateAddress(editAddress);
                        }

                        PaymentRepository.UpdateCreditCard(editCreditCard);
                        if (creditCardModel.OldAddressId != creditCardModel.AddressID &&
                            creditCardModel.AddressID != null)
                        {
                            AddressRepository.DeleteAddress((int)creditCardModel.OldAddressId);
                        }
                        scope.Complete();    
                    }
                    Payment payment = PaymentRepository.GetPaymentForUser(profile.UserProfileID);
                    Alert newAlert = _alertsManager.CreateOrDeleteAlertForPayment(payment, profile.UserProfileID);
                    if (newAlert != null)
                    {
                        alerts.Add(newAlert);
                    }  
                }
                return alerts;                  
            }
            catch (Exception exc)
            {
                throw exc;
            }                                    
        }

        public List<Alert> EditOnlineCheck(OnlineCheckModel onlineCheckModel, string userName)
        {
            try
            {
                List<Alert> alerts = new List<Alert>();
                if (onlineCheckModel != null)
                {
                    UserProfile profile = UserProfileRepository.GetUser(userName, 0);
                    OnlineCheck editOnlineCheck = new OnlineCheck();
                    editOnlineCheck.OnlineCheckID = (int)onlineCheckModel.OnlineCheckId;
                    editOnlineCheck = Mapper.DynamicMap<OnlineCheckModel, OnlineCheck>(onlineCheckModel);

                    PaymentRepository.UpdateOnlineCheck(editOnlineCheck);

                    Payment payment = PaymentRepository.GetPaymentForUser(profile.UserProfileID);
                    Alert newAlert = _alertsManager.CreateOrDeleteAlertForPayment(payment, profile.UserProfileID);
                    if (newAlert != null)
                    {
                        alerts.Add(newAlert);
                    }
                }
                return alerts;
            }
            catch (Exception exc)
            {
                throw exc;
            }        
        }

        public List<CompanyModel> ListBuildingTenants(ListBuildingTenantsModel ListBuildingTenantsModel)
        {
            DateTime fromDate = ListBuildingTenantsModel.Days != 0 ? DateTime.Now.Date.AddDays(1 - ListBuildingTenantsModel.Days) : DateTime.MinValue;
            List<Company> tenants = CompanyRepository.ListBuildingCompanies(ListBuildingTenantsModel.BuildingId, fromDate);
            List<CompanyModel> tenants1 = null; 
            
            if (tenants != null & tenants.Count !=0 )
            foreach(Company c in tenants )
            {
              CompanyModel CompanyModel = Mapper.DynamicMap<Company, CompanyModel>(c);
              tenants1.Add(CompanyModel); 
            }                        
            return tenants1;        
        }

        public BuildingModel GetBuildingWithAddress(int? BuildingId)
        {
           Building building = BuildingRepository.GetBuildingWithAddress(BuildingId);        
           BuildingModel BuildingModel = Mapper.DynamicMap<Building, BuildingModel>(building);
           BuildingModel.BuildingAddress = Mapper.DynamicMap<Address, AddressModel>(building.BuildingAddress);
           
           return BuildingModel;
        }

        public EditBuildingAddressReturnedModel EditBuildingAddress(BuildingAddressModel BuildingAddressModel, GetUserModel User)
        {                       
            int UserID = UserProfileRepository.GetUser(User.UserName, 0).UserProfileID;

            bool datavalid = true;
            int testint;

            if (string.IsNullOrEmpty(BuildingAddressModel.Address1) | string.IsNullOrWhiteSpace(BuildingAddressModel.Address1)) datavalid = false;
            if (string.IsNullOrEmpty(BuildingAddressModel.City) | string.IsNullOrWhiteSpace(BuildingAddressModel.City)) datavalid = false;
            if (string.IsNullOrEmpty(BuildingAddressModel.BuildingName) | string.IsNullOrWhiteSpace(BuildingAddressModel.BuildingName)) datavalid = false;
            if (string.IsNullOrEmpty(BuildingAddressModel.ZipCode) | string.IsNullOrWhiteSpace(BuildingAddressModel.ZipCode)) datavalid = false;
            if (!(BuildingAddressModel.StateID > 0 & BuildingAddressModel.StateID < 51)) datavalid = false;
            if (BuildingAddressModel.BuildingID < 0) datavalid = false;
            if (!int.TryParse(BuildingAddressModel.ZipCode, out testint)) datavalid = false;

            if (!datavalid)
            {
                return new  EditBuildingAddressReturnedModel{ success = false, Message = "Posted Data is not valid " };
                
            }

            Building building = new Building();
            building.BuildingAddress = new Address();
                        
            building = Mapper.DynamicMap<BuildingAddressModel, Building>(BuildingAddressModel);
            building.BuildingAddress = Mapper.DynamicMap<BuildingAddressModel, Address>(BuildingAddressModel);
            building.OwnerID = UserID;

            // Creating New Building
            if (BuildingAddressModel.BuildingID == 0)
            {
                Address BuildingAddress = Mapper.DynamicMap<BuildingAddressModel, Address>(BuildingAddressModel);
                building.AddressID = AddressRepository.CreateAddress(BuildingAddress); 
   
                BuildingAddressModel.BuildingID = BuildingRepository.CreateBuildingWithNewAddress(building);
                BuildingAddressModel.Updated = false;
                BuildingAddressModel.StateName = zStateRepository.StateName(BuildingAddressModel.StateID);

                return new EditBuildingAddressReturnedModel { success = true, Message = "", BuildingAddressModel = BuildingAddressModel };
                
            }
            else // Updating  Building by input Building Id
            {
                bool success = BuildingRepository.UpdateBuildingWithNewAddress(building);
                if (success)
                {
                    BuildingAddressModel.Updated = true;
                    BuildingAddressModel.StateName = zStateRepository.StateName(BuildingAddressModel.StateID);

                    return new EditBuildingAddressReturnedModel { success = true, Message = "", BuildingAddressModel = BuildingAddressModel };
                    
                }
                else
                {
                    return new EditBuildingAddressReturnedModel { success = false, Message = "Building by Id not exists" };                    
                }
            }                  
        }

        public bool DeleteBuilding(int BuildingId)
        {
             return BuildingRepository.DeleteBuilding(BuildingId);
        } 



    }
}
