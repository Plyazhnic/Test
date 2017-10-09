using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class Company
    {
        public Company()
        {
            this.Address = new Address();
            this.ParkingInventory = new HashSet<ParkingInventory>();
        }

        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? AddressID { get; set; }
        public bool asBuilding { get; set; }
        public int? ManagerID { get; set; }
        public int? EmployeeID { get; set; }
        public int? WorkPhoneID { get; set; }
        public int? FaxID { get; set; }
        public string Suite { get; set; }
        public string EmailAddress { get; set; }
        
        public Nullable<bool> hasPermitEmail { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }        
        public Address Address { get; set; }
        public UserProfile ManagerProfile { get; set; }
        public Phone WorkPhone { get; set; }
        public Phone Fax { get; set; }
        public virtual ICollection<ParkingInventory> ParkingInventory { get; set; }
    }
    
}
