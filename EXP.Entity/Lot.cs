using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class Lot
    {
        public Lot()
        {
            this.ParkingInventory = new HashSet<ParkingInventory>();
            this.Buildings = new HashSet<Building>();
        }

        public int LotID { get; set; }
        public string LotName { get; set; }
        public int ParkingOperatorID { get; set; }
        public int ParkingManagerID { get; set; }
        public string LotNumber { get; set; }
        public string LotLevel { get; set; }
        public int AddressID { get; set; }
        public int MailingAddressID { get; set; }
        public int ReservedPrice { get; set; }
        public int UnReservedPrice { get; set; }
        public bool IsActive { get; set; }
        public string hasMultipleBuildings { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }

        public virtual ICollection<ParkingInventory> ParkingInventory { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }

        public Address Address { get; set; }
        public Building Building { get; set; }

        public bool IsBuilding { get; set; }


        //for datatables
      //  public string BuildingID { get; set; }
        public string ParkingAttendants { get; set; }
  //      public string City { get; set; }
    //    public string Status { get; set; }
        public string Inventory { get; set; }
        public string Comments { get; set; }

        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MailingPhoneNumber { get; set; }
        public string MailingFaxNumber { get; set; }
    }
}
