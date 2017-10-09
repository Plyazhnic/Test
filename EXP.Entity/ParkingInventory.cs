using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class ParkingInventory
    {
        public int ParkingInventoryID { get; set; }
        public Nullable<int> BuildingID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> LotID { get; set; }
        public Nullable<int> ParkingStallID { get; set; }
        public Nullable<int> UserProfileID { get; set; }
        public Nullable<int> UserVehicleID { get; set; }
        public string PermitNumber { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        
        public int NumberOfStalls { get; set; }
        public decimal MontlyRate { get; set; }
        public decimal DiscountRate { get; set; }
        public bool MarketRate { get; set; }
        public decimal OverSell { get; set; }
        public int CountStalls { get; set; }

        public Nullable<int> ReservedSpaces { get; set; }
        public Nullable<int> UnReservedSpaces { get; set; }
        public Nullable<int> ReservedSpacesCost { get; set; }
        public Nullable<int> UnReservedSpacesCost { get; set; }
        
        public virtual Building Building { get; set; }
        public virtual Lot Lot { get; set; }
        public virtual ParkingStall ParkingStall { get; set; }
        public virtual Company Company { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public override string ToString()
        {
            return string.Format("ParkingInventoryID: {0}, BuildingID: {1}, TenantID: {2}, LotID: {3}, ParkingStallID: {4}, UserProfileID: {5}, UserVehicleID: {6}, PermitNumber: {7}, isActive: {8}, EffectiveFrom: {9}, EffectiveTo: {10}",
                ParkingInventoryID, BuildingID, CompanyID, LotID, ParkingStallID, UserProfileID, UserVehicleID,  PermitNumber, isActive, EffectiveFrom, EffectiveTo);
        }
    }
    
}
