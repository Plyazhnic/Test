using AutoMapper;
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
using System.Web.Security;
using System.Configuration;
using System.Web.Http;

namespace EXP.Website.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AlertsManager _alertsManager;
        private readonly EmailService _emailService;

        public AccountController()
        {
            _alertsManager = new AlertsManager();
            _emailService = new EmailService();
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
                
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public ActionResult Register(RegisterModel regModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool rezult = false;
                    MembershipCreateStatus createStatus;
                    regModel.SessionID = this.Session.SessionID;
                    ConfirmationEmailModel emailModel = new ConfirmationEmailModel()
                    {
                        EmailPath = Server.MapPath("~/Emails"),
                        ImageUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "Images/",
                        ConfirmationUrl = Url.Action("Confirmation", "Home", null, Request.Url.Scheme),
                    };
                    AccountService userService = new AccountService();
                    rezult = userService.Register(regModel, emailModel, out createStatus);
                    if (rezult)
                    {
                        var v = new { success = true, error = "", sessionId = this.Session.SessionID };
                        return Json(v);
                    }
                    else
                    {
                        var v = new { success = false, error = ErrorCodeToString(createStatus) };
                        return Json(v);
                    }
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

        public ActionResult ValidRegisterRef(string queryString)
        {
            try
            {
                string decodeFrom64 = CryptoUtils.DecodeFromBase64(queryString);
                string[] split = decodeFrom64.Split(' ');
                if (split.Length == 2)
                {
                    string userName = "";//_unitOfWork.UserProfileRepository.GetUserNameByEmail(split[0]);         //TODO: Exchange(refactor) 
                    if (String.IsNullOrEmpty(userName))
                    {
                        int tenantId = 0;
                        Int32.TryParse(split[1], out tenantId);
                        if (tenantId != 0)
                        {
                            UserProfile tempUser = new UserProfile();// _unitOfWork.UserProfileRepository.GetUserById(tenantId);       //TODO: Exchange(refactor)  
                            if (tempUser != null && tempUser.UserProfileTypeID == (int)UserProfileTypeEnum.Tenant)
                            {
                                var v = new { success = true, error = "User with that email is already registered!", email = split[0], tenantId = split[1] };
                                return Json(v);
                            }
                        }
                        else
                        {
                            var v = new { success = false, error = "User with that email is already registered!" };
                            return Json(v);
                        }
                    }
                }
                {
                    var v = new { success = false, error = "Error request!" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public ActionResult Login(LoginModel Login) 
        {
            if (!ModelState.IsValid)
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
            try
            {
                if (!Membership.ValidateUser(Login.UserName,Login.Password))
                {
                    var v = new { success = false, error = "The user name or password provided is incorrect." };
                    return Json(v);
                }

                if (Login.RememberMe)
                {
                    int version = 2;
                    string name = Login.UserName;
                    DateTime issueDate = DateTime.Now;
                    DateTime expiration = issueDate.AddDays(30);
                    string cookiePath = FormsAuthentication.FormsCookiePath;
                    //Create a custom FormsAuthenticationTicket containing application specific data for the user. 
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(version, name, issueDate, expiration,
                        true, string.Empty, cookiePath);
                    //Encrypt the ticket. 
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    //Create the cookie. 
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.Expires = expiration;
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(Login.UserName, false, FormsAuthentication.FormsCookiePath);
                }
                
                AccountService service = new AccountService();
                UserProfileModel profile = service.Login(Login);
                
                string url = string.Empty;
                if ( profile.UserProfileStatusID == (int)UserProfileStatusEnum.Active)
                {
                    url = profile.UserProfileTypeID == (int)UserProfileTypeEnum.Administrator
                              ? Url.Action("Index", "Admin")
                              : Url.Action("Dashboard", "Dashboard");
                }
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", profile.SessionID));
                return Json(new { success = true, error = "", UserStatus = profile.UserProfileStatusID, Redirect = url });
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public ActionResult ResetSession()
        {
            AccountService service = new AccountService();
            
            if (service.SearchSessionId(Session.SessionID))
            {
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            }
            var v = new { success = true, error = "" };
            return Json(v);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public ActionResult SessionClear()
        {
            this.Session.Clear();
            var v = new { success = true, error = "" };
            return Json(v);
        }

        public ActionResult CheckEmail(string email)
        {
            string name;

            try
            {
                AccountService service = new AccountService();              
                name = service.GetUserNameByEmail(email);              

                if (string.IsNullOrEmpty(name))
                {
                    var v = new { success = true, error = "" };
                    return Json(v);
                }
                else
                {
                    var v = new { success = false, error = "" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        public ActionResult CheckUsername(string userName)
        {
            try
            {
                AccountService service = new AccountService();
                GetUserModel UserModel = new GetUserModel() { UserName = userName, UserProfileStatusID = 0 };
                UserProfileModel user = service.GetUser(UserModel);

                if (user == null)
                {
                    var v = new { success = true, error = "" };
                    return Json(v);
                }
                else
                {
                    var v = new { success = false, error = "" };
                    return Json(v);
                }
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }

        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public ActionResult ForgotUsername(string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccountService service = new AccountService();

                    string userName = service.GetUserNameByEmail(email);
                    if (String.IsNullOrEmpty(userName))
                    {
                        var v = new { success = false, error = "No such user in our DB" };
                        return Json(v);
                    }
                    else
                    {
                        string emailsUrl = Server.MapPath("~/Emails");
                        string imagesUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "Images/";

                        service.ForgotUsername(new ForgotUsernameModel() { Email = email, EmailPath = Server.MapPath("~/Emails/"), ImageUrl = imagesUrl, UserName = userName });
                                                
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
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AccountService service = new AccountService();

                    string userName = service.GetUserNameByEmail(email);
                    if (String.IsNullOrEmpty(userName))
                    {
                        var v = new { success = false, error = "No such user in our DB" };
                        return Json(v);
                    }
                    else
                    {
                        string emailsUrl = Server.MapPath("~/Emails");
                            string imagesUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "Images/";

                            service.ForgotPassword(new ForgotPasswordModel() { Email = email, EmailPath = Server.MapPath("~/Emails/"), ResetUrl = Url.Action("Index", "Home", null, Request.Url.Scheme), UserName = userName, ImageUrl = imagesUrl}); 
                                                        
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
            {
                var v = new { success = false, error = "Unknown error" };
                return Json(v);
            }
        }

        public ActionResult ResetPassword(ResetPasswordModel passwordModel)
        {
            try
            {
                AccountService service = new AccountService();

                var v = service.ResetPassword(passwordModel);                     
                return Json(v);
            }
            catch (Exception exc)
            {
                var v = new { success = false, error = exc.Message };
                return Json(v);
            }
        }
    }
}
