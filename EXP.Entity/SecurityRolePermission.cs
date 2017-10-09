using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class SecurityRolePermission
    {
        public int SecurityRolePermissionID { get; set; }
        public int SecurityRoleID { get; set; }
        public string RolePrivelage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual SecurityRole SecurityRole { get; set; }
    }
    
}
