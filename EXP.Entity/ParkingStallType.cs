using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class ParkingStallType
    {
        public ParkingStallType()
        {
            this.ParkingInventory = new HashSet<ParkingInventory>();
        }
    
        public int ParkingStallTypeID { get; set; }
        public string ParkingStallType1 { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual ICollection<ParkingInventory> ParkingInventory { get; set; }
    }
    
}
