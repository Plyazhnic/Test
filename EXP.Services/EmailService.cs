using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using EXP.Core;
using EXP.Core.Util;
using EXP.Entity;
using EXP.DataAccess;
using EXP.Services.Models;
using AutoMapper;

namespace EXP.Services
{
    public class EmailService
    {
        private bool _emailSenderAlive = false;

        public void SendConfirmationEmail(ConfirmationEmailModel emailModel)
        {
            const string subject = "Verify Account";

            string body = GetFileContent(Path.Combine(emailModel.EmailPath, "verify_account.html"));
            body = body.Replace("%FIRST_NAME%", emailModel.FirstName);
            body = body.Replace("%IMAGESPATH%", emailModel.ImageUrl);
            body = body.Replace("%VERIFYURL%", string.Format("{0}?conf={1}",
                emailModel.ConfirmationUrl, CryptoUtils.EncodeToBase64(emailModel.Email + " " + emailModel.ProfilePasswordSalt)));

            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = emailModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendWelcomeSelfPayEmail(PayEmailModel payModel)
        {
            WelcomeEmailModel welcomeModel = Mapper.DynamicMap<PayEmailModel, WelcomeEmailModel>(payModel);
            welcomeModel.Subject = "Welcome. Self.";
            welcomeModel.TemplateFile = "welcome_self_pay.html";
            SendWelcomeEmail(welcomeModel);
        }

        public void SendWelcomeCompanyPayEmail(PayEmailModel payModel)
        {
            WelcomeEmailModel welcomeModel = Mapper.DynamicMap<PayEmailModel, WelcomeEmailModel>(payModel);
            welcomeModel.Subject = "Welcome. Company.";
            welcomeModel.TemplateFile = "welcome_company_pay.html";
            SendWelcomeEmail(welcomeModel);
        }

        public void SendReminderUsernameEmail(ReminderUsernameModel reminderModel)
        {
            const string subject = "Reminder username";

            string body = GetFileContent(Path.Combine(reminderModel.EmailPath, "reminder_username.html"));
            body = body.Replace("%FIRSTNAME%", reminderModel.FirstName);
            body = body.Replace("%USER_NAME%", reminderModel.UserName);
            body = body.Replace("%IMAGESPATH%", reminderModel.ImageUrl);
            
            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = reminderModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendRestorePasswordEmail(RestorePasswordModel restoreModel)
        {
            const string subject = "Reset password";

            string body = GetFileContent(Path.Combine(restoreModel.EmailPath, "reset_password.html"));
            body = body.Replace("%FIRSTNAME%", restoreModel.FirstName);
            body = body.Replace("%RESETURL%", string.Format("{0}?reset={1}",
                restoreModel.ResetUrl, CryptoUtils.EncodeToBase64(restoreModel.UserName.Trim() + " " + restoreModel.ProfilePasswordSalt.Trim())));
            body = body.Replace("%IMAGESPATH%", restoreModel.ImageUrl);
            
            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = restoreModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendTenantEmail(TenantEmailModel tenantModel)
        {
            const string subject = "Employee approved";

            string body = GetFileContent(Path.Combine(tenantModel.EmailPath, "tenant-notified.html"));
            body = body.Replace("%FIRST_NAME%", tenantModel.FirstName);
            body = body.Replace("%IMAGESPATH%", tenantModel.ImageUrl);
            body = body.Replace("%EMPLOYEE%", tenantModel.EmployeeName);
            body = body.Replace("%COMPANY%", tenantModel.CompanyName);
            body = body.Replace("%APPROVEURL%", tenantModel.ApproveUrl);
            body = body.Replace("%DISAPPROVEURL%", tenantModel.DisapproveUrl);

            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = tenantModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendLandlordEmail(LandlordEmailModel landlordModel)
        {
            const string subject = "Tenant approved";

            string body = GetFileContent(Path.Combine(landlordModel.EmailPath, "landlord-notified.html"));
            body = body.Replace("%FIRST_NAME%", landlordModel.FirstName);
            body = body.Replace("%IMAGESPATH%", landlordModel.ImageUrl);
            body = body.Replace("%TENANT%", landlordModel.TenantName);
            body = body.Replace("%BUILDING%", landlordModel.BuildingName);
            body = body.Replace("%APPROVEURL%", landlordModel.ApproveUrl);
            body = body.Replace("%DISAPPROVEURL%", landlordModel.DisapproveUrl);
            
            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = landlordModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendManagerEmail(ManagerEmailModel managerModel)
        {
            const string subject = "Manager request registration";

            string body = GetFileContent(Path.Combine(managerModel.EmailPath, "tenant-notified-manager.html"));
            body = body.Replace("%FIRST_NAME%", managerModel.FirstName);
            body = body.Replace("%IMAGESPATH%", managerModel.ImageUrl);
            body = body.Replace("%TENANTNAME%", managerModel.TenantName);
            body = body.Replace("%BUILDING%", managerModel.BuildingName);
            body = body.Replace("%REGISTRATIONURL%", managerModel.RegistrationUrl);
           
            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = managerModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendEmployeeEmail(EmployeeEmailModel employeeModel)
        {
            const string subject = "Parking approved";

            string body = GetFileContent(Path.Combine(employeeModel.EmailPath, "tenant-employee-approved-or-disapproved.html"));
            body = body.Replace("%FIRST_NAME%", employeeModel.FirstName);
            body = body.Replace("%IMAGESPATH%", employeeModel.ImageUrl);
            body = body.Replace("%TENANTNAME%", employeeModel.TenantName);
            body = body.Replace("%BUILDING%", employeeModel.BuildingName);
            body = body.Replace("%HAS%", employeeModel.IsHas ? "has" : "");
            body = body.Replace("%APPROVEURL%", employeeModel.ApproveUrl);
            body = body.Replace("%DISAPPROVEURL%", employeeModel.DisapproveUrl);

            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = employeeModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendInviteTenantEmail(InviteTenantModel inviteModel)
        {
            const string subject = "Tenant invitation";

            string body = GetFileContent(Path.Combine(inviteModel.EmailPath, "tenant-building.html"));
            body = body.Replace("%IMAGESPATH%", inviteModel.ImageUrl);
            body = body.Replace("%BUILDING%", inviteModel.BuildingName);
            body = body.Replace("%COMPANYNAME%", inviteModel.CompanyName);
            body = body.Replace("%REGISTRATIONURL%", string.Format("{0}?reg={1}",
                inviteModel.ServerPath, CryptoUtils.EncodeToBase64(inviteModel.Email.Trim())));

            SaveToDatabaseModel saveModel = new SaveToDatabaseModel()
            {
                Subject = subject,
                Body = body,
                Destination = inviteModel.Email,
            };
            SaveToDatabase(saveModel);
        }

        public void SendContactEmail(ContactUsModel contact)
        {
            using (var smtp = new SmtpClient())
            {
                var message = new MailMessage();
                {
                    message.Subject = "New message";
                    message.Body = String.Format("FROM: name:{0}, email: {1}. Message:{2}", contact.Name, contact.Email, contact.Message);
                    message.To.Add(contact.EmailTo);
                }
                smtp.Send(message);
            }
        }

        private void SendWelcomeEmail(WelcomeEmailModel welcomeModel)
        {
            string body = GetFileContent(Path.Combine(welcomeModel.EmailPath, welcomeModel.TemplateFile));
            body = body.Replace("%FIRST_NAME%", welcomeModel.FirstName);
            body = body.Replace("%IMAGESPATH%", welcomeModel.ImageUrl);
            
            SaveToDatabaseModel saveModel = Mapper.DynamicMap<WelcomeEmailModel, SaveToDatabaseModel>(welcomeModel);
            saveModel.Destination = welcomeModel.Email;
            saveModel.Body = body;
            SaveToDatabase(saveModel);
        }

        private void SaveToDatabase(SaveToDatabaseModel saveModel)
        {
            Email email = Mapper.DynamicMap<SaveToDatabaseModel, Email>(saveModel);
            EmailRepository repo = new EmailRepository();
            repo.CreateEmail(email);
        }

        private static string GetFileContent(string filePath)
        {
            string content;
            using (TextReader tr = new StreamReader(filePath))
            {
                content = tr.ReadToEnd();
            }

            return content;
        }

        public void StartEmailSender()
        {
            _emailSenderAlive = true;
            var mailThread = new Thread(ThreadFunc);
            mailThread.Start();
        }

        public void StopEmailSender()
        {
            _emailSenderAlive = false;
        }

        private void ThreadFunc()
        {
            while (_emailSenderAlive)
            {
                var mailThread = new Thread(MailThread);
                mailThread.Start();
                mailThread.Join();
                Thread.Sleep(new TimeSpan(0, 0, 0, 30));
            }
        }

        private void MailThread()
        {
            while (true)
            {
                try
                {
                    EmailRepository repo = new EmailRepository();
                    Email email = repo.GetNextEmail();
                    if (email == null)
                    {
                        break;
                    }

                    Send(email);

                    repo.SetEmailSent(email);
                }
                catch (Exception ex)
                {
                    Logger.Error("Thread period error", ex);
                }
            }
        }

        private static void Send(Email email)
        {
            var toAddress = new MailAddress(email.Destination);
            using (var smtp = new SmtpClient())
            {
                var message = new MailMessage();
                {
                    message.Subject = email.Subject;
                    message.Body = email.Body;
                    message.IsBodyHtml = true;
                    message.To.Add(toAddress);
                }

                smtp.Send(message);
            }
        }                
    }
}