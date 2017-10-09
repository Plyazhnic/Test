using System;
using System.ComponentModel.DataAnnotations;
using EXP.Entity;
using System.Collections.Generic;

namespace EXP.Services.Models
{
    public class RemoveVehicleModel
    {
        [Required(ErrorMessage = "Vehicle Id required")]
        public int VehicleId { get; set; }
    }
       
    
    public class NewVehicleModel
    {
        public int? VehicleMakeID { get; set; }

        public int? VehicleModelID { get; set; }

        public int? Year { get; set; }

        [StringLength(32, ErrorMessage = "Max License Number length is 32")]
        public string LicensePlateNumber { get; set; }

        [StringLength(32, ErrorMessage = "Max Color length is 32")]
        public string Color { get; set; }

        [StringLength(32, ErrorMessage = "Max Permit Number length is 32")]
        public string PermitNumber { get; set; }
    }

    public class EditVehicleModel
    {
        [Required(ErrorMessage = "Vehicle Id required")]
        public int? VehicleID { get; set; }

        public int? VehicleMakeID { get; set; }

        public int? VehicleModelID { get; set; }

        public int? Year { get; set; }

        [StringLength(32, ErrorMessage = "Max License Number length is 32")]
        public string LicensePlateNumber { get; set; }

        [StringLength(32, ErrorMessage = "Max Color length is 32")]
        public string Color { get; set; }

        [StringLength(32, ErrorMessage = "Max Permit Number length is 32")]
        public string PermitNumber { get; set; }
    }

    public class ResetPasswordModel
    {
        public string qString { get; set; }

        [StringLength(short.MaxValue, MinimumLength = 6, ErrorMessage = "Min password length is 6")]
        public string Password { get; set; }
        
        public bool check { get; set; }
    }

    public class EditPasswordModel
    {
        public string OldPassword { get; set; }

        [StringLength(short.MaxValue, MinimumLength = 6, ErrorMessage = "Min password length is 6")]
        
        public string NewPassword { get; set; }
    }

    public class ChangeEmailModel
    {
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format address")]
        public string NewEmail { get; set; }
    }

    public class BuildingAddressModel
    {
        public bool? Updated { get; set; }

        public int BuildingID { get; set; }

        public int StateID { get; set; }

        [StringLength(10, ErrorMessage = "Max Zip Code length is 10")]
        public string ZipCode { get; set; }

        [StringLength(256, ErrorMessage = "Max Building name length is 256")]
        public string BuildingName { get; set; }

        [StringLength(256, ErrorMessage = "Max Building address1 length is 256")]
        public string Address1 { get; set; }

        [StringLength(50, ErrorMessage = "Max City length is 50")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "State Name length is 50")]
        public string StateName { get; set; }

    }
   
    public class PersonalDataModel
    {
        [StringLength(256, ErrorMessage = "Max first name length is 256")]
        public string FirstName { get; set; }

        [StringLength(256, ErrorMessage = "Max last name is 256")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email format address")]
        public string Email { get; set; }
    }

    public class PersonalAddressModel
    {
        public int AddressID { get; set; }

        [StringLength(50, ErrorMessage = "Max city length is 50")]
        public string City { get; set; }

        [StringLength(256, ErrorMessage = "Max address1 length is 256")]
        public string Address1 { get; set; }
        
        [StringLength(256, ErrorMessage = "Max address2 length is 256")]
        public string Address2 { get; set; }
        
        public int? StateID { get; set; }

        [StringLength(10, ErrorMessage = "Max zip length is 10")]
        public string Zipcode { get; set; }
    }

    public class NewPhoneModel
    {
        [Required(ErrorMessage = "Select type")]
        public int PhoneTypeID { get; set; }

        [StringLength(8, ErrorMessage = "Max Area Code length is 8")]
        public string AreaCode { get; set; }

        [StringLength(15, ErrorMessage = "Max PhoneNumber length is 15")]
        public string PhoneNumber { get; set; }
    }

    public class EditPhoneModel
    {
        public int PhoneID { get; set; }

        [Required(ErrorMessage = "Select type")]
        public int PhoneTypeID { get; set; }

        [StringLength(8, ErrorMessage = "Max Area Code length is 8")]
        public string AreaCode { get; set; }

        [StringLength(15, ErrorMessage = "Max PhoneNumber length is 15")]
        public string PhoneNumber { get; set; }
    }

    public class RemovePhoneModel
    {
        [Required(ErrorMessage = "Phone Id required")]
        public int PhoneID { get; set; }
    }

    [Serializable()]
    public class OnlineCheckModel
    {
        public int? OnlineCheckId { get; set; }
        
        public string NameOnAccount { get; set; }

        public int OnlineCheckingTypeID { get; set; }

        public string BankName { get; set; }

        public string RoutingNumber { get; set; }

        public string CheckingAccountNumber { get; set; }
        
        public bool isTenant { get; set; }
    }

    [Serializable()]
    public class CreditCardModel
    {
        public int? CreditCardID { get; set; }

        public int? AddressID { get; set; }

        public int? OldAddressId { get; set; }

        public string CHFirstName { get; set; }

        public string CHLastName { get; set; }

        public string CardNumber { get; set; }

        public string ExpDateMount { get; set; }

        public string ExpDateYear { get; set; }

        public string CVV { get; set; }

        public bool AutoPay { get; set; }
        
        public bool isHome { get; set; }

        public bool isOffice { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public int? StateID { get; set; }

        public string ZipCode { get; set; }
        public bool isTenant { get; set; }
    }

    public class ParkingInfoModel
    {
        public string UserName { get; set; }
        public int LotID { get; set; }
        public bool isTenant { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public Nullable<DateTime> StartDate { get; set; }
        public int ReservedSpaces { get; set; }
        public Nullable<int> ReservedSpacesCost { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> EndDate { get; set; }
        public int UnReservedSpaces { get; set; }
        public Nullable<int> UnReservedSpacesCost { get; set; }
    }

    public class EditCompanyEmployee
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "")]
        [DataType(DataType.EmailAddress, ErrorMessage = "")]
        public string Email { get; set; }
        public bool Personally { get; set; }
        public int CompanyID { get; set; }
    }
    public class NewVehicleReturnedModel
    {
       public Alert Alert { get; set; }
       public int deleteAlertId { get; set; }
       public int VehicleID { get; set; }
       public bool success { get; set; }
       public string Message { get; set; }
    }
    public class EditVehicleReturnedModel
    {
        public Alert Alert { get; set; }
        public int VehicleID { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }
    }

    public class RemoveVehicleReturnedModel
    {
        public List<Alert> Alerts { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }
    }
    public class GetVehicleModel
    {
        public string UserName { get; set; }        
        public int VehicleID { get; set; }
    }
    public class GetVehicleReturnedModel
    {
        public Vehicle Vehicle { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }    
    }
    public class ListVehiclesModelsReturnedModel
    {
        public List<Vehicle> ListVehicles { get; set; }
    }

    public class ListVehicleMakesModelsReturnedModel
    {
       public List<VehicleMake> ListVehicleMakes { get; set; }    
    }

    public class ListVehicleModelsReturnedModel
    {
       public  List<VehicleModel> ListVehicleModels { get; set; }
    }

    public class PersonallyAddressModelReturnedModel
    {
        public Address Address { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }     
    }
    public class EditPersonalAddressModelReturnedModel
    {
        public Alert Alert { get; set; }
        public bool success { get; set; }
        public string Message { get; set; } 
    }

    public class DashboardReturnedModel
    {
        public string CreditCard { get; set; }
        public UserProfile UserProfile { get; set; }
        public Address Address { get; set; }
        public List<Alert> Alerts { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Vehicle> Vehicles { get; set; }        
        public List<Building> Buildings { get; set; }
    }
    public class MessageReturnedModel
    {
        public bool success { get; set; }
        public string Message { get; set; }            
    }

    public class ListPhoneTypesReturnedModel
    {
        public List<PhoneType> PhoneTypes { get; set; }    
    }

    public class GetPhonesReturnedModel
    {
        public dynamic PhoneObject { get; set; }
        public bool success { get; set; }
        public string Message { get; set; }            
    }
    public class NewPhoneReturnedModel
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public int NewPhoneId { get; set; }       
    }
    public class ListPhonesReturnedModel
    {
        public List<Phone> Phones { get; set; }       
    }

    public class EditPhoneReturnedModel
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public int PhoneId { get; set; }
    }

    public class RemovePhoneReturnedModel
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public int PhonesCount { get; set; }
    }
    public class ListStatesReturnedModel
    {
        public List<zState> States { get; set; }       
    }

    public class GetPhoneModel
    {
        public int PhoneId { get; set; }
        public string UserName { get; set; }
    }

    public class GetPhoneReturnedModel
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public Phone phone { get; set; }
    }

    public class ListBuildingTenantsModel
    { 
      public int BuildingId {get; set;}
      public int Days { get; set; }
    }
    public class EditBuildingAddressReturnedModel
    {
        public bool success { get; set; }
        public string Message { get; set; }
        public BuildingAddressModel BuildingAddressModel { get; set; }     
    }
}