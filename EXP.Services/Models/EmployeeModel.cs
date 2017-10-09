using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Models
{
    [Serializable()]
    public class EmployeeModel
    {
        public EmployeeModel()
        {
            this.ParkingInventory = new ParkingInventoryModel();
            this.Profile = new UserProfileModel();
            this.Address = new AddressModel();
            this.Phone = new PhoneModel();
            this.Vehicle = new VehicleModels();
            this.Payment = new PaymentModel();
        }

        public virtual bool Myself { get; set; }
        
        public virtual ParkingInventoryModel ParkingInventory { get; set; }
        public virtual UserProfileModel Profile { get; set; }
        public virtual AddressModel Address { get; set; }
        public virtual PhoneModel Phone { get; set; }
        public virtual VehicleModels Vehicle { get; set; }
        public virtual PaymentModel Payment { get; set; }
    }
}
