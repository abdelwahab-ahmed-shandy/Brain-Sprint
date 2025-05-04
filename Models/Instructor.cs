using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Instructor : BaseModel
    {
        public int? Rating { get; set; }
        public bool IsVerified { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public List<Course> Courses { get; set; } = new List<Course>();
    }
}


