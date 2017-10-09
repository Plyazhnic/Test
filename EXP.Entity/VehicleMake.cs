using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class VehicleMake
    {
        public VehicleMake()
        {
            this.Vehicle = new HashSet<Vehicle>();
        } 
        public int VehicleMakeID { get; set; }
        public string VehicleMake1 { get; set; }
        public string VehicleMakeDescription { get; set; }
        public bool isActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    
        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
    
}
