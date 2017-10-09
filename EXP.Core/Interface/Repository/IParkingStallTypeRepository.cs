using EXP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Core.Interface
{
    public interface IParkingStallTypeRepository
    {
        /// <summary>
        /// Get list all Parking Stall Types
        /// </summary>
        /// <returns></returns>
        List<ParkingStallType> ListParkingStallTypes();
    }
}
