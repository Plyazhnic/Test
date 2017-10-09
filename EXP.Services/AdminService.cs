using AutoMapper;
using EXP.Core;
using EXP.Core.Interface;
using EXP.DataAccess;
using EXP.Entity;
using EXP.Entity.Enumerations;
using EXP.Services.Interface;
using EXP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Security;

namespace EXP.Services
{
    public class AdminService : IAdminService
    {
        #region private
        private readonly EmailService _emailService;       
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IAddressRepository _addressRepo;
        private readonly IPhoneRepository _phoneRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IBuildingRepository _buildingRepo;
        private readonly ILotRepository _lotRepo;
        private readonly IParkingStallRepository _parkingStallRepo;
        private readonly IParkingStallTypeRepository _parkingStallTypeRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IParkingInventoryRepository _parkingInventoryRepo;
        private readonly IVehicleRepository _vehicleRepo;
        private readonly IVehicleMakeRepository _vehicleMakeRepo;
        private readonly IVehicleModelRepository _vehicleModelRepo;
        private readonly IUserProfileTypeRepository _userProfileTypeRepo;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IKeyCardRepository _keyCardRepo;
        private readonly IzStateRepository _zStateRepo;
        private readonly ILeaseRepository _leaseRepo;
        private readonly IValidationBookRepository _validationBookRepo;
        #endregion

        public AdminService() 
        {
            _emailService = new EmailService();          
            _userProfileRepo = new UserProfileRepository(); 
            _addressRepo = new AddressRepository();
            _phoneRepo = new PhoneRepository(); 
            _paymentRepo = new PaymentRepository();
            _buildingRepo = new BuildingRepository();
            _lotRepo = new LotRepository();
            _parkingStallRepo = new ParkingStallRepository();
            _parkingStallTypeRepo = new ParkingStallTypeRepository();
            _companyRepo = new CompanyRepository();
            _parkingInventoryRepo = new ParkingInventoryRepository();
            _vehicleRepo = new VehicleRepository();
            _vehicleMakeRepo = new VehicleMakeRepository();
            _vehicleModelRepo = new VehicleModelRepository();
            _userProfileTypeRepo = new UserProfileTypeRepository();
            _leaseRepository = new LeaseRepository();
            _keyCardRepo = new KeyCardRepository();
            _zStateRepo = new zStateRepository();
            _leaseRepo = new LeaseRepository();
            _validationBookRepo = new ValidationBookRepository();
        }

        #region Users
        /// <summary>
        /// return list of users by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<UserProfileModel> ListUsers(JQueryDataTableParamModel param)
        {
            TableList listParam = new TableList()
            {
                Start = param.iDisplayStart,
                End = param.iDisplayStart + param.iDisplayLength,
                SearchString = param.sSearch ?? "",
            };

            List<UserProfile> list = _userProfileRepo.ListUsers(listParam);
            List<UserProfileModel> profiles = new List<UserProfileModel>();
            UserProfileModel profile;
            foreach (var item in list)
            {
                profile = Mapper.DynamicMap<UserProfile, UserProfileModel>(item);
                profile.UserName = item.LoginName;
                profiles.Add(profile);
            }
            return profiles; 

        }
        
        /// <summary>
        /// return count of users by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountUsers(string searchString)
        {
            return _userProfileRepo.GetUsersCount(searchString);
        }

        /// <summary>
        /// Get user by UserProfileID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProfileModel GetUser(int userId)
        {
            UserProfile user = _userProfileRepo.GetUserById(userId);
            UserProfileModel model = Mapper.DynamicMap<UserProfile,UserProfileModel>(user);
            return model;
        }
        
        /// <summary>
        /// set staus of user by id
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="status"></param>
        public void SetStatus(int userProfileId, int status)
        {
            _userProfileRepo.SetStatus(userProfileId, status);
        }
        
        /// <summary>
        /// return EditUserModels by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditUserModel GetEditUserModel(int userId)
        {
            EditUserModel editUser = new EditUserModel();
            UserProfile user = _userProfileRepo.GetUserById(userId);
            if (user != null)
            {
                editUser.User = Mapper.DynamicMap<UserProfile,UserProfileModel>(user);
                editUser.User.UserName = user.LoginName;
            }
            if (editUser.User != null)
            {
                if (editUser.User.AddressID > 0)
                {
                    Address address = _addressRepo.GetAddress((int)editUser.User.AddressID);
                    editUser.PersonalAddress = Mapper.DynamicMap<Address, AddressModel>(address);
                }
                
                if (editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingManager &&
                        editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingAttendant)
                {
                    Company company = _companyRepo.GetCompanyForUser(userId);
                    editUser.Company = Mapper.DynamicMap<Company,CompanyModel>(company);
                    editUser.Company.Address = Mapper.DynamicMap<Address, AddressModel>(company.Address);
                    if (editUser.PersonalAddress.AddressID == editUser.Company.AddressID)
                    {
                        editUser.iSCompany = true;
                    }
                    if (editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.Tenant || editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.BuildingOwner ||
                                    editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.BuildingManager || editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.ParkingOperator)
                    {
                        editUser.BuildingID = _buildingRepo.GetBuildingForCompany(editUser.Company.CompanyID).BuildingID;
                    }
                }
                List<Phone> phones = _phoneRepo.GetAllPhonesForUser(userId);
                PhoneModel phoneModel;
                foreach (var phone in phones)
                {
                    phoneModel = Mapper.DynamicMap<Phone,PhoneModel>(phone);
                    switch (phoneModel.PhoneTypeID)
                    {
                        case (int)PhoneTypeEnum.Work :
                            editUser.WorkPhone = phoneModel;
                            break;
                        case (int)PhoneTypeEnum.Fax:
                            editUser.WorkFax = phoneModel;
                            break;
                        case (int)PhoneTypeEnum.Mobile:
                            editUser.CellPhone = phoneModel;
                            break;
                    }
                }
                //editUser.WorkPhone = Mapper.DynamicMap<Phone,PhoneModel>(_phoneRepo.GetPhoneForUserByType(userId, (int)PhoneTypeEnum.Work));
                //editUser.WorkFax = Mapper.DynamicMap<Phone,PhoneModel>(_phoneRepo.GetPhoneForUserByType(userId, (int)PhoneTypeEnum.Fax));
                //editUser.CellPhone = Mapper.DynamicMap<Phone,PhoneModel>(_phoneRepo.GetPhoneForUserByType(userId, (int)PhoneTypeEnum.Mobile));
            }
            return editUser;
        }

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="editUser"></param>
        public void UpdateUser(EditUserModel editUser)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (editUser.iSCompany)
                {
                    if (editUser.User.AddressID != editUser.Company.AddressID)
                    {
                        _addressRepo.DeleteAddress(editUser.User.AddressID);
                        editUser.User.AddressID = (int)editUser.Company.AddressID;
                    }
                }
                else
                {
                    Address address = Mapper.DynamicMap<AddressModel,Address>(editUser.PersonalAddress);
                    if (editUser.Company.AddressID == null || editUser.Company.AddressID == 0 && editUser.User.AddressID == editUser.Company.AddressID)
                    {
                        editUser.User.AddressID = _addressRepo.CreateAddress(address);
                    }
                    else
                    {
                        _addressRepo.UpdateAddress(address);
                    }
                }
                //TODO:!!!! update building relationship
                UserProfile newUser = Mapper.DynamicMap<UserProfileModel, UserProfile>(editUser.User);
                newUser.LoginName = editUser.User.UserName;
                _userProfileRepo.UpdateProfile(newUser);

                editUser.WorkPhone.PhoneTypeID = (int)PhoneTypeEnum.Work;
                if (editUser.WorkPhone.PhoneID == 0)
                {
                    if (editUser.WorkPhone.PhoneNumber != null)
                    {
                        _phoneRepo.CreatePhoneForUser(Mapper.DynamicMap<PhoneModel,Phone>(editUser.WorkPhone), editUser.User.UserProfileID);
                    }
                }
                else
                {
                    _phoneRepo.UpdatePhone(Mapper.DynamicMap<PhoneModel, Phone>(editUser.WorkPhone));
                }
                editUser.WorkFax.PhoneTypeID = (int)PhoneTypeEnum.Fax;
                if (editUser.WorkFax.PhoneID == 0)
                {
                    if (editUser.WorkFax.PhoneNumber != null)
                    {
                        _phoneRepo.CreatePhoneForUser(Mapper.DynamicMap<PhoneModel, Phone>(editUser.WorkFax), editUser.User.UserProfileID);
                    }
                }
                else
                {
                    _phoneRepo.UpdatePhone(Mapper.DynamicMap<PhoneModel, Phone>(editUser.WorkFax));
                }
                editUser.CellPhone.PhoneTypeID = (int)PhoneTypeEnum.Mobile;
                if (editUser.CellPhone.PhoneID == 0)
                {
                    if (editUser.CellPhone.PhoneNumber != null)
                    {
                        _phoneRepo.CreatePhoneForUser(Mapper.DynamicMap<PhoneModel, Phone>(editUser.CellPhone), editUser.User.UserProfileID);
                    }
                }
                else
                {
                    _phoneRepo.UpdatePhone(Mapper.DynamicMap<PhoneModel, Phone>(editUser.CellPhone));
                }

                if (editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingManager &&
                            editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingAttendant &&
                            editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.Employee)
                {
                    _addressRepo.UpdateAddress(Mapper.DynamicMap<AddressModel,Address>(editUser.Company.Address));

                    editUser.Company.WorkPhone.PhoneTypeID = (int)PhoneTypeEnum.Work;
                    if (editUser.Company.WorkPhone.PhoneID != 0)
                    {
                        _phoneRepo.UpdatePhone(Mapper.DynamicMap<PhoneModel,Phone>(editUser.Company.WorkPhone));
                    }
                    else
                    {
                        if (editUser.Company.WorkPhone.PhoneNumber != null)
                        {
                            editUser.Company.WorkPhoneID = _phoneRepo.CreatePhone(Mapper.DynamicMap<PhoneModel,Phone>(editUser.Company.WorkPhone));
                        }
                    }

                    editUser.Company.Fax.PhoneTypeID = (int)PhoneTypeEnum.Fax;
                    if (editUser.Company.Fax.PhoneID != 0)
                    {
                        _phoneRepo.UpdatePhone(Mapper.DynamicMap<PhoneModel,Phone>(editUser.Company.Fax));
                    }
                    else
                    {
                        if (editUser.Company.Fax.PhoneNumber != null)
                        {
                            editUser.Company.FaxID = _phoneRepo.CreatePhone(Mapper.DynamicMap<PhoneModel,Phone>(editUser.Company.Fax));
                        }
                    }
                    _companyRepo.UpdateCompany(Mapper.DynamicMap<CompanyModel,Company>(editUser.Company));
                }
                scope.Complete();
            }
        }
        
        /// <summary>
        /// create user
        /// </summary>
        /// <param name="editUser"></param>
        public void CreateUser(EditUserModel editUser)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(editUser.User.UserName, editUser.Password, editUser.User.EmailAddress, passwordQuestion: null, passwordAnswer: null,
                    isApproved: false, providerUserKey: null, status: out createStatus);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    if (editUser.Company.CompanyID == 0)
                    {
                        if (editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingManager &&
                            editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingAttendant)
                        {
                            editUser.Company.AddressID = _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editUser.Company.Address));
                            if (editUser.Company.WorkPhone.PhoneNumber != null && editUser.Company.WorkPhone.PhoneNumber.Trim() != String.Empty)
                            {
                                editUser.Company.WorkPhone.PhoneTypeID = (int)PhoneTypeEnum.Work;
                                editUser.Company.WorkPhoneID = _phoneRepo.CreatePhone(Mapper.DynamicMap<PhoneModel,Phone>(editUser.Company.WorkPhone));
                            }
                            if (editUser.Company.Fax.PhoneNumber != null && editUser.Company.Fax.PhoneNumber.Trim() != String.Empty)
                            {
                                editUser.Company.Fax.PhoneTypeID = (int)PhoneTypeEnum.Fax;
                                editUser.Company.FaxID = _phoneRepo.CreatePhone(Mapper.DynamicMap<PhoneModel,Phone>(editUser.Company.Fax));
                            }
                            editUser.Company.CompanyID = _companyRepo.CreateCompany(Mapper.DynamicMap<CompanyModel,Company>(editUser.Company));

                            if (editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.Tenant || editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.BuildingOwner ||
                                editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.BuildingManager || editUser.User.UserProfileTypeID == (int)UserProfileTypeEnum.ParkingOperator)
                            {
                                if (editUser.BuildingID != 0)
                                {
                                    BuildingToCompany relation = new BuildingToCompany
                                    {
                                        BuildingID = editUser.BuildingID,
                                        CompanyID = editUser.Company.CompanyID,
                                    };
                                    _buildingRepo.AddBuildingToCompany(relation);
                                }
                            }
                        }
                    }

                    editUser.User.AddressID = editUser.iSCompany ? (int)editUser.Company.AddressID :
                        _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editUser.PersonalAddress));

                    UserProfile user = _userProfileRepo.GetUser(editUser.User.UserName, 0);
                    editUser.User.UserProfileStatusID = (int)UserProfileStatusEnum.Active;
                    UserProfile newUser = Mapper.DynamicMap<UserProfileModel, UserProfile>(editUser.User);
                    newUser.LoginName = editUser.User.UserName;
                    _userProfileRepo.UpdateProfile(newUser);

                    if (editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingManager &&
                            editUser.User.UserProfileTypeID != (int)UserProfileTypeEnum.ParkingAttendant)
                    {
                        _companyRepo.AddUserToCompany(editUser.Company.CompanyID, user.UserProfileID);
                    }

                    if (editUser.WorkPhone.PhoneNumber != null && editUser.WorkPhone.PhoneNumber.Trim() != String.Empty)
                    {
                        editUser.WorkPhone.PhoneTypeID = (int)PhoneTypeEnum.Work;
                        _phoneRepo.CreatePhoneForUser(Mapper.DynamicMap<PhoneModel,Phone>(editUser.WorkPhone), user.UserProfileID);
                    }
                    if (editUser.WorkFax.PhoneNumber != null && editUser.WorkFax.PhoneNumber.Trim() != String.Empty)
                    {
                        editUser.WorkFax.PhoneTypeID = (int)PhoneTypeEnum.Fax;
                        _phoneRepo.CreatePhoneForUser(Mapper.DynamicMap<PhoneModel,Phone>(editUser.WorkFax), user.UserProfileID);
                    }
                    if (editUser.CellPhone.PhoneNumber != null && editUser.CellPhone.PhoneNumber.Trim() != String.Empty)
                    {
                        editUser.CellPhone.PhoneTypeID = (int)PhoneTypeEnum.Mobile;
                        _phoneRepo.CreatePhoneForUser(Mapper.DynamicMap<PhoneModel,Phone>(editUser.CellPhone), user.UserProfileID);
                    }
                    scope.Complete();
                }
            }
        }

        #endregion

        #region Buildings
        
        /// <summary>
        /// return list of buildings by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<BuildingModel> ListBuildings(JQueryDataTableParamModel param)
        {
            TableList listParam = new TableList()
            {
                Start = param.iDisplayStart,
                End = param.iDisplayStart + param.iDisplayLength,
                SearchString = param.sSearch ?? "",
            };
            List<Building> list = _buildingRepo.ListBuildingsForPage(listParam);
            List<BuildingModel> buildings = new List<BuildingModel>();
            BuildingModel building;
            foreach (var item in list)
            {
                building = Mapper.DynamicMap<Building, BuildingModel>(item);
                building.Owner = Mapper.DynamicMap<UserProfile, UserProfileModel>(item.Owner);
                buildings.Add(building);
            }
            return buildings; 
        }

        /// <summary>
        /// return count of buildings by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountBuildings(string searchString)
        {
            return _buildingRepo.GetBuildingsCount(searchString);
        }

        /// <summary>
        /// get EditBuildingModels by edit of building
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public EditBuildingModel GetEditBuildingModel(int buildingId)
        {
            EditBuildingModel building = new EditBuildingModel();
            building.Building = Mapper.DynamicMap<Building,BuildingModel>(_buildingRepo.GetBuilding(buildingId));
            building.BuildingAddress = Mapper.DynamicMap<Address,AddressModel>(_addressRepo.GetAddress(building.Building.AddressID));
            if (building.Building.AddressID != 0 && building.Building.MailingAddressID == building.Building.AddressID)
            {
                building.iSMailing = true;
                building.MailingAddress = building.BuildingAddress;
            }
            else
            {
                building.MailingAddress = Mapper.DynamicMap<Address,AddressModel>(_addressRepo.GetAddress(building.Building.MailingAddressID));
            }
            return building;
        }

        /// <summary>
        /// update building
        /// </summary>
        /// <param name="editBuilding"></param>
        public void UpdateBuilding(EditBuildingModel editBuilding)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _addressRepo.UpdateAddress(Mapper.DynamicMap<AddressModel,Address>(editBuilding.BuildingAddress));
                if (editBuilding.Building.MailingAddressID == editBuilding.Building.AddressID)
                {
                    if (!editBuilding.iSMailing)
                    {
                        editBuilding.Building.MailingAddressID = _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editBuilding.MailingAddress));
                    }
                    _buildingRepo.UpdateBuilding(Mapper.DynamicMap<BuildingModel, Building>(editBuilding.Building));
                }
                else
                {
                    if (!editBuilding.iSMailing)
                    {
                        _addressRepo.UpdateAddress(Mapper.DynamicMap<AddressModel,Address>(editBuilding.MailingAddress));
                        _buildingRepo.UpdateBuilding(Mapper.DynamicMap<BuildingModel, Building>(editBuilding.Building));
                    }
                    else
                    {
                        int oldAddressId = editBuilding.Building.MailingAddressID;
                        editBuilding.Building.MailingAddressID = editBuilding.Building.AddressID;
                        _buildingRepo.UpdateBuilding(Mapper.DynamicMap<BuildingModel, Building>(editBuilding.Building));
                        _addressRepo.DeleteAddress(oldAddressId);
                    }
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// create building
        /// </summary>
        /// <param name="editBuilding"></param>
        public void CreateBuilding(EditBuildingModel editBuilding)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                editBuilding.Building.AddressID = _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editBuilding.BuildingAddress));
                editBuilding.Building.MailingAddressID = editBuilding.iSMailing == false ?
                    _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editBuilding.MailingAddress)) : editBuilding.Building.AddressID;

                _buildingRepo.CreateBuilding(Mapper.DynamicMap<BuildingModel,Building>(editBuilding.Building));

                scope.Complete();
            }
        }

        /// <summary>
        /// get building by buildingId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BuildingModel GetBuilding(int buildingId)
        {
            Building building = _buildingRepo.GetBuilding(buildingId);
            BuildingModel model = Mapper.DynamicMap<Building,BuildingModel>(building);
            Address address = _addressRepo.GetAddress(building.AddressID);
            model.BuildingAddress = Mapper.DynamicMap<Address,AddressModel>(address);
            return model;
        }

        /// <summary>
        /// return list building by search string
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<BuildingWideModel> SearchBuilding(SearchBuildingModel model)
        {
            SearchBuilding searchParam = new SearchBuilding(){
                BuildingName = (model.BuildingName ?? "").Trim(),
                OwnersFirstName = (model.OwnersFirstName ?? "").Trim(),
                OwnersLastName = (model.OwnersLastName ?? "").Trim(),
                ManagerFirstName = (model.ManagerFirstName ?? "").Trim(),
                ManagerLastName = (model.ManagerLastName ?? "").Trim(),
                City = (model.City ?? "").Trim(),
                StateID = (model.StateID != 0 ? model.StateID : new Int32()),
                ZipCode = (model.ZipCode ?? "").Trim()
            };
            List<BuildingWide> buildings = _buildingRepo.SearchBuildings(searchParam);
            List<BuildingWideModel> newBuildings = new List<BuildingWideModel>();
            BuildingWideModel newBuilding;
            foreach (var item in buildings)
            {
                newBuilding = Mapper.DynamicMap<BuildingWide,BuildingWideModel>(item);
                newBuildings.Add(newBuilding);
            }
            return newBuildings;
        }
        
        /// <summary>
        /// delete building by buildingId
        /// </summary>
        /// <param name="buildingId"></param>
        public void DeleteBuilding(int buildingId)
        {
            _buildingRepo.DeleteBuilding(buildingId);
        }

        /// <summary>
        /// get building by lotid
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public BuildingModel GetBuildingByLotID(int lotId)
        {
            //TODO: BuildingAddress
            Building building = _lotRepo.GetBuildingAddress(lotId);
            BuildingModel model = Mapper.DynamicMap<Building, BuildingModel>(building);
            AddressModel address = Mapper.DynamicMap<Address, AddressModel>(building.BuildingAddress);
            model.BuildingAddress = address;
            return model;
        }
        #endregion

        #region Lots
        /// <summary>
        /// return lot by id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public LotModel GetLot(int lotId)
        {
            Lot lot = _lotRepo.GetLot(lotId);
            LotModel model = Mapper.DynamicMap<Lot,LotModel>(lot);
            AddressModel address = Mapper.DynamicMap<Address,AddressModel>(lot.Address);
            StateModel state = Mapper.DynamicMap<zState,StateModel>(lot.Address.State);
            address.State = state;
            model.Address = address;
            return model;
        }

        /// <summary>
        /// return list lots by search string
        /// </summary>
        /// <param name="dtModel"></param>
        /// <returns></returns>
        public List<LotModel> ListLots(DataTableLotModel dtModel)
        {
            TableList listParam = new TableList()
            {
                Start = dtModel.Start,
                End = dtModel.End,
                SearchString = dtModel.SearchString,
                BuildingID = dtModel.BuildingID == 0 ? null : dtModel.BuildingID,
            };
            List<Lot> list = _lotRepo.ListLotsForPage(listParam);
            List<LotModel> lots = new List<LotModel>();
            LotModel lot;
            BuildingModel building;
            AddressModel address;
            foreach (var item in list)
            {
                lot = Mapper.DynamicMap<Lot, LotModel>(item);
                building = Mapper.DynamicMap<Building, BuildingModel>(item.Building);
                address = Mapper.DynamicMap<Address,AddressModel>(item.Address);
                lot.Building = building;
                lot.Address = address;
                lots.Add(lot);
            }
            return lots; 
        }

        /// <summary>
        /// return count of lots by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountLots(DataTableLotModel dtModel)
        {
            TableList listParam = new TableList()
            {
                SearchString = dtModel.SearchString,
                BuildingID = dtModel.BuildingID == 0 ? null : dtModel.BuildingID,
            };
            return _lotRepo.GetLotsCount(listParam);
        }

        /// <summary>
        /// return editlotModel by lotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public EditLotModel GetEditLotModel(int lotId)
        {
            EditLotModel editLot = new EditLotModel();
            editLot.Lot = Mapper.DynamicMap<Lot,LotModel>(_lotRepo.GetLot(lotId));
            editLot.LotAddress = Mapper.DynamicMap<Address,AddressModel>(_addressRepo.GetAddress(editLot.Lot.AddressID));
            if (editLot.Lot.IsBuilding)
            {
                editLot.iSBuilding = true;
            }
            if (editLot.Lot.AddressID == editLot.Lot.MailingAddressID)
            {
                editLot.iSLot = true;
                editLot.MailingAddress = editLot.LotAddress;
            }
            else
            {
                editLot.MailingAddress = Mapper.DynamicMap<Address,AddressModel>(_addressRepo.GetAddress(editLot.Lot.MailingAddressID));
            }
            List<Building> buildings = _buildingRepo.GetBuildingsForLot(editLot.Lot.LotID);   
            foreach (var item in buildings)
            {
                editLot.BuildingIDString = buildings.IndexOf(item) == 0 ? item.BuildingID.ToString() : String.Concat(editLot.BuildingIDString, "$", item.BuildingID.ToString());
            }

            return editLot;
        }

        /// <summary>
        /// update lot
        /// </summary>
        /// <param name="editBuilding"></param>
        public void UpdateLot(EditLotModel editLot)
        {
            List<String> buildingIDs = new List<String>();
            using (TransactionScope scope = new TransactionScope())
            {
                if (editLot.Lot.MailingAddressID == editLot.Lot.AddressID)
                {
                    if (!editLot.iSLot)
                    {
                        editLot.Lot.MailingAddressID = _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.MailingAddress));
                    }
                    if (!editLot.iSBuilding)
                    {
                        if (!editLot.Lot.IsBuilding)
                        {
                            _addressRepo.UpdateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.LotAddress));
                        }
                        else
                        {
                            editLot.Lot.AddressID = _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.LotAddress));
                        }
                    }
                    else
                    {
                        if (!editLot.Lot.IsBuilding)
                        {
                            _addressRepo.DeleteAddress(editLot.Lot.AddressID);
                            editLot.Lot.AddressID = editLot.LotAddress.AddressID;
                        }
                    }
                    editLot.Lot.MailingAddressID = editLot.iSLot ? editLot.Lot.AddressID : editLot.Lot.MailingAddressID;
                }
                else
                {
                    if (!editLot.iSBuilding)
                    {
                        if (!editLot.Lot.IsBuilding)
                        {
                            _addressRepo.UpdateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.LotAddress));
                        }
                        else
                        {
                            editLot.Lot.AddressID = _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.LotAddress));
                        }
                    }
                    else
                    {
                        if (!editLot.Lot.IsBuilding)
                        {
                            _addressRepo.DeleteAddress(editLot.Lot.AddressID);
                            editLot.Lot.AddressID = editLot.LotAddress.AddressID;
                        }
                    }

                    if (!editLot.iSLot)
                    {
                        _addressRepo.UpdateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.MailingAddress));
                    }
                    else
                    {
                        _addressRepo.DeleteAddress(editLot.Lot.MailingAddressID);
                        editLot.Lot.MailingAddressID = editLot.Lot.AddressID;
                    }
                }
                List<Building> buildings = _buildingRepo.GetBuildingsForLot(editLot.Lot.LotID);    
                int[] buildingIdMas = new int[buildings.Count];
                for (int i = 0; i < buildings.Count; i++)
                {
                    buildingIdMas[i] = buildings[0].BuildingID;
                }

                BuildingToLot relation = new BuildingToLot();
                relation.LotID = editLot.Lot.LotID;
                buildingIDs = editLot.BuildingIDString.Split('$').ToList();
                bool exit = false;
                foreach (var item in buildings)
                {
                    for (int i = 0; i < buildingIDs.Count(); i++)
                    {
                        if (buildingIDs[i] == item.BuildingID.ToString())
                        {
                            buildingIDs.RemoveAt(i);
                            exit = true;
                        }
                    }
                    if (!exit)
                    {
                        relation.BuildingID = item.BuildingID;
                        _lotRepo.DeleteBuildingToLotEntry(relation);
                    }
                    exit = false;
                }
                if (buildingIDs.Count > 0)
                {
                    foreach (var id in buildingIDs)
                    {
                        relation.BuildingID = Int32.Parse(id);
                        _lotRepo.AddBuildingToLot(relation);
                    }
                }
                editLot.Lot.IsBuilding = editLot.iSBuilding;
                _lotRepo.UpdateLot(Mapper.DynamicMap<LotModel,Lot>(editLot.Lot));

                scope.Complete();
            }
        }

        /// <summary>
        /// create lot
        /// </summary>
        /// <param name="editBuilding"></param>
        public void CreateLot(EditLotModel editLot)
        {
            List<String> buildingIDs = new List<String>();
            using (TransactionScope scope = new TransactionScope())
            {
                editLot.Lot.AddressID = editLot.iSBuilding == false ?
                        _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.LotAddress)) : editLot.LotAddress.AddressID;
                editLot.Lot.MailingAddressID = editLot.iSLot == false ?
                    _addressRepo.CreateAddress(Mapper.DynamicMap<AddressModel,Address>(editLot.MailingAddress)) : editLot.Lot.AddressID;

                editLot.Lot.IsBuilding = editLot.iSBuilding;
                BuildingToLot relation = new BuildingToLot();
                relation.LotID = _lotRepo.CreateLot(Mapper.DynamicMap<LotModel,Lot>(editLot.Lot));
                buildingIDs = editLot.BuildingIDString.Split('$').ToList();
                foreach (var id in buildingIDs)
                {
                    relation.BuildingID = Int32.Parse(id);
                    _lotRepo.AddBuildingToLot(relation);
                }

                scope.Complete();
            }
        }

        public List<LotWideModel> SearchLots(SearchLotModel model)
        {
            SearchLot searchParam = new SearchLot() { 
                LotName = (model.LotName ?? "").Trim(),
                City = (model.City ?? "").Trim(),
                ZipCode = (model.ZipCode ?? "").Trim(),
                StateID = model.StateID,
            };

            List<LotWide> list = _lotRepo.SearchLot(searchParam);
            List<LotWideModel> lots = new List<LotWideModel>();
            LotWideModel lot;
            foreach (var item in list)
            {
                lot = Mapper.DynamicMap<LotWide, LotWideModel>(item);
                lots.Add(lot);
            }
            return lots; 

        }
        
        /// <summary>
        /// delete lot
        /// </summary>
        /// <param name="lotId"></param>
        public void DeleteLot(int lotId)
        {
            _lotRepo.DeleteLot(lotId);
        }
        #endregion

        #region Make
        /// <summary>
        /// return list of makes by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<EditMakeModel> ListMakes(JQueryDataTableParamModel param)
        {
            TableList listParam = new TableList()
            {
                Start = param.iDisplayStart,
                End = param.iDisplayStart + param.iDisplayLength,
                SearchString = param.sSearch ?? "",
            };

            List<VehicleMake> listMakes = _vehicleMakeRepo.ListVehicleMakesForPage(listParam);
            List<EditMakeModel> listEditMake = new List<EditMakeModel>();
            foreach (var item in listMakes)
            {
                EditMakeModel editModel = Mapper.DynamicMap<VehicleMake, EditMakeModel>(item);
                listEditMake.Add(editModel);
            }
            return listEditMake;
        }

        /// <summary>
        /// return count of make by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountMakes(string searchString)
        {
            return _vehicleMakeRepo.GetVehicleMakesCount(searchString);
        }

        /// <summary>
        /// create vehicle make
        /// </summary>
        /// <param name="NewModel"></param>
        /// <returns></returns>
        public bool CreateMake(EditMakeModel NewModel)
        {
            VehicleMake make = Mapper.DynamicMap<EditMakeModel,VehicleMake>(NewModel);
            return _vehicleMakeRepo.AddVehicleMake(make);
        }

        /// <summary>
        /// return editmakeModel by makeId
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public EditMakeModel GetEditMakeModel(int makeId)
        {
            VehicleMake make = _vehicleMakeRepo.GetVehicleMakeById(makeId);
            EditMakeModel model = Mapper.DynamicMap<VehicleMake,EditMakeModel>(make);
            return model;
        }

        /// <summary>
        /// update make
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        public bool UpdateMake(EditMakeModel editModel)
        {
            VehicleMake make = Mapper.DynamicMap<EditMakeModel,VehicleMake>(editModel);
            return _vehicleMakeRepo.UpdateVehicleMake(make);
        }
        
        /// <summary>
        /// delete make by id
        /// </summary>
        /// <param name="makeId"></param>
        public void DeleteMake(int makeId)
        {
            _vehicleMakeRepo.DeleteVehicleMake(makeId);
        }
        #endregion
        
        #region Model
        /// <summary>
        /// return list of model by makeid
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<EditVehicleModelModel> ListModels(DataTableVehicleModelModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableVehicleModelModel,TableList>(model);

            List<VehicleModel> listModel = _vehicleMakeRepo.ListVehicleModelsForPage(listParam);
            List<EditVehicleModelModel> listEditModel = new List<EditVehicleModelModel>();
            foreach (var item in listModel)
	        {
		        EditVehicleModelModel editModel = Mapper.DynamicMap<VehicleModel,EditVehicleModelModel>(item);
                listEditModel.Add(editModel);
	        }
            return listEditModel;
        }

        /// <summary>
        /// return count of model by makeid and search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountModels(DataTableVehicleModelModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableVehicleModelModel, TableList>(model);
            return _vehicleMakeRepo.GetVehicleModelsCount(listParam);
        }

        /// <summary>
        /// create vehicle model
        /// </summary>
        /// <param name="NewModel"></param>
        /// <returns></returns>
        public bool CreateModel(EditVehicleModelModel NewModel)
        {
            VehicleModel model = Mapper.DynamicMap<EditVehicleModelModel,VehicleModel>(NewModel);
            return _vehicleMakeRepo.AddVehicleModel(model);
        }

        /// <summary>
        /// return editmodelModel by modelId
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public EditVehicleModelModel GetEditModelModel(int modelId)
        {
            VehicleModel vehicleModel = _vehicleMakeRepo.GetVehicleModelById(modelId);
            EditVehicleModelModel model = Mapper.DynamicMap<VehicleModel, EditVehicleModelModel>(vehicleModel);
            return model;
        }

        /// <summary>
        /// update vehicle model
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        public bool UpdateModel(EditVehicleModelModel editModel)
        {
            VehicleModel model = Mapper.DynamicMap<EditVehicleModelModel, VehicleModel>(editModel);
            return _vehicleMakeRepo.ChangeVehicleModel(model);
        }
        #endregion

        #region Stalls

        /// <summary>
        /// return edit stall model
        /// </summary>
        /// <param name="getEditStall"></param>
        /// <returns></returns>
        public EditStallModel GetEditStallModel(GetEditStallModel getEditStall)
        {
            EditStallModel editStall = new EditStallModel();
            editStall.LotID = getEditStall.LotID; //TODO LotID Fix value

            if (getEditStall.ID > 0)
            {
                ParkingStall stall = _parkingStallRepo.GetParkingStall(getEditStall.ID);
                editStall = Mapper.DynamicMap<ParkingStall, EditStallModel>(stall);
            }
            else
            {

                if (getEditStall.Visitor != null && (bool)getEditStall.Visitor)
                {
                    editStall.ParkingStallTypeID = (int)ParkingStallTypeEnum.Visitor; // type of visitor
                }
            }
            return editStall;
        }

        /// <summary>
        /// update stall
        /// </summary>
        /// <param name="editStall"></param>
        public void UpdateStall(EditStallModel editStall)
        {
            ParkingStall stall = Mapper.DynamicMap<EditStallModel, ParkingStall>(editStall);
            _parkingStallRepo.UpdateParkingStall(stall);
        }

        /// <summary>
        /// create stall
        /// </summary>
        /// <param name="editStall"></param>
        public void CreateStall(EditStallModel editStall)
        {
            ParkingStall stall = Mapper.DynamicMap<EditStallModel, ParkingStall>(editStall);
            _parkingStallRepo.CreateParkingStall(stall);
        }

        /// <summary>
        /// return list of montly stall by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<ParkingStallModel> ListMontlyStalls(DataTableStallModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableStallModel,TableList>(model);
            List<ParkingStall> list = _parkingStallRepo.ListMontlyParkingStallsForPage(listParam);
            List<ParkingStallModel> stalls = new List<ParkingStallModel>();
            ParkingStallModel stall;
            ParkingStallTypeModel type;
            foreach (var item in list)
            {
                stall = Mapper.DynamicMap<ParkingStall, ParkingStallModel>(item);
                type = Mapper.DynamicMap<ParkingStallType,ParkingStallTypeModel>(item.ParkingStallType);
                stall.ParkingStallType = type;
                stalls.Add(stall);
            }
            return stalls; 
        }

        /// <summary>
        /// return count of montly stall by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountMontlyStalls(DataTableStallModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableStallModel, TableList>(model);
            return _parkingStallRepo.GetMontlyStallsCount(listParam);
        }

        /// <summary>
        /// return list of visitor stall by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<ParkingStallModel> ListVisitorStalls(DataTableStallModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableStallModel, TableList>(model);
            List<ParkingStall> list = _parkingStallRepo.ListVisitorParkingStallsForPage(listParam);
            List<ParkingStallModel> stalls = new List<ParkingStallModel>();
            ParkingStallModel stall;
            ParkingStallTypeModel type;
            foreach (var item in list)
            {
                stall = Mapper.DynamicMap<ParkingStall, ParkingStallModel>(item);
                type = Mapper.DynamicMap<ParkingStallType, ParkingStallTypeModel>(item.ParkingStallType);
                stall.ParkingStallType = type;
                stalls.Add(stall);
            }
            return stalls; 
        }

        /// <summary>
        /// return count of visitor stall by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountVisitorStalls(DataTableStallModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableStallModel, TableList>(model);
            return _parkingStallRepo.GetMontlyStallsCount(listParam);
        }

        /// <summary>
        /// return list of allocated stall by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<ParkingInventoryModel> ListAllocatedStalls(DataTableAllocatedStallModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableAllocatedStallModel, TableList>(model);
            List<ParkingInventory> list = _parkingInventoryRepo.ListAllocatedStallsForPage(listParam);
            List<ParkingInventoryModel> invetories = new List<ParkingInventoryModel>();
            ParkingInventoryModel inventory;
            foreach (var item in list)
            {
                inventory = Mapper.DynamicMap<ParkingInventory, ParkingInventoryModel>(item);
                invetories.Add(inventory);
            }
            return invetories; 
        }

        /// <summary>
        /// return count of allocated stall by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountAllocatedStalls(DataTableAllocatedStallModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableAllocatedStallModel, TableList>(model);
            return _parkingInventoryRepo.GetAllocatedStallsCount(listParam);
        }

        /// <summary>
        /// get parking stall by id
        /// </summary>
        /// <param name="stallId"></param>
        /// <returns></returns>
        public EditStallModel GetParkingStall(int stallId)
        {
            ParkingStall stall = _parkingStallRepo.GetParkingStall(stallId);
            EditStallModel editStall = Mapper.DynamicMap<ParkingStall,EditStallModel>(stall);
            return editStall;
        }

        public ParkingInventoryModel GetAllocatedStall(int stallId)
        {
            ParkingInventory inventory = _parkingInventoryRepo.GetAllocatedStall(stallId);
            ParkingInventoryModel model = Mapper.DynamicMap<ParkingInventory,ParkingInventoryModel>(inventory);
            BuildingModel building = Mapper.DynamicMap<Building, BuildingModel>(inventory.Building);
            LotModel lot = Mapper.DynamicMap<Lot,LotModel>(inventory.Lot);
            CompanyModel company = Mapper.DynamicMap<Company,CompanyModel>(inventory.Company);
            model.Company = company;
            model.Building = building;
            model.Lot = lot;
            return model;
        }
        
        /// <summary>
        /// delete parking stall by id
        /// </summary>
        /// <param name="stallId"></param>
        public void DeleteParkingStall(int stallId)
        {
            _parkingStallRepo.DeleteParkingStall(stallId);
        }

        #endregion
       
        #region Books
        /// <summary>
        /// return list of validation books by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<ValidationBookModel> ListValidationBooks(DataTableValidationBookModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableValidationBookModel, TableList>(model);
            List<ValidationBook> list = _validationBookRepo.ListValidationBooksForPage(listParam);
            List<ValidationBookModel> books = new List<ValidationBookModel>();
            ValidationBookModel book;
            foreach (var item in list)
            {
                book = Mapper.DynamicMap<ValidationBook, ValidationBookModel>(item);
                books.Add(book);
            }
            return books; 

        }

        /// <summary>
        /// return count of validation books by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountValidationBooks(DataTableValidationBookModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableValidationBookModel, TableList>(model);
            return _validationBookRepo.GetValidationBooksCount(listParam);
        }

        /// <summary>
        /// return edit book model
        /// </summary>
        /// <param name="getEditStall"></param>
        /// <returns></returns>
        public EditBookModel GetEditBookModel(int bookId)
        {
            ValidationBook book = _validationBookRepo.GetValidationBookById(bookId);
            EditBookModel validationBook = Mapper.DynamicMap<ValidationBook,EditBookModel>(book);
            return validationBook;
        }

        /// <summary>
        /// update Validation Book
        /// </summary>
        /// <param name="editStall"></param>
        public bool UpdateValidationBook(EditBookModel editBook)
        {
            ValidationBook book = Mapper.DynamicMap<EditBookModel, ValidationBook>(editBook);
            return _validationBookRepo.UpdateValidationBook(book);
        }

        /// <summary>
        /// create Validation Book
        /// </summary>
        /// <param name="editStall"></param>
        public bool CreateValidationBook(EditBookModel editBook)
        {
            ValidationBook book = Mapper.DynamicMap<EditBookModel, ValidationBook>(editBook);
            return _validationBookRepo.CreateValidationBook(book);
        }

        /// <summary>
        /// get ValidationBook by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EditBookModel GetValidationBookById(int bookId)
        {
            ValidationBook book = _validationBookRepo.GetValidationBookById(bookId);
            EditBookModel editBook = Mapper.DynamicMap<ValidationBook, EditBookModel>(book);
            
            return editBook;
        }

        /// <summary>
        /// delete validation book by id
        /// </summary>
        /// <param name="validationBookId"></param>
        public void DeleteValidationBook(int validationBookId)
        {
            _validationBookRepo.DeleteValidationBook(validationBookId);
        }
        #endregion
        
        #region KeyCard
        /// <summary>
        /// return list of keycard by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<KeyCardModel> ListKeyCards(DataTableKeyCardModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableKeyCardModel, TableList>(model);
            List<KeyCard> list = _keyCardRepo.ListKeyCardsForPage(listParam);
            List<KeyCardModel> cards = new List<KeyCardModel>();
            KeyCardModel card;
            foreach (var item in list)
            {
                card = Mapper.DynamicMap<KeyCard, KeyCardModel>(item);
                cards.Add(card);
            }
            return cards; 

        }

        /// <summary>
        /// return count of key card by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountKeyCards(DataTableKeyCardModel model)
        {
            TableList listParam = Mapper.DynamicMap<DataTableKeyCardModel, TableList>(model);
            return _keyCardRepo.GetKeyCardsCount(listParam);
        }

        /// <summary>
        /// return edit key card model
        /// </summary>
        /// <param name="getEditStall"></param>
        /// <returns></returns>
        public EditKeyCardModel GetEditKeyCardModel(int cardId)
        {
            KeyCard key = _keyCardRepo.GetKeyCardById(cardId);
            EditKeyCardModel editKey = Mapper.DynamicMap<KeyCard, EditKeyCardModel>(key);
            return editKey;
        }

        /// <summary>
        /// update key card
        /// </summary>
        /// <param name="editStall"></param>
        public bool UpdateKeyCard(EditKeyCardModel editKeyCard)
        {
            KeyCard keyCard = Mapper.DynamicMap<EditKeyCardModel, KeyCard>(editKeyCard);
            return _keyCardRepo.UpdateKeyCard(keyCard);
        }

        /// <summary>
        /// create key card
        /// </summary>
        /// <param name="editStall"></param>
        public bool CreateKeyCard(EditKeyCardModel editKeyCard)
        {
            KeyCard keyCard = Mapper.DynamicMap<EditKeyCardModel, KeyCard>(editKeyCard);
            return _keyCardRepo.CreateKeyCard(keyCard);
        }

        /// <summary>
        /// get key card by Id
        /// </summary>
        /// <param name="keyCardId"></param>
        /// <returns></returns>
        public EditKeyCardModel GetKeyCardById(int keyCardId)
        {
            KeyCard keyCard = _keyCardRepo.GetKeyCardById(keyCardId);
            EditKeyCardModel editKeyCard = Mapper.DynamicMap<KeyCard, EditKeyCardModel>(keyCard);
            return editKeyCard;
        }

        /// <summary>
        /// delete key card by id
        /// </summary>
        /// <param name="keyCardId"></param>
        public void DeleteKeyCard(int keyCardId)
        {
            _keyCardRepo.DeleteKeyCard(keyCardId);
        }
        #endregion

        #region Lease
        /// <summary>
        /// get lease for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public LeaseModel GetLeaseForCompany(int companyId)
        {
            Lease lease = _leaseRepo.GetLeaseForCompany(companyId);
            if (lease.Company.AddressID != null)
            {
                lease.Company.Address = lease.Company.AddressID == lease.Building.BuildingAddress.AddressID ? lease.Building.BuildingAddress :
                    _addressRepo.GetAddress((int)lease.Company.AddressID);
            }
            LeaseModel model = Mapper.DynamicMap<Lease,LeaseModel>(lease);
            CompanyModel company = Mapper.DynamicMap<Company, CompanyModel>(lease.Company);
            AddressModel address = Mapper.DynamicMap<Address,AddressModel>(lease.Company.Address);
            company.Address = address;
            model.Company = company;
            return model;
        }

        /// <summary>
        /// get lease by id
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public LeaseModel GetLeaseById(int leaseId)
        {
            Lease lease = _leaseRepo.GetLease(leaseId);
            LeaseModel model = Mapper.DynamicMap<Lease,LeaseModel>(lease);
            return model;
        }
        
        /// <summary>
        /// update lease
        /// </summary>
        /// <param name="lease"></param>
        public void UpdateLease(LeaseModel leaseModel)
        {
            Lease lease = Mapper.DynamicMap<LeaseModel,Lease>(leaseModel);
            _leaseRepo.UpdateLease(lease);
        }

        /// <summary>
        /// return list of montly parking
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<ParkingInventoryModel> ListMontlyParking(int companyId)
        {
            List<ParkingInventory> list = _parkingInventoryRepo.GetParkingInventoryForCompany(companyId);
            List<ParkingInventoryModel> inventories = new List<ParkingInventoryModel>();
            ParkingInventoryModel inventory;
            foreach (var item in list)
            {
                inventory = Mapper.DynamicMap<ParkingInventory, ParkingInventoryModel>(item);
                inventories.Add(inventory);
            }
            return inventories; 

        }

        /// <summary>
        /// get edit lease abstract model
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="inventoryId"></param>
        /// <returns></returns>
        public EditLeaseAbstractModel GetEditLeaseAbstractModel(int companyId, int? inventoryId)
        {
            EditLeaseAbstractModel model = new EditLeaseAbstractModel();
            Lease lease = _leaseRepo.GetLeaseForCompany(companyId);
            model.Lease = Mapper.DynamicMap<Lease, LeaseModel>(lease);
            model.Lease.Building = Mapper.DynamicMap<Building,BuildingModel>(lease.Building);
            model.Lease.Building.BuildingAddress = Mapper.DynamicMap<Address,AddressModel>(lease.Building.BuildingAddress);
            model.Lease.Building.BuildingAddress.State = Mapper.DynamicMap<zState, StateModel>(lease.Building.BuildingAddress.State);
            model.Lease.Company = Mapper.DynamicMap<Company, CompanyModel>(lease.Company);
            if (model.Lease.Company.AddressID != null)
            {
                model.Lease.Company.Address = model.Lease.Company.AddressID == model.Lease.Building.BuildingAddress.AddressID ? model.Lease.Building.BuildingAddress :
                    Mapper.DynamicMap<Address,AddressModel>(_addressRepo.GetAddress((int)model.Lease.Company.AddressID));
            }

            if (inventoryId != null && inventoryId > 0)
            {
                model.Inventory = Mapper.DynamicMap<ParkingInventory,ParkingInventoryModel>
                    (_parkingInventoryRepo.GetParkingInventoryByID((int)inventoryId));
            }
            else
            {
                model.Inventory.isActive = true;
                model.Inventory.BuildingID = model.Lease.BuildingID;
                model.Inventory.CompanyID = model.Lease.CompanyID;
            }
            return model;
        }

        /// <summary>
        /// update parking inventory
        /// </summary>
        /// <param name="editModel"></param>
        public void UpdateParkingInventory(EditLeaseAbstractModel editModel)
        {
            ParkingInventory inventory = Mapper.DynamicMap<ParkingInventoryModel,ParkingInventory>(editModel.Inventory);
            _parkingInventoryRepo.UpdateParkingInventory(inventory);
        }

        /// <summary>
        /// create parking inventory
        /// </summary>
        /// <param name="editModel"></param>
        public void CreateParkingInventory(EditLeaseAbstractModel editModel)
        {
            ParkingInventory inventory = Mapper.DynamicMap<ParkingInventoryModel, ParkingInventory>(editModel.Inventory);
            _parkingInventoryRepo.CreateParkingInventory(inventory);
        }

        /// <summary>
        /// get parking inventory by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ParkingInventoryModel GetParkingInventoryByID(int inventoryId)
        {
            ParkingInventoryModel inventory = Mapper.DynamicMap<ParkingInventory, ParkingInventoryModel>
                (_parkingInventoryRepo.GetParkingInventoryByID(inventoryId));
            return inventory;
        }
        
        /// <summary>
        /// delete parking inventory by id
        /// </summary>
        /// <param name="inventoryId"></param>
        public void DeleteParkingInventory(int inventoryId)
        {
            _parkingInventoryRepo.DeleteParkingInventory(inventoryId);
        }

        #endregion

        #region Company
        /// <summary>
        /// return list of employees by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<UserProfileModel> ListEmployees(DataTableEmployeeModel dtModel)
        {
            TableList listParam = Mapper.DynamicMap<DataTableEmployeeModel,TableList>(dtModel);
            List<UserProfile> list = _companyRepo.ListEmployeesForPage(listParam);
            List<UserProfileModel> users = new List<UserProfileModel>();
            UserProfileModel user;
            foreach (var item in list)
            {
                user = Mapper.DynamicMap<UserProfile, UserProfileModel>(item);
                users.Add(user);
            }
            return users; 
        }

        /// <summary>
        /// return count of employees by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public int CountEmployees(DataTableEmployeeModel dtModel)
        {
            TableList listParam = Mapper.DynamicMap<DataTableEmployeeModel, TableList>(dtModel);
            return _companyRepo.GetEmployeesCount(listParam);
        }

        /// <summary>
        /// list of tenants
        /// </summary>
        /// <param name="dtModel"></param>
        /// <returns></returns>
        public List<TenantModel> ListTenants(DataTableCompanyModel dtModel)
        {            
            TableList listParam = Mapper.DynamicMap<DataTableCompanyModel, TableList>(dtModel);
            List<Company>List = _companyRepo.ListTenantsForPage(listParam);
            List<TenantModel> tenants = new List<TenantModel>();

            foreach(Company c in List)
            {
                TenantModel tenant = new TenantModel();
                tenant.Company = Mapper.DynamicMap<Company, CompanyModel>(c);
                tenant.ManagerAddress = Mapper.DynamicMap<Address, AddressModel>(c.Address);
                tenant.ManagerProfile = Mapper.DynamicMap<UserProfile, UserProfileModel>(c.ManagerProfile);

                tenants.Add(tenant);
            }
            return tenants;           
        }

        /// <summary>
        /// return count of tenants by search string
        /// </summary>
        /// <param name="dtModel"></param>
        /// <returns></returns>
        public int CountTenants(DataTableCompanyModel dtModel)
        {
            TableList listParam = Mapper.DynamicMap<DataTableCompanyModel, TableList>(dtModel);
            return _companyRepo.GetTenantsCount(listParam);
        }

        /// <summary>
        /// get address for company by companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public AddressModel GetCompanyAddress(int companyId)
        { 
            AddressModel address = new AddressModel();
            int? addressId = null;
            if (companyId > 0)
            {
                addressId = _companyRepo.GetCompany(companyId).AddressID;
            }

            if (addressId != null)
            {
                address = Mapper.DynamicMap<Address,AddressModel>(_addressRepo.GetAddress((int)addressId));
            }
            return address;
        }

        #endregion

        #region SelectList
        /// <summary>
        /// list of zStates
        /// </summary>
        /// <returns></returns>
        public List<zStateModel> ListzStates()
        {
            List<zState> list = _zStateRepo.ListStates();
            List<zStateModel> states = new List<zStateModel>();
            zStateModel state;
            foreach (var item in list)
            {
                state = Mapper.DynamicMap<zState, zStateModel>(item);
                states.Add(state);
            }
            return states; 
        }

        /// <summary>
        /// list of users by type id
        /// </summary>
        /// <returns></returns>
        public List<UserProfileModel> ListUsersByType(int typeId)
        {
            List<UserProfile> list = _userProfileRepo.GetUsersByType(typeId);
            List<UserProfileModel> users = new List<UserProfileModel>();
            UserProfileModel user;
            foreach (var item in list)
            {
                user = Mapper.DynamicMap<UserProfile, UserProfileModel>(item);
                user.UserName = item.LoginName;
                users.Add(user);
            }
            return users; 

        }

        /// <summary>
        /// list of buildings
        /// </summary>
        /// <returns></returns>
        public List<BuildingModel> ListBuildings()
        {
            List<Building> list = _buildingRepo.ListBuildings();
            List<BuildingModel> buildings = new List<BuildingModel>();
            BuildingModel building;
            foreach (var item in list)
            {
                building = Mapper.DynamicMap<Building, BuildingModel>(item);
                buildings.Add(building);
            }
            return buildings; 
        }
        
        /// <summary>
        /// list of user profile type
        /// </summary>
        /// <returns></returns>
        public List<UserProfileTypeModel> ListUserProfileTypes()
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
        
        /// <summary>
        /// return list of parking stall types
        /// </summary>
        /// <returns></returns>
        public List<ParkingStallTypeModel> ListParkingStallTypes()
        {
            List<ParkingStallType> list = _parkingStallTypeRepo.ListParkingStallTypes();
            List<ParkingStallTypeModel> types = new List<ParkingStallTypeModel>();
            ParkingStallTypeModel type;
            foreach (var item in list)
            {
                type = Mapper.DynamicMap<ParkingStallType, ParkingStallTypeModel>(item);
                types.Add(type);
            }
            return types; 

        }

        /// <summary>
        /// return list of companies
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// return list of parking stall
        /// </summary>
        /// <returns></returns>
        public List<ParkingStallModel> ListParkingStall()
        {
            List<ParkingStall> list = _parkingStallRepo.ListParkingStall();
            List<ParkingStallModel> stalls = new List<ParkingStallModel>();
            ParkingStallModel stall;
            foreach (var item in list)
            {
                stall = Mapper.DynamicMap<ParkingStall, ParkingStallModel>(item);
                stalls.Add(stall);
            }
            return stalls; 

        }
        
        /// <summary>
        /// return list of lots for building by buildingId
        /// </summary>
        /// <returns></returns>
        public List<LotModel> ListLotsForBuilding(int buildingId)
        {
            List<Lot> list = _lotRepo.ListLotsForBuilding(buildingId);
            List<LotModel> lots = new List<LotModel>();
            LotModel lot;
            foreach (var item in list)
            {
                lot = Mapper.DynamicMap<Lot, LotModel>(item);
                lots.Add(lot);
            }
            return lots; 

        }

        #endregion
    }
}
