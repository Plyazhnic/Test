using System;
using System.ComponentModel.DataAnnotations;

namespace EXP.Services.Models
{
    [Serializable()]
    public class ProfileTypeModel
    {
        public int ProfileTypeID { get; set; }
        public int BuildingToLotID { get; set; }
        public int TenantID { get; set; }
        public int LotID { get; set; }
        public string Path { get; set; }
        public string RootUrl { get; set; }
    }

    #region Tenant
    public class PersonalModel
    {
        public string UserName { get; set; }

        [StringLength(256, ErrorMessage = "Max first name length is 256")]
        public string FirstName { get; set; }

        [StringLength(256, ErrorMessage = "Max last name is 256")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format address")]
        public string EmailAddress { get; set; }

    }

    [Serializable()]
    public class AddressModel
    {
        public int AddressID { get; set; }
        public int UserProfileID { get; set; }

   //     [StringLength(50, ErrorMessage = "Max city length is 50")]
        public string City { get; set; }

  //      [StringLength(256, ErrorMessage = "Max address1 length is 256")]
        public string Address1 { get; set; }

 //       [StringLength(256, ErrorMessage = "Max address2 length is 256")]
        public string Address2 { get; set; }

        public int? StateID { get; set; }

 //       [StringLength(10, ErrorMessage = "Max zip length is 10")]
        public string ZipCode { get; set; }

        public StateModel State { get; set; }
    }

    [Serializable()]
    public class PhoneModel
    {
        public int PhoneID { get; set; }
        

        public int PhoneTypeID { get; set; }

      //  [StringLength(8, ErrorMessage = "Max Area Code length is 8")]
        public string AreaCode { get; set; }

     //   [StringLength(15, ErrorMessage = "Max PhoneNumber length is 15")]
        public string PhoneNumber { get; set; }

        public PhoneTypeModel PhoneType { get; set; }
    }
    
    [Serializable()]
    public class PhoneTypeModel
    {
        public int PhoneTypeID { get; set; }
        public string PhoneType1 { get; set; }
        public string PhoneTypeDescription { get; set; }
        public bool isActive { get; set; }         
    }

    [Serializable()]
    public class CompanyModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Suite { get; set; }
        public bool asBuilding { get; set; }
        public int? AddressID { get; set; }
        public int? WorkPhoneID { get; set; }
        public int? FaxID { get; set; }
        public AddressModel Address { get; set; }
        public PhoneModel WorkPhone { get; set; }
        public PhoneModel Fax { get; set; }
    }

    public class EditCompanyManager
    {
        public string UserName { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "")]
        [DataType(DataType.EmailAddress, ErrorMessage = "")]
        public string EmailAddress { get; set; }
        public bool isManager { get; set; }
        public bool isMailing { get; set; }
    }
    
    public class CompanyEmailModel
    {
        public string EmailsString { get; set; }
        public string NamesString { get; set; }
        public string PayingString { get; set; }
    }

    [Serializable()]
    public class ParkingInventoryModel
    {
        public int ParkingInventoryID { get; set; }
        public Nullable<int> BuildingID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> LotID { get; set; }
        public Nullable<int> ParkingStallID { get; set; }
        public Nullable<int> UserProfileID { get; set; }
        public Nullable<int> UserVehicleID { get; set; }
        public string PermitNumber { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }

        public Nullable<int> ReservedSpaces { get; set; }
        public Nullable<int> UnReservedSpaces { get; set; }
        public Nullable<int> ReservedSpacesCost { get; set; }
        public Nullable<int> UnReservedSpacesCost { get; set; }

        public int NumberOfStalls { get; set; }
        public decimal MontlyRate { get; set; }
        public decimal DiscountRate { get; set; }
        public bool MarketRate { get; set; }
        public decimal OverSell { get; set; }
        public int CountStalls { get; set; }

        public CompanyModel Company { get; set; }
        public LotModel Lot { get; set; }
        public BuildingModel Building { get; set; }
        public ParkingStallModel ParkingStall { get; set; }
    }

    [Serializable()]
    public class VehicleModels
    {
        public string UserName { get; set; }
        public string UserProfileID { get; set; }
        public bool isTenant { get; set; }
        public int? Year { get; set; }
        public int? VehicleMakeID { get; set; }
        public int? VehicleModelID { get; set; }
        public string Color { get; set; }
        public string LicensePlateNumber { get; set; }
        public string PermitNumber { get; set; }
    }

    [Serializable()]
    public class PaymentModel
    {
        public int? PaymentID { get; set; }
        public int UserProfileID { get; set; }
        public bool isCreditCard { get; set; }
        public int? CreditCardID { get; set; }
        public int? OnlineCheckID { get; set; }
        public virtual OnlineCheckModel OnlineCheck { get; set; }
        public virtual CreditCardModel CreditCard { get; set; }
    }
    
    [Serializable()]
    public class BuildingModel
    {
        public BuildingModel()
        {
            this.BuildingAddress = new AddressModel();
            this.BankAccount = new OnlineCheckModel();
            this.Owner = new UserProfileModel();
        }
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public int OwnerID { get; set; }
        public int? ManagerID { get; set; }
        public int AddressID { get; set; }
        public int MailingAddressID { get; set; }
        public string MailingPhoneNumber { get; set; }
        public string MailingFaxNumber { get; set; }        
        public string PrimaryPhoneNumber { get; set; }
        public string PrimaryFaxNumber { get; set; }
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
        public virtual AddressModel BuildingAddress { get; set; }
        public virtual OnlineCheckModel BankAccount { get; set; }
        public virtual UserProfileModel Owner { get; set; }
    }
    
    [Serializable()]
    public class ParkingStallModel
    {
        public int ParkingStallID { get; set; }
        public int ParkingStallTypeID { get; set; }
        public int LotID { get; set; }
        public string StallLocation { get; set; }
        public int StallNumber { get; set; }
        public bool isActive { get; set; }
        public int? TimeIncrement { get; set; }
        public int? GracePeriod { get; set; }
        public int NumberStalls { get; set; }  
        public int BookedStalls { get; set; }
        public decimal? MontlyRate { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MaxRate { get; set; }
        public decimal? FlatRate { get; set; }
        public decimal? OverSell { get; set; }
        public decimal? ReservedRate { get; set; }
        public decimal? UnreservedRate { get; set; }
        public int? ReservedSpace { get; set; }
        public int? UnreservedSpace { get; set; }

        public string StallDescription { get; set; }
        public string StallName { get; set; }
        public Nullable<byte> LotLevel { get; set; }
        public string StallImagePath { get; set; }

        public ParkingStallTypeModel ParkingStallType { get; set; }
        public ParkingInventoryModel ParkingInventory { get; set; }
    }

    public class UserProfileTypeModel
    {
        public int UserProfileTypeID { get; set; }
        public string UserProfileType1 { get; set; }
    }
    
    public class BuildingToLotModel
    {
        public int BuildingToLotID { get; set; }
        public int BuildingID { get; set; }
        public int LotID { get; set; }
        public LotModel Lot { get; set; }
        public BuildingModel Building { get; set; }
    }

    [Serializable()]
    public class StateModel
    {
        public int StateID { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }

    #endregion

    #region Owner

    public class BuildingInformationModel
    {
        [Required(ErrorMessage = "Enter building name")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "Enter building address")]
        public string Address1 { get; set; }

        public string City { get; set; }

        [Required(ErrorMessage = "Enter zip code")]
        public string ZipCode { get; set; }

        public int StateID { get; set; }
    }

    public class OwnershipInformationModel
    {
        [Required(ErrorMessage = "Enter owner name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        public int PhoneTypeID { get; set; }

        [Required(ErrorMessage = "Enter telephone number")]
        public string PhoneNumber { get; set; }
    }

    public class PropertyManagementInformationModel
    {
        [Required(ErrorMessage = "Enter company name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        public int PhoneTypeID { get; set; }

        [Required(ErrorMessage = "Enter telephone number")]
        public string PhoneNumber { get; set; }
    }

    public class ParkingManagementInformationModel
    {
        public bool Same { get; set; }

        [Required(ErrorMessage = "Enter company name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        public int PhoneTypeID { get; set; }

        [Required(ErrorMessage = "Enter telephone number")]
        public string PhoneNumber { get; set; }
    }

    public class BankAccountInformationModel
    {
        [Required(ErrorMessage = "Enter bank name")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Enter account number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Enter holder name")]
        public string AccountHolderName { get; set; }

        [Required(ErrorMessage = "Enter routing number")]
        public string BankRoutingNumber { get; set; }
    }

    public class StallsInformationModel
    {
        [Required(ErrorMessage = "Enter number stalls")]
        public int NumberStalls { get; set; }

        [Required(ErrorMessage = "Enter stall location")]
        public string StallLocation { get; set; }

        [Required(ErrorMessage = "Enter parking rate")]
        public decimal Rate { get; set; }

        public int StallNumber { get; set; }
    }

    public class StallsUnreservedInformationModel
    {
        [Required(ErrorMessage = "Enter number stalls")]
        public int NumberStalls { get; set; }

        [Required(ErrorMessage = "Enter stall location")]
        public string StallLocation { get; set; }

        [Required(ErrorMessage = "Enter parking rate")]
        public decimal Rate { get; set; }

        [Required(ErrorMessage = "Enter oversell number")]
        public decimal? Oversell { get; set; }
    }

    [Serializable()]
    public class ContactModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneTypeID { get; set; }
        public string PhoneNumber { get; set; }
    }

    #endregion
}
