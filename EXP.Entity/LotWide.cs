using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public class LotWide
    {
        public string LotName { get; set; }
        public string BuildingName { get; set; }
        public int ParkingOperatorID { get; set; }
        public int ParkingManagerID { get; set; }
        public string ParkingAttendants { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public string Inventory { get; set; }
        public int LotID { get; set; }
    }
}
