using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Models
{
    [Serializable()]
    public class BuildingOwnerModel
    {
        public virtual bool Same { get; set; }
        public virtual int HandlerType { get; set; }
        public virtual BuildingModel Building { get; set; }
        public virtual ContactModel OwnerContact { get; set; }
        public virtual ContactModel PropertyManagementContact { get; set; }
        public virtual ContactModel ParkingManagementContact { get; set; }
        public virtual ParkingStallModel StallReserved { get; set; }
        public virtual ParkingStallModel StallUnreserved { get; set; }
        public virtual ParkingStallModel StallVisitors { get; set; }



        private readonly List<string> _tenants = new List<string>();
        public List<string> Companies = new List<string>();
        public virtual List<string> Tenants
        {
            get
            {
                return _tenants;
            }
        }  
    }
}