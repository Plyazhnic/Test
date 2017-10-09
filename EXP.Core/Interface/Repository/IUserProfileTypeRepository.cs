using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IUserProfileTypeRepository
    {
        /// <summary>
        /// get list of profile type
        /// </summary>
        /// <returns></returns>
        List<UserProfileType> ListUserProfileType();
    }
}
