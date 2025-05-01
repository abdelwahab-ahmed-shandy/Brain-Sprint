using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum NodeType
    {
        Undefined = 0,
        Video = 1,
        Text = 2,
        Image = 3,
        Audio = 4
    }
}

/*
 
 Undefined =>  نوع غير معرف أو غير محدد، ويمكن استخدامه لتحديد الحالات التي لا يوجد فيها نوع معروف.
 Video =>      نوع الفيديو، يستخدم لتمثيل محتوى الفيديو.
 Text =>       نوع النص، يستخدم لتمثيل محتوى نصي.
 Image =>      نوع الصورة، يستخدم لتمثيل محتوى صورة.
 Audio =>      نوع الصوت، يستخدم لتمثيل محتوى صوتي.
 
 */