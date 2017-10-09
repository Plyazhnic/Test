using System;
using System.Linq;
using EXP.Entity;
using EXP.Core;
using EXP.Core.Util;
using EXP.Entity.Enumerations;
using EXP.DataAccess;
using System.Web.Security;
using EXP.Services.Models;
using EXP.Services.Interface;
using System.Collections.Generic;
using System.Transactions;
using AutoMapper;
using EXP.Core.Interface;

namespace EXP.Services
{
    public class RegistrationService : IRegistrationService
    {
        #region private
        private readonly AlertsManager _alertsManager;
        private readonly EmailService _emailService;       
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IAddressRepository _addressRepo;
        private readonly IPhoneRepository _phoneRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IBuildingRepository _buildingRepo;
        private readonly ILotRepository _lotRepo;
        private readonly IParkingStallRepository _parkingStallRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IParkingInventoryRepository _parkingInventoryRepo;
        private readonly IVehicleRepository _vehicleRepo;
        private readonly IUserProfileTypeRepository _userProfileTypeRepo;
        private readonly ILeaseRepository _leaseRepository;
        #endregion

        public RegistrationService() 
        {
            _alertsManager = new AlertsManager();
            _emailService = new EmailService();          
            _userProfileRepo = new UserProfileRepository(); 
            _addressRepo = new AddressRepository();
            _phoneRepo = new PhoneRepository(); 
            _paymentRepo = new PaymentRepository();
            _buildingRepo = new BuildingRepository();
            _lotRepo = new LotRepository();
            _parkingStallRepo = new ParkingStallRepository();
            _companyRepo = new CompanyRepository();
            _parkingInventoryRepo = new ParkingInventoryRepository();
            _vehicleRepo = new VehicleRepository();
            _userProfileTypeRepo = new UserProfileTypeRepository();
            _leaseRepository = new LeaseRepository();
        }

        #region RegisterStep
        
        public byte GetCurrentStep() 
        {
            return RegistrationData.Current.Step;
        }
        
        int GetProfileType() 
        {
            return RegistrationData.Current.ProfileTypeID;
        }

        public PhoneModel GetProfilePhone()
        {
            return RegistrationData.Current.Tenant.isManager ? RegistrationData.Current.Tenant.ManagerPhone : null;
        }
        
        public AddressModel GetHomeAddress() 
        {
            return RegistrationData.Current.Employee.Address;
        }

        public AddressModel GetCompanyAddress() 
        {
            return RegistrationData.Current.Tenant.Company.Address;
        }
        
        public void SetCurrentStep(byte step) 
        {
            RegistrationData.Current.Step = step;
            RegistrationData.Current.Save();
        }

        public RegistrationData GetStoredInfo() 
        {
            return RegistrationData.Current;
        }
        
        public void SetStatus(string userName) 
        {
            switch (GetProfileType())
            {
                case (int)UserProfileTypeEnum.Employee:
                    SaveEmployee(userName);
                    break;
                case (int)UserProfileTypeEnum.Tenant:
                    SaveTenant(userName);
                    break;
            }

            UserProfile profile = _userProfileRepo.GetUser(userName, 0);

            string emailsPath = String.Format("{0}Emails/", RegistrationData.Current.ServerPath);
            string imagesUrl = String.Format("{0}Images/", RegistrationData.Current.RootUrl);

            int profileTypeId = GetProfileType();
            PayEmailModel payModel = new PayEmailModel() { 
                FirstName = profile.FirstName,
                Email = profile.EmailAddress,
                EmailPath = emailsPath,
                ImageUrl = imagesUrl,
            };
            if (profileTypeId == (int)UserProfileTypeEnum.Employee)
            {
                _emailService.SendWelcomeSelfPayEmail(payModel);
            }
            else
            {
                _emailService.SendWelcomeCompanyPayEmail(payModel);
            }

            Payment payment = _paymentRepo.GetPaymentForUser(profile.UserProfileID);   //TODO: PAY

            using (TransactionScope scope = new TransactionScope())
            {
                UserProfile user = new UserProfile() { 
                    LoginName = profile.LoginName,
                    UserProfileTypeID = profileTypeId,
                    UserProfileStatusID = (int)UserProfileStatusEnum.Active,
                };
                _userProfileRepo.UpdateProfile(user);
                if (payment == null)
                {
                    _alertsManager.CreateEmptyPaymentAlert(profile.UserProfileID);
                }
                else
                {
                    _alertsManager.CreateOrDeleteAlertForPayment(payment, profile.UserProfileID);
                }

                scope.Complete();
            }
        }

        public int SaveProfileInfo(ProfileTypeModel profileType)
        {
            int lotId = _lotRepo.GetBuildingToLot(profileType.BuildingToLotID).LotID;

            RegistrationData.Current.ProfileTypeID = profileType.ProfileTypeID;
            RegistrationData.Current.LotID = lotId;
            RegistrationData.Current.BuildingToLotID = profileType.BuildingToLotID;
            RegistrationData.Current.TenantID = profileType.TenantID;
            RegistrationData.Current.ServerPath = profileType.Path;
            RegistrationData.Current.RootUrl = profileType.RootUrl;

            byte step;
            switch (profileType.ProfileTypeID)
            {
                case (int)UserProfileTypeEnum.Employee:
                    RegistrationData.Current.Employee = new EmployeeModel();
                    step = 2;
                    break;
                case (int)UserProfileTypeEnum.BuildingOwner:
                    RegistrationData.Current.Owner = new BuildingOwnerModel();
                    step = 13;
                    break;
                default:
                    RegistrationData.Current.Tenant = new TenantModel();
                    step = 7;
                    break;
            }
            SetCurrentStep(step);
            return lotId;
        }
        
        public List<UserProfileTypeModel> ListUserProfileType()
        {
            List<UserProfileType> list = _userProfileTypeRepo.ListUserProfileType();
            List<UserProfileTypeModel> types = new List<UserProfileTypeModel>(); 
            UserProfileTypeModel type;    
            foreach (var item in list)
            {
                type = Mapper.DynamicMap<UserProfileType, UserProfileTypeModel>(item);
                types.Add(type);
            }
            return types;
        }

        public List<BuildingToLotModel> ListLots()
        {
            List<BuildingToLot> list = _lotRepo.GetListLots();
            List<BuildingToLotModel> lots = new List<BuildingToLotModel>();
            BuildingToLotModel buildingToLot;
            LotModel lot;
            BuildingModel building;
            foreach (var item in list)
            {
                buildingToLot = Mapper.DynamicMap<BuildingToLot, BuildingToLotModel>(item);
                lot = Mapper.DynamicMap<Lot,LotModel>(item.Lot);
                building = Mapper.DynamicMap<Building, BuildingModel>(item.Building);
                lots.Add(buildingToLot);
            }
            return lots; 

        }

        public List<CompanyModel> ListCompanies()
        {
            List<Company> list = _companyRepo.ListCompanies();
            List<CompanyModel> companies = new List<CompanyModel>();
            CompanyModel company;
            foreach (var item in list)
            {
                company = Mapper.DynamicMap<Company, CompanyModel>(item);
                companies.Add(company);
            }
            return companies; 
        }

        public List<VehicleModels> ListVehicles(string userName)
        {
            int profileId = _userProfileRepo.GetUser(userName, (int)UserProfileStatusEnum.Active).UserProfileID;
            List<Vehicle> list = _vehicleRepo.ListVehicles(profileId);
            List<VehicleModels> vehicles = new List<VehicleModels>();
            VehicleModels vehicle;
            foreach (var item in list)
            {
                vehicle = Mapper.DynamicMap<Vehicle, VehicleModels>(item);
                vehicles.Add(vehicle);
            }
            return vehicles; 
        }

        public ParkingStallModel GetParkingStall(int lotId)
        {
            ParkingStall stall = _parkingStallRepo.GetParkingInfoForLot(lotId);
            ParkingStallModel model = Mapper.DynamicMap<ParkingStall,ParkingStallModel>(stall);
            return model;
        }

        #endregion

        #region Employee

        public void SetPersonalData(PersonalModel dataModel, PhoneModel phoneModel)
        {
            UserProfile profile = _userProfileRepo.GetUser(dataModel.UserName, (int)UserProfileStatusEnum.Verified);
            UserProfileModel model = Mapper.DynamicMap<PersonalModel,UserProfileModel>(dataModel);
            _alertsManager.CreateOrDeleteAlertForPersonInfo(null, null, null, null, profile.UserProfileID);
            SaveEmployeeAccountInfo(model, phoneModel);
        }

        public void CreateVehicle(VehicleModels vehicleModel)
        {
            UserProfile profile = _userProfileRepo.GetUser(vehicleModel.UserName, 0);

            if (vehicleModel.VehicleMakeID == null && vehicleModel.VehicleModelID == null //if all fields is empty
                && String.IsNullOrEmpty(vehicleModel.LicensePlateNumber) && vehicleModel.Year == null
                && String.IsNullOrEmpty(vehicleModel.Color) && String.IsNullOrEmpty(vehicleModel.PermitNumber))
            {
                _alertsManager.CreateEmptyVehicleAlert(profile.UserProfileID);
            }
            if (vehicleModel.isTenant)
            {
                SaveTenantVehicleInfo(vehicleModel);
            }
            else
            {
                SaveEmployeeVehicleInfo(vehicleModel);
            }
        }

        public UserProfileModel GetProfile(string userName)
        {
            UserProfile user = _userProfileRepo.GetUser(userName, (int)UserProfileStatusEnum.Verified);
            UserProfileModel newUser = Mapper.DynamicMap<UserProfile,UserProfileModel>(user);
            return newUser;

        }

        public void SetParkingInfo(ParkingInfoModel parkingInfo)
        {
            UserProfile profile = _userProfileRepo.GetUser(parkingInfo.UserName, 0);
            ParkingInventoryModel newParkingInventory = new ParkingInventoryModel();
            if (parkingInfo.ReservedSpaces + parkingInfo.UnReservedSpaces > 0)
            {
                newParkingInventory = Mapper.DynamicMap<ParkingInfoModel, ParkingInventoryModel>(parkingInfo);
                newParkingInventory.EffectiveFrom = parkingInfo.StartDate;
                newParkingInventory.EffectiveTo = parkingInfo.EndDate;

                _alertsManager.DeleteParkingInformationAlert(profile.UserProfileID);
            }
            else
            {
                _alertsManager.CreateParkingInformationAlert(profile.UserProfileID);
            }
            if (parkingInfo.isTenant)
            {
                SaveTenantParkingInfo(newParkingInventory);
            }
            else
            {
                SaveEmployeeParkingInfo(newParkingInventory);
            }
        }

        public void SaveEmployeeParkingInfo(ParkingInventoryModel parkingInventory)
        {
            RegistrationData.Current.Employee.ParkingInventory = parkingInventory;
            SetCurrentStep(3);
        }
        
        public void SaveEmployeeAccountInfo(UserProfileModel profile, PhoneModel phone)
        {
            RegistrationData.Current.Employee.Profile = profile;
            RegistrationData.Current.Employee.Phone = phone;
            SetCurrentStep(4);
        }
        
        public void SaveEmployeeVehicleInfo(VehicleModels vehicle)
        {
            RegistrationData.Current.Employee.Vehicle = vehicle;
            SetCurrentStep(5);
        }

        public void SaveEmployeePaymentInfo(bool myself)
        {
            RegistrationData.Current.Employee.Myself = myself;
            SetCurrentStep(6);
        }

        public void SaveEmployeePaymentMethod(PaymentModel payment)
        {
            RegistrationData.Current.Employee.Payment = payment;
            SetCurrentStep(6);
        }

        void SaveEmployee(string loginName)
        {
            /////TODO Server.MapPath
            string emailsPath = String.Format("{0}Emails/", RegistrationData.Current.ServerPath);
            string imagesUrl = String.Format("{0}Images/", RegistrationData.Current.RootUrl);

            UserProfile profile;
            using (TransactionScope scope = new TransactionScope())
            {
               // int addressID = 0;
                Company company = new Company();
                profile = _userProfileRepo.GetUser(loginName,0);
                if (RegistrationData.Current.Employee.Profile != null)
                {
                    //Address newAddress = Mapper.DynamicMap<AddressModel,Address>(RegistrationData.Current.Employee.Address);
                    //addressID = _addressRepo.CreateAddress(newAddress);
                    UserProfile user = Mapper.DynamicMap<UserProfileModel,UserProfile>(RegistrationData.Current.Employee.Profile);
                    user.UserProfileTypeID = RegistrationData.Current.ProfileTypeID;
                    //user.AddressID = addressID;
                    _userProfileRepo.UpdateProfile(user);
                    if (RegistrationData.Current.Employee.Phone.PhoneTypeID != 0 && !String.IsNullOrEmpty(RegistrationData.Current.Employee.Phone.PhoneNumber))
                    {
                        Phone newPhone = Mapper.DynamicMap<PhoneModel,Phone>(RegistrationData.Current.Employee.Phone);
                        _phoneRepo.CreatePhoneForUser(newPhone, profile.UserProfileID);
                    }
                }
                if (RegistrationData.Current.TenantID != null)
                {
                    //Send email tenant
                    try
                    {
                        ///TODO: Need tenant, approveUrl, disapproveUrl
                        UserProfile tenant = Mapper.DynamicMap<UserProfileModel,UserProfile>(RegistrationData.Current.Employee.Profile);
                        string employeeName = String.Format("{0} {1}", RegistrationData.Current.Employee.Profile.FirstName, RegistrationData.Current.Employee.Profile.LastName);
                        //TODO: IF tenant == null ?
                        company = _companyRepo.GetCompany((int)RegistrationData.Current.TenantID);
                        TenantEmailModel tenantModel = new TenantEmailModel()
                        {
                            FirstName = tenant.FirstName,
                            Email = tenant.EmailAddress,
                            EmployeeName = employeeName,
                            CompanyName = company.CompanyName,
                            EmailPath = emailsPath,
                            ImageUrl = imagesUrl,
                            ApproveUrl = "http://expresspark.cleversoftware.by",
                            DisapproveUrl = "http://expresspark.cleversoftware.by",
                        };
                        _emailService.SendTenantEmail(tenantModel);
                    }
                    catch (Exception)
                    {

                    }
                }

                if (RegistrationData.Current.Employee.ParkingInventory.LotID != null && RegistrationData.Current.Employee.ParkingInventory.LotID > 0)
                {
                    RegistrationData.Current.Employee.ParkingInventory.CompanyID = company.CompanyID;
                    ParkingInventory inventory = Mapper.DynamicMap<ParkingInventoryModel,ParkingInventory>(RegistrationData.Current.Employee.ParkingInventory);
                    inventory.CompanyID = inventory.CompanyID == 0 ? null : inventory.CompanyID;
                    _parkingInventoryRepo.CreateParkingInventoryForCompany(inventory);
                }

                if (RegistrationData.Current.Employee.Vehicle.VehicleMakeID != null || RegistrationData.Current.Employee.Vehicle.VehicleModelID != null
                    || !String.IsNullOrEmpty(RegistrationData.Current.Employee.Vehicle.LicensePlateNumber) || RegistrationData.Current.Employee.Vehicle.Year != null
                    || !String.IsNullOrEmpty(RegistrationData.Current.Employee.Vehicle.Color) || !String.IsNullOrEmpty(RegistrationData.Current.Employee.Vehicle.PermitNumber))
                {
                    Vehicle vehicle = Mapper.DynamicMap<VehicleModels,Vehicle>(RegistrationData.Current.Employee.Vehicle);
                    vehicle.UserProfileID = profile.UserProfileID;
                    vehicle.VehicleID = _vehicleRepo.CreateVehicle(vehicle);

                    //TODO: ERROR!      _alertsManager.CreateOrDeleteAlertForVehicle(vehicle, profile.UserProfileID);
                }

                if (RegistrationData.Current.Employee.Payment.CreditCard != null || RegistrationData.Current.Employee.Payment.OnlineCheck != null)
                {
                    Payment newPayment = Mapper.DynamicMap<PaymentModel,Payment>(RegistrationData.Current.Employee.Payment);
                    newPayment.UserProfileID = profile.UserProfileID;
                    if (RegistrationData.Current.Employee.Payment.isCreditCard)
                    {
                        Address address = Mapper.DynamicMap<CreditCardModel, Address>(RegistrationData.Current.Employee.Payment.CreditCard);
                        RegistrationData.Current.Employee.Payment.CreditCard.AddressID = _addressRepo.CreateAddress(address);
                        CreditCard card = Mapper.DynamicMap<CreditCardModel,CreditCard>(RegistrationData.Current.Employee.Payment.CreditCard);
                        newPayment.CreditCard = card;
                        if (!String.IsNullOrEmpty(newPayment.CreditCard.CardNumber.Trim()))
                        {
                            newPayment.CreditCard.CardNumber = RegistrationData.Current.Employee.Payment.CreditCard.CardNumber.Replace(" ", "");
                            if (!newPayment.CreditCard.IsCardNumberValid())
                                _alertsManager.CreateCreditCardAlert(profile.UserProfileID);
                        }
                        _paymentRepo.CreatePaymentCreditCard(newPayment);
                    }
                    else
                    {
                        OnlineCheck check = Mapper.DynamicMap<OnlineCheckModel, OnlineCheck>(RegistrationData.Current.Employee.Payment.OnlineCheck);
                        check.OnlineCheckingTypeID = check.OnlineCheckingTypeID == 0 ? 1 : check.OnlineCheckingTypeID; 
                        newPayment.OnlineCheck = check;
                        _paymentRepo.CreatePaymentOnlineCheck(newPayment);
                    }
                }
                scope.Complete();
            }
        }

        #endregion
        
        #region Tenant
        public AddressModel GetBuildingAddress(int buildingToLotId)
        {
            Address address = _addressRepo.GetBuildingAddressByLot(buildingToLotId);
            AddressModel newAddress = Mapper.DynamicMap<Address,AddressModel>(address);
            return newAddress;
        }

        public void SaveTenantCompanyInfo(CompanyModel companyModel, AddressModel newAddress)
        {
            companyModel.Address = newAddress;
            RegistrationData.Current.Tenant.Company = companyModel;
            SetCurrentStep(8);
        }

        public void SaveTenantCompanyManagerInfo(EditCompanyManager manager, PhoneModel newPhone, AddressModel newAddress)
        {
            UserProfileModel profile = manager.isManager ? Mapper.DynamicMap<UserProfile, UserProfileModel>(_userProfileRepo.GetUser(manager.UserName, 0)) : Mapper.DynamicMap<EditCompanyManager, UserProfileModel>(manager);

            RegistrationData.Current.Tenant.ManagerProfile = profile;
            RegistrationData.Current.Tenant.ManagerAddress = newAddress;
            RegistrationData.Current.Tenant.ManagerPhone = newPhone;
            RegistrationData.Current.Tenant.isManager = manager.isManager;
            RegistrationData.Current.Tenant.isMailing = manager.isMailing;
            SetCurrentStep(9);
        }

        public void SaveTenantParkingInfo(ParkingInventoryModel parkingInventory)
        {
            RegistrationData.Current.Tenant.ParkingInventory = parkingInventory;
            SetCurrentStep(10);
        }

        public void SaveTenantPersonally(bool personally)
        {
            RegistrationData.Current.Tenant.personally = personally;
            SetCurrentStep((byte)(personally ? 10 : 11));
        }

        public void SaveTenantVehicleInfo(VehicleModels vehicle)
        {
            RegistrationData.Current.Tenant.Vehicle = vehicle;
            SetCurrentStep(11);
        }

        public void SaveTenantPayingInfo(CompanyEmailModel model)
        {
            RegistrationData.Current.Tenant.EmailString = model.EmailsString;
            RegistrationData.Current.Tenant.NameString = model.NamesString;
            RegistrationData.Current.Tenant.PayingString = model.PayingString;
            SetCurrentStep(12);
        }

        public void SaveTenantPaymentMethod(PaymentModel payment)
        {
            RegistrationData.Current.Tenant.Payment = payment;
            SetCurrentStep(0);
        }

        void SaveTenant(string loginName)
        {
            ///TODO Server.MapPath
            string emailsPath = String.Format("{0}Emails/", RegistrationData.Current.ServerPath);
            string imagesUrl = String.Format("{0}Images/", RegistrationData.Current.RootUrl);

            UserProfile profile;
            using (TransactionScope scope = new TransactionScope())
            {
                int? companyAddressID = 0;
                int companyId = 0;
                int buildingId = _lotRepo.GetBuildingToLot((int)RegistrationData.Current.BuildingToLotID).BuildingID;
                Building building = _buildingRepo.GetBuilding(buildingId);
                //Company
                if (!RegistrationData.Current.Tenant.Company.asBuilding)
                {
                    Address newAddress = Mapper.DynamicMap<AddressModel, Address>(RegistrationData.Current.Tenant.Company.Address);
                    companyAddressID = _addressRepo.CreateAddress(newAddress);
                }
                else
                {
                    buildingId = _lotRepo.GetBuildingToLot((int)RegistrationData.Current.BuildingToLotID).BuildingID;
                    companyAddressID = _buildingRepo.GetBuilding(buildingId).AddressID;
                }
                RegistrationData.Current.Tenant.Company.AddressID = companyAddressID;
                Company newCompany = Mapper.DynamicMap<CompanyModel,Company>(RegistrationData.Current.Tenant.Company);
                companyId = _companyRepo.CreateCompany(newCompany);

                profile = _userProfileRepo.GetUser(loginName,0);

                //Manager
                UserProfile manager = Mapper.DynamicMap<UserProfileModel,UserProfile>(RegistrationData.Current.Tenant.ManagerProfile);
                if (RegistrationData.Current.Tenant.isManager)
                {
                    manager.UserProfileTypeID = RegistrationData.Current.ProfileTypeID;
                    _userProfileRepo.UpdateProfile(manager);
                    manager = profile;
                }
                else
                {
                    manager.UserProfileTypeID = (int)UserProfileTypeEnum.ParkingManager;
                    string login = CreateUser(manager);
                    if (!String.IsNullOrEmpty(login))
                    {
                        if (RegistrationData.Current.Tenant.isMailing)
                        {
                            manager.AddressID = (int)companyAddressID;
                        }
                        else
                        {
                            Address address = Mapper.DynamicMap<AddressModel, Address>(RegistrationData.Current.Tenant.ManagerAddress);
                            manager.AddressID = _addressRepo.CreateAddress(address);
                        }
                        manager.LoginName = login;
                        //TODO: Create manager
                        _userProfileRepo.UpdateProfile(manager);
                        manager = _userProfileRepo.GetUser(login, 0);
                    }
                    try
                    {
                        //TODO: Need registrationUrl
                        ManagerEmailModel managerModel = new ManagerEmailModel()
                        {
                            Email = RegistrationData.Current.Tenant.ManagerProfile.EmailAddress,
                            FirstName = RegistrationData.Current.Tenant.ManagerProfile.FirstName,
                            TenantName = RegistrationData.Current.Tenant.ManagerProfile.FirstName,  //TODO: Need tenantName
                            BuildingName = building.BuildingName,
                            EmailPath = emailsPath,
                            ImageUrl = imagesUrl,
                            RegistrationUrl = "http://expresspark.cleversoftware.by",
                        };
                        _emailService.SendManagerEmail(managerModel);
                    }
                    catch (Exception)
                    {
                    }
                }
                if (RegistrationData.Current.Tenant.ManagerPhone.PhoneTypeID != 0 && !String.IsNullOrEmpty(RegistrationData.Current.Tenant.ManagerPhone.PhoneNumber))
                {
                    Phone newPhone = Mapper.DynamicMap<PhoneModel,Phone>(RegistrationData.Current.Tenant.ManagerPhone);
                    _phoneRepo.CreatePhoneForUser(newPhone, manager.UserProfileID);
                }

                //Parking
                if (RegistrationData.Current.Tenant.ParkingInventory.LotID != null && RegistrationData.Current.Tenant.ParkingInventory.LotID > 0)
                {
                    RegistrationData.Current.Tenant.ParkingInventory.CompanyID = companyId;
                    ParkingInventory inventory = Mapper.DynamicMap<ParkingInventoryModel,ParkingInventory>(RegistrationData.Current.Tenant.ParkingInventory);
                    _parkingInventoryRepo.CreateParkingInventoryForCompany(inventory);
                    Lease newLease = new Lease
                    {
                        CompanyID = companyId,
                        BuildingID = building.BuildingID,
                        TermStart = RegistrationData.Current.Tenant.ParkingInventory.EffectiveFrom == null ? DateTime.MinValue : (DateTime)RegistrationData.Current.Tenant.ParkingInventory.EffectiveFrom,
                        TermEnd = RegistrationData.Current.Tenant.ParkingInventory.EffectiveTo == null ? DateTime.MinValue : (DateTime)RegistrationData.Current.Tenant.ParkingInventory.EffectiveTo,
                        LateFee = 0,
                        TaxExempt = true,
                        MarketRate = false,
                    };
                    _leaseRepository.CreateLease(newLease);
                }

                //Send email landlord
                try
                {
                    ///TODO: Need landlord profile, approveUrl, disapproveUrl
                    UserProfile landlord = _userProfileRepo.GetUserById(building.OwnerID);
                    string tenantName = String.Format("{0} {1}", RegistrationData.Current.Tenant.ManagerProfile.FirstName, RegistrationData.Current.Tenant.ManagerProfile.LastName);
                    string companyName = _companyRepo.GetCompany((int)RegistrationData.Current.TenantID).CompanyName;
                    LandlordEmailModel landlordModel = new LandlordEmailModel()
                    {
                        FirstName = landlord.FirstName,
                        TenantName = tenantName,
                        BuildingName = building.BuildingName,
                        EmailPath = emailsPath,
                        ImageUrl = imagesUrl,
                        Email = landlord.EmailAddress,
                        ApproveUrl = "http://expresspark.cleversoftware.by",
                        DisapproveUrl = "http://expresspark.cleversoftware.by",
                    };
                    _emailService.SendLandlordEmail(landlordModel);
                }
                catch (Exception)
                {
                }

                //Account
                //Vehicle
                if (RegistrationData.Current.Tenant.Vehicle.VehicleMakeID != null || RegistrationData.Current.Tenant.Vehicle.VehicleModelID != null
                    || !String.IsNullOrEmpty(RegistrationData.Current.Tenant.Vehicle.LicensePlateNumber) || RegistrationData.Current.Tenant.Vehicle.Year != null
                    || !String.IsNullOrEmpty(RegistrationData.Current.Tenant.Vehicle.Color) || !String.IsNullOrEmpty(RegistrationData.Current.Tenant.Vehicle.PermitNumber))
                {
                    Vehicle newVehicle = Mapper.DynamicMap<VehicleModels,Vehicle>(RegistrationData.Current.Tenant.Vehicle);
                    newVehicle.UserProfileID = profile.UserProfileID;
                    newVehicle.VehicleID = _vehicleRepo.CreateVehicle(newVehicle);
            //TODO: ERROR!        _alertsManager.CreateOrDeleteAlertForVehicle(newVehicle, manager.UserProfileID);
                }

                //Company paying
                if (!String.IsNullOrEmpty(RegistrationData.Current.Tenant.EmailString))
                {
                    string[] emails;
                    string[] names;
                    string[] paying;
                    emails = RegistrationData.Current.Tenant.EmailString.Split(' ');
                    names = RegistrationData.Current.Tenant.NameString.Split('$');
                    paying = RegistrationData.Current.Tenant.PayingString.Split('$');
                    if (emails != null)
                    {
                        //TODO: need approve and disapprove urls
                        try
                        {
                            EmployeeEmailModel employeeModel = new EmployeeEmailModel()
                            {
                                TenantName = String.Concat(RegistrationData.Current.Tenant.ManagerProfile.FirstName, " ", RegistrationData.Current.Tenant.ManagerProfile.LastName),
                                BuildingName = building.BuildingName,
                                EmailPath = emailsPath,
                                ImageUrl = imagesUrl,
                                ApproveUrl = "http://expresspark.cleversoftware.by",
                                DisapproveUrl = "http://expresspark.cleversoftware.by",
                            };
                            string companyName = _companyRepo.GetCompany((int)RegistrationData.Current.TenantID).CompanyName;
                            for (int i = 0; i < emails.Length; i++)
                            {
                                employeeModel.Email = emails[i];
                                employeeModel.FirstName = names[i];
                                employeeModel.IsHas = paying[i] == "1";
                                _emailService.SendEmployeeEmail(employeeModel);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (RegistrationData.Current.Tenant.Payment.CreditCard != null || RegistrationData.Current.Tenant.Payment.OnlineCheck != null)
                {
                    Payment newPayment = Mapper.DynamicMap<PaymentModel,Payment>(RegistrationData.Current.Tenant.Payment);
                    newPayment.UserProfileID = profile.UserProfileID;
                    if (RegistrationData.Current.Tenant.Payment.isCreditCard)
                    {
                        if (!RegistrationData.Current.Tenant.Payment.CreditCard.isHome)
                        {
                            Address address = Mapper.DynamicMap<CreditCardModel, Address>(RegistrationData.Current.Tenant.Payment.CreditCard);
                            RegistrationData.Current.Tenant.Payment.CreditCard.AddressID = _addressRepo.CreateAddress(address);
                            
                        }
                        else
                        {
                            RegistrationData.Current.Tenant.Payment.CreditCard.AddressID = companyAddressID;
                        }
                        CreditCard card = Mapper.DynamicMap<CreditCardModel,CreditCard>(RegistrationData.Current.Tenant.Payment.CreditCard);
                        newPayment.CreditCard = card;
                        if (!String.IsNullOrEmpty(newPayment.CreditCard.CardNumber.Trim()))
                        {
                            newPayment.CreditCard.CardNumber = RegistrationData.Current.Tenant.Payment.CreditCard.CardNumber.Replace(" ", "");
                            if (!newPayment.CreditCard.IsCardNumberValid())
                            _alertsManager.CreateCreditCardAlert(profile.UserProfileID);
                        }
                        _paymentRepo.CreatePaymentCreditCard(newPayment);
                    }
                    else
                    {
                        OnlineCheck check = Mapper.DynamicMap<OnlineCheckModel, OnlineCheck>(RegistrationData.Current.Tenant.Payment.OnlineCheck);
                        check.OnlineCheckingTypeID = check.OnlineCheckingTypeID == 0 ? 1 : check.OnlineCheckingTypeID; 
                        newPayment.OnlineCheck = check;
                        _paymentRepo.CreatePaymentOnlineCheck(newPayment);
                    }
                }
                scope.Complete();
            }
        }

        #endregion

        #region Building Owner

        public void SaveBuildingInformation(BuildingInformationModel model)
        {
            BuildingModel building = Mapper.DynamicMap<BuildingInformationModel,BuildingModel>(model);
            AddressModel address = Mapper.DynamicMap<BuildingInformationModel,AddressModel>(model);
            building.BuildingAddress = address;

            RegistrationData.Current.Owner.Building = building;
            SetCurrentStep(13);
        }

        public void SaveOwnershipInformation(OwnershipInformationModel model)
        {
            ContactModel contact = Mapper.DynamicMap<OwnershipInformationModel, ContactModel>(model);
            RegistrationData.Current.Owner.OwnerContact = contact;
            SetCurrentStep(14);
        }

        public void SavePropertyManagementInformation(PropertyManagementInformationModel model)
        {
            ContactModel contact = Mapper.DynamicMap<PropertyManagementInformationModel, ContactModel>(model);
            RegistrationData.Current.Owner.PropertyManagementContact = contact;
            SetCurrentStep(15);
        }

        public void SaveParkingManagementInformation(ParkingManagementInformationModel model)
        {
            ContactModel contact = Mapper.DynamicMap<ParkingManagementInformationModel, ContactModel>(model);  
            RegistrationData.Current.Owner.ParkingManagementContact = contact;
            RegistrationData.Current.Owner.Same = model.Same;
            SetCurrentStep(16);
        }

        public void SaveBankAccountInformation(BankAccountInformationModel model)
        {
            OnlineCheckModel onlineCheck = new OnlineCheckModel()
            {
                BankName = model.BankName,
                CheckingAccountNumber = model.AccountNumber,
                NameOnAccount = model.AccountHolderName,
                RoutingNumber = model.BankRoutingNumber,
                OnlineCheckingTypeID = 2
            };
            RegistrationData.Current.Owner.Building.BankAccount = onlineCheck;
            SetCurrentStep(17);
        }
        
        public void SaveHandlerInformation(int handlerType)
        {
            RegistrationData.Current.Owner.HandlerType = handlerType;
            SetCurrentStep(18);
        }

        public void SaveParkingLotInformationReserved(StallsInformationModel model)
        {
            ParkingStallModel stall = Mapper.DynamicMap<StallsInformationModel,ParkingStallModel>(model);
            RegistrationData.Current.Owner.StallReserved = stall;
            SetCurrentStep(19);
        }

        public void SaveParkingLotInformationUnreserved(StallsUnreservedInformationModel model)
        {
            ParkingStallModel stall = Mapper.DynamicMap<StallsUnreservedInformationModel, ParkingStallModel>(model);
            RegistrationData.Current.Owner.StallUnreserved = stall;
            SetCurrentStep(20);
        }

        public void SaveParkingLotInformationVisitor(StallsInformationModel model)
        {
            ParkingStallModel stall = Mapper.DynamicMap<StallsInformationModel, ParkingStallModel>(model);
            RegistrationData.Current.Owner.StallVisitors = stall;
            SetCurrentStep(21);
        }

        public void SaveTenant(TenantModel model)
        {
            string[] emails = new string[] { };
            string[] companies = new string[] { };
            RegistrationData.Current.Owner.Companies = new List<string>();
            if (!String.IsNullOrEmpty(model.Emails))
            {
                emails = model.Emails.Split(' ');
            }
            RegistrationData.Current.Owner.Tenants.Clear();
            if (emails.Count() > 0)
            {
                foreach (var email in emails)
                {
                    RegistrationData.Current.Owner.Tenants.Add(email);
                }
            }
            if (!String.IsNullOrEmpty(model.Companies))
            {
                companies = model.Companies.Split('$');
            }
            if (RegistrationData.Current.Owner.Companies != null && RegistrationData.Current.Owner.Companies.Count != 0)
            {
                RegistrationData.Current.Owner.Companies.Clear();
            }
            if (companies.Count() > 0)
            {
                foreach (var company in companies)
                {
                    RegistrationData.Current.Owner.Companies.Add(company);
                }
            } 
        }

        public void SaveBuildingOwner(string loginName)
        {
            string emailsPath = String.Format("{0}Emails/", RegistrationData.Current.ServerPath);
            string imagesUrl = String.Format("{0}Images/", RegistrationData.Current.RootUrl);

            UserProfile profile = _userProfileRepo.GetUser(loginName, (int)UserProfileStatusEnum.Verified);

            BuildingOwnerModel owner = RegistrationData.Current.Owner;
            ContactModel ownerContact = owner.OwnerContact;

            BuildingModel building = owner.Building;
            building.IsActive = true;
            building.OwnerID = profile.UserProfileID;
            building.ManagerID = profile.UserProfileID;

            using (TransactionScope scope = new TransactionScope())
            {
                Address newsAddress = Mapper.DynamicMap<AddressModel, Address>(building.BuildingAddress);
                int addressID = _addressRepo.CreateAddress(newsAddress);
                building.AddressID = addressID;
                building.MailingAddressID = addressID;

                UserProfile user = new UserProfile()
                {
                    LoginName = loginName,
                    UserProfileTypeID = (int)UserProfileTypeEnum.BuildingOwner,
                    EmailAddress = ownerContact.Email,
                    FirstName = ownerContact.FirstName,
                    LastName = ownerContact.LastName,
                    UserProfileStatusID = (int)UserProfileStatusEnum.Active,
                    AddressID = addressID,
                };
                _userProfileRepo.UpdateProfile(user);

                if (!String.IsNullOrEmpty(RegistrationData.Current.Owner.OwnerContact.PhoneNumber))
                {
                    Phone phone = new Phone
                                      {
                                          PhoneNumber = owner.OwnerContact.PhoneNumber,
                                          PhoneTypeID = owner.OwnerContact.PhoneTypeID
                                      };

                    _phoneRepo.CreatePhoneForUser(phone, profile.UserProfileID);
                }

                OnlineCheck check = Mapper.DynamicMap<OnlineCheckModel,OnlineCheck>(building.BankAccount);
                Payment payment = new Payment
                {
                    isCreditCard = false,
                    OnlineCheck = check,
                    UserProfileID = profile.UserProfileID
                };

                _paymentRepo.CreatePaymentOnlineCheck(payment);

                //TODO: Need property management email template
                if (owner.HandlerType == 1)
                {
                    try
                    {
                        InviteTenantModel inviteModel = new InviteTenantModel()
                        {
                            ServerPath = RegistrationData.Current.RootUrl,
                            Email = owner.OwnerContact.Email,
                            BuildingName = building.BuildingName,
                     //       TenantName = 
                            EmailPath = emailsPath,
                            ImageUrl = imagesUrl,
                        };
                        _emailService.SendInviteTenantEmail(inviteModel);
                    }
                    catch (Exception)
                    { }
                }
                //TODO: Need save handler type
                Building newBuilding = Mapper.DynamicMap<BuildingModel,Building>(building);
                newBuilding.BuildingID = _buildingRepo.CreateBuilding(newBuilding);

                Lot lot = new Lot
                              {
                                  ParkingManagerID = profile.UserProfileID,
                                  ParkingOperatorID = profile.UserProfileID,
                                  IsActive = true
                              };
                lot.LotID = _lotRepo.CreateLot(lot);

                BuildingToLot buildingToLot = new BuildingToLot()
                {
                    BuildingID = newBuilding.BuildingID,
                    LotID = lot.LotID,
                };
                _lotRepo.AddBuildingToLot(buildingToLot);

                owner.StallReserved.LotID = lot.LotID;
                owner.StallReserved.ParkingStallTypeID = 1;
                owner.StallReserved.isActive = true;
                ParkingStall stall = Mapper.DynamicMap<ParkingStallModel,ParkingStall>(owner.StallReserved);
                _parkingStallRepo.CreateParkingStall(stall);

                owner.StallUnreserved.LotID = lot.LotID;
                owner.StallUnreserved.ParkingStallTypeID = 2;
                owner.StallUnreserved.isActive = true;
                stall = Mapper.DynamicMap<ParkingStallModel, ParkingStall>(owner.StallUnreserved);
                _parkingStallRepo.CreateParkingStall(stall);

                owner.StallVisitors.LotID = lot.LotID;
                owner.StallVisitors.ParkingStallTypeID = 5;
                owner.StallVisitors.isActive = true;
                stall = Mapper.DynamicMap<ParkingStallModel, ParkingStall>(owner.StallVisitors);
                _parkingStallRepo.CreateParkingStall(stall);

                try
                {
                    if (owner.Tenants.Count > 0)
                    {
                        InviteTenantModel inviteModel = new InviteTenantModel()
                        {
                            ServerPath = RegistrationData.Current.RootUrl,
                            BuildingName = building.BuildingName,
                            EmailPath = emailsPath,
                            ImageUrl = imagesUrl,
                        };
                        for (int i = 0; i < owner.Tenants.Count; i++)
                        {
                            inviteModel.Email = owner.Tenants[i];
                            inviteModel.CompanyName = owner.Companies[i];
                            _emailService.SendInviteTenantEmail(inviteModel);
                        }
                    }
                }
                catch (Exception)
                {
                    //scope.Complete();
                }
                scope.Complete();
            }
        }

        #endregion Building Owner

        string CreateUser(UserProfile profile)
        {
            MembershipCreateStatus createStatus = new MembershipCreateStatus();
            Random rd = new Random();
            string login = String.Format("test{0}",rd.Next(10,99));
            Membership.CreateUser(login, "test", profile.EmailAddress,
                passwordQuestion: null, passwordAnswer: null, isApproved: false, providerUserKey: null, status: out createStatus);//TODO: userName, password - fix value

            if (createStatus == MembershipCreateStatus.Success)
            {
                return login;
            }
            return null;
        }
    }
}
