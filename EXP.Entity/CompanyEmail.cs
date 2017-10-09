using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class CompanyEmail
    {
        public int CompanyEmailsID { get; set; }
        public int CompanyID { get; set; }
        public string Email { get; set; }
        public bool Send { get; set; }
        public bool Active { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
    
}
