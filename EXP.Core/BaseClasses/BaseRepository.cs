using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using EXP.Core.Util;

namespace EXP.Core.BaseClasses
{
    public class BaseRepository
    {
        //protected readonly IUnitOfWork UnitOfWork;

        //public BaseRepository(IUnitOfWork unitOfWork)
        //{
        //    UnitOfWork = unitOfWork;
        //}

        //public BaseRepository() { }

        //protected IDisposable EnterCommandScope()
        //{
        //    return UnitOfWork.EnterCommandScope();
        //}

        //protected SqlCommand CreateProcedureCommand(string name)
        //{
        //    return UnitOfWork.CreateProcedureCommand(name);
        //}
    }
}
