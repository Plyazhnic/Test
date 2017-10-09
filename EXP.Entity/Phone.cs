using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class Phone
    {
        public int PhoneID { get; set; }
        public int PhoneTypeID { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneDescription { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public virtual PhoneType PhoneType { get; set; }

        public override string ToString()
        {
            return string.Format("PhoneID: {0}, PhoneTypeID: {1}, AreaCode: {2}, PhoneNumber: {3}, PhoneDescription: {4}",
                PhoneID, PhoneTypeID, AreaCode, PhoneNumber, PhoneDescription);
        }
    }
}
