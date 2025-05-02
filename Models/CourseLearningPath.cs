using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CourseLearningPath : BaseModel
    {
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; } = new Course();

        public int LearningPathId { get; set; }
        [ForeignKey("LearningPathId")]
        public LearningPath LearningPath { get; set; } = new LearningPath();
    }
}

/*
    CourseId             => المعرف الخاص بالدورة
    Course               => الكائن الذي يمثل الدورة المرتبطة بهذا المسار التعليمي

    LearningPathId       => المعرف الخاص بالمسار التعليمي
    LearningPath         => الكائن الذي يمثل المسار التعليمي المرتبط بهذه الدورة
*/
