using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class OnlineCheck
    {
        public int OnlineCheckID { get; set; }
        public string NameOnAccount { get; set; }
        public int OnlineCheckingTypeID { get; set; }
        public string BankName { get; set; }
        public string RoutingNumber { get; set; }
        public string CheckingAccountNumber { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual OnlineCheckingType OnlineCheckingType { get; set; }

        public override string ToString()
        {
            return string.Format("OnlineCheckID: {0}, NameOnAccount: {1}, OnlineCheckingTypeID: {2}, BankName: {3}, RoutingNumber: {4}, CheckingAccountNumber: {5}, UpdatedDate: {6}, CreatedDate: {7}",
                OnlineCheckID, NameOnAccount, OnlineCheckingTypeID, BankName, RoutingNumber, CheckingAccountNumber, UpdatedDate, CreatedDate);
        }
    }
}
