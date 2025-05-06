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
        public int Id { get; set; }
        // LearningPaths
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IconUrl { get; set; }

        // Courses
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int Duration { get; set; }
        public string? VideoUrl { get; set; }
        public string? ImgUrl { get; set; }


    }
    public class LearningPathsVM
    {
        public IEnumerable<ContentManagementVM> LearningPaths { get; set; }
        public PaginationVM Pagination { get; set; }
    }
}
