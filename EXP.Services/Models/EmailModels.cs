using EXP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Models
{
    public class ConfirmationEmailModel
    {
        public string ConfirmationUrl { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string ProfilePasswordSalt { get; set; }
    }

    public class PayEmailModel
    {
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }

    public class WelcomeEmailModel
    {
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string TemplateFile { get; set; }
        public string Subject { get; set; }
    }

    public class ReminderUsernameModel
    {
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
    
    public class ForgotUsernameModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string EmailPath { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string ResetUrl { get; set; }
        public string EmailPath { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
    }
    public class SaveToDatabaseModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Destination { get; set; }
    }

    public class RestorePasswordModel
    {
        public string ResetUrl { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfilePasswordSalt { get; set; }
    }
    
    public class TenantEmailModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string ApproveUrl { get; set; }
        public string DisapproveUrl { get; set; }
    }

    public class LandlordEmailModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string TenantName { get; set; }
        public string BuildingName { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string ApproveUrl { get; set; }
        public string DisapproveUrl { get; set; }
    }

    public class ManagerEmailModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string TenantName { get; set; }
        public string BuildingName { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string RegistrationUrl { get; set; }
    }

    public class EmployeeEmailModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string TenantName { get; set; }
        public string BuildingName { get; set; }
        public bool IsHas { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
        public string ApproveUrl { get; set; }
        public string DisapproveUrl { get; set; }
    }

    public class InviteTenantModel
    {
        public string ServerPath { get; set; }
        public string Email { get; set; }
        public string BuildingName { get; set; }
        public string CompanyName { get; set; }
        public string EmailPath { get; set; }
        public string ImageUrl { get; set; }
    }
}
