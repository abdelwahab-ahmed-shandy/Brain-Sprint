using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        DebitCard = 3,
        PayPal = 4,
        BankTransfer = 5,
        Stripe = 6
    }
}

/*

 Cash =>          الدفع نقدًا عند الاستلام أو في نقطة البيع.
 CreditCard =>    الدفع باستخدام بطاقة ائتمان مثل Visa أو MasterCard.
 DebitCard =>     الدفع باستخدام بطاقة خصم مباشر مرتبط بالحساب البنكي.
 PayPal =>        الدفع عبر حساب بايبال.
 BankTransfer =>  الدفع عن طريق تحويل بنكي مباشر إلى الحساب.
 Stripe =>        الدفع باستخدام Stripe، وهي بوابة دفع إلكترونية شهيرة.

*/
