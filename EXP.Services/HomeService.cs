using EXP.Services.Interface;
using EXP.Services.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace EXP.Services
{
    public class HomeService : IHomeService
    {
        private readonly EmailService _emailService;

        public HomeService()
        {
            _emailService = new EmailService();
        }

        public void SendMessage(ContactUsModel contact)
        {
            contact.EmailTo = ConfigurationManager.AppSettings["contactEmail"];
            _emailService.SendContactEmail(contact);
        }
    }
}
