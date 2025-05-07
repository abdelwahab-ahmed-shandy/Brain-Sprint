using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ContentManagementVM
    {
        //Base:
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public string? BlockedBy { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }

        // LearningPaths
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        // Courses
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int? Duration { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImgUrl { get; set; }


    }
    public class LearningPathsVM
    {
        public IEnumerable<ContentManagementVM> LearningPaths { get; set; }
        public PaginationVM Pagination { get; set; }
    }

    public class CoursesVM
    {
        public IEnumerable<ContentManagementVM> Courses { get; set; }
        public PaginationVM Pagination { get; set; }

    }

}
