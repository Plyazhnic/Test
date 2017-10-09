using System;

namespace EXP.Entity
{
    public partial class Vehicle
    {
        public int VehicleID { get; set; }
        public int? UserProfileID { get; set; }
        public int? Year { get; set; }
        public int? VehicleMakeID { get; set; }
        public int? VehicleModelID { get; set; }
        public string Color { get; set; }
        public string LicensePlateNumber { get; set; }
        public string PermitNumber { get; set; }
        public bool? isActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual VehicleMake VehicleMake { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }

        public override string ToString()
        {
            return string.Format("VehicleID: {0}, UserProfileID: {1}, Year: {2}, VehicleMakeID: {3}, VehicleModelID: {4}, Color: {5}, LicensePlateNumber: {6}, PermitNumber: {7}, isActive: {8}, CreatedDate: {9}, UpdatedDate: {10}",
                VehicleID, UserProfileID, Year, VehicleMakeID, VehicleModelID, Color, LicensePlateNumber, PermitNumber, isActive, CreatedDate, UpdatedDate);
        }
    }
    
}
