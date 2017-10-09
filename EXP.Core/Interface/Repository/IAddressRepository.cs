using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IAddressRepository
    {
        /// <summary>
        /// Create address
        /// </summary>
        /// <param name="newAddress"></param>
        /// <returns></returns>
        int CreateAddress(Address newAddress);

        /// <summary>
        /// Update address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        void UpdateAddress(Address address);

        /// <summary>
        /// Get address by id
        /// </summary>
        /// <param name="addressID"></param>
        /// <returns></returns>
        Address GetAddress(int addressID);

        /// <summary>
        /// Delete address by id
        /// </summary>
        /// <param name="addressId"></param>
        void DeleteAddress(int addressId);

        /// <summary>
        /// Get building address by lot id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        Address GetBuildingAddressByLot(int buildingToLotId);
    }
}
