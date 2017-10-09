using EXP.Core;
using EXP.Core.Util;
using EXP.DataAccess;
using EXP.Entity;
using EXP.Entity.Enumerations;
using EXP.Services;
using EXP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EXP.Website.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly RegistrationService _service;

        public RegistrationController()
        {
            _service = new RegistrationService();
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetStoredRegistrationData()
        {
            string sesID = Session.SessionID;
            RegistrationData data = _service.GetStoredInfo();
            if (data != null)
            {
                var v = new { success = true, error = "", data = data };
                return Json(v);
            }
            else
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetProfileType(ProfileTypeModel profileType)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                if (profileType.ProfileTypeID != 1)
                {
                    profileType.Path = Server.MapPath("~");
                    profileType.RootUrl = Url.Action("Index", "Home", null, Request.Url.Scheme);

                    int lotId = _service.SaveProfileInfo(profileType);
                    {
                        var v = new { success = true, error = "", lotId = lotId };
                        return Json(v);
                    }
                }
                else
                {
                    var v = new { success = false, error = "Unknown error" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetStatus(int statusId)
        {
            if (statusId == (int)UserProfileStatusEnum.Active)
            {
                _service.SetStatus(User.Identity.Name);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            else
            {
                var v = new { success = false, error = "Error" };
                return Json(v);
            }
        }                

        #region Employee

        [HttpPost]
        [Authorize]
        public ActionResult ListUserProfileType()
        {
            List<UserProfileTypeModel> profileTypes = _service.ListUserProfileType();
            return Json(profileTypes);
        }                              

        [HttpPost]
        [Authorize]
        public ActionResult ListLots()
        {
            List<BuildingToLotModel> lots = _service.ListLots();
            return Json(lots);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListTenants()
        {
            List<CompanyModel> tenants = _service.ListCompanies();
            return Json(tenants);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetPersonalData(PersonalModel dataModel, PhoneModel phoneModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            try
            {
                dataModel.UserName = User.Identity.Name;
                _service.SetPersonalData(dataModel,phoneModel);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateVehicle(VehicleModels vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            try
            {
                vehicleModel.UserName = User.Identity.Name;
                _service.CreateVehicle(vehicleModel);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListVehicles()
        {
            List<VehicleModels> list = _service.ListVehicles(User.Identity.Name);
            return Json(list);
        }

        [Authorize]
        public ActionResult GetPersonalData()
        {
            try
            {
                UserProfileModel profile = _service.GetProfile(User.Identity.Name);

                var v = new { success = true, error = "", firstName = profile.FirstName, lastName = profile.LastName, EmailAddress = profile.EmailAddress };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [Authorize]
        public ActionResult GetBuildingAddressByLot(int buildingToLotId)
        {
            try
            {
                AddressModel address = _service.GetBuildingAddress(buildingToLotId);
  
                var v = new { success = true, error = "", address = address };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetAddress()
        {
            AddressModel address = _service.GetHomeAddress();
            var v = new { success = true, error = "", address = address };
            return Json(v);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetParkingInfo(int lotID)
        {
            ParkingStallModel stall = _service.GetParkingStall(lotID);

            var v = new { success = true, error = "", stall = stall };
            return Json(v);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetParkingInfo(ParkingInfoModel parkingInfo)
        {
            if (ModelState.IsValid)
            {
                parkingInfo.UserName = User.Identity.Name;
                _service.SetParkingInfo(parkingInfo);
                
                var v = new { success = true, error = "" };
                return Json(v);
            }
            else
            {
                var v = new { success = false, error = "Error" };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewPaymentCreditCard(CreditCardModel creditCardModel)
        {
            try
            {
                PaymentModel newPayment = new PaymentModel();
                newPayment.CreditCard = creditCardModel;
                newPayment.isCreditCard = true;
                
                if (!creditCardModel.isTenant)
                {
                    newPayment.CreditCard = creditCardModel;
                    _service.SaveEmployeePaymentMethod(newPayment);
                }
                else
                {
                    newPayment.CreditCard = creditCardModel;
                    newPayment.CreditCard.isHome = (bool)creditCardModel.isOffice;
                    _service.SaveTenantPaymentMethod(newPayment);
                }
                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewPaymentOnlineCheck(OnlineCheckModel onlineCheckModel)
        {
            try
            {
                PaymentModel newPayment = new PaymentModel();
                newPayment.OnlineCheck = onlineCheckModel;

                if (!onlineCheckModel.isTenant)
                {
                    _service.SaveEmployeePaymentMethod(newPayment);
                }
                else
                {
                    _service.SaveTenantPaymentMethod(newPayment);
                }
                {
                    var v = new { success = true, error = "" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        #endregion

        #region Tenant

        [HttpPost]
        [Authorize]
        public ActionResult SetCompany(CompanyModel companyModel, AddressModel address)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
            try
            {
                _service.SaveTenantCompanyInfo(companyModel,address);

                var v = new { success = true, error = "" };
                return Json(v);
                //}
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetPersonally(bool personally)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
            try
            {
                _service.SaveTenantPersonally(personally);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetCompanyManager(EditCompanyManager manager, PhoneModel phone, AddressModel address)
        {
            try
            {
                manager.UserName = User.Identity.Name;
                _service.SaveTenantCompanyManagerInfo(manager, phone, address);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendCompanyEmails(CompanyEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
            try
            {
                _service.SaveTenantPayingInfo(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetCompanyAddress()
        {
            AddressModel address = _service.GetCompanyAddress();
            var v = new { success = true, error = "", address = address };
            return Json(v);
        }

        [Authorize]
        public ActionResult GetPersonalPhone()
        {
            try
            {
                PhoneModel phone = _service.GetProfilePhone();

                var v = new { success = true, error = "", phone = phone };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        #endregion

        #region BuildingOwner

        [HttpPost]
        [Authorize]
        public ActionResult SendBuildingInformation(BuildingInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveBuildingInformation(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendOwnershipInformation(OwnershipInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveOwnershipInformation(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendPropertyManagementInformation(PropertyManagementInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SavePropertyManagementInformation(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendParkingManagementInformation(ParkingManagementInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveParkingManagementInformation(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendBankAccountInformation(BankAccountInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveBankAccountInformation(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendHandlerInformation(int handlerType)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveHandlerInformation(handlerType);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendParkingLotInformationReserved(StallsInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveParkingLotInformationReserved(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendParkingLotInformationUnreserved(StallsUnreservedInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveParkingLotInformationUnreserved(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendParkingLotInformationVisitor(StallsInformationModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveParkingLotInformationVisitor(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SendTenant(TenantModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveTenant(model);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveBuildingOwner()
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            try
            {
                _service.SaveBuildingOwner(User.Identity.Name);

                var v = new { success = true, error = "" };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }          

        #endregion

        private List<string> GetModelStateErrors()
        {
            List<string> errors = new List<string>();
            foreach (string key in ModelState.Keys)
            {
                if (ModelState.IsValidField(key))
                {
                    continue;
                }

                foreach (ModelError error in ModelState[key].Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors;
        }
    }
}
