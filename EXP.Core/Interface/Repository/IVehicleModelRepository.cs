using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IVehicleModelRepository
    {
        /// <summary>
        /// List Models
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        List<VehicleModel> ListModels(int makeId);
        /// <summary>
        /// Get Model
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        VehicleModel GetModel(string modelName);

    }
}
