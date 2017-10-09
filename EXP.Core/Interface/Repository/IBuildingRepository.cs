using System;
using System.Collections.Generic;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IBuildingRepository
    {
        /// <summary>
        /// Create Building
        /// </summary>
        /// <param name="building"></param>
        int CreateBuilding(Building building);

        /// <summary>
        /// Get list of buildings for page
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="search"> </param>
        /// <returns></returns>
        List<Building> ListBuildingsForPage(TableList listParam);

        List<BuildingWide> SearchBuildings(SearchBuilding searchParam);

        //TODO: DELETE
        //List<Lot> ListBuildingLotsForPage(TableList listParam);

        /// <summary>
        /// Get all owner's buildings
        /// </summary>
        /// <returns></returns>
        List<Building> ListBuildings(int ownerId);
        /// <summary>
        /// Get list all buildings
        /// </summary>
        /// <returns></returns>
        List<Building> ListBuildings();
        /// <summary>
        /// Get building by id
        /// </summary>
        /// <param name="buildingId"></param>
        Building GetBuilding(int buildingId);

        /// <summary>
        /// Get buildings by lot id
        /// </summary>
        /// <param name="buildingId"></param>
        List<Building> GetBuildingsForLot(int lotId);

        /// <summary>
        /// get building for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Building GetBuildingForCompany(int companyId);

        /// <summary>
        /// get count of buildings in DB
        /// </summary>
        /// <returns></returns>
        int GetBuildingsCount(string search);

        /// <summary>
        /// Get Building Lots Count
        /// </summary>
        /// <param name="search"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
    //    int GetBuildingLotsCount(string search, int buildingId);

        /// <summary>
        /// delete building by id
        /// </summary>
        /// <param name="buildingId"></param>
        bool DeleteBuilding(int buildingId);
        /// <summary>
        /// Update building data
        /// </summary>
        /// <param name="building"></param>
        void UpdateBuilding(Building building);
        /// <summary>
        /// List Buildings For Dashboard
        /// </summary>
        /// <returns></returns>
        List<Building> ListBuildingsForDashboard(int UserProfileID);
        /// <summary>
        /// Create Building. Argument data contains info only about building addres and name
        /// </summary>
        /// <returns> Returns id of created building</returns>
        int CreateBuildingWithNewAddress(Building Building);
        /// <summary>
        /// Update Building
        /// </summary>
        /// <param name="BuildingID"></param>
        /// <returns>return weather operation returns succes</returns>
        bool UpdateBuildingWithNewAddress(Building Building);

        /// <summary>
        /// Create Building To Company Relationship
        /// </summary>
        /// <param name="bRelationship"></param>
        void AddBuildingToCompany(BuildingToCompany bRelationship);

        //TODO DELETE
        /// <summary>
        /// Get Building With Address
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        Building GetBuildingWithAddress(int? buildingId);
    }
}
