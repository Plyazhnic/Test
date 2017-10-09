using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IPhoneTypeRepository
    {
        /// <summary>
        /// get list types of phone
        /// </summary>
        /// <returns></returns>
        List<PhoneType> ListPhoneTypes();
    }
}
