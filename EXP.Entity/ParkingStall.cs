using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class ParkingStall
    {
        public ParkingStall()
        {
            this.ParkingInventory = new HashSet<ParkingInventory>();
        }
    
        public int ParkingStallID { get; set; }
        public int ParkingStallTypeID { get; set; }
        public int LotID { get; set; }
        public string StallLocation { get; set; }
        public int StallNumber { get; set; }

        public decimal? MontlyRate { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MaxRate { get; set; }
        public decimal? FlatRate { get; set; }
        public int? TimeIncrement { get; set; }
        public int? GracePeriod { get; set; }
        public int NumberStalls { get; set; }  //COUNT
        public int BookedStalls { get; set; }
        public decimal? OverSell { get; set; }

        public string StallDescription { get; set; }
        public bool isActive { get; set; }

        // for registration---------------
        public decimal? ReservedRate { get; set; }
        public decimal? UnreservedRate { get; set; }
        public int? ReservedSpace { get; set; }
        public int? UnreservedSpace { get; set; }
        //---------------------

        public string StallName { get; set; }
        public Nullable<byte> LotLevel { get; set; }
        public string StallImagePath { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public virtual ParkingStallType ParkingStallType { get; set; }
    
        public virtual ICollection<ParkingInventory> ParkingInventory { get; set; }
    }
    
}
