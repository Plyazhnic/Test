using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class OnlineCheckingType
    {
        public int OnlineCheckingTypeID { get; set; }
        public string OnlineCheckingType1 { get; set; }
        public string OnlineCheckingTypeDescription { get; set; }
        public string isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
