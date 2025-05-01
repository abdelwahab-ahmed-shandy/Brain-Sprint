using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CourseLearningPath : BaseModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; }
    }
}

/*
    CourseId             => المعرف الخاص بالدورة
    Course               => الكائن الذي يمثل الدورة المرتبطة بهذا المسار التعليمي

    LearningPathId       => المعرف الخاص بالمسار التعليمي
    LearningPath         => الكائن الذي يمثل المسار التعليمي المرتبط بهذه الدورة
*/
