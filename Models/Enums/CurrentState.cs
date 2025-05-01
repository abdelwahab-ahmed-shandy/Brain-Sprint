using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum CurrentState
    {
        Inactive = 0,
        Active = 1,
        Archived = 2,
        Deleted = 3
    }
}

/*

 Inactive =>   العنصر غير نشط حاليًا ولا يظهر في الاستخدام العام.
 Active =>     العنصر مفعل ويُستخدم بشكل طبيعي.
 Archived =>   العنصر تم حفظه للأرشفة، عادة لا يتم تعديله أو التفاعل معه.
 Deleted =>    العنصر محذوف من الواجهة أو الاستخدام (منطقيًا أو فعليًا).

*/