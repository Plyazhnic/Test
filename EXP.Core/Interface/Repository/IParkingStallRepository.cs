using EXP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Core.Interface
{
    public interface IParkingStallRepository
    {
        /// <summary>
        /// Create Parking Stall
        /// </summary>
        /// <param name="parkingStall"></param>
        int CreateParkingStall(ParkingStall parkingStall);

        /// <summary>
        /// Get Parking Stall by id
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        ParkingStall GetParkingStall(int parkingStallID);

        /// <summary>
        /// Update Parking Stall
        /// </summary>
        /// <param name="parkingStall"></param>
        void UpdateParkingStall(ParkingStall parkingStall);

        /// <summary>
        /// Get parking info for lot
        /// </summary>
        /// <param name="lotID"></param>
        /// <returns></returns>
        ParkingStall GetParkingInfoForLot(int lotID);

        /// <summary>
        /// List parking stalls 
        /// </summary>
        /// <returns></returns>
        List<ParkingStall> ListParkingStall();

        /// <summary>
        /// List parking stalls by stall type
        /// </summary>
        /// <param name="stallType"></param>
        /// <returns></returns>
        List<ParkingStall> ListParkingStallByType(int lotId, int stallType);

        /// <summary>
        /// List montly parking stalls for page
        /// </summary>
        /// <param name="stallType"></param>
        /// <returns></returns>
        List<ParkingStall> ListMontlyParkingStallsForPage(TableList listParam);

        /// <summary>
        /// get count of montly stalls in DB
        /// </summary>
        /// <returns></returns>
        int GetMontlyStallsCount(TableList listParam);

        /// <summary>
        /// List visitor parking stalls for page
        /// </summary>
        /// <param name="stallType"></param>
        /// <returns></returns>
        List<ParkingStall> ListVisitorParkingStallsForPage(TableList listParam);

        /// <summary>
        /// get count of visitor stalls in DB
        /// </summary>
        /// <returns></returns>
        int GetVisitorStallsCount(TableList listParam);

        /// <summary>
        /// delete parking stall by id
        /// </summary>
        /// <param name="lotId"></param>
        void DeleteParkingStall(int stallId);
    }
}
