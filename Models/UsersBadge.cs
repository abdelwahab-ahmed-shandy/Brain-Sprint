using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Index(nameof(StudentId), nameof(BadgeId), IsUnique = true)]
    public class UsersBadge : BaseModel
    {
        public DateTime DateEarned { get; set; } = DateTime.UtcNow;

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;

        public int BadgeId { get; set; }
        [ForeignKey("BadgeId")]
        public Badge Badge { get; set; } = null!;
    }
}

