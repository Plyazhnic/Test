using EXP.Entity;
using EXP.Services.Models;
using System.Collections.Generic;

namespace EXP.Services.Interface
{
    public interface IRegistrationService
    {
        byte GetCurrentStep();
        PhoneModel GetProfilePhone();
        AddressModel GetHomeAddress();
        AddressModel GetCompanyAddress();
        void SetCurrentStep(byte step);
        RegistrationData GetStoredInfo();       
        void SetStatus(string userName);      
        int SaveProfileInfo(ProfileTypeModel profileType);
        List<UserProfileTypeModel> ListUserProfileType();
        List<BuildingToLotModel> ListLots();
        List<CompanyModel> ListCompanies();
        void SetPersonalData(PersonalModel dataModel, PhoneModel phoneModel);
        List<VehicleModels> ListVehicles(string userName);
        ParkingStallModel GetParkingStall(int lotId);


        #region Employee
        void CreateVehicle(VehicleModels vehicleModel);
        UserProfileModel GetProfile(string userName);
        void SetParkingInfo(ParkingInfoModel parkingInfo);
        void SaveEmployeeParkingInfo(ParkingInventoryModel parkingInventory);
        void SaveEmployeeAccountInfo(UserProfileModel profile, PhoneModel phone);
        void SaveEmployeeVehicleInfo(VehicleModels vehicle);
        void SaveEmployeePaymentInfo(bool myself);
        void SaveEmployeePaymentMethod(PaymentModel payment);
        #endregion

        #region Tenant
        AddressModel GetBuildingAddress(int buildingToLotId);
        void SaveTenantCompanyInfo(CompanyModel companyModel, AddressModel newAddress);
        void SaveTenantCompanyManagerInfo(EditCompanyManager manager, PhoneModel newPhone, AddressModel newAddress);
        void SaveTenantParkingInfo(ParkingInventoryModel parkingInventory);
        void SaveTenantVehicleInfo(VehicleModels vehicle);
        void SaveTenantPayingInfo(CompanyEmailModel model);
        void SaveTenantPaymentMethod(PaymentModel payment);
        #endregion

        #region Owner
        void SaveBuildingInformation(BuildingInformationModel model);
        void SaveOwnershipInformation(OwnershipInformationModel model);
        void SavePropertyManagementInformation(PropertyManagementInformationModel model);
        void SaveParkingManagementInformation(ParkingManagementInformationModel model);
        void SaveBankAccountInformation(BankAccountInformationModel model);
        void SaveHandlerInformation(int handlerType);
        void SaveParkingLotInformationReserved(StallsInformationModel model);
        void SaveParkingLotInformationUnreserved(StallsUnreservedInformationModel model);
        void SaveParkingLotInformationVisitor(StallsInformationModel model);
        void SaveTenant(TenantModel model);
        void SaveBuildingOwner(string loginName);
        #endregion
    }
}
