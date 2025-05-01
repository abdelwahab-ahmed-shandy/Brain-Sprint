using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserSession
    {
        [Key]
        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }

        public string IpAddress { get; set; }

        public string DeviceInfo { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public bool IsCurrent { get; set; }
    }
}

/*
 
 SessionId => معرف الجلسة
 UserId => معرف المستخدم
 LoginTime => وقت تسجيل الدخول
 LogoutTime => وقت تسجيل الخروج
 IpAddress => عنوان IP
 DeviceInfo => معلومات الجهاز
 IsActive => حالة الجلسة (نشطة أو غير نشطة)
 Carts => Represents the shopping carts of users

 */
