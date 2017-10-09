using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EXP.Entity;
using System;

namespace EXP.Services.Models
{
    #region BuldingModels
    
    public class EditBuildingModel
    {
        public EditBuildingModel()
        {
            this.Building = new BuildingModel();
            this.BuildingAddress = new AddressModel();
            this.MailingAddress = new AddressModel();
        }
        
        public bool iSMailing { get; set; }

        public BuildingModel Building { get; set; }
        public AddressModel BuildingAddress { get; set; }
        public AddressModel MailingAddress { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Owners { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
        public IEnumerable<SelectListItem> MFBillngDays { get; set; }
        public IEnumerable<SelectListItem> InvoiceCutoffs { get; set; }
    }
    
    public class SearchBuildingModel
    {
        public string BuildingName { get; set; }
        public string OwnersFirstName { get; set; }
        public string OwnersLastName { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public string ZipCode { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public List<BuildingWideModel> Buildings { get; set; }
    }

    public class BuildingWideModel
    {
        public int BuildingID { get; set; }
        public string BuildingName { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
    }

    #endregion

    #region CompanyModels
    public class DataTableEmployeeModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int CompanyID { get; set; }
    }

    public class DataTableCompanyModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int BuildingID { get; set; }
    }
    #endregion

    #region LotModels
    public class EditLotModel
    {
        public EditLotModel()
        {
            this.Lot = new LotModel();
            this.LotAddress = new AddressModel();
            this.MailingAddress = new AddressModel();
        }
        public string BuildingIDString { get; set; }
        public bool iSBuilding { get; set; }
        public bool iSLot { get; set; }

        public LotModel Lot { get; set; }
        public AddressModel LotAddress { get; set; }
        public AddressModel MailingAddress { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> Buildings { get; set; }
        public IEnumerable<SelectListItem> Operators { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
    }

    public class SearchLotModel
    {
        public string LotName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int? StateID { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public List<LotWideModel> Lots { get; set; }
    }

    public class DataTableLotModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int? BuildingID { get; set; }
    }
    
    public class DataTableVehicleModelModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int MakeID { get; set; }
    }

    [Serializable()]
    public class LotModel
    {
        public LotModel() 
        {
            this.IsActive = true;
            this.Building = new BuildingModel();
            this.Address = new AddressModel();
        }
        public int LotID { get; set; }
        public string LotName { get; set; }
        public string ParkingAttendants { get; set; }
        public int AddressID { get; set; }
        public int MailingAddressID { get; set; }
        public string LotNumber { get; set; }
        public string LotLevel { get; set; }
        public int ParkingOperatorID { get; set; }
        public int ParkingManagerID { get; set; }
        public bool IsBuilding { get; set; }
        public bool IsActive { get; set; }
        public string Inventory { get; set; }
        public string Comments { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MailingPhoneNumber { get; set; }
        public string MailingFaxNumber { get; set; }
        public BuildingModel Building { get; set; }
        public AddressModel Address { get; set; }

        public int ReservedPrice { get; set; }
        public int UnReservedPrice { get; set; }
        public string hasMultipleBuildings { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
    }
    
    public class LotWideModel
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
    #endregion

    #region MakeModels
    public class EditMakeModel
    {
        public EditMakeModel()
        {
            this.isActive = true;
        }
        public int VehicleMakeID { get; set; }
        public string VehicleMake1 { get; set; }
        public string VehicleMakeDescription { get; set; }
        public bool isActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    #endregion
    
    #region VehicleModelModels
    public class EditVehicleModelModel
    {
        public int VehicleModelID { get; set; }
        public int VehicleMakeID { get; set; }
        public string VehicleModel1 { get; set; }
        public int Year1 { get; set; }
        public string VehicleModelDescription { get; set; }
        public bool? isActive { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    #endregion

    #region UserModels
    public class EditUserModel
    {
        public EditUserModel()
        {
            this.PersonalAddress = new AddressModel();
            this.WorkPhone = new PhoneModel();
            this.WorkFax = new PhoneModel();
            this.CellPhone = new PhoneModel();
            this.User = new UserProfileModel();
        }

        public bool iSCompany { get; set; }
        public int BuildingID { get; set; }

        public UserProfileModel User { get; set; }
        public CompanyModel Company { get; set; }
        public AddressModel PersonalAddress { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> ProfileTypes { get; set; }
        public IEnumerable<SelectListItem> Titles { get; set; }
        public IEnumerable<SelectListItem> ParkingOperators { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; } 
        public IEnumerable<SelectListItem> Buildings { get; set; }

        public PhoneModel WorkPhone { get; set; }
        public PhoneModel WorkFax { get; set; }
        public PhoneModel CellPhone { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }

    }
    #endregion

    #region AddressModels
    public class MailingAddressModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "*")]
        public string BuildingAddress { get; set; }

        [StringLength(256, ErrorMessage = "*")]
        public string AddressCont { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "*")]
        public string City { get; set; }

        [Required]
        public int StateID { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "*")]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "*")]
        public string Phone { get; set; }

        [StringLength(15, ErrorMessage = "*")]
        public string Fax { get; set; }
    }
    #endregion

    #region StallModels
    public class EditStallModel
    {
        public int ParkingStallID { get; set; }
        public int LotID { get; set; }
        public string StallLocation { get; set; }
        public int ParkingStallTypeID { get; set; }
        public decimal? MontlyRate { get; set; }
        public decimal? Rate { get; set; }
        public decimal? MaxRate { get; set; }
        public decimal? FlatRate { get; set; }
        public int? TimeIncrement { get; set; }
        public int? GracePeriod { get; set; }
        public int NumberStalls { get; set; }
        public int StallNumber { get; set; }
        public decimal? OverSell { get; set; }
        public string StallDescription { get; set; }

        public IEnumerable<SelectListItem> StallTypes { get; set; }
    }

    public class GetEditStallModel
    {
        public int ID { get; set; }
        public int LotID { get; set; }
        public bool Visitor { get; set; }
    }

    public class DataTableStallModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int LotID { get; set; }
    }
    
    public class DataTableAllocatedStallModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int StallID { get; set; }
    }
    #endregion

    #region BookModels
    public class DataTableValidationBookModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int? LotID { get; set; }
    }
    
    public class EditBookModel
    {
        public EditBookModel()
        {
            this.IsActive = true;
        }
        public int BookID { get; set; }
        public int LotID { get; set; }
        public bool IsActive { get; set; }
        public string BookName { get; set; }
        public int? TicketCount { get; set; }
        public decimal? Rate { get; set; }
    }

    public class ValidationBookModel
    {
        public int BookID { get; set; }
        public int LotID { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string BookName { get; set; }
        public int? TicketCount { get; set; }
        public decimal? Rate { get; set; }
    }

    #endregion

    #region KeyCards
    public class DataTableKeyCardModel
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string SearchString { get; set; }
        public int? LotID { get; set; }
    }
    
    public class EditKeyCardModel
    {
        public EditKeyCardModel()
        {
            this.IsActive = true;
        }
        public int KeyCardID { get; set; }
        public int LotID { get; set; }
        public bool IsActive { get; set; }
        public string KeyCardName { get; set; }
        public decimal? Rate { get; set; }
    }
    
    public class KeyCardModel
    {
        public int KeyCardID { get; set; }
        public int LotID { get; set; }
        public bool IsActive { get; set; }
        public string KeyCardName { get; set; }
        public decimal? Rate { get; set; }
    }
    #endregion

    #region LeaseModels
    public class EditLeaseAbstractModel
    {
        public EditLeaseAbstractModel()
        {
            this.Lease = new LeaseModel();
            this.Inventory = new ParkingInventoryModel();
        }
        public LeaseModel Lease { get; set; }
        public ParkingInventoryModel Inventory { get; set; }

        public IEnumerable<SelectListItem> Lots { get; set; }
        public IEnumerable<SelectListItem> StallTypes { get; set; }
    }
    
    public class EditLeaseModel
    {
        public int LeaseID { get; set; }
        public int CompanyID { get; set; }
        public int BuildingID { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
        public decimal LateFee { get; set; }
        public bool TaxExempt { get; set; }
        public bool MarketRate { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
    }
    
    public class LeaseModel
    {
        public LeaseModel()
        {
            this.Building = new BuildingModel();
            this.Company = new CompanyModel();
        }
        public int LeaseID { get; set; }
        public int CompanyID { get; set; }
        public int BuildingID { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime TermEnd { get; set; }
        public decimal LateFee { get; set; }
        public bool TaxExempt { get; set; }
        public bool MarketRate { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public CompanyModel Company { get; set; }
        public BuildingModel Building { get; set; }
    }
    #endregion

    #region SelectListModels
    public class zStateModel
    {
        public int StateID { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
    }

    [Serializable()]
    public class ParkingStallTypeModel
    {
        public int ParkingStallTypeID { get; set; }
        public string ParkingStallType1 { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
    #endregion
}