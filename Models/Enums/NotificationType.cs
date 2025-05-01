using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum NotificationType
    {
        Success = 0,
        Error = 1,
        Warning = 2,
        Information = 3
    }
}

/*
 
 Success =>        نجاح العملية، يستخدم لإظهار رسالة نجاح.
 Error =>          خطأ في العملية، يستخدم لإظهار رسالة خطأ في حال حدوث مشكلة.
 Warning =>        تحذير، يستخدم لإظهار رسالة تحذير في حال وجود مشكلة محتملة.
 Information =>    معلومات، يستخدم لإظهار إشعارات معلوماتية أو تعليمية للمستخدم.
 
 */