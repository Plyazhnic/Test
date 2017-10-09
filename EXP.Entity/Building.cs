using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class Building
    {
        public Building()
        {
            this.ParkingInventory = new HashSet<ParkingInventory>();
        }
    
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public int OwnerID { get; set; }
        public int? ManagerID { get; set; }
        public int AddressID { get; set; }
        public int MailingAddressID { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string PrimaryFaxNumber { get; set; }
        public string MailingPhoneNumber { get; set; }
        public string MailingFaxNumber { get; set; }
        public bool IsActive { get; set; }
        public string HasLots { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string Notes { get; set; }
        public decimal? TaxRate { get; set; }
        public int DueDateReminder { get; set; }
        public int ReminderFrequency { get; set; }
        public int ReminderCutoff { get; set; }
        public int? InvoiceCutoff { get; set; }
        public decimal? ACHDiscountRate { get; set; }
        public decimal? CCDiscountRate { get; set; }
        public decimal? MontlyFee { get; set; }
        public int? MFBillingDay { get; set; }
        public string VBNotification { get; set; }

        public string HasMultipleTenants { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        
        public virtual UserProfile Owner { get; set; }
        public virtual Address BuildingAddress { get; set; }
        public virtual OnlineCheck BankAccount { get; set; }

        public virtual ICollection<ParkingInventory> ParkingInventory { get; set; }

        //for datatables
        public string Tenants { get; set; }
        public string ParkingLots { get; set; }
    }
    
}
