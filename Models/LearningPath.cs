using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LearningPath : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        public List<CourseLearningPath> CourseLearningPaths { get; set; } = new List<CourseLearningPath>();
    }
}

/*
    Name                  => اسم المسار التعليمي

    Description           => وصف المسار التعليمي

    IconUrl               => رابط الأيقونة التي تمثل المسار التعليمي (اختياري)

    CourseLearningPaths   => قائمة الدورات التدريبية التي تنتمي إلى هذا المسار التعليمي
*/

