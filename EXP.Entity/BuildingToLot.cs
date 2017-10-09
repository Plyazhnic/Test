using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class BuildingToLot
    {
        public int BuildingToLotID { get; set; }
        public int BuildingID { get; set; }
        public int LotID { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual Building Building { get; set; }
        public virtual Lot Lot { get; set; }
    }
    
}
