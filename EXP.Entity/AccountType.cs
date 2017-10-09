using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class AccountType
    {
        public AccountType()
        {
            this.Account = new HashSet<Account>();
        }
    
        public int AccountTypeID { get; set; }
        public string AccountType1 { get; set; }
        public string AccountTypeDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual ICollection<Account> Account { get; set; }
    }
    
}
