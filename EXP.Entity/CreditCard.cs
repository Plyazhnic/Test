using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXP.Entity
{
    public partial class CreditCard
    {
        public int CreditCardID { get; set; }
        public string CHFirstName { get; set; }
        public string CHLastName { get; set; }
        public string CardNumber { get; set; }
        public string ExpDateMount { get; set; }
        public string ExpDateYear { get; set; }
        public string CVV { get; set; }
        public bool AutoPay { get; set; }
        public int? AddressID { get; set; }
        public bool isHome { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public virtual Address Address { get; set; }

        public override string ToString()
        {
            return string.Format("CreditCardID: {0}, CHFirstName: {1}, CHLastName: {2}, CardNumber: {3}, ExpDateMount: {4}, ExpDateYear: {5}, CVV: {6}, AutoPay: {7}, AddressID: {8}, isHome: {9}, UpdatedDate: {10}, CreatedDate: {11}",
                CreditCardID, CHFirstName, CHLastName, CardNumber, ExpDateMount, ExpDateYear, CVV, AutoPay, AddressID, isHome, UpdatedDate, CreatedDate);
        }

        public bool IsCardNumberValid()
        {
            int total = 0;
            bool alt = false;
            char[] digits = this.CardNumber.ToCharArray();
            if (digits.Length != 16)
                return false;
            int first = 0;
            Int32.TryParse(digits[0].ToString(), out first);
            if (first == 3 || first == 4 || first == 5)
            {
                for (int i = digits.Length - 1; i >= 0; i--)
                {
                    int curDigit = (int)char.GetNumericValue(digits[i]);
                    if (alt)
                    {
                        curDigit *= 2;
                        if (curDigit > 9)
                            curDigit -= 9;
                    }
                    total += curDigit;
                    alt = !alt;
                }
                return total % 10 == 0;
            }
            return false;
        }
    }
}
