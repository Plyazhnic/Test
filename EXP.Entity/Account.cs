using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class Account
    {
        public Account()
        {
            this.AccountsToProfiles = new HashSet<AccountsToProfiles>();
            this.BuildingRelationship = new HashSet<BuildingRelationship>();
        }
    
        public int AccountID { get; set; }
        public int AccountTypeID { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string CreatedDate { get; set; }
    
        public virtual AccountType AccountType { get; set; }
        public virtual ICollection<AccountsToProfiles> AccountsToProfiles { get; set; }
        public virtual ICollection<BuildingRelationship> BuildingRelationship { get; set; }
    }
    
}
