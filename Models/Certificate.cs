using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Certificate : BaseModel
    {
        public string StudentScore { get; set; }

        public int EnrollmentCourseId { get; set; }

        [ForeignKey("EnrollmentCourseId")]
        public EnrollmentCourse EnrollmentCourse { get; set; }
    }
}
