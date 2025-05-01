using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Badge : BaseModel
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int? PointsRequired { get; set; }

        public List<UsersBadge> UsersBadges { get; set; } = new();
    }
}

/*
    Name => اسم الشارة
    Description => وصف توضيحي للشارة (اختياري)
    ImageUrl => رابط لصورة تمثل الشارة
    PointsRequired => عدد النقاط المطلوبة للحصول على الشارة (إن وجد)
    UsersBadges => قائمة بالعلاقات بين المستخدمين وهذه الشارة
*/
