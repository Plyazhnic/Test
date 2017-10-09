using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class PhoneType
    {
        public int PhoneTypeID { get; set; }
        public string PhoneType1 { get; set; }
        public string PhoneTypeDescription { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
