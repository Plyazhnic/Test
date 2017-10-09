using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EXP.Core;
using EXP.Core.Exceptions;
using EXP.Core.Interface;
using EXP.Core.Util;
using EXP.DataAccess;
using EXP.Entity;
using EXP.Entity.Enumerations;
using EXP.Services.Models;
using System.Web.Security;
using AutoMapper;
using System.IO;
using EXP.Services;


namespace EXP.Website.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AdminService _service;

        public AdminController()
        {
            _service = new AdminService();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Users
        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ListUsers(JQueryDataTableParamModel param)
        {
            try
            {
                List<UserProfileModel> persons = _service.ListUsers(param);
                int usersCount = _service.CountUsers(param.sSearch ?? "");
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = usersCount,
                    iTotalDisplayRecords = usersCount,
                    aaData = persons
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DeleteUser(int id)
        {
            if (id > 0)
	        {
                UserProfileModel profile = _service.GetUser(id);
                ViewBag.User = profile;

                if (Request.HttpMethod.ToString() == "GET")
                {
                    return View();
                }
                _service.SetStatus(profile.UserProfileID,(int)UserProfileStatusEnum.Deactived);
            }
            return new RedirectResult(Url.Action("Users", "Admin"));
        }

        public ActionResult ResetPassword(int id)
        {
            UserProfileModel profile = _service.GetUser(id);
            ViewBag.User = profile;

            return View();
        }

        [HttpGet]
        public ActionResult EditUser(int? id)
        {
            EditUserModel editUser = new EditUserModel();
            if (id != null && id > 0)
            {
                editUser = _service.GetEditUserModel((int)id);
            }
            editUser.States = GetStates(0);
            editUser.Titles = GetTitles();
            editUser.ProfileTypes = GetProfileTypes(0);
            editUser.Buildings = GetBuildings(0);
            editUser.Companies = GetCompanies(0);
            editUser.ParkingOperators = GetParkingOperators(0);
            return View(editUser);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserModel editUser)
        {
            if (editUser.User.UserProfileID == 0)
            {
                _service.CreateUser(editUser);
            }
            else
            {
                _service.UpdateUser(editUser);
            }
            return RedirectToAction("Users");
        }

        public ActionResult SearchUser()
        {
            return View();
        }

        #endregion

        #region Buildings
        public ActionResult Buildings()
        {
            return View();
        }

        public ActionResult ListBuildings(JQueryDataTableParamModel param)
        {
            try
            {
                List<BuildingModel> buildings = _service.ListBuildings(param);
                int buildingsCount = _service.CountBuildings(param.sSearch ?? "");

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = buildingsCount,
                    iTotalDisplayRecords = buildingsCount,
                    aaData = buildings}, 
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult EditBuilding(int? id)
        {
            EditBuildingModel building = new EditBuildingModel();
            if (id!= null && id>0)
            {
                building = _service.GetEditBuildingModel((int)id);
            }
            else
            {
                building.Building.IsActive = true;
            }
            building.Owners = GetUsers((int)UserProfileTypeEnum.BuildingOwner, 0);
            building.Managers = GetUsers((int)UserProfileTypeEnum.BuildingManager, 0);
            building.States = GetStates(0);
            building.InvoiceCutoffs = GetInvoiceCutoffs();
            building.MFBillngDays = GetMFBillngDays();
            return View(building);               
        }

        [HttpPost]
        public ActionResult EditBuilding(EditBuildingModel editBuilding)
        {
            if (editBuilding.Building.BuildingID > 0)
            {
                _service.UpdateBuilding(editBuilding);
            }
            else
            {
                _service.CreateBuilding(editBuilding);
            }
            return RedirectToAction("Buildings");
        }

        [HttpPost]
        public ActionResult GetBuilding(int buildingId)
        {
            BuildingModel building = _service.GetBuilding(buildingId);
            return Json(building);
        }

        [HttpGet]
        public ActionResult SearchBuilding()
        {
            SearchBuildingModel model = new SearchBuildingModel();
            model.States = GetStates(0);

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchBuilding(SearchBuildingModel model)
        {
            model.States = GetStates(0);
            List<BuildingWideModel> buildings = _service.SearchBuilding(model);
            model.Buildings = buildings;
            return View(model);
        }

        public ActionResult BuildingStatistics()
        {
            return View();
        }

        public ActionResult DeleteBuilding(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        ViewBag.BuildingID = id;
                        return View();
                    }
                    else
                    {
                        _service.DeleteBuilding(id);
                    }
                }
                return new RedirectResult(Url.Action("Buildings", "Admin"));
            }
            catch (Exception ex)
            {
                return new RedirectResult(Url.Action("Buildings", "Admin"));
            }
        }
        #endregion

        #region Tenant
        public ActionResult Tenants(int id)
        {
            ViewData["BuildingID"] = id;
            return View();
        }

        public ActionResult ListTenants(int BuildingId, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableCompanyModel model = new DataTableCompanyModel() { 
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    BuildingID = BuildingId,
                };

                List<TenantModel> tenants = _service.ListTenants(model);                
                int tenantsCount = _service.CountTenants(model);

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = tenantsCount,
                    iTotalDisplayRecords = tenantsCount,
                    aaData = tenants
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Lots
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">BuildingId</param>
        /// <returns></returns>
        public ActionResult Lots(int? id) 
        {
            ViewData["BuildingId"] = id == null ? 0 : id;
            return View();
        }

        public ActionResult ListLots(int? BuildingId, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableLotModel model = new DataTableLotModel(){
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    BuildingID = BuildingId,
                };
                List<LotModel> lots = _service.ListLots(model);
                int lotsCount = _service.CountLots(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = lotsCount,
                    iTotalDisplayRecords = lotsCount,
                    aaData = lots}, 
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult EditLot(int? id)
        {
            EditLotModel editLot = new EditLotModel();
            editLot.States = GetStates(0);
            if (id != null && id > 0)
            {
                editLot = _service.GetEditLotModel((int)id);
            }
            editLot.States = GetStates(0);
            editLot.Buildings = GetBuildings(0);
            editLot.Managers = GetUsers((int)UserProfileTypeEnum.ParkingManager, 0);
            editLot.Operators = GetUsers((int)UserProfileTypeEnum.ParkingOperator, 0);
            return View(editLot);
        }

        [HttpPost]
        public ActionResult EditLot(EditLotModel editLot)
        {
            if (editLot.Lot.LotID > 0)
            {
                _service.UpdateLot(editLot);
            }
            else
            {
                _service.CreateLot(editLot);
            }
            return RedirectToAction("Lots");
        }
               
        public ActionResult LotStatistics()
        {
            return View();
        }

        public ActionResult DeleteLot(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        LotModel lot = _service.GetLot(id);
                        ViewBag.Lot = lot;
                        return View();
                    }
                    else
                    {
                        _service.DeleteLot(id);
                    }
                }
                return new RedirectResult(Url.Action("Lots", "Admin"));
            }
            catch (Exception)
            { 
                return new RedirectResult(Url.Action("Lots", "Admin"));
            }
        }

        [HttpGet]
        public ActionResult SearchLot()
        {
            SearchLotModel model = new SearchLotModel();
            model.States = GetStates(0);

            return View(model);
        }

        [HttpPost]
        public ActionResult SearchLot(SearchLotModel model)
        {
            model.States = GetStates(0);

            List<LotWideModel> lots = _service.SearchLots(model);

            model.Lots = lots;

            return View(model);
        }


        #endregion

        #region VehicleMake
        
        public ActionResult Makes()
        {
            return View();
        }

        public ActionResult ListMakes(JQueryDataTableParamModel param)
        {
            try
            {
                List<EditMakeModel> vehicleMakes = _service.ListMakes(param);
                int vehicleMakeCount = _service.CountMakes(param.sSearch ?? "");
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = vehicleMakeCount,
                    iTotalDisplayRecords = vehicleMakeCount,
                    aaData = vehicleMakes
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public ActionResult CreateMake()
        {
            return View(new EditMakeModel());
        }

        [HttpPost]
        public ActionResult CreateMake(EditMakeModel NewModel)
        {
            if (string.IsNullOrEmpty(NewModel.VehicleMake1) | NewModel.VehicleMake1.Trim() == string.Empty)
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }

            bool result = _service.CreateMake(NewModel);

            if (result == false)
            {
                var Message = "Мodel already exists";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            else
            {
                return RedirectToAction("Makes");
            }
        }
        
        [HttpGet]
        public ActionResult EditMake(int? id )
        {
            EditMakeModel make = null;

            if (id!=null && id>0)
            {
                make = _service.GetEditMakeModel((int)id);
               if (make == null)
               {
                   var Message = "Make by Id not found";
                   return RedirectToAction("ParamtedError", new
                   {
                    ErrorMessage = Message,
                   });
               }
             }
            else
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });    
            }
            return View(make);
        }
        
        [HttpPost]
        public ActionResult EditMake(EditMakeModel EditMakeModel)
        {
          
            if (string.IsNullOrEmpty(EditMakeModel.VehicleMake1) | EditMakeModel.VehicleMake1.Trim() == string.Empty)
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }

            bool result = _service.UpdateMake(EditMakeModel);

            if (result == false)
            {
                var Message = "Мake is not exists";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            else
            {
                return RedirectToAction("Makes");
            }
        }

        public ActionResult DeleteMake(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        ViewBag.MakeID = id;
                        return View();
                    }
                    else
                    {
                        _service.DeleteMake(id);
                    }
                }
                return new RedirectResult(Url.Action("Makes", "Admin"));
            }
            catch (Exception ex)
            {
                return new RedirectResult(Url.Action("Makes", "Admin"));
            }
        }

        #endregion

        #region VehicleModel

        public ActionResult Models(int id)
        {
            ViewData["MakeId"] = id;
            return View();
        }

        public ActionResult ListModels(int MakeId, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableVehicleModelModel model = new DataTableVehicleModelModel()
                {
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    MakeID = MakeId,
                };
                List<EditVehicleModelModel> vehicleModels = _service.ListModels(model);
                int vehicleModelCount = _service.CountModels(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = vehicleModelCount,
                    iTotalDisplayRecords = vehicleModelCount,
                    aaData = vehicleModels
                },
                    JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult CreateModel(int MakeId)
        {
            EditVehicleModelModel VehicleModel = new EditVehicleModelModel() { VehicleMakeID = MakeId };
            return View(VehicleModel);
        }
        
        [HttpPost]
        public ActionResult CreateModel(EditVehicleModelModel model)
        {
            if (string.IsNullOrEmpty(model.VehicleModel1) | model.VehicleModel1.Trim() == string.Empty)
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }

            bool result = _service.CreateModel(model);

            if (result == false)
            {
                var Message = "Мodel already exists";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            else
            {
                return RedirectToAction("Makes");
            }
        }

        [HttpGet]
        public ActionResult EditModel(int? id)
        {
            EditVehicleModelModel model = null;

            if (id != null && id > 0)
            {
                model = _service.GetEditModelModel((int)id);
                if (model == null)
                {
                    var Message = "Model by Id not found";
                    return RedirectToAction("ParamtedError", new
                    {
                        ErrorMessage = Message,
                    });
                }
            }
            else
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditModel(EditVehicleModelModel EditModelModel)
        {

            if (string.IsNullOrEmpty(EditModelModel.VehicleModel1) | EditModelModel.VehicleModel1.Trim() == string.Empty)
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }

            bool result = _service.UpdateModel(EditModelModel);

            if (result == false)
            {
                var Message = "Мodel is not exists";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            else
            {
                return RedirectToAction("Makes");
            }
        }

        #endregion

        #region Vehicle

        public ActionResult SearchVehicle()
        {
            return View();
        }

        #endregion
        
        #region Stalls

        public ActionResult Inventory(int? id)
        {
            LotModel lot = new LotModel(); 
            if (id != null && id > 0)
	        {
		        ViewData["LotId"] = id;
                lot = _service.GetLot((int)id);
	        }
            return View(lot);
        }

        [HttpGet]
        public ActionResult EditStall(GetEditStallModel getEditStall)
        {
            EditStallModel editStall = new EditStallModel() ;
		    editStall = _service.GetEditStallModel(getEditStall);
            editStall.StallTypes = GetStallTypes(0);
            return View(editStall);
        }

        [HttpPost]
        public ActionResult EditStall(EditStallModel editStall)
        {
            if (ModelState.IsValid)
            {
                if (editStall.ParkingStallID > 0)
                {
                    _service.UpdateStall(editStall);   
                }
                else
                {
                    _service.CreateStall(editStall);    
                }
            }
            return RedirectToAction("Lots");
        }

        [ChildActionOnly]
        public ActionResult MontlyStalls()
        {
            return PartialView();
        }

        public ActionResult ListMontlyStalls(int lotID, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableStallModel model = new DataTableStallModel()
                {
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    LotID = lotID,
                };
                List<ParkingStallModel> stalls = _service.ListMontlyStalls(model);
                int stallsCount = _service.CountMontlyStalls(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = stallsCount,
                    iTotalDisplayRecords = stallsCount,
                    aaData = stalls
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [ChildActionOnly]
        public ActionResult VisitorStalls()
        {
            return PartialView();
        }

        public ActionResult ListVisitorStalls(int lotID, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableStallModel model = new DataTableStallModel(){
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    LotID = lotID,
                };
                List<ParkingStallModel> stalls = _service.ListVisitorStalls(model);
                int stallsCount = _service.CountVisitorStalls(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = stallsCount,
                    iTotalDisplayRecords = stallsCount,
                    aaData = stalls
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AllocatedStall(int? id)
        {
            ParkingInventoryModel inventory = new ParkingInventoryModel();
            if (id != null && id > 0)
            {
                inventory = _service.GetAllocatedStall((int)id);
            }
            return View(inventory);
        }

        public ActionResult ListAllocatedStalls(int stallID, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableAllocatedStallModel model = new DataTableAllocatedStallModel()
                {
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    StallID = stallID,
                };
                //TODO: Need discount value
                IEnumerable<ParkingInventoryModel> stalls = _service.ListAllocatedStalls(model);
                int stallsCount = _service.CountAllocatedStalls(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = stallsCount,
                    iTotalDisplayRecords = stallsCount,
                    aaData = stalls
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DeleteStall(int stallId, int lotId)
        {
            try
            {
                EditStallModel stall;
                if (stallId > 0)
                {
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        stall = _service.GetParkingStall(stallId);
                        ViewBag.Stall = stall;
                        return View();
                    }
                    else
                    {
                        _service.DeleteParkingStall(stallId);
                    }
                }
                return new RedirectResult(Url.Action("Inventory", "Admin", new { id = lotId }));
            }
            catch (Exception)
            {
                return new RedirectResult(Url.Action("Inventory", "Admin", new { id = lotId }));
            }
        }

        #endregion
        
        #region ValidationBooks
        
        public ActionResult ValidationBooks(int? id)
        {   
            BuildingModel Building = new BuildingModel();

            ViewData["LotId"] = (int?)(id == null ? 0 : id);

            if (id != null & id > 0)
                Building = _service.GetBuildingByLotID((int)id);

            return View(Building);
        }

        public ActionResult ListValidationBooks(int? LotId, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableValidationBookModel model = new DataTableValidationBookModel()
                {
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    LotID = LotId,
                };
                List<ValidationBookModel> ValidationBooks = _service.ListValidationBooks(model);
                int lotsCount = _service.CountValidationBooks(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = lotsCount,
                    iTotalDisplayRecords = lotsCount,
                    aaData = ValidationBooks
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
       
        }

        [HttpGet]
        public ActionResult EditValidationBook(int? id, int? bookid)
        {
            EditBookModel ValidationBook = null;

            if (id != null && id > 0 && bookid == null)
            {
                ValidationBook = new EditBookModel() { LotID = (int)id };
                return View(ValidationBook);
            }
            if (id == null && bookid != null && bookid > 0)
            {
                ValidationBook = _service.GetEditBookModel((int)bookid);
                if (ValidationBook == null)
                {
                    var Message = "Validation Book by Id not found";
                    return RedirectToAction("ParamtedError", new
                    {
                        ErrorMessage = Message,
                    });
                }
                else
                {
                    return View(ValidationBook);
                }
            }
            var Message1 = "Incorrectly entered data";
            return RedirectToAction("ParamtedError", new
            {
                ErrorMessage = Message1,
            });
        }
        
        [HttpPost]
        public ActionResult EditValidationBook(EditBookModel model)
        {
            // Server input parametr validation
            if (string.IsNullOrEmpty(model.BookName) | model.BookName.Trim() == string.Empty | model.TicketCount == null | model.TicketCount < 0 | model.Rate == null | model.Rate < 0 | Math.Round((decimal)model.Rate, 2) != model.Rate)
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            
            var Message1 = string.Empty;

            bool result = false;

            if (model.BookID == 0)
            {
                result = _service.CreateValidationBook(model);
                if (!result)
                {
                    Message1 = "Validation Book already exists ";
                }
            }
            else
            {
                result = _service.UpdateValidationBook(model);
                if (!result)
                {
                    Message1 = "Validation Book by Id not found ";
                }
            }
            if (result == false)
            {
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message1,
                });
            }
            else
            {
                return RedirectToAction("ValidationBooks/" + model.LotID);
            }     
        }

        public ActionResult DeleteValidationBook(int? id, int? Lotid)
        {
            try
            {
                if (id != null && id > 0)
                {
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        EditBookModel book = _service.GetValidationBookById((int)id);
                        ViewBag.Book = book;
                        return View();
                    }
                    else
                    {
                        _service.DeleteValidationBook((int)id);
                    }
                }
                return new RedirectResult(Url.Action("ValidationBooks", "Admin", new { id = Lotid })); 
            }
            catch (Exception)
            {
                return new RedirectResult(Url.Action("ValidationBooks", "Admin", new { id = Lotid }));
            }        
        }
        #endregion

        #region KeyCards
        public ActionResult KeyCards(int? id)
        {
            BuildingModel Building = null;

            ViewData["LotId"] = (int?)(id == null ? 0 : id);

            if (id != null & id > 0)
                Building = _service.GetBuildingByLotID((int)id);

            return View(Building);
        
        }

        public ActionResult ListKeyCards(int? LotId, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableKeyCardModel model = new DataTableKeyCardModel()
                {
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    LotID = LotId,
                };
                List<KeyCardModel> keyCard = _service.ListKeyCards(model);
                int keycardsCount = _service.CountKeyCards(model);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = keycardsCount,
                    iTotalDisplayRecords = keycardsCount,
                    aaData = keyCard
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        [HttpGet]
        public ActionResult EditKeyCard(int? id, int? cardId)
        {
            EditKeyCardModel KeyCard = null;

            if (id != null && id > 0 && cardId == null)
            {
                KeyCard = new EditKeyCardModel() { LotID = (int)id };
                return View(KeyCard);
            }
            if (id == null && cardId != null && cardId > 0)
            {
                KeyCard = _service.GetEditKeyCardModel((int)cardId);
                if (KeyCard == null)
                {
                    var Message = "Key Card by Id not found";
                    return RedirectToAction("ParamtedError", new
                    {
                        ErrorMessage = Message,
                    });
                }
                else
                {
                    return View(KeyCard);
                }
            }
            var Message1 = "Incorrectly entered data";
            return RedirectToAction("ParamtedError", new
            {
                ErrorMessage = Message1,
            });       
        }
        
        [HttpPost]
        public ActionResult EditKeyCard(EditKeyCardModel model)
        {
            // Server input parametr validation
            if (string.IsNullOrEmpty(model.KeyCardName) | model.KeyCardName.Trim() == string.Empty |  model.Rate == null | model.Rate < 0 | Math.Round((decimal)model.Rate, 2) != model.Rate)
            {
                var Message = "Incorrectly entered data";
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message,
                });
            }
            var Message1 = string.Empty;
            bool result = false;

            if (model.KeyCardID== 0)
            {
                result = _service.CreateKeyCard(model);
                if (!result)
                {
                    Message1 = "Key Card already exists ";
                }
            }
            else
            {
                result = _service.UpdateKeyCard(model);
                if (!result)
                {
                    Message1 = "Key Card by Id not found ";
                }
            }
            if (result == false)
            {
                return RedirectToAction("ParamtedError", new
                {
                    ErrorMessage = Message1,
                });
            }
            else
            {
                return RedirectToAction("KeyCards/" + model.LotID);
            }             
        }

        public ActionResult DeleteKeyCard(int? id, int? Lotid)
        {
            try
            {
                if (id != null && id > 0)
                {
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        EditKeyCardModel card = _service.GetKeyCardById((int)id);
                        ViewBag.card = card;
                        return View();
                    }
                    else
                    {
                        _service.DeleteKeyCard((int)id);
                    }
                }
                return new RedirectResult(Url.Action("KeyCards", "Admin", new { id = Lotid }));
            }
            catch (Exception)
            {
                return new RedirectResult(Url.Action("KeyCards", "Admin", new { id = Lotid }));
            }        
        }
        #endregion

        #region Lease

        public ActionResult TenantLease(int? id)
        {
            LeaseModel lease = new LeaseModel();
            if (id != null && id > 0)
            {
                lease = _service.GetLeaseForCompany((int)id);
            }
            return View(lease);
        }
        
        [HttpGet]
        public ActionResult EditTenantLease(int? id)
        {
            LeaseModel lease = new LeaseModel();
            if (id!=null && id > 0)
            {
                lease = _service.GetLeaseById((int)id);
            }
            return View(lease);
        }
        
        [HttpPost]
        public ActionResult EditTenantLease(LeaseModel editLease, HttpPostedFileBase uploadfile)
        {
            string fileName;

            if (uploadfile != null && uploadfile.ContentLength > 0)
            {
                string path = Server.MapPath("~/Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!String.IsNullOrEmpty(editLease.DocumentName) && System.IO.File.Exists(Path.Combine(path, editLease.DocumentName)))
                {
		            System.IO.File.Delete(Path.Combine(path, editLease.DocumentName));
                }
                fileName = String.Concat(editLease.Company.CompanyName,"-",Path.GetFileName(uploadfile.FileName));
                string fullpath = Path.Combine(path ,fileName);
                uploadfile.SaveAs(fullpath);
                editLease.DocumentName = fileName;
            }

            if (editLease.LeaseID > 0)
            {
                _service.UpdateLease(editLease);
                return RedirectToAction("TenantLease", new {id = editLease.CompanyID});
            }
            return View();
        }

        public ActionResult LeaseAbstract(int? id)
        {
            LeaseModel lease = new LeaseModel();
            if (id != null && id > 0)
            {
                lease = _service.GetLeaseForCompany((int)id);
            }
            return View(lease);
        }

        [ChildActionOnly]
        public ActionResult MontlyParking(int companyId)
        {
            List<ParkingInventoryModel> inventories = new List<ParkingInventoryModel>();
            if (companyId > 0)
            {
                inventories = _service.ListMontlyParking(companyId);
            }
            return PartialView(inventories);
        }

        [HttpGet]
        public ActionResult EditLeaseAbstract(int? id, int? inventoryId)
        {
            EditLeaseAbstractModel model = new EditLeaseAbstractModel();

            if (id != null && id > 0)
            {
                model = _service.GetEditLeaseAbstractModel((int)id, inventoryId);
            }
            model.Lots = GetLots(model.Lease.BuildingID, model.Inventory.LotID);
            model.StallTypes = GetStallTypes(model.Inventory.ParkingStall != null ? model.Inventory.ParkingStall.ParkingStallTypeID : 0);
            ViewData["InventoryId"] = inventoryId == null ? 0 : inventoryId;

            return View(model);
        }
        
        [HttpPost]
        public ActionResult EditLeaseAbstract(EditLeaseAbstractModel editModel)
        {
            if (editModel.Inventory.ParkingInventoryID > 0)
            {
                _service.UpdateParkingInventory(editModel);
            }
            else
            {
                _service.CreateParkingInventory(editModel);
            }
            return RedirectToAction("LeaseAbstract", new { id = editModel.Inventory.CompanyID });
        }

        public ActionResult DeleteLeaseAbstract(int id)
        {
            try
            {
                ParkingInventoryModel inventory = new ParkingInventoryModel();
                if (id > 0)
                {
                    inventory = _service.GetParkingInventoryByID(id);
                    if (Request.HttpMethod.ToString() == "GET")
                    {
                        ViewBag.Inventory = inventory;
                        return View();
                    }
                    else
                    {
                        _service.DeleteParkingInventory(id);
                    }
                }
                return new RedirectResult(Url.Action("LeaseAbstract", "Admin", new { id = inventory.CompanyID}));
            }
            catch (Exception)
            {
                return new RedirectResult(Url.Action("LeaseAbstract", "Admin"));
            }
        }

        #endregion

        #region Company
        public ActionResult Employees(int id)
        {
            ViewData["CompanyID"] = id;
            return View();
        }

        public ActionResult ListEmployees(int companyId, JQueryDataTableParamModel param)
        {
            try
            {
                DataTableEmployeeModel dtModel = new DataTableEmployeeModel()
                {
                    Start = param.iDisplayStart,
                    End = param.iDisplayStart + param.iDisplayLength,
                    SearchString = param.sSearch ?? "",
                    CompanyID = companyId
                };
                List<UserProfileModel> employees = _service.ListEmployees(dtModel);
                int employeesCount = _service.CountEmployees(dtModel);

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = employeesCount,
                    iTotalDisplayRecords = employeesCount,
                    aaData = employees
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult GetCompanyAddress(int companyId)
        {
            AddressModel address = _service.GetCompanyAddress(companyId);
            
            var v = new { success = true, error = "", address = address };
            return Json(v);
        }

        #endregion
        
        #region SelectLists

        private SelectList GetStates(int? stateId)
        {
            List<zStateModel> states = _service.ListzStates();
            if (stateId != null && stateId > 0)
            {
                return new SelectList(states, "StateID", "StateName", stateId);
            }
            else
            {
                return new SelectList(states, "StateID", "StateName", 0);
            }
        }

        private SelectList GetUsers(int typeId, int id)
        {
            List<UserProfileModel> users = _service.ListUsersByType(typeId);
            return new SelectList(users, "UserProfileID", "UserName", id);
        }

        private SelectList GetBuildings(int? buildingId)
        {
            List<BuildingModel> buildings = _service.ListBuildings();
            if (buildingId != null && buildingId > 0)
            {
                return new SelectList(buildings, "BuildingID", "BuildingName", buildingId);
            }
            else
            {
                return new SelectList(buildings, "BuildingID", "BuildingName");
            }
        }

        private SelectList GetInvoiceCutoffs()
        {
            SelectList sl = new SelectList(new[]{
              new SelectListItem{ Text="1", Value="1"},
              new SelectListItem{ Text="2", Value="2"},
              new SelectListItem{ Text="3", Value="3"},
              new SelectListItem{ Text="4", Value="4"},
              new SelectListItem{ Text="5", Value="5"},
              new SelectListItem{ Text="6", Value="6"},
              new SelectListItem{ Text="7", Value="7"},
              new SelectListItem{ Text="8", Value="8"}
            }, "Value", "Text");
            return sl;
        }

        private SelectList GetMFBillngDays()
        {
            SelectList sl = new SelectList(new[]{
              new SelectListItem{ Text="1", Value="1"},
              new SelectListItem{ Text="2", Value="2"},
              new SelectListItem{ Text="3", Value="3"},
              new SelectListItem{ Text="4", Value="4"},
              new SelectListItem{ Text="5", Value="5"},
              new SelectListItem{ Text="6", Value="6"},
              new SelectListItem{ Text="7", Value="7"},
              new SelectListItem{ Text="8", Value="8"}
            }, "Value", "Text");
            return sl;
        }

        private SelectList GetProfileTypes(int typeId)
        {
            List<UserProfileTypeModel> types = _service.ListUserProfileTypes();
            return new SelectList(types, "UserProfileTypeID", "UserProfileType1", typeId);
        }

        private SelectList GetParkingOperators(int? parkingOperatorId)
        {
            List<UserProfileModel> users = _service.ListUsersByType((int)UserProfileTypeEnum.ParkingOperator);
            return new SelectList(users, "UserProfileID", "UserName", parkingOperatorId);
        }

        private SelectList GetTitles()
        {
            SelectList sl = new SelectList(new[]{
              new SelectListItem{ Text="Mr.", Value="Mr."},
              new SelectListItem{ Text="Mrs.", Value="Mrs."},
              new SelectListItem{ Text="Ms.", Value="Ms."},
              new SelectListItem{ Text="Miss", Value="Miss"}
            }, "Value", "Text");
            return sl;
        }

        private SelectList GetStallTypes(int? stalTypesId)
        {
            List<ParkingStallTypeModel> stalTypes = _service.ListParkingStallTypes();
            if (stalTypesId != null && stalTypesId > 0)
            {
                return new SelectList(stalTypes, "ParkingStallTypeID", "ParkingStallType1", stalTypesId);
            }
            else
            {
                return new SelectList(stalTypes, "ParkingStallTypeID", "ParkingStallType1", 0);
            }
        }

        private SelectList GetCompanies(int? companyId)
        {
            List<CompanyModel> companies = _service.ListCompanies();
            if (companyId != null && companyId > 0)
            {
                return new SelectList(companies, "CompanyID", "CompanyName", companyId);
            }
            else
            {
                return new SelectList(companies, "CompanyID", "CompanyName", 0);
            }
        }

        private SelectList GetStalls(int? stallId)
        {
            List<ParkingStallModel> stalls = _service.ListParkingStall();
            if (stallId != null && stallId > 0)
            {
                return new SelectList(stalls, "ParkingStallID", "StallLocation", stallId);
            }
            else
            {
                return new SelectList(stalls, "ParkingStallID", "StallLocation", 0);
            }
        }
        
        private SelectList GetLots(int buildingId, int? lotId)
        {
            List<LotModel> lots = _service.ListLotsForBuilding(buildingId);
            if (lotId != null && lotId > 0)
            {
                return new SelectList(lots, "LotID", "LotName", lotId);
            }
            else
            {
                return new SelectList(lots, "LotID", "LotName", 0);
            }
        }

        #endregion

        #region ErrorPage

        public ActionResult ParamtedError(string ErrorMessage)
        {
            ViewData["ErrorMessage"] = ErrorMessage;
            return View();
        }
        
        public ActionResult Error(Exception model)
        {
            ViewBag.Exception = model;
            return View();
        }

        public ActionResult InvoiceAdjustments()
        {
            return View();
        }

        public ActionResult UnderConstruction()
        {
            return View();
        }
        #endregion
    }
}
        