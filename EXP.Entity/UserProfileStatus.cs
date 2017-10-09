using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class UserProfileStatus
    {
        public UserProfileStatus()
        {
            this.UserProfile = new HashSet<UserProfile>();
        }
    
        public byte UserProfileStatusID { get; set; }
        public string UserProfileStatus1 { get; set; }
        public string UserProfileStatusDescription { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual ICollection<UserProfile> UserProfile { get; set; }
    }
    
}
