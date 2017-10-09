using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IVehicleMakeRepository
    {

        /// <summary>
        /// List of vehicle for page
        /// </summary>
        /// <returns></returns>

        List<VehicleModel> ListVehicleModelsForPage(TableList listParam);

        /// <summary>
        /// List of vehicle makes for page
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>

        List<VehicleMake> ListVehicleMakesForPage(TableList listParam);

        /// <summary>
        /// get count of vehicle makes in DB
        /// </summary>
        /// <returns></returns>
        int GetVehicleMakesCount(string search);
        
        /// <summary>
        /// get count of vehicle models in DB
        /// </summary>
        /// <returns></returns>
        int GetVehicleModelsCount(TableList listParam);

        /// <summary>
        /// Add new vehicle make
        /// </summary>
        bool AddVehicleMake(VehicleMake make);

        /// <summary>
        /// delete vehicle make by id
        /// </summary>
        /// <param name="vehicleMakeId"></param>
        void DeleteVehicleMake(int vehicleMakeId);
        /// <summary>
        /// Get Vehicle Make by Id
        /// </summary>
        /// <param name="MakeId"></param>
        /// <returns></returns>
        VehicleMake GetVehicleMakeById(int MakeId);
        /// <summary>
        /// Change Vehicle Make
        /// </summary>
        /// <returns></returns>
        bool UpdateVehicleMake(VehicleMake make);
        /// <summary>
        /// Get Vehicle Model By Id
        /// </summary>
        /// <param name="ModelId"></param>
        /// <returns></returns>
        VehicleModel GetVehicleModelById(int ModelId);
        /// <summary>
        /// Change Vehicle Model
        /// </summary>
        /// <returns></returns>
        bool ChangeVehicleModel(VehicleModel model);
        /// <summary>
        /// Add Vehicle Model
        /// </summary>
        /// <returns></returns>
        bool AddVehicleModel(VehicleModel model);
        /// <summary>
        /// List Makes
        /// </summary>
        /// <returns></returns>
        List<VehicleMake> ListMakes();
        /// <summary>
        /// Get Make
        /// </summary>
        /// <param name="makeName"></param>
        /// <returns></returns>
        VehicleMake GetMake(string makeName);
    }
}
