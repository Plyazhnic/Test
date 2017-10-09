using System;
using System.Collections.Generic;
using EXP.Entity.Enumerations;

namespace EXP.Entity
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            this.ParkingInventory = new HashSet<ParkingInventory>();
            this.UserProfilePreferences = new HashSet<UserProfilePreferences>();
            this.Vehicle = new HashSet<Vehicle>();
        }
    
        public int UserProfileID { get; set; }
        public int? UserProfileTypeID { get; set; }
        public int UserProfileEncryptionTypeID { get; set; }
        public byte? UserProfileStatusID { get; set; }
        public string SessionID { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string ProfilePassword { get; set; }
        public string ProfilePasswordSalt { get; set; }
        public string EmailAddress { get; set; }
        public string TitlePreffix { get; set; }
        public string TitleSuffix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? AddressID { get; set; }
        public int? OperatorID { get; set; }
        public int? TenantID { get; set; }
        public string Comments { get; set; }
        public bool isSupervisor { get; set; }
        public Nullable<DateTime> HireDate { get; set; }
        public string AgreementVersion { get; set; }
        public System.DateTime AgreementDate { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public System.DateTime CreateDate { get; set; }

        public string UserProfileTypeDescription
        {
            get { return UserProfileTypeID==null ? "" : ((UserProfileTypeEnum)UserProfileTypeID).ToString(); }
        }

        public string UserProfileStatusDescription
        {
            get { return UserProfileStatusID == null ? "" : ((UserProfileStatusEnum)UserProfileStatusID).ToString(); }
        }
    
        public virtual ICollection<ParkingInventory> ParkingInventory { get; set; }
        public virtual ICollection<UserProfilePreferences> UserProfilePreferences { get; set; }
        public virtual UserProfileStatus UserProfileStatus { get; set; }
        public virtual UserProfileType UserProfileType { get; set; }
        public virtual ICollection<Vehicle> Vehicle { get; set; }
        public virtual ICollection<Alert> Alert { get; set; }

        public override string ToString()
        {
            return string.Format("userName: {0}, profileTypeId: {1}, email: {2}, firstName: {3}, lastName: {4}, statusId: {5}, addressId: {6}, comments: {7}, tPrefix: {8}, SessionID: {9}, TenantID: {10}, isSupervisor: {11}, HireDate: {12}, OperatorID: {13}",
                    LoginName, UserProfileTypeID, EmailAddress, FirstName, LastName, UserProfileStatusID, AddressID, Comments, TitlePreffix, SessionID, TenantID, isSupervisor, HireDate, OperatorID);
        }
    }
    
}
