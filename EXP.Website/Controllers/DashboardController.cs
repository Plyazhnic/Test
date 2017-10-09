using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EXP.Core;
using EXP.Core.Util;
using EXP.DataAccess;
using EXP.Entity;
using EXP.Entity.Enumerations;
using EXP.Services;
using EXP.Services.Models;
using System.Configuration;
using System.Transactions;


namespace EXP.Website.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AlertsManager _alertsManager;
        private readonly EmailService _emailService;
        private readonly DashboardService _dashboardService;
        private readonly AccountService _accountService;

        private readonly UserProfileRepository UserProfileRepository;
        private readonly AddressRepository AddressRepository;
        private readonly AlertRepository  AlertRepository;
        private readonly PhoneRepository PhoneRepository;
        private readonly VehicleRepository VehicleRepository;
        private readonly BuildingRepository BuildingRepository;
        private readonly PaymentRepository PaymentRepository;
        private readonly VehicleMakeRepository VehicleMakeRepository;
        private readonly PhoneTypeRepository PhoneTypeRepository;
        private readonly zStateRepository zStateRepository;
        private readonly VehicleModelRepository VehicleModelRepository;

        public DashboardController()
        {
            _alertsManager = new AlertsManager();
            _emailService = new EmailService();
            _dashboardService = new DashboardService();
            _accountService = new AccountService();

            UserProfileRepository =  new UserProfileRepository();
            AddressRepository  = new AddressRepository();
            AlertRepository = new AlertRepository();
            PhoneRepository = new PhoneRepository();
            VehicleRepository = new VehicleRepository();
            BuildingRepository = new BuildingRepository();
            PaymentRepository = new PaymentRepository();
            VehicleMakeRepository = new VehicleMakeRepository();
            PhoneTypeRepository = new PhoneTypeRepository();
            zStateRepository = new zStateRepository();
            VehicleModelRepository = new VehicleModelRepository();
        }

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

        #region Account
        [HttpPost]
        [Authorize]
        public ActionResult ChangeEmail(ChangeEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }

            try
            {
                string userName = Membership.GetUserNameByEmail(model.NewEmail);

                if (!string.IsNullOrEmpty(userName))
                {
                    var v = new { success = false, error = "Email already exists" };
                    return Json(v);
                }
                UserProfileModel editUser = new UserProfileModel() 
                { 
                    UserName = User.Identity.Name,
                    EmailAddress = model.NewEmail,
                };
                bool changed = _dashboardService.UserProfile(editUser);
                if (changed)
                {
                    var v = new { success = true, error = "" };
                    return Json(v);
                }
                else
                {
                    var v = new { success = false, error = "Cannot change email" };
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
        public ActionResult ChangePassword(EditPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }

            try
            {
                if (!Membership.ValidateUser(User.Identity.Name, model.OldPassword))
                {
                    var v = new { success = false, error = "Password provided is incorrect." };
                    return Json(v);
                }                            

                NewPasswordModel PasswordModel = new NewPasswordModel(){NewPassword = model.NewPassword, UserName = User.Identity.Name };
                bool changed = _dashboardService.ChangePassword(PasswordModel);

                if (changed)
                {
                    var v = new { success = true, error = "" };
                    return Json(v);
                }
                else
                {
                    var v = new { success = false, error = "Cannot change password" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        #endregion Account

        #region Vehicle

        [HttpPost]
        [Authorize]
        public ActionResult NewVehicle(NewVehicleModel vehicleModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    var v = new { success = false, error = GetModelStateErrors() };
            //    return Json(v);
            //}
            NewVehicleReturnedModel ReturnedModel = _dashboardService.NewVehicle(vehicleModel, new GetUserModel() { UserName = User.Identity.Name, UserProfileStatusID = 0 });
            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = ReturnedModel.success, error = "", alert = ReturnedModel.Alert, deleteAlertId = ReturnedModel.deleteAlertId, newVehicleId = ReturnedModel.VehicleID };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);
                    }
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditVehicle(EditVehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }


            EditVehicleReturnedModel ReturnedModel = _dashboardService.EditVehicle(vehicleModel, new GetUserModel() { UserName = User.Identity.Name, UserProfileStatusID = 0 });
            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = true, error = "", alert = ReturnedModel.Alert, VehicleId = ReturnedModel.VehicleID };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);
                    }
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);
        }


        [HttpPost]
        [Authorize]
        public ActionResult RemoveVehicle(RemoveVehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            
                RemoveVehicleReturnedModel ReturnedModel = _dashboardService.RemoveVehicle(vehicleModel, new GetUserModel() { UserName = User.Identity.Name, UserProfileStatusID = 0 });
                
                switch (ReturnedModel.success)
                {
                    case true:
                        {

                            var v = new { success = ReturnedModel.success, error = ReturnedModel.Message, alerts = ReturnedModel.Alerts, vehicles = ReturnedModel.Vehicles };
                            return Json(v);
                        }
                    case false:
                        {

                            var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                            return Json(v);
                        }
                }
                var V = new { success = false, error = "Unknown error" };
                return Json(V);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetVehicle(int vehicleId)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            GetVehicleModel GetVehicleModel = new GetVehicleModel() { UserName = User.Identity.Name, VehicleID = vehicleId };
            GetVehicleReturnedModel ReturnedModel = _dashboardService.GetVehicle(GetVehicleModel);

            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = ReturnedModel.success, error = "", vehicle = ReturnedModel.Vehicle };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);
                    }
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListVehicles()
        {
           GetUserModel GetUserModel = new GetUserModel(){ UserName = User.Identity.Name};
           ListVehiclesModelsReturnedModel ReturnedModel =  _dashboardService.ListVehicles(GetUserModel);                      

           return Json(ReturnedModel.ListVehicles);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListMakes()
        {
            ListVehicleMakesModelsReturnedModel ReturnedModel = _dashboardService.ListMakes();
            return Json(ReturnedModel.ListVehicleMakes);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ListModels(int makeId)
        {
           ListVehicleModelsReturnedModel ReturnedModel = _dashboardService.ListModels(makeId);

           return Json(ReturnedModel.ListVehicleModels);
        }

        #endregion Vehicle

        #region Personal Address

        [HttpPost]
        [Authorize]
        public ActionResult GetPersonallyAddress()
        {

            GetUserModel GetUser = new GetUserModel() { UserName = User.Identity.Name, UserProfileStatusID = 0 };
            PersonallyAddressModelReturnedModel ReturnedModel = _dashboardService.GetPersonallyAddress(GetUser);

            var v = new { success = ReturnedModel.success, error = ReturnedModel.Message, address = ReturnedModel.Address };
            return Json(v);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditPersonalAddress(PersonalAddressModel addressModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }

            GetUserModel GetUser = new GetUserModel(){ UserName = User.Identity.Name, UserProfileStatusID = 0 };
            EditPersonalAddressModelReturnedModel ReturnedModel = _dashboardService.EditPersonalAddress(addressModel, GetUser);

            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message, alert = ReturnedModel.Alert };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);
                    }                       
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);                            
        }

        #endregion Personal Address

        #region Dashboard

        [Authorize]
        public ActionResult Dashboard()
        {        
            GetUserModel GetUser = new GetUserModel() { UserName = User.Identity.Name };
            DashboardReturnedModel ReturnedModel = _dashboardService.Dashboard(GetUser);
            
            ViewBag.UserProfile = ReturnedModel.UserProfile;
            ViewBag.Address = ReturnedModel.Address;
            ViewBag.Alerts = ReturnedModel.Alerts;
            ViewBag.Phones = ReturnedModel.Phones;
            ViewBag.Vehicles = ReturnedModel.Vehicles;
            ViewBag.CreditCard = ReturnedModel.CreditCard;
            ViewBag.Buildings = ReturnedModel.Buildings;

            return View();
        }

        #endregion Dashboard

        #region Alerts

        [HttpPost]
        [Authorize]
        public ActionResult DeleteAlert(int[] alertIds)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }

            MessageReturnedModel ReturnedModel = _dashboardService.DeleteAlert(alertIds);

            var V = new { success = ReturnedModel.success, error = ReturnedModel.Message };
            return Json(V);            
        }
        #endregion Alerts

        #region Phones Managment

        [HttpPost]
        [Authorize]
        public ActionResult ListPhoneTypes()
        {
            ListPhoneTypesReturnedModel ReturnedModel = _dashboardService.ListPhoneTypes();
            return Json(ReturnedModel.PhoneTypes);
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetPhones()
        {
            GetUserModel GetUser = new GetUserModel{ UserName = User.Identity.Name  };
            GetPhonesReturnedModel ReturnedModel =  _dashboardService.GetPhones(GetUser);

            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var V1 = new { success = ReturnedModel.success, error = ReturnedModel.Message, phoneobj = ReturnedModel.PhoneObject };
                        return Json(V1);
                    }
                case false:
                    {
                        var V = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(V);                    
                    }
            }
            var v = new { success = false, error = "Unknown error" };
            return Json(v);                        
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult NewPhone(NewPhoneModel newPhone)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }

            GetUserModel GetUser = new GetUserModel { UserName = User.Identity.Name };
            NewPhoneReturnedModel ReturnedModel = _dashboardService.NewPhone(newPhone, GetUser);

            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message, newPhoneId = ReturnedModel.NewPhoneId };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);
                    }
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);                        
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult ListPhones()
        {            
            GetUserModel GetUser = new GetUserModel { UserName = User.Identity.Name };
            ListPhonesReturnedModel ReturnedModel = _dashboardService.ListPhones(GetUser);

            return Json(ReturnedModel.Phones);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditPhone(EditPhoneModel editPhone)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            GetUserModel GetUser = new GetUserModel { UserName = User.Identity.Name };
            EditPhoneReturnedModel ReturnedModel = _dashboardService.EditPhone(editPhone, GetUser);

            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message, phoneId = ReturnedModel.PhoneId };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);                    
                    }            
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);                        
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemovePhone(RemovePhoneModel removePhone)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }

            GetUserModel GetUser = new GetUserModel { UserName = User.Identity.Name };
            RemovePhoneReturnedModel ReturnedModel = _dashboardService.RemovePhone(removePhone, GetUser);
            
            switch (ReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message, phonesCount = ReturnedModel.PhonesCount};
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = ReturnedModel.success, error = ReturnedModel.Message };
                        return Json(v);
                    }
            }
            var V = new { success = false, error = "Unknown error" };
            return Json(V);                                      
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetPhone(int phoneId)
        {
            try
            {
                if (phoneId > 0)
                {
                    UserProfileModel profile = _dashboardService.GetUser(User.Identity.Name);
                    PhoneModel phone = _dashboardService.GetPhone(phoneId);
                    
                    var v = new { success = true, error = "", phone = phone };
                    return Json(v);
                }
                else
                {
                    var v = new { success = "", error = "Error" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        #endregion Phones Managment

        #region States
        [HttpPost]
        [Authorize]
        public ActionResult ListStates()
        {
           ListStatesReturnedModel ReturnedModel = _dashboardService.ListStates();

           return Json(ReturnedModel.States);
        }
        #endregion States

        #region Payment

        [HttpPost]
        [Authorize]
        public ActionResult NewPaymentCreditCard(CreditCardModel creditCardModel)
        {
            List<Alert> alerts;
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }

            try
            {
                GetUserModel GetUser = new GetUserModel{ UserName = User.Identity.Name};
                alerts = _dashboardService.NewPaymentCreditCard(creditCardModel, GetUser);
                var v = new { success = true, error = "", alerts = alerts };
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
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            try
            {
               List<Alert> alerts = new List<Alert>();
               GetUserModel GetUser = new GetUserModel{ UserName = User.Identity.Name };
               alerts = _dashboardService.NewPaymentOnlineCheck(onlineCheckModel, GetUser);
                              
                var v = new { success = true, error = "", alerts = alerts};
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
        public ActionResult GetPayment()
        {
            GetUserModel UserModel = new GetUserModel { UserName = User.Identity.Name };
            PaymentModel PaymentModel = _dashboardService.GetPayment(UserModel);
                        
            return Json(PaymentModel);


        }

        [HttpPost]
        [Authorize]
        public ActionResult EditCreditCard(CreditCardModel creditCardModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            try
            {
                List<Alert> alerts = new List<Alert>();
                alerts = _dashboardService.EditCreditCard(creditCardModel, User.Identity.Name);
                var v = new { success = true, error = "", alerts = alerts};
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
        public ActionResult EditOnlineCheck(OnlineCheckModel onlineCheckModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = GetModelStateErrors() };
                return Json(v);
            }
            try
            {
                List<Alert> alerts;
                alerts = _dashboardService.EditOnlineCheck(onlineCheckModel, User.Identity.Name);
                var v = new { success = true, error = "", alerts = alerts };
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }
        #endregion

        #region Reports

        [HttpPost]
        [Authorize]
        public ActionResult ListBuildingTenants(int buildingId, int days)
        {
            ListBuildingTenantsModel ListBuildingTenantsModel = new ListBuildingTenantsModel{ BuildingId = buildingId, Days = days};
            List<CompanyModel> tenants = _dashboardService.ListBuildingTenants(ListBuildingTenantsModel);
            
            return Json(tenants);
        }

        #endregion Reports

        #region Building Address

        [HttpPost]
        [Authorize]
        public ActionResult GetBuildingAddress(int? BuildingId)
        {
            BuildingModel building = new BuildingModel();
            building.BuildingAddress = new AddressModel();

            if (BuildingId == 0)
            {
                var v = new { success = true, error = "", building = building };
                return Json(v);
            }
            if (BuildingId != null && BuildingId > 0)
            {
                building = _dashboardService.GetBuildingWithAddress(BuildingId);
            }
            else
            {
                var v = new { success = false, error = "building does not exist" };
                return Json(v);
            }
            if (building != null && building.BuildingID > 0)
            {
                var v = new { success = true, error = "", building = building };
                return Json(v);
            }
            else
            {
                var v = new { success = false, error = "building does not exist" };
                return Json(v);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditBuildingAddress(BuildingAddressModel BuildingAddressModel)
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Posted Data is not valid " };
                return Json(v);
            }
            GetUserModel GetUser = new GetUserModel { UserName = User.Identity.Name };
            EditBuildingAddressReturnedModel EditBuildingAddressReturnedModel = _dashboardService.EditBuildingAddress(BuildingAddressModel, GetUser);

            switch (EditBuildingAddressReturnedModel.success)
            {
                case true:
                    {
                        var v = new { success = EditBuildingAddressReturnedModel.success, error = EditBuildingAddressReturnedModel.Message, editBuildingAddress = EditBuildingAddressReturnedModel.BuildingAddressModel };
                        return Json(v);
                    }
                case false:
                    {
                        var v = new { success = EditBuildingAddressReturnedModel.success, error = EditBuildingAddressReturnedModel.Message };
                        return Json(v);
                    }                                  
            }
            var V = new { success = false, error = "" };
            return Json(V);
        }               

        [HttpPost]
        [Authorize]
        public ActionResult DeleteBuildingAddress(int? BuildingId)
        {
            bool deleted = false;

            if (BuildingId != null && BuildingId > 0)
            {
                deleted = _dashboardService.DeleteBuilding((int)BuildingId);
            }
            else
            {
                var v = new { success = false, error = "Invalid input parameter" };
                return Json(v);
            }

            var V = new { success = deleted, error = deleted ? "" : "Building by Id not found" };
            return Json(V);
        }

        #endregion Building Address

    }
}
