using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EXP.Core.Exceptions;
using EXP.Core.Util;

namespace EXP.Website.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext exceptionContext)
        {
            exceptionContext.ExceptionHandled = true;

            if (exceptionContext.Exception is ExpDatabaseException)
            {
                ExpDatabaseException exception = exceptionContext.Exception as ExpDatabaseException;

                Logger.Error("Database error occured", exception);
//                this.View("~/Areas/Admin/Views/Errors/SecurityError.aspx", exception).
//                    ExecuteResult(this.ControllerContext);

                return;
            }

            Logger.Error("Error occured", exceptionContext.Exception);

            this.View("Error").ExecuteResult(this.ControllerContext);
        }
    }
}
