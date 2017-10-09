using EXP.Services.Models;
using System.Web.Security;
using EXP.Entity;
using System.Collections.Generic;

namespace EXP.Services.Interface
{
    public interface IDashboardService
    {
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        bool ChangePassword(NewPasswordModel NewPassword);
        /// <summary>
        /// New vehicle
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <param name="User"></param>
        NewVehicleReturnedModel NewVehicle(NewVehicleModel vehicleModel, GetUserModel User);
        /// <summary>
        /// Edit Vehicle
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        EditVehicleReturnedModel EditVehicle(EditVehicleModel vehicleModel, GetUserModel User);
        /// <summary>
        /// Remove Vehicle
        /// </summary>
        /// <param name="vehicleModel"></param>
        /// <returns></returns>
        RemoveVehicleReturnedModel RemoveVehicle(RemoveVehicleModel vehicleModel, GetUserModel User);
        /// <summary>
        /// Get Vehicle
        /// </summary>
        /// <param name="GetVehicleModel"></param>
        /// <returns></returns>
        GetVehicleReturnedModel GetVehicle(GetVehicleModel GetVehicleModel);
        /// <summary>
        /// List Vehicles
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        ListVehiclesModelsReturnedModel ListVehicles(GetUserModel User);
        /// <summary>
        /// List Makes
        /// </summary>
        /// <returns></returns>
        ListVehicleMakesModelsReturnedModel ListMakes();
        /// <summary>
        /// List Models
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        ListVehicleModelsReturnedModel ListModels(int makeId);
        /// <summary>
        /// Get Personally Address
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        PersonallyAddressModelReturnedModel GetPersonallyAddress(GetUserModel User);
        /// <summary>
        /// Edit Personal Address
        /// </summary>
        /// <param name="addressModel"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        EditPersonalAddressModelReturnedModel EditPersonalAddress(PersonalAddressModel addressModel, GetUserModel User);
        /// <summary>
        /// Dashboard
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        DashboardReturnedModel Dashboard(GetUserModel User);
        /// <summary>
        /// Delete Alert
        /// </summary>
        /// <param name="alertIds"></param>
        /// <returns></returns>
        MessageReturnedModel DeleteAlert(int[] alertIds);
        /// <summary>
        /// List Phone Types
        /// </summary>
        /// <returns></returns>
        ListPhoneTypesReturnedModel ListPhoneTypes();
        /// <summary>
        /// New Phone
        /// </summary>
        /// <param name="newPhone"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        NewPhoneReturnedModel NewPhone(NewPhoneModel newPhone, GetUserModel User);
        /// <summary>
        /// List Phones
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        ListPhonesReturnedModel ListPhones(GetUserModel User);
        /// <summary>
        /// Edit Phone
        /// </summary>
        /// <param name="editPhone"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        EditPhoneReturnedModel EditPhone(EditPhoneModel editPhone, GetUserModel User);
        /// <summary>
        /// Remove Phone
        /// </summary>
        /// <param name="removePhone"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        RemovePhoneReturnedModel RemovePhone(RemovePhoneModel removePhone, GetUserModel User);
        /// <summary>
        /// List States
        /// </summary>
        /// <returns></returns>
        ListStatesReturnedModel ListStates();
        /// <summary>
        /// Get Phone
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        PhoneModel GetPhone(int phoneId);
        /// <summary>
        /// Save Employee Payment Method
        /// </summary>
        /// <param name="payment"></param>
  //      void SaveEmployeePaymentMethod(PaymentModel payment);
        /// <summary>
        /// Save Tenant Payment Method
        /// </summary>
        /// <param name="payment"></param>
  //      void SaveTenantPaymentMethod(PaymentModel payment);
        /// <summary>
        /// New Payment Credit Card
        /// </summary>
        /// <param name="creditCardModel"></param>
        /// <param name="User"></param>
        List<Alert> NewPaymentCreditCard(CreditCardModel creditCardModel, GetUserModel User);
        /// <summary>
        /// New Payment Online Check
        /// </summary>
        /// <param name="onlineCheckModel"></param>
        /// <param name="User"></param>
        List<Alert> NewPaymentOnlineCheck(OnlineCheckModel onlineCheckModel, GetUserModel User);
        /// <summary>
        /// Get Payment
        /// </summary>
        /// <returns></returns>
        PaymentModel GetPayment(GetUserModel User);
        /// <summary>
        /// Edit Credit Card
        /// </summary>
        /// <param name="creditCardModel"></param>
        List<Alert> EditCreditCard(CreditCardModel creditCardModel, string userName);
        /// <summary>
        /// Edit Online Check
        /// </summary>
        /// <param name="onlineCheckModel"></param>
        List<Alert> EditOnlineCheck(OnlineCheckModel onlineCheckModel, string userName);
        /// <summary>
        /// ListBuildingTenants
        /// </summary>
        List<CompanyModel> ListBuildingTenants(ListBuildingTenantsModel ListBuildingTenantsModel);
        /// <summary>
        /// Get Building With Address
        /// </summary>
        /// <param name="BuildingId"></param>
        /// <returns></returns>
        BuildingModel GetBuildingWithAddress(int? BuildingId);
        /// <summary>
        /// Edit Building Address
        /// </summary>
        /// <param name="BuildingAddressModel"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        EditBuildingAddressReturnedModel EditBuildingAddress(BuildingAddressModel BuildingAddressModel, GetUserModel User);
        /// <summary>
        /// Delete Building
        /// </summary>
        /// <param name="BuildingId"></param>
        /// <returns></returns>
        bool DeleteBuilding(int BuildingId);
    }
}
