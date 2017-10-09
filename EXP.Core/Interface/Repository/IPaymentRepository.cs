using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EXP.Entity;

namespace EXP.Core.Interface
{
    public interface IPaymentRepository
    {
        /// <summary>
        /// Create payment credit card for user
        /// </summary>
        /// <param name="newPayment"></param>
        void CreatePaymentCreditCard(Payment newPayment);

        /// <summary>
        /// Create payment online check for user
        /// </summary>
        /// <param name="newPayment"></param>
        void CreatePaymentOnlineCheck(Payment newPayment);

        /// <summary>
        /// Get payment for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Payment GetPaymentForUser(int userId);

        /// <summary>
        /// Update credit card data 
        /// </summary>
        /// <param name="creditCard"></param>
        void UpdateCreditCard(CreditCard creditCard);

        /// <summary>
        /// Update online check data 
        /// </summary>
        /// <param name="onlineCheck"></param>
        void UpdateOnlineCheck(OnlineCheck onlineCheck);
    }
}
