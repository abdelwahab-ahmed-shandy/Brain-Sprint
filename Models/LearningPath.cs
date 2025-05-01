using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LearningPath : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? IconUrl { get; set; }

        public List<CourseLearningPath> CourseLearningPaths { get; set; }
    }
}

/*
    Name                  => اسم المسار التعليمي

    Description           => وصف المسار التعليمي

    IconUrl               => رابط الأيقونة التي تمثل المسار التعليمي (اختياري)

    CourseLearningPaths   => قائمة الدورات التدريبية التي تنتمي إلى هذا المسار التعليمي
*/

