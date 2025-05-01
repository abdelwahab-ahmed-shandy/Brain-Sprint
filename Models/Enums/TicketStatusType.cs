using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum TicketStatusType
    {
        Opened = 1,
        InProgress = 2,
        Resolved = 3,
        Closed = 4
    }
}

/*

 Opened =>      التذكرة تم إنشاؤها ومفتوحة بانتظار مراجعتها من الفريق المختص.
 InProgress =>  فريق الدعم بدأ العمل على المشكلة المطروحة في التذكرة.
 Resolved =>    المشكلة تم حلها، في انتظار تأكيد العميل أو الرد النهائي.
 Closed =>      التذكرة أُغلقت نهائيًا، إما بعد التأكيد أو بسبب انتهاء الحالة.

*/