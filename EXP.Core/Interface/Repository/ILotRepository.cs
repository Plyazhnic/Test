using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface ILotRepository
    {
        /// <summary>
        /// Create Lot
        /// </summary>
        /// <param name="lot"></param>
        int CreateLot(Lot lot);

        /// <summary>
        /// Get lot by id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        Lot GetLot(int? lotID);

        /// <summary>
        /// get list of lots
        /// </summary>
        /// <returns></returns>
        List<Lot> ListLots();

        /// <summary>
        /// get list of lots for building
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        List<Lot> ListLotsForBuilding(int buildingId);

        /// <summary>
        /// List of lots for page
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        List<Lot> ListLotsForPage(TableList listParam);
        /// <summary>
        /// Get Building Address by Lot ID
        /// </summary>
        /// <param name="LotID"></param>
        /// <returns></returns>
        Building GetBuildingAddress(int LotID);

        /// <summary>
        /// get count of lots in DB
        /// </summary>
        /// <returns></returns>
        int GetLotsCount(TableList listParam);

        /// <summary>
        /// delete lot by id
        /// </summary>
        /// <param name="lotId"></param>
        void DeleteLot(int lotId);

        /// <summary>
        /// Update Lot
        /// </summary>
        /// <param name="lot"></param>
        void UpdateLot(Lot lot);
        /// <summary>
        /// Search Lot
        /// </summary>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        List<LotWide> SearchLot(SearchLot searchParam);

        /// <summary>
        /// get list of lots to building
        /// </summary>
        /// <returns></returns>
        List<BuildingToLot> GetListLots();

        /// <summary>
        /// delete  BuildingToLot Entry in db
        /// </summary>
        /// <param name="bRelationship"></param>
        void DeleteBuildingToLotEntry(BuildingToLot bRelationship);

        /// <summary>
        /// Create Building To Lot Relationship
        /// </summary>
        /// <param name="bRelationship"></param>
        void AddBuildingToLot(BuildingToLot bRelationship);

        /// <summary>
        /// Get building to lot record by id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        BuildingToLot GetBuildingToLot(int buildingToLotID);
    }
}
