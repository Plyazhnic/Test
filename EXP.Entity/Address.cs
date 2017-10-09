using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class Address
    {
        public int AddressID { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? StateID { get; set; }
        public string ZipCode { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public System.DateTime CreateDate { get; set; }

        public zState State { get; set; }

        public override string ToString()
        {
            return string.Format("AddressID: {0}, City: {1}, Address1: {2}, Address2: {3}, StateID: {4}, ZipCode: {5}, UpdateDate: {6}, CreateDate: {7}",
                AddressID, City, Address1, Address2, StateID, ZipCode, UpdateDate, CreateDate);
        }
    }
}
