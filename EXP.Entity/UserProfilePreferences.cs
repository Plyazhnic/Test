using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class UserProfilePreferences
    {
        public int ProfilePreferencesID { get; set; }
        public Nullable<int> UserProfileID { get; set; }
        public string ProfilePreference { get; set; }
        public string PreferenceValue { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
    
}
