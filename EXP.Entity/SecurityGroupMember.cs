using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class SecurityGroupMember
    {
        public int SecurityGroupID { get; set; }
        public int SecurityRoleID { get; set; }
        public int AccountToProfileID { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual SecurityGroup SecurityGroup { get; set; }
        public virtual SecurityRole SecurityRole { get; set; }
    }
    
}
