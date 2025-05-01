using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum CartStatusType
    {
        Active = 1,
        PendingPayment = 2,
        Completed = 3,
        Cancelled = 4,
        Expired = 5
    }
}

/* 
 Active =>           عربة التسوق نشطة وقيد الاستخدام، ويمكن للعميل تعديلها أو إتمام عملية الدفع.
 PendingPayment =>   عربة التسوق في انتظار الدفع، حيث تم إضافة العناصر ولكن لم يتم الدفع بعد.
 Completed =>        تم إتمام عملية الشراء بنجاح، وتم تحويل العربة إلى حالة مكتملة.
 Cancelled =>        تم إلغاء عربة التسوق، سواء من قبل العميل أو من قبل الإدارة.
 Expired =>          عربة التسوق انتهت صلاحيتها، وتم إغلاقها بعد فترة محددة دون إتمام الشراء.
 
 */