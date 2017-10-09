using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    /// <summary>
    /// Create record parking inventory 
    /// </summary>
    /// <param name="countStall"></param>
    public interface IParkingInventoryRepository
    {
        /// <summary>
        /// Create record parking inventory for company(reserved and unreserved)
        /// </summary>
        /// <param name="editParkingInventory"></param>
        /// <param name="countStall"></param>
        void CreateParkingInventoryForCompany(ParkingInventory newParkingInventory);

        /// <summary>
        /// Create record parking inventory 
        /// </summary>
        /// <param name="newParkingInventory"></param>
        /// <param name="countStall"></param>
        void CreateParkingInventory(ParkingInventory newParkingInventory);

        /// <summary> 
        /// Get ParkingInventory by parkingIventoryID 
        /// </summary> 
        /// <param name="inventoryId"></param> 
        /// <returns></returns> 
        ParkingInventory GetParkingInventoryByID(int inventoryId);

        /// <summary>
        /// Get allocated stall for stall 
        /// </summary>
        /// <param name="newParkingInventory"></param>
        /// <param name="countStall"></param>
        ParkingInventory GetAllocatedStall(int stallId);

        /// <summary>
        /// List allocated parking stalls for page
        /// </summary>
        /// <param name="newParkingInventory"></param>
        /// <param name="countStall"></param>
        List<ParkingInventory> ListAllocatedStallsForPage(TableList listParam);

        /// <summary>
        /// get count of allocated stalls in DB
        /// </summary>
        /// <returns></returns>
        int GetAllocatedStallsCount(TableList listParam);

        /// <summary>
        /// Get list of inventories for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        List<ParkingInventory> GetParkingInventoryForCompany(int companyId);

        /// <summary>
        /// Update parking inventory
        /// </summary>
        /// <param name="editParkingInventory"></param>
        void UpdateParkingInventory(ParkingInventory editParkingInventory);

        /// <summary>
        /// Delete parking inventory by id
        /// </summary>
        /// <param name="inventoryId"></param>
        void DeleteParkingInventory(int inventoryId);
    }
}
