using System;
using System.Collections.Generic;

namespace EXP.Entity
{
    public partial class zState
    {
        public int StateID { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
    
}
