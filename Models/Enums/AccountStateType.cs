using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public enum AccountStateType
    {
        Active = 1,
        PendingActivation = 2,
        Banned = 3,
        Blocked = 4,
        Deleted = 5
    }
}



/*
 
 Active =>              الحساب مفعل وقيد الاستخدام
 PendingActivation =>   الحساب في انتظار التفعيل
 Banned =>             الحساب محظور بسبب خرق القواعد أو لأسباب أخرى 
 Blocked =>            الحساب مغلق مؤقتًا من قبل المسؤول أو المستخدم
 Deleted =>            الحساب محذوف نهائيًا
 
 */