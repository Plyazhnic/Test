using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class SecurityGroup
    {
        public SecurityGroup()
        {
            this.SecurityGroupMember = new HashSet<SecurityGroupMember>();
        }
    
        public int SecurityGroupID { get; set; }
        public string SecurityGroup1 { get; set; }
        public string SecurityGroupDescription { get; set; }
    
        public virtual ICollection<SecurityGroupMember> SecurityGroupMember { get; set; }
    }
    
}
