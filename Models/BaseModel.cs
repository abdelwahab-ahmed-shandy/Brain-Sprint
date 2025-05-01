using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public CurrentState CurrentState { get; set; } = CurrentState.Active;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDateUtc { get; set; }
    }
}

/*
    Id => المعرف الأساسي الفريد للكيان
    CurrentState => الحالة الحالية للعنصر (نشط، مؤرشف، محذوف، إلخ)
    CreatedBy => اسم المستخدم الذي أنشأ الكيان
    CreatedDateUtc => وقت إنشاء الكيان (بتوقيت UTC)
    UpdatedBy => اسم آخر مستخدم قام بتعديل الكيان (إن وجد)
    UpdatedDateUtc => وقت آخر تعديل (بتوقيت UTC) - اختياري
*/