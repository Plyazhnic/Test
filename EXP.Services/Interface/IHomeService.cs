using EXP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Interface
{
    public interface IHomeService
    {
        void SendMessage(ContactUsModel contact);
    }
}
