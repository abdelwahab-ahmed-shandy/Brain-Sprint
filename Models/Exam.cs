using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Exam : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public int Score { get; set; }
        public TimeSpan? MaximumTime { get; set; }

        public int CourseModuleId { get; set; }

        [ForeignKey("CourseModuleId")]
        public CourseModule CourseModule { get; set; } = null!;

        public List<Question> Questions { get; set; }
        public List<UserExamAttemp> UserExamAttemps { get; set; }
    }
}

