using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IPhoneRepository
    {
        /// <summary>
        /// Create phone
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="userProfileId"></param>
        int CreatePhone(Phone phone);

        /// <summary>
        /// Create phone for user 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="userProfileId"></param>
        int CreatePhoneForUser(Phone phone, int userProfileId);

        /// <summary>
        /// Update phone
        /// </summary>
        /// <param name="phone"></param>
        void UpdatePhone(Phone phone);

        /// <summary>
        /// Remove user phone
        /// </summary>
        /// <param name="phoneID"></param>
        /// <param name="profileID"></param>
        void RemovePhoneForUser(int phoneID, int profileID);

        /// <summary>
        /// get phone data by id
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        Phone GetPhone(int phoneId);

        /// <summary>
        /// list phones for current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Phone> ListPhonesForUser(int userId);

         /// <summary>
        /// get phones for current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Phone> GetPhonesForUser(int userId);
        /// <summary>
        /// Get all phones for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Phone> GetAllPhonesForUser(int userId);

        /// <summary>
        /// get phones for current user by type
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Phone GetPhoneForUserByType(int userId, int phoneTypeId);
    }
}
