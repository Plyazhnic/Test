using System.IO;
using System.Net.Mail;
using EXP.Entity;

namespace EXP.Core.Util
{
    public static class EmailUtils
    {
        public static void SendConfirmationEmail(UserProfile user, string confirmationUrl, string emailsPath, string emailsUrl)
        {
            var toAddress = new MailAddress(user.EmailAddress);
            const string subject = "Verify Account";

            string body = GetFileContent(Path.Combine(emailsPath, "verify_account.html"));
            body = body.Replace("%FIRST_NAME%", user.FirstName);
            body = body.Replace("%EMAILSPATH%", emailsUrl);
            body = body.Replace("%VERIFYURL%", string.Format("{0}?{1}",
                confirmationUrl, CryptoUtils.EncodeToBase64(user.EmailAddress + " " + user.ProfilePasswordSalt)));

            Send(subject, body, toAddress);
        }

        public static void SendWelcomeSelfPayEmail(UserProfile user, string emailsPath, string emailsUrl)
        {
            SendWelcomeEmail(user, "Welcome. Self.", emailsPath, "welcome_self_pay.html", emailsUrl);
        }

        public static void SendWelcomeCompanyPayEmail(UserProfile user, string emailsPath, string emailsUrl)
        {
            SendWelcomeEmail(user, "Welcome. Company.", emailsPath, "welcome_company_pay.html", emailsUrl);
        }

        private static void SendWelcomeEmail(UserProfile user, string subject, string emailsPath, string emailTemplateFile, string emailsUrl)
        {
            var toAddress = new MailAddress(user.EmailAddress);

            string body = GetFileContent(Path.Combine(emailsPath, emailTemplateFile));
            body = body.Replace("%FIRST_NAME%", user.FirstName);
            body = body.Replace("%EMAILSPATH%", emailsUrl);

            Send(subject, body, toAddress);
        }

        private static void Send(string subject, string body, MailAddress toAddress)
        {
            using (var smtp = new SmtpClient())
            {
                var message = new MailMessage();
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.To.Add(toAddress);
                }

                smtp.Send(message);
            }
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
    }
}