using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class VehicleModel
    {
        public VehicleModel()
        {
            this.Vehicle = new HashSet<Vehicle>();
        }
        public int VehicleModelID { get; set; }
        public int VehicleMakeID { get; set; }
        public string VehicleModel1 { get; set; }
        public int Year1 { get; set; }
        public string VehicleModelDescription { get; set; }
        public bool? isActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    
        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
    
}
