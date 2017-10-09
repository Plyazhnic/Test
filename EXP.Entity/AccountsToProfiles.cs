using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class AccountsToProfiles
    {
        public AccountsToProfiles()
        {
            this.SecurityGroupMember = new HashSet<SecurityGroupMember>();
        }
    
        public int AccountToProfileID { get; set; }
        public int AccountID { get; set; }
        public int UserProfileID { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<SecurityGroupMember> SecurityGroupMember { get; set; }
    }
    
}
