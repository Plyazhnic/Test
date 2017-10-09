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
using System.Web.Http;

namespace EXP.Website.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {
        }

        [System.Web.Http.AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public ActionResult ContactUs(ContactUsModel contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HomeService service = new HomeService();
                    service.SendMessage(contact);

                    var v = new { success = true, error = "" };
                    return Json(v);
                }
                catch (Exception exc)
                {
                    var v = new { success = false, error = exc.Message };
                    return Json(v);
                }
            }
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
        }

        public ActionResult Confirmation()
        {
            string decodeFrom64 = CryptoUtils.DecodeFromBase64(Request.Url.Query.Replace("?conf=", ""));
            string[] split = decodeFrom64.Split(' ');
            if (split.Length != 2)
            {
                ViewBag.Message = "Confirmation code invalid";
                return View();
            }
            AccountService service = new AccountService();
            ConfirmationModel Confirm = new ConfirmationModel() { Email = split[0], Salt = split[1] };
            bool success = service.ActivateUser(Confirm);

            ViewBag.Message = decodeFrom64 + ' ' + success;
            return View();
        }
    }
}
