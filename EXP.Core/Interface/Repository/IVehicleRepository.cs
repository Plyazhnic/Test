using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IVehicleRepository
    {
        /// <summary>
        /// List Vehicles
        /// </summary>
        /// <param name="userProfileID"></param>
        /// <returns></returns>
        List<Vehicle> ListVehicles(int userProfileID);
        /// <summary>
        /// Get Vehicles
        /// </summary>
        /// <param name="userProfileID"></param>
        /// <returns></returns>
        List<Vehicle> GetVehicles(int userProfileID);
        /// <summary>
        /// Get Vehicle
        /// </summary>
        /// <param name="userProfileID"></param>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        Vehicle GetVehicle(int vehicleId);
        /// <summary>
        /// Create Vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        int CreateVehicle(Vehicle vehicle);
        /// <summary>
        /// Update Vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        void UpdateVehicle(Vehicle vehicle);
        /// <summary>
        /// Remove Vehicle
        /// </summary>
        /// <param name="vehicleId"></param>
        void RemoveVehicle(int vehicleId);
    }
}
