using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EXP.Services.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class NewPasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ChangePasswordCryptoModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Hashed Password")]
        public string HashedPassword { get; set; }
        [Required]
        [Display(Name = "Salt")]
        public string Salt { get; set; }  
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ConfirmationModel
    {
        public string Email { get; set; }
        public string Salt { get; set; }
    }
    
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Subscribe { get; set; }
        public int? TenantID { get; set; }
        public string SessionID { get; set; }
    }

    [Serializable()]
    public class UserProfileModel
    {
        public int UserProfileID { get; set; }
        public string UserName { get; set; }
        public int? UserProfileTypeID { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte? UserProfileStatusID { get; set; }
        public string ProfilePasswordSalt { get; set; }
        public int AddressID { get; set; }
        public string Comments { get; set; }
        public string TitlePreffix { get; set; }
        public string SessionID { get; set; }
        public int? TenantID { get; set; }
        public bool isSupervisor { get; set; }
        public DateTime? HireDate { get; set; }
        public int? OperatorID { get; set; }
        public string UserProfileTypeDescription { get; set; }
        public string UserProfileStatusDescription { get; set; }
    }

    public class GetUserModel
    {
        public string UserName { get; set; }
        public int UserProfileStatusID { get; set; }
    }
}
