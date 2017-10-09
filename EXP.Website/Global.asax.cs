using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EXP.Core.Util;
using EXP.DataAccess;
using EXP.Services;
using EXP.Entity;

namespace EXP.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private EmailService _emailService;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            StartEmailSender();
            BundleTable.EnableOptimizations = false;
            Logger.Debug("Application started");
        }

        protected void Application_End()
        {
            Logger.Debug("Stop email sender");

            if(_emailService != null)
            _emailService.StopEmailSender();
        }

        private void StartEmailSender()
        {
            Logger.Debug("Start email sender");

            _emailService = new EmailService();
            _emailService.StartEmailSender();
        }
    }
}