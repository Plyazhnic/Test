using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class Payment
    {
        public int PaymentID { get; set; }
        public int UserProfileID { get; set; }
        public bool isCreditCard { get; set; }
        public int? CreditCardID { get; set; }
        public int? OnlineCheckID { get; set; }

        public virtual OnlineCheck OnlineCheck { get; set; }
        public virtual CreditCard CreditCard { get; set; }

        public override string ToString()
        {
            return string.Format("PaymentID: {0}, UserProfileID: {1}, isCreditCard: {2}, CreditCardID: {3}, OnlineCheckID: {4}",
                PaymentID, UserProfileID, isCreditCard, CreditCardID, OnlineCheckID);
        }
    }
}
