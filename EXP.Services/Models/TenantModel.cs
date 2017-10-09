using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Services.Models
{
    [Serializable()]
    public class TenantModel
    {
        public TenantModel()
        {
            this.isManager = false;
            this.isMailing = false;

            this.Company = new CompanyModel();
            this.ManagerProfile = new UserProfileModel();
            this.ManagerAddress = new AddressModel();
            this.ManagerPhone = new PhoneModel();
            this.ParkingInventory = new ParkingInventoryModel();
            this.Vehicle = new VehicleModels();
            this.Payment = new PaymentModel();
        }

        public virtual bool isManager { get; set; }
        public virtual bool isMailing { get; set; }
        public virtual bool personally { get; set; }
        public virtual string EmailString { get; set; }
        public virtual string NameString { get; set; }
        public virtual string PayingString { get; set; }
        public string Emails { get; set; }
        public string Companies { get; set; }

        public virtual CompanyModel Company { get; set; }
        public virtual UserProfileModel ManagerProfile { get; set; }
        public virtual AddressModel ManagerAddress { get; set; }
        public virtual PhoneModel ManagerPhone { get; set; }
        public virtual ParkingInventoryModel ParkingInventory { get; set; }

        public virtual VehicleModels Vehicle { get; set; }
        public virtual PaymentModel Payment { get; set; }
    }
}
