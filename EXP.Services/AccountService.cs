using EXP.Core.Interface;
using EXP.Entity;
using EXP.Services.Models;
using AutoMapper;
using EXP.DataAccess;
using System.EnterpriseServices;
using System.Web.Security;
using System.Reflection;
using System.Transactions;
using EXP.Core;
using EXP.Entity.Enumerations;
using EXP.Services.Interface;
using System;
using System.Web;
using EXP.Core.Util;
using EXP.Core.Exceptions;

namespace EXP.Services
{
    public class AccountService : IAccountService
    {
        private readonly EmailService _emailService;
        private readonly IUserProfileRepository _userProfileRepo;
  
        public AccountService()
        {
            _emailService = new EmailService();
            _userProfileRepo = new UserProfileRepository();
        }

        public bool Register(RegisterModel regModel, ConfirmationEmailModel emailModel, out MembershipCreateStatus createStatus)
        {
            bool rezult = false;
            createStatus = MembershipCreateStatus.UserRejected;
            try
            {
                Membership.CreateUser(regModel.UserName, regModel.Password, regModel.Email, passwordQuestion: null, passwordAnswer: null,
                    isApproved: false, providerUserKey: null, status: out createStatus);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    UserProfile profile = Mapper.DynamicMap<RegisterModel, UserProfile>(regModel);
                    profile.LoginName = regModel.UserName;
                    rezult = _userProfileRepo.UpdateProfile(profile);

                    UserProfile user = _userProfileRepo.GetUser(profile.LoginName, (int)UserProfileStatusEnum.Pending);
                    emailModel.FirstName = user.FirstName;
                    emailModel.Email = user.EmailAddress;
                    emailModel.ProfilePasswordSalt = user.ProfilePasswordSalt;
                    _emailService.SendConfirmationEmail(emailModel);
                }
            }
            catch (ExpDatabaseException dbExc)
            {
                Logger.Error(dbExc.Message, dbExc);
            }
            catch (Exception exc)
            { 
                Logger.Error(exc.Message, exc);
            }
            return rezult;
        }

        public bool SearchSessionId(string sessionId)
        {
            return _userProfileRepo.SearchSessionId(sessionId);
        }
        
        public string GetUserNameByEmail(string email)
        {
            return _userProfileRepo.GetUserNameByEmail(email);
        }

        public UserProfileModel GetUser(GetUserModel user) 
        {
            UserProfile userProfile = _userProfileRepo.GetUser(user.UserName, user.UserProfileStatusID);
            Mapper.CreateMap<UserProfile, UserProfileModel>()
                .ForMember(m => m.UserName, opt => opt.MapFrom(r => r.LoginName));
            UserProfileModel model = Mapper.DynamicMap<UserProfile, UserProfileModel>(userProfile);
            return model;
        }
       
        public bool ChangePassword(ChangePasswordCryptoModel password)
        {
            return _userProfileRepo.ChangePassword(password.UserName, password.HashedPassword, password.Salt);      
        }

        public bool ActivateUser(ConfirmationModel confirm)
        {
            return _userProfileRepo.ActivateUser(confirm.Email, confirm.Salt);
        }

        public UserProfileModel Login(LoginModel Login)
        {           
            GetUserModel UserModel = new GetUserModel() { UserName = Login.UserName, UserProfileStatusID = 0 };
            return  GetUser(UserModel);
        }
   
        public void ForgotUsername(ForgotUsernameModel ForgotUser)
        {
            GetUserModel UserModel = new GetUserModel() { UserName = ForgotUser.UserName, UserProfileStatusID = 0 };
            UserProfileModel user = GetUser(UserModel);

            ReminderUsernameModel reminderModel = new ReminderUsernameModel()
            {
                UserName = ForgotUser.UserName,
                FirstName = user.FirstName,
                EmailPath = ForgotUser.EmailPath,
                ImageUrl = ForgotUser.ImageUrl,
                Email = ForgotUser.Email,
            };
            _emailService.SendReminderUsernameEmail(reminderModel);
        }

        public void ForgotPassword(ForgotPasswordModel ForgotPassword)
        {
            GetUserModel UserModel = new GetUserModel() { UserName = ForgotPassword.UserName, UserProfileStatusID = 0 };
            UserProfileModel user = GetUser(UserModel);

            RestorePasswordModel restoreModel = new RestorePasswordModel()
            {
                ResetUrl = ForgotPassword.ResetUrl,
                UserName = user.UserName,
                FirstName = user.FirstName,
                EmailPath = ForgotPassword.EmailPath,
                ImageUrl = ForgotPassword.ImageUrl,
                Email = ForgotPassword.Email,
                ProfilePasswordSalt = user.ProfilePasswordSalt,
            };
            _emailService.SendRestorePasswordEmail(restoreModel);     
        }

        public dynamic ResetPassword(ResetPasswordModel passwordModel)
       {
           string decodeFrom64 = CryptoUtils.DecodeFromBase64(passwordModel.qString);
           string[] split = decodeFrom64.Split(' ');
           if (split.Length == 2)
           {
               GetUserModel UserModel = new GetUserModel() { UserName = split[0], UserProfileStatusID = 0 };
               UserProfileModel user = GetUser(UserModel);
               if (user != null)
               {
                   if (user.ProfilePasswordSalt == split[1])
                   {
                       if (passwordModel.check)
                       {
                           return new { success = true, error = "" }; 
                       }
                       else
                       {
                           ChangePasswordCryptoModel password = new ChangePasswordCryptoModel();

                           password.UserName = user.UserName;
                           password.Salt = CryptoUtils.CreateSalt();
                           password.HashedPassword = CryptoUtils.CreatePasswordHash(passwordModel.Password.Trim(), password.Salt);

                           bool changed = ChangePassword(password);

                           if (changed)
                           {
                               return new { success = true, error = "" }; 
                           }
                       }                      
                   }
                   else
                   {
                       return new { success = false, error = "" }; 
                   }
               }
               return new { success = false, error = "" };
           }
           {
               return new { success = false, error = "Cannot change password" };
           }          
       }
    }
}
