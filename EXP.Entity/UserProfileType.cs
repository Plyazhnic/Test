using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class UserProfileType
    {
        public UserProfileType()
        {
            this.UserProfile = new HashSet<UserProfile>();
        }
    
        public int UserProfileTypeID { get; set; }
        public string UserProfileType1 { get; set; }
        public string UserProfileTypeDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
    
}
