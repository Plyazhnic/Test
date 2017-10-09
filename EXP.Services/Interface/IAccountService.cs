using EXP.Services.Models;
using System.Web.Security;
using EXP.Entity;

namespace EXP.Services.Interface
{
    public interface IAccountService
    {
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="regModel"></param>
        /// <param name="emailModel"></param>
        /// <param name="createStatus"></param>
        /// <returns></returns>
        bool Register(RegisterModel regModel, ConfirmationEmailModel emailModel, out MembershipCreateStatus createStatus);
        /// <summary>
        /// Search SessionId
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        bool SearchSessionId(string sessionId);
        /// <summary>
        /// Get User Name By Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        string GetUserNameByEmail(string email);
        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        UserProfileModel GetUser(GetUserModel user); 
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePassword(ChangePasswordCryptoModel password);
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        UserProfileModel Login(LoginModel Login);
        /// <summary>
        /// Forgot User Name
        /// </summary>
        /// <param name="ForgotUser"></param>
        void ForgotUsername(ForgotUsernameModel ForgotUser);
        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="ForgotPassword"></param>
        void ForgotPassword(ForgotPasswordModel ForgotPassword);
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="passwordModel"></param>
        /// <returns></returns>
        dynamic ResetPassword(ResetPasswordModel passwordModel);
    }
}
