using System;
using System.Web.Security;
using System.Collections.Specialized;
using EXP.Core;
using EXP.Core.Util;
using EXP.DataAccess;
using EXP.Entity;
using EXP.Entity.Enumerations;

namespace EXP.Services
{
    public class MembershipProviderService : MembershipProvider
    {
        private string _applicationName;
        private bool _enablePasswordReset;
        private const bool _EnablePasswordRetrieval = false;
        private const bool _RequiresQuestionAndAnswer = false;
        private const bool _RequiresUniqueEmail = true;
        private int _maxInvalidPasswordAttempts;
        private int _passwordAttemptWindow;
        private int _minRequiredPasswordLength;
        private int _minRequiredNonalphanumericCharacters;
        private string _passwordStrengthRegularExpression;
        private const MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;

        #region Properties
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonalphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _PasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _RequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _RequiresUniqueEmail; }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _EnablePasswordRetrieval; }
        }

        #endregion

        #region Methods

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Membership Provider");
            }

            base.Initialize(name, config);

            _applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonalphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));

        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string userName, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved,
                                                  object providerUserKey,
                                                  out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(userName, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            MembershipUser u = GetUser(userName, false);

            if (u == null)
            {
                var salt = CryptoUtils.CreateSalt();
                var hashedPassword = CryptoUtils.CreatePasswordHash(password, salt);

                UserProfileRepository UserProfileRepository = new UserProfileRepository(); 
                status = UserProfileRepository.CreateUser(userName, hashedPassword, salt, email, false) ? MembershipCreateStatus.Success : 
                                                                                                              MembershipCreateStatus.UserRejected;
                return GetUser(userName, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        public override bool ValidateUser(string username, string password)
        {
            bool result = false;

            UserProfileRepository UserProfileRepository = new UserProfileRepository(); 
            UserProfile user = UserProfileRepository.GetUser(username, 0);
            user.UserProfileStatusID = 5;
            if (user != null)
            {
                var hashedPassword = CryptoUtils.CreatePasswordHash(password, user.ProfilePasswordSalt);
                result = user.ProfilePassword == hashedPassword && 
                    (user.UserProfileStatusID == (int)UserProfileStatusEnum.Active ||
                    user.UserProfileStatusID == (int)UserProfileStatusEnum.Verified);
            }

            return result;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            UserProfileRepository UserProfileRepository = new UserProfileRepository();
            var user = UserProfileRepository.GetUser(username, 0);

            return user != null ? new MembershipUser("MembershipProviderService",
                                      user.LoginName,
                                      null,
                                      user.EmailAddress,
                                      String.Empty,
                                      String.Empty,
                                      true, //TODO: Fix Approved according to Account settings
                                      false,
                                      user.CreateDate,
                                      user.LastLoginDate,
                                      user.UpdateDate ?? DateTime.MinValue,
                                      DateTime.MinValue,
                                      DateTime.MinValue) : null;

        }

        public override string GetUserNameByEmail(string email)
        {
            return new UserProfileRepository().GetUserNameByEmail(email);
        }
        #endregion

        #region Not Implemented

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}