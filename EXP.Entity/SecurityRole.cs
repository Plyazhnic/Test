using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class SecurityRole
    {
        public SecurityRole()
        {
            this.SecurityGroupMember = new HashSet<SecurityGroupMember>();
            this.SecurityRolePermission = new HashSet<SecurityRolePermission>();
        }
    
        public int SecurityRoleID { get; set; }
        public string SecurityRole1 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual ICollection<SecurityGroupMember> SecurityGroupMember { get; set; }
        public virtual ICollection<SecurityRolePermission> SecurityRolePermission { get; set; }
    }
    
}
