using EXP.Entity;
using EXP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Interface
{
    public interface IAdminService
    {
        #region Users
        /// <summary>
        /// return list of users by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<UserProfileModel> ListUsers(JQueryDataTableParamModel param);

        /// <summary>
        /// return count of users by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountUsers(string searchString);

        /// <summary>
        /// Get user by UserProfileID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserProfileModel GetUser(int userId);

        /// <summary>
        /// set staus of user by id
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="status"></param>
        void SetStatus(int userProfileId, int status);

        /// <summary>
        /// return EditUserModels by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        EditUserModel GetEditUserModel(int userId);

        /// <summary>
        /// update user
        /// </summary>
        /// <param name="editUser"></param>
        void UpdateUser(EditUserModel editUser);

        /// <summary>
        /// create user
        /// </summary>
        /// <param name="editUser"></param>
        void CreateUser(EditUserModel editUser);
        #endregion

        #region Buildings

        /// <summary>
        /// return list of buildings by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<BuildingModel> ListBuildings(JQueryDataTableParamModel param);

        /// <summary>
        /// return count of buildings by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountBuildings(string searchString);

        /// <summary>
        /// get EditBuildingModels by edit of building
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        EditBuildingModel GetEditBuildingModel(int buildingId);
        

        /// <summary>
        /// create building
        /// </summary>
        /// <param name="editBuilding"></param>
        void CreateBuilding(EditBuildingModel editBuilding);

        /// <summary>
        /// get building by buildingId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        BuildingModel GetBuilding(int buildingId);

        /// <summary>
        /// return list building by search string
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<BuildingWideModel> SearchBuilding(SearchBuildingModel model);

        /// <summary>
        /// delete building by buildingId
        /// </summary>
        /// <param name="buildingId"></param>
        void DeleteBuilding(int buildingId);

        /// <summary>
        /// get building by lotid
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        BuildingModel GetBuildingByLotID(int lotId);
        #endregion

        #region Lots
        /// <summary>
        /// return lot by id
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        LotModel GetLot(int lotId);

        /// <summary>
        /// return list lots by search string
        /// </summary>
        /// <param name="dtModel"></param>
        /// <returns></returns>
        List<LotModel> ListLots(DataTableLotModel dtModel);

        /// <summary>
        /// return count of lots by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountLots(DataTableLotModel dtModel);

        /// <summary>
        /// return editlotModel by lotId
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        EditLotModel GetEditLotModel(int lotId);

        /// <summary>
        /// update lot
        /// </summary>
        /// <param name="editBuilding"></param>
        void UpdateLot(EditLotModel editLot);

        /// <summary>
        /// create lot
        /// </summary>
        /// <param name="editBuilding"></param>
        void CreateLot(EditLotModel editLot);

        List<LotWideModel> SearchLots(SearchLotModel model);

        /// <summary>
        /// delete lot
        /// </summary>
        /// <param name="lotId"></param>
        void DeleteLot(int lotId);
        #endregion

        #region Make
        /// <summary>
        /// return list of makes by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<EditMakeModel> ListMakes(JQueryDataTableParamModel param);

        /// <summary>
        /// return count of make by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountMakes(string searchString);

        /// <summary>
        /// create vehicle make
        /// </summary>
        /// <param name="NewModel"></param>
        /// <returns></returns>
        bool CreateMake(EditMakeModel NewModel);

        /// <summary>
        /// return editmakeModel by makeId
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        EditMakeModel GetEditMakeModel(int makeId);

        /// <summary>
        /// update make
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        bool UpdateMake(EditMakeModel editModel);

        /// <summary>
        /// delete make by id
        /// </summary>
        /// <param name="makeId"></param>
        void DeleteMake(int makeId);
        #endregion

        #region Model
        /// <summary>
        /// return list of model by makeid
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<EditVehicleModelModel> ListModels(DataTableVehicleModelModel model);

        /// <summary>
        /// return count of model by makeid and search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountModels(DataTableVehicleModelModel model);

        /// <summary>
        /// create vehicle model
        /// </summary>
        /// <param name="NewModel"></param>
        /// <returns></returns>
        bool CreateModel(EditVehicleModelModel NewModel);

        /// <summary>
        /// return editmodelModel by modelId
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        EditVehicleModelModel GetEditModelModel(int modelId);

        /// <summary>
        /// update vehicle model
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        bool UpdateModel(EditVehicleModelModel editModel);
        #endregion

        #region Stalls
        /// <summary>
        /// return edit stall model
        /// </summary>
        /// <param name="getEditStall"></param>
        /// <returns></returns>
        EditStallModel GetEditStallModel(GetEditStallModel getEditStall);

        /// <summary>
        /// update stall
        /// </summary>
        /// <param name="editStall"></param>
        void UpdateStall(EditStallModel editStall);

        /// <summary>
        /// create stall
        /// </summary>
        /// <param name="editStall"></param>
        void CreateStall(EditStallModel editStall);

        /// <summary>
        /// return list of montly stall by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<ParkingStallModel> ListMontlyStalls(DataTableStallModel model);

        /// <summary>
        /// return count of montly stall by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountMontlyStalls(DataTableStallModel model);

        /// <summary>
        /// return list of visitor stall by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<ParkingStallModel> ListVisitorStalls(DataTableStallModel model);

        /// <summary>
        /// return count of visitor stall by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountVisitorStalls(DataTableStallModel model);

        /// <summary>
        /// return list of allocated stall by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<ParkingInventoryModel> ListAllocatedStalls(DataTableAllocatedStallModel model);

        /// <summary>
        /// return count of allocated stall by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountAllocatedStalls(DataTableAllocatedStallModel model);

        /// <summary>
        /// get parking stall by id
        /// </summary>
        /// <param name="stallId"></param>
        /// <returns></returns>
        EditStallModel GetParkingStall(int stallId);

        ParkingInventoryModel GetAllocatedStall(int stallId);

        /// <summary>
        /// delete parking stall by id
        /// </summary>
        /// <param name="stallId"></param>
        void DeleteParkingStall(int stallId);

        #endregion

        #region Books
        /// <summary>
        /// return list of validation books by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<ValidationBookModel> ListValidationBooks(DataTableValidationBookModel model);

        /// <summary>
        /// return count of validation books by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountValidationBooks(DataTableValidationBookModel model);

        /// <summary>
        /// return edit book model
        /// </summary>
        /// <param name="getEditStall"></param>
        /// <returns></returns>
        EditBookModel GetEditBookModel(int bookId);

        /// <summary>
        /// update Validation Book
        /// </summary>
        /// <param name="editStall"></param>
        bool UpdateValidationBook(EditBookModel editBook);

        /// <summary>
        /// create Validation Book
        /// </summary>
        /// <param name="editStall"></param>
        bool CreateValidationBook(EditBookModel editBook);

        /// <summary>
        /// get ValidationBook by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        EditBookModel GetValidationBookById(int bookId);

        /// <summary>
        /// delete validation book by id
        /// </summary>
        /// <param name="validationBookId"></param>
        void DeleteValidationBook(int validationBookId);
        #endregion

        #region KeyCard
        /// <summary>
        /// return list of keycard by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<KeyCardModel> ListKeyCards(DataTableKeyCardModel model);

        /// <summary>
        /// return count of key card by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountKeyCards(DataTableKeyCardModel model);

        /// <summary>
        /// return edit key card model
        /// </summary>
        /// <param name="getEditStall"></param>
        /// <returns></returns>
        EditKeyCardModel GetEditKeyCardModel(int cardId);

        /// <summary>
        /// update key card
        /// </summary>
        /// <param name="editStall"></param>
        bool UpdateKeyCard(EditKeyCardModel editKeyCard);

        /// <summary>
        /// create key card
        /// </summary>
        /// <param name="editStall"></param>
        bool CreateKeyCard(EditKeyCardModel editKeyCard);

        /// <summary>
        /// get key card by Id
        /// </summary>
        /// <param name="keyCardId"></param>
        /// <returns></returns>
        EditKeyCardModel GetKeyCardById(int keyCardId);

        /// <summary>
        /// delete key card by id
        /// </summary>
        /// <param name="keyCardId"></param>
        void DeleteKeyCard(int keyCardId);
        #endregion

        #region Lease
        /// <summary>
        /// get lease for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        LeaseModel GetLeaseForCompany(int companyId);

        /// <summary>
        /// get lease by id
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        LeaseModel GetLeaseById(int leaseId);

        /// <summary>
        /// update lease
        /// </summary>
        /// <param name="lease"></param>
        void UpdateLease(LeaseModel lease);

        /// <summary>
        /// return list of montly parking
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        List<ParkingInventoryModel> ListMontlyParking(int companyId);

        /// <summary>
        /// get edit lease abstract model
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="inventoryId"></param>
        /// <returns></returns>
        EditLeaseAbstractModel GetEditLeaseAbstractModel(int companyId, int? inventoryId);

        /// <summary>
        /// update parking inventory
        /// </summary>
        /// <param name="editModel"></param>
        void UpdateParkingInventory(EditLeaseAbstractModel editModel);

        /// <summary>
        /// create parking inventory
        /// </summary>
        /// <param name="editModel"></param>
        void CreateParkingInventory(EditLeaseAbstractModel editModel);

        /// <summary>
        /// get parking inventory by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        ParkingInventoryModel GetParkingInventoryByID(int inventoryId);

        /// <summary>
        /// delete parking inventory by id
        /// </summary>
        /// <param name="inventoryId"></param>
        void DeleteParkingInventory(int inventoryId);

        #endregion

        #region Company
        /// <summary>
        /// return list of employees by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<UserProfileModel> ListEmployees(DataTableEmployeeModel dtModel);

        /// <summary>
        /// return count of employees by search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        int CountEmployees(DataTableEmployeeModel dtModel);

        /// <summary>
        /// list of tenants
        /// </summary>
        /// <param name="dtModel"></param>
        /// <returns></returns>
        List<TenantModel> ListTenants(DataTableCompanyModel dtModel);

        /// <summary>
        /// return count of tenants by search string
        /// </summary>
        /// <param name="dtModel"></param>
        /// <returns></returns>
        int CountTenants(DataTableCompanyModel dtModel);

        /// <summary>
        /// get address for company by companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        AddressModel GetCompanyAddress(int companyId);

        #endregion

        #region SelectList
        /// <summary>
        /// list of zStates
        /// </summary>
        /// <returns></returns>
        List<zStateModel> ListzStates();

        /// <summary>
        /// list of users by type id
        /// </summary>
        /// <returns></returns>
        List<UserProfileModel> ListUsersByType(int typeId);

        /// <summary>
        /// list of buildings
        /// </summary>
        /// <returns></returns>
        List<BuildingModel> ListBuildings();

        /// <summary>
        /// list of user profile type
        /// </summary>
        /// <returns></returns>
        List<UserProfileTypeModel> ListUserProfileTypes();

        /// <summary>
        /// return list of parking stall types
        /// </summary>
        /// <returns></returns>
        List<ParkingStallTypeModel> ListParkingStallTypes();

        /// <summary>
        /// return list of companies
        /// </summary>
        /// <returns></returns>
        List<CompanyModel> ListCompanies();

        /// <summary>
        /// return list of parking stall
        /// </summary>
        /// <returns></returns>
        List<ParkingStallModel> ListParkingStall();

        /// <summary>
        /// return list of lots for building by buildingId
        /// </summary>
        /// <returns></returns>
        List<LotModel> ListLotsForBuilding(int buildingId);

        #endregion
    }
}
