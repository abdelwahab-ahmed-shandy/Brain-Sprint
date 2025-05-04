using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CourseReview : BaseModel
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}

