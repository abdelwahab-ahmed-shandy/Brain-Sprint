using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ContentManagementCreateVM
    {
        // LearningPaths
        public class LearningPathsCreateVM
        {
            [Required(ErrorMessage = "Name is required.")]
            [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Description is required.")]
            [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
            public string Description { get; set; } = string.Empty;

            [Url(ErrorMessage = "Please enter a valid URL.")]
            public string? IconUrl { get; set; }
        }

        // Courses
        public class CoursesCreateVM
        {
            public string Title { get; set; } = string.Empty;
            public double Price { get; set; }
            public double? Discount { get; set; }
            public int Duration { get; set; }
            public string? VideoUrl { get; set; }
            public string? ImgUrl { get; set; }
        }

    }
}
